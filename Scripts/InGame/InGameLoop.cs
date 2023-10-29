using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.InGame.OrderList;
using Game.Scripts.InGame.Score;
using Game.Scripts.InGame.SupplyWaffle;
using Game.Scripts.InGame.Toppings;
using Game.Scripts.Result;
using Game.Scripts.Retry;
using MochaLib.Audio;
using MochaLib.GameReady.CountDown;
using MochaLib.InGame;
using MochaLib.InGame.CountDown;
using MochaLib.Settings;
using UniRx;
using UnityEngine;
using VContainer.Unity;
namespace Game.Scripts.InGame
{
    public class InGameLoop : IInitializable, IDisposable
    {
        private readonly AudioPlayer _audioPlayer;
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly Character.Character _character;
        private readonly CompositeDisposable _disposable = new();
        private readonly GameReadyPresenter _gameReadyPresenter;
        private readonly GameResultUseCase _gameResultUseCase;
        private readonly InGameStateUseCase _inGameStateUseCase;
        private readonly OrderList.OrderList _orderList;
        private readonly OrderProcessor _orderProcessor;
        private readonly RemainingTimer _remainingTimer;
        private readonly RetryUseCase _retryUseCase;
        private readonly ScorePresenter _scorePresenter;

        private readonly StateUseCase _stateUseCase;
        private readonly TaskAwaiter _taskAwaiter = new();
        private readonly TimeCounter _timeCounter;
        private readonly ToppingAnimationPresenter _toppingAnimationPresenter;
        private readonly WaffleSupplyPresenter _waffleSupplyPresenter;

        public InGameLoop(
            StateUseCase stateUseCase,
            GameResultUseCase gameResultUseCase,
            GameReadyPresenter gameReadyPresenter,
            InGameStateUseCase inGameStateUseCase,
            TimeCounter timeCounter,
            RemainingTimer remainingTimer,
            AudioPlayer audioPlayer,
            RetryUseCase retryUseCase,
            Character.Character character,
            OrderProcessor orderProcessor,
            OrderList.OrderList orderList,
            ScorePresenter scorePresenter,
            ToppingAnimationPresenter toppingAnimationPresenter,
            WaffleSupplyPresenter waffleSupplyPresenter
        )
        {
            _stateUseCase = stateUseCase;
            _gameResultUseCase = gameResultUseCase;
            _gameReadyPresenter = gameReadyPresenter;
            _inGameStateUseCase = inGameStateUseCase;
            _timeCounter = timeCounter;
            _remainingTimer = remainingTimer;
            _audioPlayer = audioPlayer;
            _retryUseCase = retryUseCase;

            _character = character;
            _orderProcessor = orderProcessor;
            _orderList = orderList;
            _scorePresenter = scorePresenter;
            _toppingAnimationPresenter = toppingAnimationPresenter;
            _waffleSupplyPresenter = waffleSupplyPresenter;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _cancellationTokenSource?.Dispose();
        }

        public void Initialize()
        {
            // リトライ検出
            _retryUseCase.OnRetry()
                .Merge(Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.Slash)).AsUnitObservable())
                .Where(_ => _inGameStateUseCase.IsState(InGameStateType.Playing) || _inGameStateUseCase.IsState(InGameStateType.Result))
                .Subscribe(_ =>
                {
                    Debug.Log("retry");
                    StartGame(_cancellationTokenSource.Token).Forget();
                })
                .AddTo(_disposable)
                .AddTo(_cancellationTokenSource.Token);

            _stateUseCase.OnChangeState()
                .Where(_ => _stateUseCase.IsState(GameState.InGame))
                .Subscribe(_ => StartGame(_cancellationTokenSource.Token).Forget())
                .AddTo(_disposable)
                .AddTo(_cancellationTokenSource.Token);
        }

        private async UniTask StartGame(CancellationToken cancellation)
        {
            ScoreManager.ResetScore();
            _timeCounter.Reset();

            // await _audioPlayer.PlayBgm(CommonConstants.Audio.Bgm.InGame);

            _remainingTimer.SetTime(TimeSpan.FromSeconds(30));
            _inGameStateUseCase.SetState(InGameStateType.Ready);

            _toppingAnimationPresenter.Ready();
            _character.GameStart();

            await _gameReadyPresenter.Ready();
            _gameReadyPresenter.Hide();

            _inGameStateUseCase.SetState(InGameStateType.Playing);

            _timeCounter.Start();
            await UniTask.Delay(TimeSpan.FromSeconds(3f), cancellationToken: cancellation);

            _orderList.AddOrder(_orderProcessor.CreateAndHandleOrder());

            while (_inGameStateUseCase.IsState(InGameStateType.Playing))
            {
                await _taskAwaiter.WaitForConditionOrTime(_orderList.OrderedWaffles);
                var waffle = _orderProcessor.CreateAndHandleOrder();
                _orderList.AddOrder(waffle);
                waffle.IsSucceeded
                    .Subscribe(_ => _orderList.RemoveOrder(waffle))
                    .AddTo(_disposable);
            }

            _waffleSupplyPresenter.Start();

            ScoreManager.ResetIsReset();
            while (!cancellation.IsCancellationRequested)
            {
                var task = _remainingTimer.OnFinish.ToUniTask(true, cancellation);
                await task;
                break;
            }

            await GameEnd(cancellation);
        }

        private async UniTask GameEnd(CancellationToken cancellationToken)
        {
            _inGameStateUseCase.SetState(InGameStateType.Result);

            _timeCounter.Stop();
            _scorePresenter.Stop();

            _waffleSupplyPresenter.End();
            _orderList.End();
            _toppingAnimationPresenter.End();
            _character.GameEnd();

            _audioPlayer.PlaySe(CommonConstants.Audio.Se.StopPlaying);
            await UniTask.Delay(2000, cancellationToken: cancellationToken);
            _gameResultUseCase.In(new ResultData());
        }
    }
}
