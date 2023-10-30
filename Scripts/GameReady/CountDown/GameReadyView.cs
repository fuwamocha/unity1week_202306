using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
namespace MochaLib.GameReady.CountDown
{
    public class GameReadyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _countDownText;

        private readonly IReactiveProperty<int> _countDownProperty = new ReactiveProperty<int>();
        private float _countDown;
        private bool _isCount;
        public ReactiveProperty<bool> IsReady { get; } = new(false);
        private void Awake()
        {
            _countDown = 3f;
            _countDownProperty.Subscribe(count =>
            {
                if (count == 0)
                {
                    _countDownText.text = "";
                }
                else
                {
                    _countDownText.text = count.ToString();
                    _countDownText.transform.localScale = Vector3.one * 4f;
                    _countDownText.transform.DOScale(2f, 0.8f)
                        .SetLink(gameObject);
                }
            }).AddTo(gameObject);
        }
        private void Update()
        {
            if (!_isCount) return;
            if (_countDown > 0)
            {
                _countDown -= Time.deltaTime;
                _countDownProperty.Value = Mathf.CeilToInt(_countDown);
            }
            else
            {
                _countDownText.text = "";
                _isCount = false;
                IsReady.Value = true;
            }
        }
        public void Show()
        {
            _isCount = true;
        }
    }
}
