using System;
using Cysharp.Threading.Tasks;
using MochaLib.InGame;
using MochaLib.InGame.CountDown;
using MochaLib.Settings;
using UniRx;
using VContainer.Unity;
namespace Game.Scripts.Title
{
    public class StartPresenter : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposable = new();
        private readonly InGameStateUseCase _inGameStateUseCase;
        private readonly RemainingTimer _remainingTimer;
        private readonly StartView _startView;
        private readonly StateUseCase _stateUseCase;

        public StartPresenter(
            InGameStateUseCase inGameStateUseCase,
            StateUseCase stateUseCase,
            StartView startView)
        {
            _inGameStateUseCase = inGameStateUseCase;
            _stateUseCase = stateUseCase;
            _startView = startView;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        public void Initialize()
        {
            _stateUseCase.OnChangeState().Where(_ => _stateUseCase.IsState(GameState.Start))
                .Subscribe(_ =>
                {
                    Change().Forget();
                })
                .AddTo(_disposable);

            _inGameStateUseCase.OnChangeState().Where(_ => _inGameStateUseCase.IsState(InGameStateType.Result))
                .Subscribe(_ =>
                {
                    _startView.SetInActive();
                })
                .AddTo(_disposable);
        }
        private async UniTask Change()
        {
            await _startView.ChangeScene();
            _stateUseCase.SetState(GameState.InGame);
        }
    }
}
