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
            
            _mainScreenView.OnBulletSpeedChanged += HandleBulletSpeedChanged;
        }

        private void HandleBulletSpeedChanged(float speed)
        {
            OnBulletSpeedChanged?.Invoke(speed);
        }
    }
}