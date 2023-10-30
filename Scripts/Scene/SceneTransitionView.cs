using System;
using DG.Tweening;
using MochaLib.Settings;
using UniRx;
using UnityEngine;
namespace MochaLib.Scene
{
    public class SceneTransitionView : MonoBehaviour
    {
        private const float MinAlpha = 0.0f;
        private const float MaxAlpha = 1.0f;
        [SerializeField]
        private CanvasGroup[] _canvasGroups;

        private void Awake()
        {
            foreach (var canvasGroup in _canvasGroups)
            {
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }

        /// <summary>
        ///     画面をフェードインさせる
        /// </summary>
        /// <param name="fadeTime">フェードインにかかる時間 </param>
        /// <param name="canvasGroup">フェードインさせるCanvas</param>
        /// <returns></returns>
        internal IObservable<Unit> FadeIn(CanvasGroup canvasGroup, float fadeTime)
        {
            var response = new Subject<Unit>();
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            canvasGroup.alpha = MinAlpha;
            canvasGroup.DOFade(MaxAlpha, fadeTime)
                .OnComplete(() =>
                {
                    response.OnNext(Unit.Default);
                });
            return response.First();
        }
        /// <summary>
        ///     画面をフェードアウトさせる
        /// </summary>
        /// <param name="fadeTime">フェードアウトにかかる時間 </param>
        /// <param name="fadeInState">フェードアウトさせるCanvas</param>
        /// <returns></returns>
        internal IObservable<Unit> FadeIn(GameState fadeInState, float fadeTime)
        {
            return FadeIn(_canvasGroups[(int)fadeInState], fadeTime);
        }
        /// <summary>
        ///     画面をフェードアウトさせる
        /// </summary>
        /// <param name="fadeTime">フェードアウトにかかる時間 </param>
        /// <param name="canvasGroup">フェードアウトさせるCanvas</param>
        /// <returns></returns>
        internal IObservable<Unit> FadeOut(CanvasGroup canvasGroup, float fadeTime)
        {
            var response = new Subject<Unit>();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            canvasGroup.alpha = MaxAlpha;
            canvasGroup.DOFade(MinAlpha, fadeTime)
                .OnComplete(() =>
                {
                    response.OnNext(Unit.Default);
                });
            return response.First();
        }

        /// <summary>
        ///     画面をフェードアウトさせる
        /// </summary>
        /// <param name="fadeTime">フェードアウトにかかる時間 </param>
        /// <param name="fadeOutState">フェードアウトさせるCanvas</param>
        /// <returns></returns>
        internal IObservable<Unit> FadeOut(GameState fadeOutState, float fadeTime)
        {
            return FadeOut(_canvasGroups[(int)fadeOutState], fadeTime);
        }

        /// <summary>
        ///     画面を非表示にする
        /// </summary>
        /// <param name="canvasGroup">非表示にするCanvas</param>
        internal void SetInActive(CanvasGroup canvasGroup)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = MinAlpha;
        }

        /// <summary>
        ///     画面を表示する
        /// </summary>
        /// <param name="canvasGroup">表示するCanvas</param>
        internal void SetActive(CanvasGroup canvasGroup)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = MaxAlpha;
        }

        /// <summary>
        ///     画面を表示する
        /// </summary>
        /// <param name="activeState">表示するCanvas</param>
        internal void SetActive(GameState activeState)
        {
            SetActive(_canvasGroups[(int)activeState]);
        }

        /// <summary>
        ///     画面を完全にフェードアウトしてからフェードインを行う
        /// </summary>
        /// <param name="fadeOutState">フェードアウトさせるCanvas</param>
        /// <param name="fadeInState">フェードインさせるCanvas</param>
        /// <param name="fadeTime">フェードにかかる時間</param>
        /// <returns></returns>
        internal IObservable<Unit> SequentialFade(GameState fadeOutState, GameState fadeInState, float fadeTime)
        {
            return SequentialFade(_canvasGroups[(int)fadeOutState], _canvasGroups[(int)fadeInState], fadeTime, fadeTime);
        }

        /// <summary>
        ///     画面を完全にフェードアウトしてからフェードインを行う
        /// </summary>
        /// <param name="fadeOutCanvas">フェードアウトさせるCanvas</param>
        /// <param name="fadeInCanvas">フェードインさせるCanvas</param>
        /// <param name="fadeOutTime">フェードアウトにかかる時間</param>
        /// <param name="fadeInTime">フェードインにかかる時間</param>
        /// <returns></returns>
        internal IObservable<Unit> SequentialFade(CanvasGroup fadeOutCanvas, CanvasGroup fadeInCanvas, float fadeOutTime, float fadeInTime)
        {
            return FadeOut(fadeOutCanvas, fadeOutTime)
                .SelectMany(_ => FadeIn(fadeInCanvas, fadeInTime))
                .First();
        }

        /// <summary>
        ///     画面のフェードイン・フェードアウトを同時に行う
        /// </summary>
        /// <param name="fadeOutState">フェードアウトさせるCanvas</param>
        /// <param name="fadeInState">フェードインさせるCanvas</param>
        /// <param name="fadeTime">フェードにかかる時間</param>
        /// <returns></returns>
        internal IObservable<Unit> CrossFade(GameState fadeOutState, GameState fadeInState, float fadeTime)
        {
            var fadeOutStream = FadeOut(_canvasGroups[(int)fadeOutState], fadeTime);
            var fadeInStream = FadeIn(_canvasGroups[(int)fadeInState], fadeTime);

            return fadeOutStream.Merge(fadeInStream).First();
        }

        /// <summary>
        ///     画面を一瞬で切り替える
        /// </summary>
        /// <param name="inActiveState">非表示にするCanvas</param>
        /// <param name="activeState">表示するCanvas</param>
        /// <returns></returns>
        internal void InstantChange(GameState inActiveState, GameState activeState)
        {
            SetInActive(_canvasGroups[(int)inActiveState]);
            SetActive(_canvasGroups[(int)activeState]);
        }
    }
}
