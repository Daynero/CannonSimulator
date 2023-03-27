using Core;

namespace Screens.LoadingScreen
{
    public class LoadingScreenPresenter
    {
        private IScreenNavigationSystem _screenNavigationSystem;

        public LoadingScreenPresenter(ILoadingScreenView loadingScreenView, IScreenNavigationSystem screenNavigationSystem)
        {
            _screenNavigationSystem = screenNavigationSystem;
        }

        public void OnStart()
        {
            _screenNavigationSystem.ExecuteNavigationCommand(
                new NavigationCommand().ShowNextScreen(ScreenName.MainScreen).CloseAfterNextScreenShown());
        }
    }
}