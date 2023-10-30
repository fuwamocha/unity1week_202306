using System;
using UniRx;
using VContainer.Unity;
namespace MochaLib.InGame.CountDown
{
    public class RemainingTimePresenter : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposable = new();
        private readonly RemainingTimer _remainingTimer;
        private readonly TimeCounter _timeCounter;

        public RemainingTimePresenter(TimeCounter timeCounter, RemainingTimer remainingTimer)
        {
            _timeCounter = timeCounter;
            _remainingTimer = remainingTimer;
        }

        public void Dispose() => _disposable?.Dispose();

        public void Initialize()
        {
            _timeCounter.ElapsedTime
                .Where(_ => !_remainingTimer.IsFinish())
                .Subscribe(elapsedTime => _remainingTimer.SetElapsedTime(elapsedTime))
                .AddTo(_disposable);
        }
    }
}
