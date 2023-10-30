using System;
using MochaLib.Settings;
using UniRx;
using VContainer.Unity;
namespace MochaLib.Audio.Settings
{
    public class AudioSettingsPresenter : IInitializable, IDisposable
    {
        private readonly AudioPlayer _audioPlayer;
        private readonly AudioSettingsService _audioSettingsService;
        private readonly AudioSettingsView _audioSettingsView;
        private readonly CompositeDisposable _disposable = new();

        public AudioSettingsPresenter(
            AudioSettingsView audioSettingsView,
            AudioSettingsService audioSettingsService,
            AudioPlayer audioPlayer)
        {
            _audioSettingsView = audioSettingsView;
            _audioSettingsService = audioSettingsService;
            _audioPlayer = audioPlayer;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        public void Initialize()
        {
            _audioSettingsService.BgmVolume
                .Subscribe(volume => _audioSettingsView.SetBgmVolume(volume.Value))
                .AddTo(_disposable);

            _audioSettingsService.SeVolume
                .Subscribe(volume => _audioSettingsView.SetSeVolume(volume.Value))
                .AddTo(_disposable);

            _audioSettingsView.OnChangeBgmVolumeAsObservable()
                .Subscribe(volume => _audioSettingsService.SetBgmVolume(new AudioVolume(volume)))
                .AddTo(_disposable);

            _audioSettingsView.OnChangeSeVolumeAsObservable()
                .Subscribe(volume => _audioSettingsService.SetSeVolume(new AudioVolume(volume)))
                .AddTo(_disposable);

            _audioSettingsView.OnPointerUpSeVolumeAsObservable()
                .Subscribe(_ => _audioPlayer.PlaySe(CommonConstants.Audio.Se.Click))
                .AddTo(_disposable);
        }
    }
}
