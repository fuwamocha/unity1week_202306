using MochaLib.Cores;
using UnityEngine;
namespace MochaLib.Audio
{
    [CreateAssetMenu(fileName = "AudioResource", menuName = "MochaLib/Audio/AudioResource")]
    public class AudioResource : ScriptableObject
    {
        [SerializeField]
        private TableBase<string, AudioClip, KeyAndValue<string, AudioClip>> _audioClips;

        //public IEnumerable<AudioClip> AudioClips => _audioClips;


        public AudioClip Get(string name) => _audioClips.GetTable()[name];
    }
}
