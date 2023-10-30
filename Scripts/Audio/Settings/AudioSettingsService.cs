using MochaLib.Settings;
using UniRx;
namespace MochaLib.Audio.Settings
{
    public class AudioSettingsService
    {
        private readonly ReactiveProperty<AudioVolume> _bgmVolume = new(new AudioVolume(CommonConstants.Audio.Bgm.InitVolume));
        private readonly ReactiveProperty<AudioVolume> _seVolume = new(new AudioVolume(CommonConstants.Audio.Se.InitVolume));
        public IReadOnlyReactiveProperty<AudioVolume> BgmVolume => _bgmVolume;
        public IReadOnlyReactiveProperty<AudioVolume> SeVolume => _seVolume;

        public void SetBgmVolume(AudioVolume volume)
        {
            _bgmVolume.Value = volume;
        }

        public void SetSeVolume(AudioVolume volume)
        {
            _seVolume.Value = volume;
        }
    }
}
