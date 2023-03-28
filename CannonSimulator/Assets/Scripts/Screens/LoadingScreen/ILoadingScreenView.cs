using System;

namespace Screens.LoadingScreen
{
    public interface ILoadingScreenView
    {
        public event Action OnAnimationEnd;  
    }
}