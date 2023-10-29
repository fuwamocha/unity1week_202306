using System;
using Game.Scripts.InGame.Toppings;
using MochaLib.Novel;
using UniRx;
using VContainer.Unity;
namespace Game.Scripts.Character
{
    public class Character : IInitializable, IDisposable, INovelAction
    {
        private readonly CharacterAnimate _characterAnimate;
        private readonly CharacterMove _characterMove;
        private readonly CompositeDisposable _disposable = new();
        private readonly DoyoAction _doyoAction;
        public Character(
            CharacterAnimate characterAnimate,
            CharacterMove characterMove,
            DoyoAction doyoAction)
        {
            _characterAnimate = characterAnimate;
            _characterMove = characterMove;
            _doyoAction = doyoAction;
        }

        public void Dispose() => _disposable?.Dispose();
        public void Initialize() { }
        public void Smile()
        {
            _characterAnimate.Smile();
        }
        public void Sigh()
        {
            _characterAnimate.Sigh();
        }

        public void GameStart()
        {
            _characterAnimate.OutTentacles();
            _characterMove.MoveWorkPosition();
        }
        public void GameEnd()
        {
            _characterAnimate.Smile();
            _characterMove.MoveSidePosition();
        }

        public void Miss()
        {
            _characterAnimate.Shocked();
            _doyoAction.Animate();
        }
        public void Top(ToppingType toppingType)
        {
            _characterAnimate.Top(toppingType);
        }
    }
}
