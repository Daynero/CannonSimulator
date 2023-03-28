using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]

    public class UILayerConfigurator : MonoBehaviour
    {
        [SerializeField] private UIOrderLayer layer;
        
        private const int LayerShift = 100;
        private const int HideAnimScreenShift = 25;
        private const int ShowAnimScreenShift = 30;
        
        private Canvas _canvas;
        private int _defaultLayerNum;
        private int _nowLayerNum;
        
        private void Awake()
        {
            SetDefaultLayer(layer);
            if (_canvas != null) return;
            SetOrder(_defaultLayerNum);
        }

        private void SetOrder(int order)
        {
            _canvas ??= GetComponent<Canvas>();
            _canvas.sortingOrder = order;
        }

        public void BackToDefaultOrder()
        {
            SetOrder(_nowLayerNum);
        }

        public void SetDefaultLayer(UIOrderLayer orderLayer)
        {
            layer = orderLayer;
            _nowLayerNum = _defaultLayerNum = (int) layer * LayerShift;
            SetOrder(_nowLayerNum);
        }

        public UIOrderLayer GetOrderLayer()
        {
            return layer;
        }

        public void SetShowAnimatingOrder()
        {
            SetOrder(_defaultLayerNum + ShowAnimScreenShift);
        }
        public void SetHideAnimatingOrder()
        {
            SetOrder(_defaultLayerNum + HideAnimScreenShift);
        }

        public void ChangeShiftFromDefaultLayer(int shift = 1)
        {
            _nowLayerNum = _defaultLayerNum + shift;
            SetOrder(_nowLayerNum);
        }
    }

    [Serializable]
    public enum UIOrderLayer
    {
        Root = 1,
    }

}