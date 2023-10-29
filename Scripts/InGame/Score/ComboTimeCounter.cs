using UnityEngine;
namespace Game.Scripts.InGame.Score
{
    public class ComboTimeCounter : MonoBehaviour
    {
        public static float LastSupplyTime;

        private void Update()
        {
            LastSupplyTime += Time.deltaTime;
        }
        public static void ResetTime()
        {
            LastSupplyTime = 0;
        }
    }
}
