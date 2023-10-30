using System;
using UnityEngine;
namespace MochaLib.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SePlayer : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
        }

        public void PlayOneShot(AudioClip audioClip)
        {
            if (audioClip == null) throw new NullReferenceException($"AudioClip {audioClip} is null.");
            _audioSource.PlayOneShot(audioClip);
        }

        public void Stop()
        {
            _audioSource.Stop();
        }

        public void SetVolume(float volume)
        {
            if (volume is < 0 or > 1)
            {
                throw new ArgumentOutOfRangeException($"Volume {volume} is out of range.");
            }
            _audioSource.volume = volume;
        }
    }
}
