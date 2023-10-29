using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.InGame.Toppings;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
namespace Game.Scripts.InGame.SupplyWaffle
{
    public class ToppingsView : MonoBehaviour
    {
        [SerializeField] private Image[] _kiwiImages;
        [SerializeField] private Image _iceImage;
        [SerializeField] private Image[] _bananaImages;
        [SerializeField] private Image[] _strawberryImages;

        [SerializeField] private Button[] _toppingButtons;

        private void Awake()
        {
            HideAll();
        }
        public void ChangeToppingsActivate(bool isActive)
        {
            foreach (var button in _toppingButtons)
            {
                button.interactable = isActive;
            }
        }
        public void ShowAll()
        {
            foreach (object topping in Enum.GetValues(typeof(ToppingType)))
            {
                Show((int)topping);
            }
        }

        public void HideAll()
        {
            foreach (object topping in Enum.GetValues(typeof(ToppingType)))
            {
                Hide((int)topping);
            }
        }
        public void AnimateButton(ToppingType toppingType)
        {
            var button = _toppingButtons[(int)toppingType];
            DOTween.Sequence()
                .Append(button.gameObject.transform.DOScaleY(0.4f, 0.04f).SetEase(Ease.OutBounce))
                .Join(button.gameObject.transform.DOScaleX(0.55f, 0.06f).SetEase(Ease.OutBounce))
                .Append(button.gameObject.transform.DOScale(0.5f, 0.1f).SetEase(Ease.OutBack))
                .SetLink(button.gameObject);
        }

        public IObservable<Unit> OnClickToppingAsObservable(int index) => _toppingButtons[index].OnClickAsObservable().ThrottleFirst(TimeSpan.FromMilliseconds(10));

        public void Set(bool[] toppings)
        {
            for (int i = 0; i < toppings.Length; i++)
            {
                if (toppings[i])
                {
                    Show(i);
                }
                else
                {
                    Hide(i);
                }
            }
        }
        public void Hide(int index)
        {
            switch (index)
            {
                case 0:
                    ChangeActive(false, _kiwiImages);
                    break;
                case 1:
                    _iceImage.gameObject.SetActive(false);
                    break;
                case 2:
                    ChangeActive(false, _bananaImages);
                    break;
                case 3:
                    ChangeActive(false, _strawberryImages);
                    break;
            }
        }

        public void Show(int index)
        {
            switch (index)
            {
                case 0:
                    ChangeActive(true, _kiwiImages);
                    break;
                case 1:
                    _iceImage.gameObject.SetActive(true);
                    break;
                case 2:
                    ChangeActive(true, _bananaImages);
                    break;
                case 3:
                    ChangeActive(true, _strawberryImages);
                    break;
            }
        }

        private static void ChangeActive(bool isActive, IEnumerable<Image> images)
        {
            foreach (var image in images)
            {
                image.gameObject.SetActive(isActive);
            }
        }
    }
}
