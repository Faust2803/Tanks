using System.Collections.Generic;
using SO.Scripts;
using Tanks.Common.Scripts;
using Tanks.Common.Scripts.Player;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Tanks.NetGame.Scripts
{
    public class GameItemLoader : NetworkBehaviour
    {
       
        
        [Inject] protected  TanksConfig _tanksConfig;
        [Inject] protected  PlayerManager _playerManager;
        
        public virtual void LoadCurrent()
        {
            var currentTransform = AddTank(_tanksConfig.GetTank(_playerManager.GetCurrentTank())).transform;
            
            currentTransform.transform.position =  Vector3.zero;
            currentTransform.transform.rotation = Quaternion.identity;
        }

        private GameObject AddTank(TanksConfigItem config)
        {
            var item = Instantiate(config.Prefab, transform);
            ColorChanger.Change(item, _playerManager.GetColorTank(config.Id, 0), 0);
            ColorChanger.Change(item, _playerManager.GetColorTank(config.Id, 1), 1);
            ColorChanger.Change(item, _playerManager.GetColorTank(config.Id, 2), 2);
            return item;
        }
    }
}