using System;
using System.Collections.Generic;
using Installers;
using UI;
using UI.Panels;
using UI.Windows;
using Util;
using Zenject;

namespace Managers
{
    public class UiManager:IInitializable
    { 
        private readonly FactoryWindow _factoryWindow; 
        private readonly FactoryPanel _factoryPanels; 
        private readonly WindowsInstaller _windowPool; 
        private readonly PanelsInstaller _panelPool;

        private BaseWindowMediator _currentWindow;
        private Dictionary<WindowType, BaseWindowMediator> _allWindows;
        private Dictionary<PanelType, BasePanelMediator> _allPanels;
        public BaseWindowMediator CurrentWindow => _currentWindow;
        
        private Queue<WindowType> _windowsQueue;
        private Queue<object> _windowsDataQueue;
        private Stack<BaseWindowMediator> _windowsStack;

        public UiManager(FactoryWindow factoryWindow, FactoryPanel factoryPanels, WindowsInstaller windowPool, PanelsInstaller panelPool)
        {
            _factoryWindow = factoryWindow;
            _factoryPanels = factoryPanels;
            _windowPool = windowPool;
            _panelPool = panelPool;

            _windowsQueue = new Queue<WindowType>();
            _windowsDataQueue = new Queue<object>();
            _windowsStack = new Stack<BaseWindowMediator>();
        }
       
        public void Initialize()
        {
            _allWindows = new Dictionary<WindowType, BaseWindowMediator>();
            _allPanels = new Dictionary<PanelType, BasePanelMediator>();
        }

        public void OpenWindow(WindowType windowType, object data = null)
        {
            if (_currentWindow == null)
            {
               ShowWindow(windowType, data);
            }
            else
            {
                if (!_windowsQueue.Contains(windowType) || (data as WindowData).repeatedInQueue)
                {
                    _windowsQueue.Enqueue(windowType);
                    _windowsDataQueue.Enqueue(data);
                }
            }
        }
        
        public void ForceOpenWindow(WindowType windowType, object data = null)
        {
            if (_currentWindow != null)
            {
                if (_currentWindow.WindowType == windowType)
                {
                    return;
                }
                _windowsStack.Push(_currentWindow);
            }
            ShowWindow(windowType, data);
        }

        private void ShowWindow(WindowType windowType, object data = null)
        {
            if (_allWindows.ContainsKey(windowType))
            {
                _currentWindow = _allWindows[windowType];
            }
            else
            {
                _currentWindow = CreateWindow(windowType);
                if (_currentWindow == null) return;
                _allWindows.Add(windowType, _currentWindow);
            }
            _currentWindow.SetData(data);
            _currentWindow.Show();
        }

        public void CloseAllWindows ()
        {
            foreach (var item in _allWindows)
                item.Value.Close();
            //_allWindows.Clear();
            _windowsStack.Clear();
            _currentWindow = null;
        }

        public void CloseSelf(WindowType windowType, Action callback = null)
        {
            if (_currentWindow.WindowType == windowType)
            {
                CloseCurrentWindow(callback);
            }
            else
            {
                foreach (var item in _allWindows)
                {
                    if (item.Key == windowType )
                    {
                        item.Value.Close(callback);
                    }
                }
            }
        }
        
        public void CloseCurrentWindow(Action callback = null)
        {
            _currentWindow.Close(callback);
            if (_currentWindow.DeleteAfterClose)
            {
                _allWindows.Remove(_currentWindow.WindowType);
            }

            _currentWindow = null;

            if (_windowsStack.Count > 0)
            {
                _currentWindow = _windowsStack.Pop();
            }
            else
            {
                if (_windowsQueue.Count > 0)
                {
                    OpenWindow(_windowsQueue.Dequeue(), _windowsDataQueue.Dequeue());
                }
            }
        }
        
        public void CloseAllWindowsAndOpenOne(WindowType windowType, object data = null)
        {
            CloseAllWindows();
            OpenWindow( windowType,  data);
        }

        public BasePanelMediator OpenPanel(PanelType panelType, object data = null)
        {
            BasePanelMediator panel;
            if (_allPanels.ContainsKey(panelType))
            {
                panel = _allPanels[panelType];
            }
            else
            {
                panel = CreatePanel(panelType);
                if (panel == null) return null;
                _allPanels.Add(panelType, panel);
            }
            panel.SetData(data);
            panel.Show();
            return panel;
        }
        
        public BasePanelMediator ForceOpenPanel(PanelType panelType, object data = null)
        {
            return OpenPanel(panelType, data);
        }
        
        public void CloseAllPanels(Action callback = null)
        {
            foreach (var t in _allPanels)
            {
                t.Value.Close();
            }
            _allPanels.Clear();
        }
        
        public void ClosePanel(PanelType panelType, Action callback = null)
        {
            if (!_allPanels.ContainsKey(panelType)) return;
            var panel = _allPanels[panelType];
            panel.Close();
            if (panel.DeleteAfterClose)
            {
                _allPanels.Remove(panelType);
            }
        }

        private BaseWindowMediator CreateWindow(WindowType windowType)
        {
            var view = LoadWindowPrefab(windowType);
            if (view == null) return null;
        
            var mediator = view.Init(this) as BaseWindowMediator;
            mediator.SetType(windowType);
            view.gameObject.SetActive(false);
        
            return mediator;
        }
        
        private BasePanelMediator CreatePanel(PanelType panelType)
        {
            var view = LoadPanelPrefab(panelType);
            if (view == null) return null;
        
            var mediator = view.Init(this) as BasePanelMediator;
            mediator.SetType(panelType);
            view.gameObject.SetActive(false);
        
            return mediator;
        }
        
        private BaseView LoadWindowPrefab(WindowType windowType)
        {
            var view = _factoryWindow.Create(windowType);
            if (view == null) return null;
            view.gameObject.transform.SetParent(_windowPool.transform,false);
            return view;
        }
        
        private BasePanelView LoadPanelPrefab(PanelType panelType)
        {
            var view = _factoryPanels.Create(panelType);
            if (view == null) return null;
            view.gameObject.transform.SetParent(_panelPool.transform,false);
            return view;
        }
    }
}