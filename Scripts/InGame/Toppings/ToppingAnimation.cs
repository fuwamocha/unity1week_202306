using DG.Tweening;
using UnityEngine;
namespace Game.Scripts.InGame.Toppings
{
    public class ToppingAnimation : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvas;
        [SerializeField]
        private GameObject[] _keyBinds;
        [SerializeField] private GameObject[] _dishes;
        [SerializeField] private GameObject[] _toppings;
        private readonly Vector3 _defaultScale = Vector3.one * 0.5f;
        private readonly Vector3 _initDishesPosition = new(0, -5f, 0);
        private readonly Vector3 _initToppingsPosition = new(0, 11f, 0);
        private readonly Vector3 _smashedScale = new(0.7f, 0.4f, 0.25f);

        private Vector3[] _dishesPosition;
        private Vector3[] _toppingsPosition;

        private void Awake()
        {
            _toppingsPosition = new Vector3[_toppings.Length];
            _dishesPosition = new Vector3[_dishes.Length];
            for (int i = 0; i < _toppings.Length; i++)
            {
                _toppingsPosition[i] = _toppings[i].transform.localPosition;
                _dishesPosition[i] = _dishes[i].transform.localPosition;
                _dishes[i].transform.position += _initDishesPosition;
                _toppings[i].transform.position += _initToppingsPosition;
            }
        }

        public void InGameAnim()
        {
            _canvas.interactable = true;
            for (int i = 0; i < _toppings.Length; i++)
            {
                _keyBinds[i].SetActive(true);
                int index = i;
                _dishes[index].transform.DOLocalMove(_dishesPosition[index], 0.5f).SetEase(Ease.InOutQuart);
            }

            var sequence = DOTween.Sequence().PrependInterval(0.5f);
            sequence.Append(_toppings[2].transform.DOLocalMove(_toppingsPosition[2], 0.7f).SetEase(Ease.InOutQuart))
                .Join(_toppings[3].transform.DOLocalMove(_toppingsPosition[3], 0.7f).SetEase(Ease.InOutQuart))
                .Insert(0.5f + 0.6f, _toppings[2].transform.DOScale(_smashedScale, 0.1f).SetEase(Ease.OutBack))
                .Join(_toppings[3].transform.DOScale(_smashedScale, 0.1f).SetEase(Ease.OutBack))
                .Append(_toppings[2].transform.DOScale(_defaultScale, 0.1f).SetEase(Ease.OutBack))
                .Join(_toppings[3].transform.DOScale(_defaultScale, 0.1f).SetEase(Ease.OutBack))
                .Insert(0.5f + 0.7f, _toppings[0].transform.DOLocalMove(_toppingsPosition[0], 0.7f).SetEase(Ease.InOutQuart))
                .Join(_toppings[1].transform.DOLocalMove(_toppingsPosition[1], 0.7f).SetEase(Ease.InOutQuart))
                .Insert(0.5f + 0.7f + 0.6f, _toppings[0].transform.DOScale(_smashedScale, 0.1f).SetEase(Ease.OutBack))
                .Join(_toppings[1].transform.DOScale(_smashedScale, 0.1f).SetEase(Ease.OutBack))
                .Append(_toppings[0].transform.DOScale(_defaultScale, 0.1f).SetEase(Ease.OutBack))
                .Join(_toppings[1].transform.DOScale(_defaultScale, 0.1f).SetEase(Ease.OutBack))
                .Insert(0.5f + 0.7f, _toppings[4].transform.DOLocalMove(_toppingsPosition[4], 0.7f).SetEase(Ease.InOutQuart))
                .Insert(0.5f + 0.7f + 0.6f, _toppings[4].transform.DOScale(_smashedScale, 0.1f).SetEase(Ease.OutBack))
                .Append(_toppings[4].transform.DOScale(_defaultScale, 0.1f).SetEase(Ease.OutBack));
        }
        public void ResultAnim()
        {
            for (int i = 0; i < _toppings.Length; i++)
            {
                _keyBinds[i].SetActive(false);
                int index = i;
                if (i is 2 or 3)
                {
                    _dishes[index].transform.DOMove(_dishes[index].transform.position + Vector3.down * 4f, 0.8f).SetEase(Ease.InOutQuart);
                    _toppings[index].transform.DOMove(_toppings[index].transform.position + Vector3.down * 4f, 0.8f).SetEase(Ease.InOutQuart);
                }
                else
                {
                    _dishes[index].transform.DOMove(_dishes[index].transform.position + Vector3.down * 3f, 0.8f).SetEase(Ease.InOutQuart);
                    _toppings[index].transform.DOMove(_toppings[index].transform.position + Vector3.down * 3f, 0.8f).SetEase(Ease.InOutQuart);
                }
            }
        }

        public void ResetPosition()
        {
            foreach (var topping in _toppings)
            {
                topping.transform.position += _initToppingsPosition;
            }
        }
    }
}
