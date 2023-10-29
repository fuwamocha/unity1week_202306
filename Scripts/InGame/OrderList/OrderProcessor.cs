using System;
using Game.Scripts.InGame.Ordered;
using Game.Scripts.InGame.Score;
using UniRx;
using VContainer.Unity;
namespace Game.Scripts.InGame.OrderList
{
    public class OrderProcessor : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposable = new();
        private readonly Func<OrderedWaffle> _waffleFactory;

        public OrderProcessor(
            Func<OrderedWaffle> waffleFactory
        )
        {
            _waffleFactory = waffleFactory;
        }
        public void Dispose()
        {
            _disposable?.Dispose();
        }
        public void Initialize() { }


        public OrderedWaffle CreateAndHandleOrder()
        {
            var orderedWaffle = CreateOrderedWaffle();
            orderedWaffle.IsSucceeded
                .Subscribe(isSuccess => HandleWaffleResult(isSuccess, orderedWaffle));
            return orderedWaffle;
        }

        private OrderedWaffle CreateOrderedWaffle()
        {
            var waffle = _waffleFactory.Invoke();
            waffle.Initialize();
            return waffle;
        }

        private static void HandleWaffleResult(bool isSuccess, OrderedWaffle waffle)
        {
            if (isSuccess)
            {
                UpdateScoreOnSuccess(waffle);
            }
            else
            {
                UpdateScoreOnFailure(waffle);
            }
        }

        private static void UpdateScoreOnSuccess(OrderedWaffle waffle)
        {
            ScoreManager.AddScore(waffle.Toppings, waffle.RemainedTime);
            ScoreManager.AddCombo();
            ScoreManager.AddSupplyWaffleCount();
        }

        private static void UpdateScoreOnFailure(OrderedWaffle waffle)
        {
            ScoreManager.SubScore(waffle.Toppings);
        }
    }
}
