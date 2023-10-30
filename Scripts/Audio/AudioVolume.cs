using UnityEngine;
namespace MochaLib.Audio
{
    public readonly struct AudioVolume
    {
        public static AudioVolume Zero => new(0);

        public float Value { get; }

        public AudioVolume(float volume)
        {
            Value = Mathf.Clamp01(volume);
        }
    }
}
