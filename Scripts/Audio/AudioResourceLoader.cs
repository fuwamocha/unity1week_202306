using Cysharp.Threading.Tasks;
using UnityEngine;
namespace MochaLib.Audio
{
    public class AudioResourceLoader
    {
        private readonly AudioResource _audioResource;

        public AudioResourceLoader(AudioResource audioResource)
        {
            _audioResource = audioResource;
        }

        public async UniTask<AudioClip> LoadAsync(string name)
        {
            var audioClip = _audioResource.Get(name);
            if (audioClip.loadState != AudioDataLoadState.Loaded)
            {
                audioClip.LoadAudioData();
            }

            await UniTask.WaitUntil(() => audioClip.loadState == AudioDataLoadState.Loaded);
            return audioClip;
        }
    }
}
