using Common.Scripts.Network;
using Managers;
using Tanks.Common.Scripts;
using Tanks.Common.Scripts.Player;
using Util;
using Zenject;
namespace Installers
{
    public class ManagersInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            //Bind Manager
            Container.BindInterfacesAndSelfTo<UiManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ConfigsManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerManager>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<NetworkService>().AsSingle();
        }
    }
}