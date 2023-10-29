using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
namespace Game.Scripts.Character
{
    public class CharacterMove : MonoBehaviour
    {
        [SerializeField] private Transform _workPositionTransform;
        private readonly Vector3 _workScale = Vector3.one * 9.92f;
        private Vector3 _initPosition;
        private Transform _transform;
        private void Awake()
        {
            var trans = transform;
            _initPosition = trans.position;
            _transform = trans;
        }
        public async void MoveWorkPosition()
        {
            if (_transform.parent == _workPositionTransform) return;

            await UniTask.Delay(100);
            _transform.parent = _workPositionTransform;
            await UniTask.Delay(10);
            _transform.position = _initPosition;
            _transform.localScale = _workScale;
        }
        public void MoveSidePosition()
        {
            gameObject.transform.DOMoveX(15f, 1f).SetLink(gameObject);
        }
    }
}
