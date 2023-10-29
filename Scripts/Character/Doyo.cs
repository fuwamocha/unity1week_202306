using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
namespace Game.Scripts.Character
{
    public class Doyo
    {
        private const float Duration = 0.8f;
        private readonly Image[] _images;
        private readonly Vector3[] _positions;
        private readonly Sequence[] _sequences;
        private readonly Color _transparent = new(1, 1, 1, 0);
        public Doyo(Image[] images)
        {
            _positions = new Vector3[images.Length];
            _sequences = new Sequence[images.Length];
            _images = images;
            for (int i = 0; i < images.Length; i++)
            {
                var image = images[i];
                image.enabled = false;
                _positions[i] = image.transform.position;
            }
        }

        public void Animate()
        {
            for (int i = 0; i < _images.Length; i++)
            {
                Init(i);
                int index = i;

                if (_sequences[index] != null && _sequences[index].IsActive() && _sequences[index].IsPlaying()) _sequences[index].Kill();
                _sequences[index] = DOTween.Sequence();
                _sequences[index].Append(_images[index].DOFade(1f, Duration))
                    .Join(_images[index].transform.DOMove(_positions[index] + Vector3.down, Duration))
                    .SetLink(_images[index].gameObject);
                _sequences[index].Play();
                _sequences[index].OnComplete(() =>
                {
                    _sequences[index].Kill();
                    _images[index].enabled = false;
                });
            }
        }
        private void Init(int index)
        {
            _images[index].enabled = true;
            _images[index].color = _transparent;
            _images[index].transform.position = _positions[index];
        }
    }
}
