using System;
using UniRx;
namespace MochaLib.InGame
{
    public class InGameStateUseCase
    {
        private readonly ReactiveProperty<InGameStateType> _currentGameState = new(InGameStateType.None);

        public bool IsState(InGameStateType targetState) => _currentGameState.Value == targetState;

        public void SetState(InGameStateType inGameStateType)
        {
            _currentGameState.Value = inGameStateType;
        }

        public IObservable<InGameStateType> OnChangeState() => _currentGameState;
    }
}
