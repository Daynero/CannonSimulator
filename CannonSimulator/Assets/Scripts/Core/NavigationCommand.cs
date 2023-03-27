namespace Core
{
    public class NavigationCommand
    {
        public ScreenName NextScreenName => _nextScreenName;
        public ScreenName ScreenToClose => _screenToClose;

        public object ExtraData => _extraData;
        public bool IsCloseCurrentScreen => _isCloseCurrentScreen;
        public bool ShouldCloseAfterNextScreenShown => _closeAfterNextScreenShown;

        private object _extraData;
        private bool _isCloseLastScreen;
        private bool _isCloseCurrentScreen;
        private ScreenName _nextScreenName;
        private ScreenName _screenToClose;
        private bool _closeAfterNextScreenShown;

        public NavigationCommand ShowNextScreen(ScreenName screenName)
        {
            _nextScreenName = screenName;
            return this;
        }

        public NavigationCommand WithExtraData(object data)
        {
            _extraData = data;
            return this;
        }

        public NavigationCommand CloseScreen(ScreenName screenName)
        {
            _screenToClose = screenName;
            _isCloseCurrentScreen = true;
            return this;
        }

        public NavigationCommand CloseAfterNextScreenShown()
        {
            _closeAfterNextScreenShown = true;
            return this;
        }

        public bool IsNextScreenInQueue()
        {
            return !string.IsNullOrEmpty(_nextScreenName.ToString());
        }
    }
}