using UI;
using UI.Panels;
using UI.Windows;
using UnityEngine;
using Zenject;

namespace Util
{
    public class FactoryWindow : PlaceholderFactory<WindowType, BaseView> { }
    public class FactoryPanel : PlaceholderFactory<PanelType, BasePanelView> { }
}