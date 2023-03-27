using Enums;

namespace Core
{
    public class NavigationCommand
    {
        public ObjectName NextObjectName => _nextObjectName;
        public ObjectName ObjectToClose => _objectToClose;

        public object ExtraData => _extraData;
        public bool IsCloseCurrentScreen => _isCloseCurrentScreen;
        public bool ShouldCloseAfterNextScreenShown => _closeAfterNextScreenShown;

        private object _extraData;
        private bool _isCloseLastScreen;
        private bool _isCloseCurrentScreen;
        private ObjectName _nextObjectName;
        private ObjectName _objectToClose;
        private bool _closeAfterNextScreenShown;

        public NavigationCommand ShowNextScreen(ObjectName objectName)
        {
            _nextObjectName = objectName;
            return this;
        }

        public NavigationCommand WithExtraData(object data)
        {
            _extraData = data;
            return this;
        }

        public NavigationCommand CloseScreen(ObjectName objectName)
        {
            _objectToClose = objectName;
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
            return !string.IsNullOrEmpty(_nextObjectName.ToString());
        }
    }
}