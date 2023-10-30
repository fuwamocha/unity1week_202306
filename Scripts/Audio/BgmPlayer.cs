using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
namespace MochaLib.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class BgmPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;
        private float _volume;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }

        public void Play(AudioClip audioClip, bool isLoop)
        {
            if (audioClip == null) throw new NullReferenceException($"AudioClip {audioClip} is null.");
            SetVolume(_volume);
            _audioSource.clip = audioClip;
            _audioSource.loop = isLoop;
            _audioSource.Play();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }
        public async UniTask StopAsync(float fadeoutTime = 0)
        {
            if (fadeoutTime > 0f)
            {
                await _audioSource
                    .DOFade(0, fadeoutTime)
                    .OnComplete(() => _audioSource.Stop())
                    .AsyncWaitForCompletion();
            }

            _audioSource.Stop();
        }
        public void SetVolume(float volume)
        {
            if (volume is < 0 or > 1)
            {
                throw new ArgumentOutOfRangeException($"Volume {volume} is out of range.");
            }
            _volume = volume;
            _audioSource.volume = volume;
        }
    }
}
