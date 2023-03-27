using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XmannaSDK.Core;

namespace Core
{
    public class ScreenNavigationSystem : IScreenNavigationSystem
    {
        private readonly List<ScreenView> _navigationStack = new();
        private readonly Dictionary<ScreenName, ScreenView> _availableScreens = new();
        private readonly Queue<NavigationCommand> _nextScreenInfos = new();
        private readonly Queue<NextScreenInfo> _waitingScreenInfos = new();

        private ScreenView _currentShowAnimatedScreenView;
        private IEnumerator _showInformationProcess;
        private readonly Action<ScreenName> _getScreenAction;

        public ScreenNavigationSystem(Action<ScreenName> getScreenAction)
        {
            _getScreenAction = getScreenAction;
        }

        private void GetNewScreen(ScreenName name)
        {
            _getScreenAction.Invoke(name);
        }

        public void AddScreen(ScreenName name, ScreenView screenView)
        {
            _availableScreens.Add(name, screenView);
            screenView.MakeInvisible();
            screenView.OnClose = delegate { Close(screenView); };

            if (name != ScreenName.LoadingScreen) return;

            AddScreenToStack(screenView);
            screenView.ShowOnPosition(null);
        }

        public void ExecuteNavigationCommand(NavigationCommand navigationCommand)
        {
            if (_navigationStack.Count != 0)
            {
                if (navigationCommand.IsNextScreenInQueue())
                {
                    PreparePreviousScreens(navigationCommand);
                }
                
                if (navigationCommand.IsCloseCurrentScreen)
                {
                    ScreenView screenToClose = _navigationStack.Find(screenView =>
                        screenView.ScreenName == navigationCommand.ScreenToClose);
                    if (screenToClose != null)
                        Close(screenToClose, navigationCommand.IsNextScreenInQueue());
                }
            }

            if (!navigationCommand.IsNextScreenInQueue()) return;

            Show(navigationCommand.NextScreenName, navigationCommand.ExtraData);
        }

        private void PreparePreviousScreens(NavigationCommand navigationCommand)
        {
            if (!_availableScreens.ContainsKey(navigationCommand.NextScreenName))
            {
                GetNewScreen(navigationCommand.NextScreenName);
            }

            var nextScreen = _availableScreens[navigationCommand.NextScreenName];

            //lay stack under next screen layer to correct show animation
            LayStackUnderNextScreen(nextScreen);

            var peek = _navigationStack.Last();
            if (peek != nextScreen)
            {
                peek.LostFocus();

                void OnNewScreenActivatedAction()
                {
                    if (navigationCommand.ShouldCloseAfterNextScreenShown)
                    {
                        Close(peek);
                    }

                    nextScreen.OnScreenActivated -= OnNewScreenActivatedAction;
                }

                nextScreen.OnScreenActivated += OnNewScreenActivatedAction;
            }
        }

        private void LayStackUnderNextScreen(ScreenView nextScreen)
        {
            if (nextScreen != null)
            {
                var array = _navigationStack.ToArray();
                for (var index = 0; index < array.Length; index++)
                {
                    ScreenView screenView = array[index];
                    screenView.LayUnderScreen(index + 1);
                }

                nextScreen.LayUnderScreen(array.Length + 1);
            }
        }

        private void Show(ScreenName screenName, object extraData = null, bool withAnim = true, bool delayed = false)
        {
            Debug.Log("Try to show screen " + screenName);
            if (_navigationStack.Count != 0)
            {
                if (!_availableScreens.ContainsKey(screenName))
                {
                    Debug.LogError("Cannot show screen. No such screen type " + screenName);
                    GetNewScreen(screenName);
                }

                if (_currentShowAnimatedScreenView == _availableScreens[screenName])
                {
                    Debug.LogError("You are trying to show the same screen again. " + screenName);
                    return;
                }

                if (_currentShowAnimatedScreenView != null && withAnim)
                {
                    Debug.Log("animation of another screen is still running (WAIT)");
                    if (_waitingScreenInfos.Any(info => info.Name == screenName)) return;
                    _waitingScreenInfos.Enqueue(new NextScreenInfo
                    {
                        Name = screenName,
                        ExtraData = extraData,
                    });
                    return;
                }
            }

            var screenView = _availableScreens[screenName];
            CheckRootScreenShown(screenView);

            AddScreenToStack(screenView);

            screenView.ShowOnPosition(extraData);
        }

        private void CheckRootScreenShown(ScreenView screenView)
        {
            if (!screenView.IsRootScreen) return;
            Debug.Log("Root screen shown. Close all screens");

            for (int i = _navigationStack.Count - 1; i >= 0; i--)
            {
                if (screenView != _navigationStack[i])
                {
                    Close(_navigationStack[i]);
                }
            }
        }

        private void LogStack(string operation)
        {
            if (_navigationStack.Count == 0)
            {
                Debug.Log(operation + "\n" + "Screen stack is empty!");
            }
            else
            {
                string nowInStack = "Now in screen stack:" + "\n";
                foreach (var screen in _navigationStack)
                {
                    nowInStack += screen.name + "\n";
                }

                Debug.Log(operation + "\n" + nowInStack);
            }
        }

        private void AddScreenToStack(ScreenView screenView)
        {
            if (!screenView.ShouldPutInNavStack) return;
            _navigationStack.Add(screenView);
            LogStack("Screen ADDED to stack " + screenView.name);
        }

        private void RemoveScreenFromStack(ScreenView screenView, bool nextScreenInQueue = false)
        {
            if (_navigationStack.Count == 0 || !screenView.ShouldPutInNavStack)
            {
                return;
            }

            if (_navigationStack.Contains(screenView))
            {
                _navigationStack.Remove(screenView);
                LogStack("Screen REMOVED from stack " + screenView.name);
                screenView.LostFocus();
                screenView.DisactivateScreenWithDelay(1f);
                if (_waitingScreenInfos.Count > 0)
                {
                    var waitingScreenInfo = _waitingScreenInfos.Dequeue();
                    Show(waitingScreenInfo.Name, waitingScreenInfo.ExtraData);
                }
                else if (_navigationStack.Count > 0 && !nextScreenInQueue)
                {
                    //move focus to previous screen
                    var activeScreen = _navigationStack.Last();
                    activeScreen.GotFocus();
                }
            }
        }

        private void Close(ScreenView screenView, bool nextScreenInQueue = false)
        {
            if (!_navigationStack.Contains(screenView) && screenView.ShouldPutInNavStack) return;
            RemoveScreenFromStack(screenView, nextScreenInQueue);
            screenView.MoveToInitialPosition();
        }
        

        private class NextScreenInfo
        {
            public ScreenName Name;
            public object ExtraData;
        }

        private ScreenName _overlayOpenBy;

    }

    public enum ScreenName
    {
        LoadingScreen,
        MainScreen
    }


    public interface IScreenNavigationSystem
    {
        void ExecuteNavigationCommand(NavigationCommand navigationCommand);
    }
}