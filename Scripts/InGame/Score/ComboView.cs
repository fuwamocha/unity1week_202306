using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
namespace Game.Scripts.InGame.Score
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ComboView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _comboText;
        private readonly Vector3 _bigScale = Vector3.one * 1.5f;
        private Sequence _sequence;
        private void Awake()
        {
            ScoreManager.IsReset.Subscribe(isReset =>
            {
                if (isReset)
                {
                    _comboText.text = "";
                }
            });
            ScoreManager.Combo.Subscribe(combo =>
            {
                ComboFadeAnimation();

                _comboText.text = combo switch
                {
                    0              => "",
                    >= 1 and < 10  => $"{combo.ToString()}\nCombo!",
                    >= 10 and < 20 => $"{combo.ToString()}\nCombo!!",
                    _              => $"{combo.ToString()}\nCombo!!!"
                };
            });
        }

        private void Reset()
        {
            _comboText = GetComponent<TextMeshProUGUI>();
        }

        public void DeleteCombo()
        {
            _comboText.text = "";
        }

        private void ComboFadeAnimation()
        {
            if (_sequence != null && _sequence.IsActive() && _sequence.IsPlaying()) _sequence.Kill();
            _comboText.color = Color.white;
            _comboText.transform.localScale = _bigScale;

            _sequence = DOTween.Sequence();
            _sequence.Append(_comboText.DOScale(1f, 0.2f))
                .Join(_comboText.DOFade(0f, 4.8f))
                .SetLink(gameObject);
        }
    }
}
