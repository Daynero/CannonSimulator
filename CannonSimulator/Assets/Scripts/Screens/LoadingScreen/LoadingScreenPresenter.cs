using Core;
using Enums;

namespace Screens.LoadingScreen
{
    public class LoadingScreenPresenter
    {
        private IScreenNavigationSystem _screenNavigationSystem;
        private readonly ILoadingScreenView _loadingScreenView;

        public LoadingScreenPresenter(ILoadingScreenView loadingScreenView, IScreenNavigationSystem screenNavigationSystem)
        {
            _loadingScreenView = loadingScreenView;
            _screenNavigationSystem = screenNavigationSystem;

            _loadingScreenView.OnAnimationEnd += ShowNextScreen;
        }

        private void ShowNextScreen()
        {
            _screenNavigationSystem.ExecuteNavigationCommand(
                new NavigationCommand().ShowNextScreen(ObjectName.MainScreen).CloseAfterNextScreenShown());
        }
    }
}