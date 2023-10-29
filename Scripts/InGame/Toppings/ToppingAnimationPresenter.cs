using System;
using MochaLib.InGame;
using UniRx;
using VContainer.Unity;
namespace Game.Scripts.InGame.Toppings
{
    public class ToppingAnimationPresenter : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposable = new();
        private readonly ToppingAnimation _toppingAnimation;

        public ToppingAnimationPresenter(
            ToppingAnimation toppingAnimation
        )
        {
            _toppingAnimation = toppingAnimation;
        }

        public void Dispose() => _disposable?.Dispose();

        public void Initialize()
        {
        }

        public void Ready() => _toppingAnimation.InGameAnim();

        public void End()
        {
            _toppingAnimation.ResultAnim();
            _toppingAnimation.ResetPosition();
        }
    }
}
