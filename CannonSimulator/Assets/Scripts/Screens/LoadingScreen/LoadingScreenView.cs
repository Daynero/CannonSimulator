using System;
using System.Collections;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.LoadingScreen
{
    public class LoadingScreenView:ScreenView, ILoadingScreenView
    {
        [SerializeField] private Text text;
        private const float TargetSize = 200;
        private const float AnimationDuration = 2;

        public event Action OnAnimationEnd;  

        private void Start()
        {
            StartCoroutine(IncreaseFontSizeOverTime(TargetSize, AnimationDuration));
        }
        
        IEnumerator IncreaseFontSizeOverTime(float targetSize, float duration)
        {
            float initialSize = text.fontSize;
            float currentTime = 0.0f;

            while (currentTime < duration)
            {
                float t = currentTime / duration;
                text.fontSize = (int) Mathf.Lerp(initialSize, targetSize, t);
                currentTime += Time.deltaTime;
                yield return null;
            }

            OnAnimationEnd?.Invoke();

            text.fontSize = (int) targetSize;
        }

    }
}