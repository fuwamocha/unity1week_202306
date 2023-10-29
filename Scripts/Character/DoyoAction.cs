using UnityEngine;
using UnityEngine.UI;
using CharacterAnimation = MochaLib.Settings.CommonConstants.Character.Animation;

namespace Game.Scripts.Character
{
    public class DoyoAction : MonoBehaviour
    {
        [SerializeField] private Image[] _doyoImages;
        private Doyo _doyo;

        private void Awake()
        {
            _doyo = new Doyo(_doyoImages);
        }

        public void Animate()
        {
            _doyo.Animate();
        }
    }
}
