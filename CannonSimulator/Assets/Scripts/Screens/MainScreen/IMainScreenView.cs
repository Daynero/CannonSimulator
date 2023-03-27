using System;

namespace Screens.MainScreen
{
    public interface IMainScreenView
    {
        public event Action<float> OnBulletSpeedChanged;
    }
}