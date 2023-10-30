using System;
using UniRx;
namespace MochaLib.Settings
{
    public class StateUseCase
    {
        private readonly ReactiveProperty<GameState> _currentGameState = new(GameState.None);

        public bool IsState(GameState targetState) => _currentGameState.Value == targetState;

        public void SetState(GameState inGameStateType)
        {
            _currentGameState.Value = inGameStateType;
        }

        public IObservable<GameState> OnChangeState() => _currentGameState;
    }
}
