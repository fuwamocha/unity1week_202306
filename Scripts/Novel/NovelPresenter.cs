using System;
using Game.Scripts.Character;
using MochaLib.Settings;
using UniRx;
using VContainer.Unity;
namespace MochaLib.Novel
{
    public class NovelPresenter : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposable = new();
        private readonly INovelAction _novelAction;
        private readonly NovelManager _novelManager;
        private readonly StateUseCase _stateUseCase;

        public NovelPresenter(
            INovelAction novelAction,
            StateUseCase stateUseCase,
            NovelManager novelManager
        )
        {
            _stateUseCase = stateUseCase;
            _novelManager = novelManager;
            _novelAction = novelAction;
        }

        public void Dispose() => _disposable?.Dispose();

        public void Initialize()
        {
            _stateUseCase.OnChangeState()
                .Where(_ => _stateUseCase.IsState(GameState.Start))
                .Subscribe(_ => _novelManager.Init(_novelAction))
                .AddTo(_disposable);
        }
    }
}
