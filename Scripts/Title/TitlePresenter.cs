using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MochaLib.Audio;
using MochaLib.InGame;
using MochaLib.Scene;
using MochaLib.Settings;
using UniRx;
using VContainer.Unity;
namespace Game.Scripts.Title
{
    public class TitlePresenter : IInitializable, IDisposable
    {
        private readonly AudioPlayer _audioPlayer;
        private readonly CompositeDisposable _disposable = new();
        private readonly InGameStateUseCase _inGameStateUseCase;
        private readonly SceneTransitionView _sceneTransitionView;
        private readonly StateUseCase _stateUseCase;
        private readonly TitleView _titleView;
        private bool _isFirst = true;
        public TitlePresenter(
            InGameStateUseCase inGameStateUseCase,
            StateUseCase stateUseCase,
            TitleView titleView,
            SceneTransitionView sceneTransitionView,
            AudioPlayer audioPlayer)
        {
            _inGameStateUseCase = inGameStateUseCase;
            _stateUseCase = stateUseCase;
            _titleView = titleView;
            _sceneTransitionView = sceneTransitionView;
            _audioPlayer = audioPlayer;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        public void Initialize()
        {
            _stateUseCase.SetState(GameState.Title);

            _titleView.OnClickStartAsObservable()
                .Where(_ => _stateUseCase.IsState(GameState.Title))
                .Subscribe(_ =>
                {
                    if (_isFirst)
                    {
                        _titleView.ChangeImage();
                    }
                    ChangeScene().Forget();
                })
                .AddTo(_disposable);

            _stateUseCase.OnChangeState().Where(x => x == GameState.Title)
                .Subscribe(_ =>
                {
                    if (_isFirst) return;
                    _titleView.FadeIn1();
                    _audioPlayer.PlayBgm(CommonConstants.Audio.Bgm.Title).Forget();
                });
        }

        private async UniTask<Unit> ChangeScene()
        {
            _audioPlayer.PlaySe(CommonConstants.Audio.Se.Click);

            await _audioPlayer.StopBgm(1.2f);

            await _titleView._blackOutImage1.DOFade(1f, 0.5f)
                .OnComplete(() => _sceneTransitionView.FadeOut(GameState.Title, 0f));
            await UniTask.Delay(500);

            switch (_isFirst)
            {
                case true:
                    _titleView.Canvas();
                    await UniTask.Delay(500);
                    await _titleView._blackOutImage2.DOFade(0f, 0.5f);
                    await UniTask.Delay(500);
                    break;
                case false:
                    await _sceneTransitionView.FadeIn(GameState.InGame, 0.5f);
                    break;
            }

            _stateUseCase.SetState(_isFirst ? GameState.Start : GameState.InGame);
            _isFirst = false;
            return Unit.Default;
        }
    }
}
