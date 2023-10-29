using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Retry;
using MessagePipe;
using MochaLib.Audio;
using MochaLib.InGame;
using MochaLib.Scene;
using MochaLib.Settings;
using UniRx;
using UnityEngine;
using VContainer.Unity;
namespace Game.Scripts.Result
{
    public class ResultPresenter : IInitializable, IDisposable
    {
        private readonly AudioPlayer _audioPlayer;
        private readonly CompositeDisposable _disposable = new();
        private readonly InGameStateUseCase _inGameStateUseCase;
        private readonly ISubscriber<ResultData> _onResult;
        private readonly RetryUseCase _retryUseCase;
        private readonly SceneTransitionView _sceneTransitionView;
        private readonly StateUseCase _stateUseCase;
        private readonly Func<ResultView> _viewFactory;

        private CancellationTokenSource _cancellationTokenSource = new();
        private ResultView _view;

        public ResultPresenter(
            InGameStateUseCase inGameStateUseCase,
            StateUseCase stateUseCase,
            SceneTransitionView sceneTransitionView,
            ISubscriber<ResultData> onResult,
            Func<ResultView> viewFactory,
            RetryUseCase retryUseCase,
            AudioPlayer audioPlayer)
        {
            _stateUseCase = stateUseCase;
            _inGameStateUseCase = inGameStateUseCase;
            _sceneTransitionView = sceneTransitionView;
            _onResult = onResult;
            _viewFactory = viewFactory;
            _retryUseCase = retryUseCase;
            _audioPlayer = audioPlayer;
        }

        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
            _disposable?.Dispose();
        }

        public void Initialize()
        {
            _view = _viewFactory.Invoke();
            _onResult
                .Subscribe(_ =>
                {
                    _cancellationTokenSource = new CancellationTokenSource();
                    ResultSequence(_cancellationTokenSource.Token).Forget();
                })
                .AddTo(_disposable);

            _view.OnClickBackToTitleAsObservable()
                .Merge(Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.T)).AsUnitObservable())
                .Subscribe(_ =>
                {
                    ChangeScene().Forget();
                })
                .AddTo(_disposable);

            _inGameStateUseCase.OnChangeState()
                .Where(_ => _inGameStateUseCase.IsState(InGameStateType.Result))
                .Subscribe(_ =>
                {
                    _view.UpdateText();
                    _view.MoveCharacter().Forget();
                });
            _inGameStateUseCase.OnChangeState()
                .Where(_ => _inGameStateUseCase.IsState(InGameStateType.Ready))
                .Skip(1).Subscribe(_ => _view.ResetStatus());
            // Observable.EveryUpdate().Select(_ => StateMachine.CurrentState.Value == GameState.InGame)
            //     .Where(_ => Input.GetKeyDown(KeyCode.R)).AsUnitObservable().Subscribe(_ =>
            //     {
            //         if (StateMachine.CurrentState.Value != GameState.InGame) return;
            //         StateMachine.SetState(GameState.Reset);
            //         StateMachine.SetState(GameState.InGame);
            //     });
        }

        private async UniTask ResultSequence(CancellationToken cancellationToken)
        {
            await _audioPlayer.StopBgm(0.5f);

            await _view.Show(_audioPlayer);

            await UniTask.WhenAny(
                _view.OnClickRetryAsObservable().ToUniTask(true, cancellationToken),
                Observable.EveryUpdate().Select(_ => _inGameStateUseCase.IsState(InGameStateType.Result)
                    ).Where(_ => Input.GetKeyDown(KeyCode.R)).ToUniTask(true, cancellationToken)
                );

            _audioPlayer.PlaySe(CommonConstants.Audio.Se.Click);
            await _view.Hide();
            _retryUseCase.Retry();
        }
        private async UniTask<Unit> ChangeScene()
        {
            if (!_inGameStateUseCase.IsState(InGameStateType.Result)) return Unit.Default;
            _audioPlayer.PlaySe(CommonConstants.Audio.Se.Click);
            _audioPlayer.StopBgm(0.5f).Forget();
            await _view.Hide();
            await _sceneTransitionView.FadeOut(GameState.InGame, 0.5f);
            await _sceneTransitionView.FadeIn(GameState.Title, 0.5f);

            _stateUseCase.SetState(GameState.Title);
            return Unit.Default;
        }
    }
}
