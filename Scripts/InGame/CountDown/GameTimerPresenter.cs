using System;
using UniRx;
using UnityEngine;
using VContainer.Unity;
namespace MochaLib.InGame.CountDown
{
    public class GameTimerPresenter : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposable = new();
        private readonly GameTimerView _gameTimerView;
        private readonly InGameStateUseCase _inGameStateUseCase;
        private readonly RemainingTimer _remainingTimer;

        public GameTimerPresenter(
            InGameStateUseCase inGameStateUseCase,
            RemainingTimer remainingTimer,
            GameTimerView gameTimerView)
        {
            _inGameStateUseCase = inGameStateUseCase;
            _remainingTimer = remainingTimer;
            _gameTimerView = gameTimerView;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        public void Initialize()
        {
            _remainingTimer.RemainingTime
                .Select(f => Mathf.FloorToInt(f))
                .DistinctUntilChanged()
                .Subscribe(remainingTime => _gameTimerView.Set(TimeSpan.FromSeconds(remainingTime)))
                .AddTo(_disposable);

            _inGameStateUseCase.OnChangeState()
                .Where(_ => _inGameStateUseCase.IsState(InGameStateType.Result)).Subscribe(_ =>
                {
                    _gameTimerView.ResetText();
                });
        }
    }
}
