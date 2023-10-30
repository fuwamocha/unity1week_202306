using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
namespace MochaLib.InGame.CountDown
{
    public class GameTimerView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _timeText;
        private bool _isInit;
        public void Set(TimeSpan timeSpan)
        {
            _timeText.SetText("{0:0}", (float)timeSpan.TotalSeconds);

            var sequence = DOTween.Sequence();
            sequence.Append(_timeText.rectTransform.DOScale(1.2f, 0.2f));
            sequence.Append(_timeText.rectTransform.DOScale(1f, 0.2f));
            sequence.SetLink(gameObject);
            sequence.Play();

            if (_isInit) return;

            _isInit = true;
        }

        public void ResetText()
        {
            _timeText.text = "";
        }
    }
}
