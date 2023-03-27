using System;
using System.Collections;
using Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace XmannaSDK.Core
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(UILayerConfigurator))]
    public class ScreenView : MonoBehaviour, IScreenView
    {
        [FormerlySerializedAs("_screenName")] [Header("Screen name")] [Tooltip("This should be unique screen name for navigation system")] [SerializeField]
        private ObjectName objectName;

        public event Action<object> OnShowCallback;
        public event Action OnGotFocusCallback;
        public event Action OnLostFocusCallback;
        public Action OnClose;
        
        public event Action OnScreenActivated;
        private bool _isOnPosition;
        private float _inactiveTimer;
        private CanvasGroup _canvasGroup;
        [HideInInspector] public bool ShouldPutInNavStack = true;
        [HideInInspector] public bool IsTransparent;
        private bool _isFocused;
        private UILayerConfigurator _uiLayerConfigurator;
        private IEnumerator _turnOffProcess;

        public ObjectName ObjectName => objectName;
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
            Debug.Log("ScreenActivate: " + objectName);
            gameObject.SetActive(true);
            OnScreenActivated?.Invoke();
        }

        private void DeactivateScreen()
        {
            Debug.Log("ScreenDeactivate: " + objectName);
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
            Debug.Log("ScreenGotFocus: " + objectName);
            OnGotFocusCallback?.Invoke();
        }

        public void LostFocus()
        {
            if (!_isFocused) return;
            _isFocused = false;
            CanvasGroup.interactable = false;
            Debug.Log("ScreenLostFocus: " + objectName);
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
        ObjectName ObjectName { get; }
    }
}