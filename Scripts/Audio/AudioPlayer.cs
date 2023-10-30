using System;
using Cysharp.Threading.Tasks;
using MochaLib.Audio.Settings;
using UniRx;
using VContainer.Unity;
namespace MochaLib.Audio
{
    public class AudioPlayer : IInitializable, IDisposable
    {
        private readonly AudioSettingsService _audioSettingsService;
        private readonly BgmPlayer _bgm;
        private readonly CompositeDisposable _disposable = new();
        private readonly AudioResourceLoader _loader;
        private readonly SePlayer _se;

        public AudioPlayer(
            AudioResourceLoader loader,
            AudioSettingsService audioSettingsService,
            BgmPlayer bgm,
            SePlayer sePlayer)
        {
            _bgm = bgm;
            _se = sePlayer;
            _loader = loader;
            _audioSettingsService = audioSettingsService;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        void IInitializable.Initialize()
        {
            _audioSettingsService.BgmVolume
                .Subscribe(volume => _bgm.SetVolume(volume.Value))
                .AddTo(_disposable);

            _audioSettingsService.SeVolume
                .Subscribe(volume => _se.SetVolume(volume.Value))
                .AddTo(_disposable);
        }

        public async UniTask PlayBgm(string name)
        {
            var clip = await _loader.LoadAsync(name);
            _bgm.Play(clip, true);
        }

        public async UniTask StopBgm(float duration)
        {
            await _bgm.StopAsync(duration);
        }

        public async void PlaySe(string name)
        {
            var clip = await _loader.LoadAsync(name);
            _se.PlayOneShot(clip);
        }
    }
}
