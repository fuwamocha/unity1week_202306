using System.Collections;
using Game.Scripts.InGame.Toppings;
using MochaLib.Cores;
using UnityEngine;
using CharacterAnimation = MochaLib.Settings.CommonConstants.Character.Animation;
namespace Game.Scripts.Character
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimate : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private AnimationControl _animationControl;

        private void Awake()
        {
            _animationControl = new AnimationControl(_animator);
        }
        private void Reset()
        {
            _animator = GetComponent<Animator>();
        }
        public IEnumerator WaitUntilAnimationEnd() => _animationControl.WaitUntilAnimationEnd();

        public void Smile()
        {
            _animationControl.Animate(CharacterAnimation.Success);
        }
        public void Shocked() { _animationControl.Animate(CharacterAnimation.Miss); }
        public void Sigh() { _animationControl.Animate(CharacterAnimation.Sigh); }
        public void Top(ToppingType toppingType) { _animationControl.Animate(CharacterAnimation.Top(toppingType)); }
        public void OutTentacles() { _animationControl.Animate(CharacterAnimation.OutTentacles); }
    }
}
