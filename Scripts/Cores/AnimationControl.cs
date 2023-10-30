using System.Collections;
using Game.Scripts;
using UnityEngine;
namespace MochaLib.Cores
{
    public class AnimationControl : IAnimatable
    {
        private readonly Animator _animator;

        public AnimationControl(Animator animator)
        {
            _animator = animator;
        }

        public void Animate(int animationHash)
        {
            _animator.Play(animationHash);
        }
        public IEnumerator WaitUntilAnimationEnd()
        {
            yield return null;
            yield return new WaitAnimation(_animator, 0);
        }
    }
}
