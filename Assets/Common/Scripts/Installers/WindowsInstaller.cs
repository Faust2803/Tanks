using SO.Scripts;
using UI;
using UI.Windows;
using Util;
using Zenject;

namespace Installers
{
    public class WindowsInstaller : MonoInstaller
    {
        [Inject] private WindowsConfig _windowsConfig;
        
        public override void InstallBindings()
        {
            //Bind Factory
            Container.BindFactory< WindowType, BaseView, FactoryWindow>().FromMethod(InitWindow);
        }

        private BaseView InitWindow(DiContainer container, WindowType window)
        {
            var go =_windowsConfig.GetWindow(window);
            if (go == null) return null;
            return Container.InstantiatePrefabForComponent<BaseView>(go);
        }
    }
}