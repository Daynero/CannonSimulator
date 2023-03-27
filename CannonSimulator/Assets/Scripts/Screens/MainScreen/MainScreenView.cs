using System;
using UnityEngine;
using UnityEngine.UI;
using XmannaSDK.Core;

namespace Screens.MainScreen
{
    public class MainScreenView : ScreenView, IMainScreenView
    {
        [SerializeField] private Slider bulletSpeedSlider;
        
        public event Action<float> OnBulletSpeedChanged;

        private void Awake()
        {
            bulletSpeedSlider.onValueChanged.AddListener(value => OnBulletSpeedChanged?.Invoke(value));
        }
    }
}