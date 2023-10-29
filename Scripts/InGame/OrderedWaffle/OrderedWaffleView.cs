using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Scripts.InGame.SupplyWaffle;
using MochaLib.Cores;
using MochaLib.Settings;
using UnityEngine;
using UnityEngine.UI;
namespace Game.Scripts.InGame.Ordered
{
    public class OrderedWaffleView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Image _timerBar;
        [SerializeField] private ToppingsView _toppingsView;

        private AnimationControl _animationControl;

        private void Awake()
        {
            gameObject.transform.localScale = Vector3.zero;
            gameObject.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
            _animationControl = new AnimationControl(_animator);
        }

        public void Set(bool[] toppings)
        {
            _toppingsView.Set(toppings);
        }

        public void UpdateTimerBar(float time)
        {
            float timeRatio = time / CommonConstants.Waffle.TimeLimit;
            ChangeFillAmount(timeRatio);

            if (timeRatio < 0.5f)
            {
                ChangeTimerColor(Color.yellow);
            }
            else if (timeRatio < 0.2f)
            {
                ChangeTimerColor(Color.red);
            }
        }
        private void ChangeFillAmount(float amount)
        {
            _timerBar.fillAmount = amount;
        }
        private void ChangeTimerColor(Color color)
        {
            _timerBar.color = color;
        }

        public async void FailedAnimation()
        {
            _animationControl.Animate(CommonConstants.Waffle.Animation.DeleteOrder);
            await _animationControl.WaitUntilAnimationEnd();
            Destroy(gameObject);
        }

        public async void SuccessAnimation()
        {
            _animationControl.Animate(CommonConstants.Waffle.Animation.SuccessOrder);
            await _animationControl.WaitUntilAnimationEnd();
            Destroy(gameObject);
        }
    }
}
