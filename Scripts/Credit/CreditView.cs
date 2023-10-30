using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
namespace MochaLib.Credit
{
    public class CreditView : MonoBehaviour
    {
        [SerializeField] private Button _openButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private GameObject _contents;
        private void Awake()
        {
            gameObject.SetActive(false);
            _contents.transform.localScale = Vector3.zero;
        }
        public void Open()
        {
            gameObject.SetActive(true);
            _contents.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
        public void Close()
        {
            _contents.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
        }
        public IObservable<Unit> OnClickOpenAsObservable() => _openButton.OnClickAsObservable();
        public IObservable<Unit> OnClickCloseAsObservable() => _closeButton.OnClickAsObservable();
    }
}
