using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace XmannaSDK.Core
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(UILayerConfigurator))]
    public class ScreenView : MonoBehaviour, IScreenView
    {
        [Header("Screen name")] [Tooltip("This should be unique screen name for navigation system")] [SerializeField]
        private ScreenName _screenName;

        [HideInInspector] public event Action<object> OnShowCallback;
        [HideInInspector] public event Action OnGotFocusCallback;
        [HideInInspector] public event Action OnLostFocusCallback;
        [HideInInspector] public Action OnClose;
        
        public event Action OnScreenActivated;
        private bool _isOnPosition;
        private float _inactiveTimer;
        private CanvasGroup _canvasGroup;
        [HideInInspector] public bool ShouldPutInNavStack = true;
        [HideInInspector] public bool IsTransparent = false;
        private bool _isFocused;
        private UILayerConfigurator _uiLayerConfigurator;
        private IEnumerator _turnOffProcess;

        public ScreenName ScreenName => _screenName;
        public bool IsFocused => _isFocused;
        public CanvasGroup CanvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();
        public bool IsRootScreen => UILayerConfigurator.GetOrderLayer() == UIOrderLayer.Root;
        public UILayerConfigurator UILayerConfigurator => _uiLayerConfigurator ??= GetComponent<UILayerConfigurator>();

        public void MakeInvisible()
        {
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.interactable = false;
            CanvasGroup.alpha = 0;
        }

        public void ShowOnPosition(object extraData)
        {
            ActivateScreen();
            _isOnPosition = true;
            InvokeShowWith(extraData);

            GotFocus();
        }

        private void ActivateScreen()
        {
            Debug.Log("ScreenActivate: " + _screenName);
            gameObject.SetActive(true);
            OnScreenActivated?.Invoke();
        }

        private void DeactivateScreen()
        {
            Debug.Log("ScreenDeactivate: " + _screenName);
            MakeInvisible();
            LostFocus();
        }

        public void MoveToInitialPosition()
        {
            _isOnPosition = false;
            DeactivateScreen();
        }

        public void GotFocus()
        {
            if (_isFocused) return;
            _isFocused = true;
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
            CanvasGroup.alpha = 1;
            UILayerConfigurator.BackToDefaultOrder();
            Debug.Log("ScreenGotFocus: " + _screenName);
            OnGotFocusCallback?.Invoke();
        }

        public void LostFocus()
        {
            if (!_isFocused) return;
            _isFocused = false;
            CanvasGroup.interactable = false;
            Debug.Log("ScreenLostFocus: " + _screenName);
            OnLostFocusCallback?.Invoke();
        }

        public void DisactivateScreenWithDelay(float delay)
        {
            _turnOffProcess = ScreenTurnOff(delay);
        }
        
        private IEnumerator ScreenTurnOff(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (_isFocused) yield break;
            try
            {
                gameObject.SetActive(false);
            }
            catch {}
        }

        public void InvokeShowWith(object extraData)
        {
            OnShowCallback?.Invoke(extraData);
        }
        

        public void CloseScreen()
        {
            OnClose();
        }
        

        public void LayUnderScreen(int shift = 1)
        {
            UILayerConfigurator.ChangeShiftFromDefaultLayer(shift);
        }
    }

    public interface IScreenView
    {
        event Action<object> OnShowCallback;
        event Action OnGotFocusCallback;
        event Action OnLostFocusCallback;
        void CloseScreen();
        bool IsFocused { get; }
        UILayerConfigurator UILayerConfigurator { get; }
        ScreenName ScreenName { get; }
    }
}