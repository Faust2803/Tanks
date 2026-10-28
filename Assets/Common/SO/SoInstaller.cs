using UnityEngine;
using Zenject;

namespace SO.Scripts
{
    [CreateAssetMenu (fileName = "SoInstaller", menuName = "Configs/Create SO Installer", order = 0 )]
    public class SoInstaller : ScriptableObjectInstaller<SoInstaller>
    {
        [SerializeField] public WindowsConfig _windowsConfig;
        [SerializeField] public PanelsConfig _panelConfig;
        
        
        //For game
        [SerializeField] public TanksConfig _tanksConfig;
        [SerializeField] public ColorsConfig _colorConfig;
        [SerializeField] public LevelConfig _levelConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_windowsConfig).IfNotBound();
            Container.BindInstance(_panelConfig).IfNotBound();
            
            //For game
            Container.BindInstance(_tanksConfig).IfNotBound();
            Container.BindInstance(_colorConfig).IfNotBound();
            Container.BindInstance(_levelConfig).IfNotBound();
        }
    }
}