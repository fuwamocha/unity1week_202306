using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
namespace Game.Scripts.Title
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TitleView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] public Image _blackOutImage1;
        [SerializeField] public Image _blackOutImage2;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Button _startButton;
        [SerializeField] private CanvasGroup _titleCanvas;
        [SerializeField] private Image _characterAfterImage;

        private void Awake()
        {
            _titleCanvas.alpha = 1;
            _titleCanvas.blocksRaycasts = true;
            _titleCanvas.interactable = true;
            _characterAfterImage.CrossFadeAlpha(0f, 0f, false);
        }

        private void Reset()
        {
            _titleCanvas = GetComponent<CanvasGroup>();
        }

        public void ChangeImage()
        {
            _characterAfterImage.CrossFadeAlpha(1f, 0.8f, false);
            _backgroundImage.DOColor(Color.gray, 0.8f);
        }
        public void FadeOut()
        {
            _blackOutImage1.DOFade(1f, 0.5f);
        }

        public void Canvas()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
        }
        public void FadeIn1()
        {
            _blackOutImage1.CrossFadeAlpha(0f, 0.5f, false);
        }
        public void FadeIn2()
        {
            _blackOutImage2.CrossFadeAlpha(0f, 0.5f, false);
        }
        public IObservable<Unit> OnClickStartAsObservable() => _startButton.OnClickAsObservable().ThrottleFirst(TimeSpan.FromMilliseconds(3000));
    }
}
