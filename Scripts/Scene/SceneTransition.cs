using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
namespace MochaLib.Scene
{
    public class SceneTransition
    {
        private const float MinAlpha = 0.0f;
        private const float MaxAlpha = 1.0f;

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
            canvasGroup.alpha = MinAlpha;
            canvasGroup.DOFade(MaxAlpha, fadeTime).OnComplete(() =>
            {
                response.OnNext(Unit.Default);
            });
            return response.First();
        }

        /// <summary>
        ///     画面をフェードアウトさせる
        /// </summary>
        /// <param name="fadeTime">フェードアウトにかかる時間 </param>
        /// <param name="canvasGroup">フェードアウトさせるCanvas</param>
        /// <returns></returns>
        internal static IObservable<Unit> FadeOut(CanvasGroup canvasGroup, float fadeTime)
        {
            var response = new Subject<Unit>();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = MaxAlpha;
            canvasGroup.DOFade(MinAlpha, fadeTime)
                .OnComplete(() =>
                {
                    response.OnNext(Unit.Default);
                });
            return response.First();
        }

        /// <summary>
        ///     画面を完全にフェードアウトしてからフェードインを行う
        /// </summary>
        /// <param name="fadeOutCanvas">フェードアウトさせるCanvas</param>
        /// <param name="fadeInCanvas">フェードインさせるCanvas</param>
        /// <param name="fadeTime">フェードにかかる時間</param>
        /// <returns></returns>
        internal IObservable<Unit> SequentialFade(CanvasGroup fadeOutCanvas, CanvasGroup fadeInCanvas, float fadeTime) => SequentialFade(fadeOutCanvas, fadeInCanvas, fadeTime, fadeTime);

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
        /// <param name="fadeOutCanvas">フェードアウトさせるCanvas</param>
        /// <param name="fadeInCanvas">フェードインさせるCanvas</param>
        /// <param name="fadeTime">フェードにかかる時間</param>
        /// <returns></returns>
        internal IObservable<Unit> CrossFade(CanvasGroup fadeOutCanvas, CanvasGroup fadeInCanvas, float fadeTime)
        {
            var fadeOutStream = FadeOut(fadeOutCanvas, fadeTime);
            var fadeInStream = FadeIn(fadeInCanvas, fadeTime);

            return fadeOutStream.Merge(fadeInStream).First();
        }
    }
}
