using System.Collections.Generic;
using SO.Scripts;
using Tanks.Common.Scripts;
using Tanks.Common.Scripts.Player;
using UnityEngine;
using Zenject;

namespace Tanks.Lobby.Scripts
{
    public class ItemsLoader : MonoBehaviour
    {
        [Inject] protected  TanksConfig _tanksConfig;
        [Inject] protected  PlayerManager _playerManager;
        
        private List<Transform> _items = new List<Transform>();
        public List<Transform> GetItems => _items;
        
        public virtual Transform LoadCurrent(Transform whereT0oAdd = null)
        {
            return AddTank(_tanksConfig.GetTank(_playerManager.GetCurrentTank()), whereT0oAdd).transform;
        }

        public List<Transform> LoadAll(Transform whereT0oAdd = null)
        {
            for (var i = 0; i < _playerManager.Data.AllTanksIds.Count; i++)
            {
                AddTank(_tanksConfig.GetTank(i), whereT0oAdd);
            }

            return _items;
        }

        public GameObject AddTank(TanksConfigItem config, Transform whereT0Add = null)
        {
            if (whereT0Add == null)
            {
                whereT0Add = transform;
            }
            var item = Instantiate(config.Prefab, whereT0Add);
            _items.Add(item.transform);
            ColorChanger.Change(item, _playerManager.GetColorTank(config.Id, 0), 0);
            ColorChanger.Change(item, _playerManager.GetColorTank(config.Id, 1), 1);
            ColorChanger.Change(item, _playerManager.GetColorTank(config.Id, 2), 2);
            return item;
        }

       
    }
}