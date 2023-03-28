using System;
using Core;

namespace Screens.MainScreen
{
    public class MainScreenPresenter
    {
        private readonly IMainScreenView _mainScreenView;
        private readonly IScreenNavigationSystem _screenNavigationSystem;
        
        public event Action<float> OnBulletSpeedChanged;
        
        public MainScreenPresenter(IMainScreenView mainScreenView, IScreenNavigationSystem screenNavigationSystem)
        {
            _mainScreenView = mainScreenView;
            _screenNavigationSystem = screenNavigationSystem;
            
            _mainScreenView.OnBulletSpeedChanged += HandleBulletPowerChanged;
        }

        private void HandleBulletPowerChanged(float power)
        {
            string powerText = Math.Round(power * 100).ToString();
            _mainScreenView.SetPowerText(powerText);
            OnBulletSpeedChanged?.Invoke(power);
        }
    }
}