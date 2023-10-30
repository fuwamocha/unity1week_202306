using UnityEngine;
namespace MochaLib.Cores
{
    public class WaitAnimation : CustomYieldInstruction
    {
        private Animator _animator;
        private int _lastStateHash;
        private int _layerNo;

        public WaitAnimation(Animator animator, int layerNo)
        {
            Init(animator, layerNo, animator.GetCurrentAnimatorStateInfo(layerNo).fullPathHash);
        }

        public override bool keepWaiting
        {
            get
            {
                var currentAnimatorState = _animator.GetCurrentAnimatorStateInfo(_layerNo);
                return currentAnimatorState.fullPathHash == _lastStateHash &&
                    currentAnimatorState.normalizedTime < 1;
            }
        }

        private void Init(Animator animator, int layerNo, int hash)
        {
            _layerNo = layerNo;
            _animator = animator;
            _lastStateHash = hash;
        }
    }
}
