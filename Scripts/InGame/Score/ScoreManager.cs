using UniRx;
using UnityEngine;
using unityroom.Api;
namespace Game.Scripts.InGame.Score
{
    public static class ScoreManager
    {
        private const int WaffleScore = 1000;
        private const int KiwiScore = 330;
        private const int VanillaScore = 350;
        private const int BananaScore = 310;
        private const int StrawberryScore = 400;
        private const float StartValue = 500; // 開始金額
        private const float EndValue = -100; // 終了金額
        public static int SupplyWaffleCount { get; private set; }
        public static IReactiveProperty<bool> IsReset { get; } = new ReactiveProperty<bool>();
        public static IReactiveProperty<int> Score { get; } = new ReactiveProperty<int>();
        public static IReactiveProperty<int> Combo { get; } = new ReactiveProperty<int>();

        public static void AddScore(bool[] toppings, float time)
        {
            int addScore = 0;
            for (int i = 0; i < toppings.Length; i++)
            {
                if (i == 0 && toppings[i]) addScore += KiwiScore;
                if (i == 1 && toppings[i]) addScore += VanillaScore;
                if (i == 2 && toppings[i]) addScore += BananaScore;
                if (i == 3 && toppings[i]) addScore += StrawberryScore;
            }
            addScore += WaffleScore;

            addScore += (int)Mathf.Lerp(EndValue, StartValue, time / 13f);
#if UNITY_EDITOR
            Debug.Log(addScore + "点獲得" + addScore * 0.1 * Combo.Value + "点+" + Mathf.Lerp(EndValue, StartValue, time / 13f) + "点ボーナス");
#endif
            Score.Value += addScore + (int)(addScore * 0.1 * Combo.Value);
        }

        public static void SubScore(bool[] toppings)
        {
            int subScore = 0;
            for (int i = 0; i < toppings.Length; i++)
            {
                if (i == 0 && toppings[i]) subScore += KiwiScore;
                if (i == 1 && toppings[i]) subScore += VanillaScore;
                if (i == 2 && toppings[i]) subScore += BananaScore;
                if (i == 3 && toppings[i]) subScore += StrawberryScore;
            }
            subScore += WaffleScore;
            subScore /= 4;
            if (Score.Value - subScore < 0)
            {
                Score.Value = 0;
                return;
            }
            Score.Value -= subScore;
        }

        public static void AddSupplyWaffleCount()
        {
            SupplyWaffleCount++;
        }
        public static void AddCombo()
        {
            Combo.Value++;
#if UNITY_EDITOR
            Debug.Log("コンボ数: " + Combo.Value);
#endif
        }
        public static void ResetScore()
        {
#if UNITY_EDITOR
            Debug.Log("スコアリセット");
#endif
            SupplyWaffleCount = 0;
            Score.Value = 0;
            Combo.Value = 0;
            IsReset.Value = true;
        }
        public static void ResetIsReset()
        {
            IsReset.Value = false;
        }
        public static void ResetCombo()
        {
            if (Combo.Value == 0) return;
#if UNITY_EDITOR
            Debug.Log("コンボリセット");
#endif
            Combo.Value = 0;
        }
        public static void SendScore(int score)
        {
#if UNITY_EDITOR
            Debug.Log("score: " + score + "を送信します");
#endif
            UnityroomApiClient.Instance.SendScore(1, score, ScoreboardWriteMode.HighScoreDesc);
        }
    }
}
