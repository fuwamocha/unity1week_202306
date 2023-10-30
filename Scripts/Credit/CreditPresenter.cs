using System;
using MochaLib.Audio;
using MochaLib.Settings;
using UniRx;
using VContainer.Unity;
namespace MochaLib.Credit
{
    public class CreditPresenter : IInitializable, IDisposable
    {
        private readonly AudioPlayer _audioPlayer;
        private readonly CreditView _creditView;
        private readonly CompositeDisposable _disposable = new();

        public CreditPresenter(
            CreditView creditView,
            AudioPlayer audioPlayer)
        {
            _creditView = creditView;
            _audioPlayer = audioPlayer;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        public void Initialize()
        {
            _creditView.OnClickOpenAsObservable()
                .Subscribe(_ =>
                {
                    _creditView.Open();
                    _audioPlayer.PlaySe(CommonConstants.Audio.Se.Click);
                })
                .AddTo(_disposable);
            _creditView.OnClickCloseAsObservable()
                .Subscribe(_ =>
                {
                    _creditView.Close();
                    _audioPlayer.PlaySe(CommonConstants.Audio.Se.Click);
                })
                .AddTo(_disposable);
        }
    }
}
