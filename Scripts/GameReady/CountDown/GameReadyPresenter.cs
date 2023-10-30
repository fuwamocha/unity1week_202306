using System;
using Cysharp.Threading.Tasks;
using MochaLib.Audio;
using UniRx;
using Object = UnityEngine.Object;
namespace MochaLib.GameReady.CountDown
{
    public class GameReadyPresenter : IDisposable
    {
        private readonly AudioPlayer _audioPlayer;
        private readonly CompositeDisposable _disposable = new();
        private readonly Func<GameReadyView> _viewFactory;
        private GameReadyView _gameReadyView;

        public GameReadyPresenter(
            Func<GameReadyView> viewFactory,
            AudioPlayer audioPlayer)
        {
            _viewFactory = viewFactory;
            _audioPlayer = audioPlayer;
        }
        public void Dispose()
        {
            _disposable.Dispose();
        }

        public async UniTask Ready()
        {
            _gameReadyView = _viewFactory.Invoke();
            _gameReadyView.Show();
            await _gameReadyView.IsReady
                .Where(x => x)
                .First();
        }

        public void Hide()
        {
            Object.Destroy(_gameReadyView.gameObject);
        }
    }
}
