using SO.Scripts;
using Util;
using Zenject;

namespace UI.Panels
{
    public class PanelsInstaller : MonoInstaller
    {
        [Inject] private PanelsConfig _panelsConfig;
        public override void InstallBindings()
        {
            Container.BindFactory< PanelType, BasePanelView, FactoryPanel>().FromMethod(InitPanels);
        }

        private BasePanelView InitPanels(DiContainer container, PanelType panel)
        {
            var go = _panelsConfig.GetPanel(panel);
            if (go == null) return null;
            return Container.InstantiatePrefabForComponent<BasePanelView>(go);
        }
    }
}