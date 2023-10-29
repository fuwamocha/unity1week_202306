using System;
using MochaLib.InGame;
using UniRx;
using VContainer.Unity;
namespace Game.Scripts.InGame.Score
{
    public class ScorePresenter : IInitializable, IDisposable
    {
        private readonly ComboView _comboView;
        private readonly CompositeDisposable _disposable = new();
        private readonly InGameStateUseCase _inGameStateUseCase;
        private readonly ScoreView _scoreView;

        public ScorePresenter(
            InGameStateUseCase inGameStateUseCase,
            ScoreView scoreView,
            ComboView comboView
        )
        {
            _inGameStateUseCase = inGameStateUseCase;
            _scoreView = scoreView;
            _comboView = comboView;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        public void Initialize()
        {
            ScoreManager.Score.Skip(1).Subscribe(score =>
            {
                if (!_inGameStateUseCase.IsState(InGameStateType.Result))
                {
                    _scoreView.SetScore(score);
                }
            });
        }

        public void Stop()
        {
            _scoreView.DeleteScoreText();
            _comboView.DeleteCombo();
        }
    }
}
