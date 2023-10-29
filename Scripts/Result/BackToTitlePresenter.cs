using System;
using Cysharp.Threading.Tasks;
using MochaLib.Audio;
using MochaLib.Scene;
using MochaLib.Settings;
using UniRx;
using VContainer.Unity;
namespace Game.Scripts.Result
{
    public class BackToTitlePresenter : IInitializable, IDisposable
    {
        private readonly AudioPlayer _audioPlayer;
        private readonly BackToTitleView _backToTitleView;
        private readonly CompositeDisposable _disposable = new();
        private readonly SceneTransitionView _sceneTransitionView;
        private readonly StateUseCase _stateUseCase;

        public BackToTitlePresenter(
            StateUseCase stateUseCase,
            BackToTitleView backToTitleView,
            SceneTransitionView sceneTransitionView,
            AudioPlayer audioPlayer)
        {
            _stateUseCase = stateUseCase;
            _backToTitleView = backToTitleView;
            _sceneTransitionView = sceneTransitionView;
            _audioPlayer = audioPlayer;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        public void Initialize()
        {
            _backToTitleView.OnClickStartAsObservable()
                .Subscribe(_ =>
                {
                    ChangeScene().Forget();
                })
                .AddTo(_disposable);
        }

        private async UniTask<Unit> ChangeScene()
        {
            _audioPlayer.PlaySe(CommonConstants.Audio.Se.Click);
            _audioPlayer.StopBgm(0.5f).Forget();
            await _sceneTransitionView.FadeOut(GameState.InGame, 0.5f);
            await UniTask.Delay(500);
            await _sceneTransitionView.FadeIn(GameState.Title, 0.5f);

            _stateUseCase.SetState(GameState.Title);
            return Unit.Default;
        }
    }
}
