using System;
using DG.Tweening;
using UnityEngine;

namespace UI.Panels
{
    public class BasePanelMediator: BaseMediator
    {
        private PanelType _panelType;
        
        private Vector3  _movePanelPosition;
        private bool _isOpen = true;
        private RectTransform _movePanelRectTransform;
        private float _panelMoveOffset;
        
        private BasePanelView PanelView => BaseView as BasePanelView;
        
        public PanelType PanelType => _panelType;
        public bool DeleteAfterClose => BaseView.DeleteAfterClose;
        
        public void SetType(PanelType windowType)
        {
            _panelType = windowType;
        }

        public override void Show()
        {
            base.Show();
            ShowStart();
            BaseView.ShowStart();
            _panelMoveOffset =  PanelView.Offset;
            if (Application.platform == RuntimePlatform.Android)
            {
                //Debug.Log("Запущено на Android");
            }
            else if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                //Debug.Log("Запущено на Windows (сборка)");
                _panelMoveOffset = 0;
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                //Debug.Log("Запущено в редакторе Unity на Windows");
                _panelMoveOffset = 0;
            }
        }
        
        public void Close(Action callback = null)
        {
            if (callback!= null)
            {
                _afterCloseCallback = callback;
            }
            CloseStart();
        }
        
        protected virtual void ShowStart()
        {
            if (PanelView.MoveButton != null)
            {
                PanelView.MoveButton.onClick.AddListener(OnMoveButton);
                _movePanelPosition = PanelView.MovePanel.transform.position;
                _movePanelRectTransform = PanelView.MovePanel.GetComponent<RectTransform>();
            }
            
            if (PanelView.Panel == null || !BaseView.OpenAnimation)
            {
                OpenFinish();
                return;
            }
            OpenAnimation();
        }
        
        protected virtual void OpenAnimation()
        {
            BaseView.AnimationPanel.transform.localScale = Vector3.zero;
            BaseView.AnimationPanel.transform.DOScale(Vector3.one, BaseView.OpenCloseDuration).OnComplete(OpenFinish);
        }
        
        protected virtual void CloseStart()
        { 
            base.CloseStart();
            if (PanelView.Panel == null || !BaseView.CloseAnimation)
            {
                CloseFinish();
                return;
            }
            CloseAnimation();
            if (PanelView.MoveButton != null)
            {
                PanelView.MoveButton.onClick.RemoveListener(OnMoveButton);
            }
        }
        
        protected virtual void CloseAnimation()
        {
            BaseView.AnimationPanel.transform.DOScale(Vector3.zero, BaseView.OpenCloseDuration).OnComplete(CloseFinish);
        }
        
        private void OnMoveButton()
        {
            if (_isOpen)
            {
                MovePanel(GetMovePosition());
            }
            else
            {
                if (PanelView.MoveDirection == PanelMoveDirection.Top || PanelView.MoveDirection == PanelMoveDirection.Bottom)
                {
                    MovePanel(_movePanelPosition.y);
                }
                else
                {
                    MovePanel(_movePanelPosition.x);
                }
            }
            _isOpen = !_isOpen;
        }
        
        private void MovePanel(float endPos)
        {
            if (PanelView.MoveDirection == PanelMoveDirection.Top || PanelView.MoveDirection == PanelMoveDirection.Bottom)
            {
                PanelView.MovePanel.transform.DOMoveY(endPos, PanelView.OpenCloseDuration);
            }
            else
            {
                PanelView.MovePanel.transform.DOMoveX(endPos, PanelView.OpenCloseDuration);
            }
        }

        private float GetMovePosition()
        {
            var position = 0F;
            switch (PanelView.MoveDirection)
            {
                case PanelMoveDirection.Top:
                    position = PanelView.AnimationPanel.transform.position.y + _movePanelRectTransform.rect.height + _panelMoveOffset;
                    break;
                case PanelMoveDirection.Bottom:
                    position = PanelView.AnimationPanel.transform.position.y - _movePanelRectTransform.rect.height - _panelMoveOffset;
                    break;
                case PanelMoveDirection.Left:
                    position = PanelView.AnimationPanel.transform.position.x - _movePanelRectTransform.rect.width - _panelMoveOffset;
                    break;
                case PanelMoveDirection.Right:
                    position = PanelView.AnimationPanel.transform.position.x + _movePanelRectTransform.rect.width + _panelMoveOffset;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return position;
        }
        
        protected virtual void CloseSelf(Action callback = null)
        {
            _uiManager.ClosePanel(_panelType);
        }
    }
    
    public abstract class BasePanelMediator<T, Z> : BasePanelMediator where T : BasePanelView where Z : UIData
    {
        public T Target => BaseView as T;

        public Z Data => _data as Z;
    }
}