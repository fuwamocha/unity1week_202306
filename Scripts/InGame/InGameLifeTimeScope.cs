using Game.Scripts.Character;
using Game.Scripts.InGame.Ordered;
using Game.Scripts.InGame.OrderList;
using Game.Scripts.InGame.Score;
using Game.Scripts.InGame.SupplyWaffle;
using Game.Scripts.InGame.Toppings;
using Game.Scripts.Result;
using Game.Scripts.Retry;
using MessagePipe;
using MochaLib.GameReady.CountDown;
using MochaLib.InGame.CountDown;
using MochaLib.Novel;
using UnityEngine;
using VContainer;
using VContainer.Unity;
namespace Game.Scripts.InGame
{
    public class InGameLifetimeScope : LifetimeScope
    {
        [SerializeField] private Transform _frontCanvasRoot;
        [SerializeField] private GameReadyView _gameReadyViewPrefab;
        [SerializeField] private OrderedWaffle _orderPrefab;
        [SerializeField] private Transform _orderParent;
        [SerializeField] private ResultView _resultViewPrefab;
        protected override void Configure(IContainerBuilder builder)
        {
            // Time
            builder.Register<TimeCounter>(Lifetime.Scoped).As<ITickable>().AsSelf();
            builder.Register<RemainingTimer>(Lifetime.Scoped);
            builder.RegisterEntryPoint<GameTimerPresenter>();
            builder.RegisterEntryPoint<RemainingTimePresenter>();
            builder.RegisterComponentInHierarchy<GameTimerView>();
            var options = builder.RegisterMessagePipe();

            // Ready
            builder.RegisterFactory(() => Instantiate(_gameReadyViewPrefab, _frontCanvasRoot));
            builder.Register<GameReadyPresenter>(Lifetime.Scoped);
            builder.Register<RetryUseCase>(Lifetime.Scoped);

            builder.RegisterFactory(() => Instantiate(_orderPrefab, _orderParent));

            // Waffle
            builder.RegisterEntryPoint<OrderList.OrderList>().AsSelf();
            builder.RegisterEntryPoint<OrderProcessor>().AsSelf();
            builder.RegisterComponentInHierarchy<ToppingAnimation>();
            builder.RegisterEntryPoint<ToppingAnimationPresenter>().AsSelf();

            builder.RegisterComponentInHierarchy<SuppliedWaffleView>();
            builder.RegisterComponentInHierarchy<ToppingsView>();
            builder.Register<WaffleSupplyModel>(Lifetime.Scoped);
            builder.RegisterEntryPoint<WaffleSupplyPresenter>().AsSelf();

            // Character
            builder.RegisterComponentInHierarchy<DoyoAction>().AsSelf();
            builder.RegisterComponentInHierarchy<CharacterAnimate>();
            builder.RegisterComponentInHierarchy<CharacterMove>();
            builder.RegisterEntryPoint<Character.Character>().As<INovelAction>().AsSelf();
            // Novel
            builder.RegisterComponentInHierarchy<NovelManager>();
            builder.RegisterEntryPoint<NovelPresenter>();

            // Result
            builder.RegisterMessageBroker<ResultData>(options);
            builder.Register<GameResultUseCase>(Lifetime.Scoped);
            builder.RegisterFactory(() => Instantiate(_resultViewPrefab, _frontCanvasRoot));
            builder.RegisterEntryPoint<ResultPresenter>();

            // Score
            builder.RegisterComponentInHierarchy<ScoreView>();
            builder.RegisterComponentInHierarchy<ComboView>();
            builder.RegisterEntryPoint<ScorePresenter>().AsSelf();


            // Loop
            builder.RegisterEntryPoint<InGameLoop>();
        }
    }
}
