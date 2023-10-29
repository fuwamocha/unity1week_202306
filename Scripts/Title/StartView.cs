using Cysharp.Threading.Tasks;
using MochaLib.Novel;
using MochaLib.Scene;
using MochaLib.Settings;
using UniRx;
using UnityEngine;
namespace Game.Scripts.Title
{
    public class StartView : MonoBehaviour
    {
        [SerializeField] private GameObject _counter;
        [SerializeField] private SceneTransitionView _sceneTransitionView;

        public void SetInActive()
        {
            _counter.SetActive(false);
        }
        public async UniTask ChangeScene()
        {
            await UniTask.WaitUntil(() => NovelManager.IsEnd.Value);
            _sceneTransitionView.SetActive(GameState.InGame);
            await _sceneTransitionView.FadeOut(GameState.Start, 0.8f);
            await UniTask.Delay(1000);
        }
    }
}
