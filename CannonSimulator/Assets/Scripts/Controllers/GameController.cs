using Core;
using Screens.MainScreen;

namespace Controllers
{
    public class GameController
    {
        private CannonController _cannonController;
        private readonly IScreenNavigationSystem _screenNavigationSystem;
        private readonly MainScreenPresenter _mainScreenPresenter;

        public GameController(IScreenNavigationSystem screenNavigationSystem, MainScreenPresenter mainScreenPresenter)
        {
            _screenNavigationSystem = screenNavigationSystem;
            _mainScreenPresenter = mainScreenPresenter;
        }
    }
}