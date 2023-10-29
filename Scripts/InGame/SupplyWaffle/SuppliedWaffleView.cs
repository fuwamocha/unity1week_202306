using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
namespace Game.Scripts.InGame.SupplyWaffle
{
    public class SuppliedWaffleView : MonoBehaviour
    {
        [SerializeField] private Button _waffleButton;

        private void Awake()
        {
            Hide();
        }
        public void ChangeWaffleActivate(bool isActive)
        {
            _waffleButton.interactable = isActive;
        }
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public void AnimateButton()
        {
            DOTween.Sequence()
                .Append(_waffleButton.gameObject.transform.DOScaleY(0.4f, 0.04f).SetEase(Ease.OutBounce))
                .Join(_waffleButton.gameObject.transform.DOScaleX(0.55f, 0.06f).SetEase(Ease.OutBounce))
                .Append(_waffleButton.gameObject.transform.DOScale(0.5f, 0.1f).SetEase(Ease.OutBack))
                .SetLink(_waffleButton.gameObject);
        }

        public IObservable<Unit> OnClickWaffleAsObservable() => _waffleButton.OnClickAsObservable().ThrottleFirst(TimeSpan.FromMilliseconds(10));
    }
}
