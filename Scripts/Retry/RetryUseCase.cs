using System;
using UniRx;
namespace Game.Scripts.Retry
{
    public class RetryUseCase
    {
        private readonly ISubject<Unit> _onRetry = new Subject<Unit>();

        public IObservable<Unit> OnRetry() => _onRetry;

        public void Retry() => _onRetry.OnNext(Unit.Default);
    }
}
