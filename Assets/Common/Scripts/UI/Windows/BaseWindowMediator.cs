using System;
using DG.Tweening;
using UnityEngine;

namespace UI.Windows
{
    public class BaseWindowMediator: BaseMediator
    {
        private WindowType _windowType;
        
        public WindowType WindowType => _windowType;
        public bool DeleteAfterClose => BaseView.DeleteAfterClose;

        private bool _closeLock;
        
        public void SetType(WindowType windowType)
        {
            _windowType = windowType;
        }
        
        public override void Show()
        {
            base.Show();
            ShowStart();
            BaseView.ShowStart();
            if(BaseView.CloseButton)
                BaseView.CloseButton.onClick.AddListener(()=>CloseSelf());

            _closeLock = false;
        }
        
        public void Close(Action callback = null)
        {
            if (callback!= null)
            {
                _afterCloseCallback = callback;
            }
            _closeLock = true;
            if(BaseView.CloseButton)
                BaseView.CloseButton.onClick.RemoveListener(()=>CloseSelf());
            CloseStart();
        }

        protected virtual void ShowStart()
        {
            if (BaseView.AnimationPanel == null || !BaseView.OpenAnimation)
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
        
        protected override void CloseStart()
        { 
            base.CloseStart();
            if (BaseView.AnimationPanel == null || !BaseView.CloseAnimation)
            {
                CloseFinish();
                return;
            }
            CloseAnimation();
        }
        
        protected virtual void CloseAnimation()
        {
            BaseView.AnimationPanel.transform.DOScale(Vector3.zero, BaseView.OpenCloseDuration).OnComplete(CloseFinish);
        }
        
        protected virtual void CloseSelf(Action callback = null)
        {
            if(_closeLock) return;
            _uiManager.CloseSelf(_windowType, callback);
        }
    }
    
    public abstract class BaseWindowMediator<T, Z> : BaseWindowMediator where T : BaseView where Z : UIData
    {
        public T Target => BaseView as T;

        public Z Data => _data as Z;
    }
}