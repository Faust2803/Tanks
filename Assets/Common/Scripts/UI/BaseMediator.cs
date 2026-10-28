using System;
using Managers;
using UnityEngine;
using Zenject;


namespace UI
{
    public class BaseMediator
    {
        protected Action _afterCloseCallback;
        protected object _data;
        protected UiManager _uiManager;
        protected Camera _camera;
        
        public  BaseMediator()
        {
            ProjectContext.Instance.Container.Inject(this);
        }
        
        protected BaseView BaseView { get; private set; }
        
        public virtual void SetData(object data)
        {
            _data = data;
        }
        
        public virtual void Mediate(BaseView value, UiManager uiManager)
        {
            BaseView = value;
            _uiManager = uiManager;
        }
        
        public virtual void Show()
        {
            if (BaseView.CameraOff)
            {
                _camera = Camera.main;
                _camera.enabled = false;
            } 
        }
        
        protected virtual void CloseFinish()
        { 
            BaseView.Close();
            if (_afterCloseCallback!= null)
            {
                _afterCloseCallback.Invoke();
            }
        }
        
        protected virtual void CloseStart()
        {
            if (_camera != null && BaseView.CameraOff)
            {
                _camera.enabled = true;
            } 
        }
        
        protected virtual void OpenFinish()
        {

        }
    }
}