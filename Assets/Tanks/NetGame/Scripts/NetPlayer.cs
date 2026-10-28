using Common.Scripts.Network;
using Managers;
using SO.Scripts;
using Tanks.Common.Scripts;
using Tanks.Common.Scripts.Player;
using Tanks.Game.Scripts;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Tanks.NetGame.Scripts
{
    public class NetPlayer : NetworkBehaviour 
    {

        [Inject] public  PlayerManager _playerManager;
        [Inject] public  TanksConfig _tanksConfig;
        
        public NetworkVariable<int> ModelId = new NetworkVariable<int>(0,
            NetworkVariableReadPermission.Everyone, 
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<Color> ColorFirst= new NetworkVariable<Color>(Color.red,
            NetworkVariableReadPermission.Everyone, 
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<Color> ColorSecond = new NetworkVariable<Color>(Color.red,
            NetworkVariableReadPermission.Everyone, 
            NetworkVariableWritePermission.Owner);
        public NetworkVariable<Color> ColorAdditional = new NetworkVariable<Color>(Color.black,
            NetworkVariableReadPermission.Everyone, 
            NetworkVariableWritePermission.Owner);

        public GameObject _item;

        // public override void OnNetworkSpawn()
        // {
        //     ModelId.OnValueChanged += OnModelIdChanged;
        //     ColorFirst.OnValueChanged += OnColorFirstChanged;
        //     ColorSecond.OnValueChanged += OnColorSecondtChanged;
        //     ColorAdditional.OnValueChanged += OnColorAdditionalChanged;
        // }
        //
        // public override void OnNetworkDespawn()
        // {
        //     ModelId.OnValueChanged -= OnModelIdChanged;
        //     ColorFirst.OnValueChanged -= OnColorFirstChanged;
        //     ColorSecond.OnValueChanged -= OnColorSecondtChanged;
        //     ColorAdditional.OnValueChanged -= OnColorAdditionalChanged;
        // }

        // private void OnModelIdChanged(int previousvalue, int newvalue)
        // {
        //     Debug.Log($"OnModelIdChanged {previousvalue} -> {newvalue}");
        //     
        //     // if (IsOwner)
        //     // {
        //     //     ColorFirst.Value = _playerManager.GetColorTank(_tanksConfig.GetTank(ModelId.Value).Id, 0);
        //     //     ColorSecond.Value = _playerManager.GetColorTank(_tanksConfig.GetTank(ModelId.Value).Id, 1);
        //     //     ColorAdditional.Value = _playerManager.GetColorTank(_tanksConfig.GetTank(ModelId.Value).Id, 2);
        //     // }
        // }
        //
        public void SetVariables()
        {
            if (IsOwner)
            {
                ModelId.Value = _playerManager.GetCurrentTank();
                ColorFirst.Value = _playerManager.GetColorTank(_tanksConfig.GetTank(ModelId.Value).Id, 0);
                ColorSecond.Value = _playerManager.GetColorTank(_tanksConfig.GetTank(ModelId.Value).Id, 1);
                ColorAdditional.Value = _playerManager.GetColorTank(_tanksConfig.GetTank(ModelId.Value).Id, 2);
            }
        }
        
        // private void OnColorFirstChanged(Color previousvalue, Color newvalue)
        // {
        //     Debug.Log($"OnColorFirstChanged {previousvalue} -> {newvalue}");
        //     //ColorChanger.Change(_item, newvalue, 0);
        // }
        //
        // private void OnColorAdditionalChanged(Color previousvalue, Color newvalue)
        // {
        //     Debug.Log($"OnColorAdditionalChanged {previousvalue} -> {newvalue}");
        //     //ColorChanger.Change(_item, newvalue, 2);
        // }
        //
        // private void OnColorSecondtChanged(Color previousvalue, Color newvalue)
        // {
        //     Debug.Log($"OnColorSecondtChanged {previousvalue} -> {newvalue}");
        //     //ColorChanger.Change(_item, newvalue, 1);
        // }

        public void PrepareForGame()
        {
            
            
            LoadCurrent();
        }
        
        private void LoadCurrent()
        {
            _item = Instantiate(_tanksConfig.GetTank(ModelId.Value).Prefab, transform);
            ColorChanger.Change(_item, ColorFirst.Value, 0);
            ColorChanger.Change(_item, ColorSecond.Value, 1);
            ColorChanger.Change(_item, ColorAdditional.Value, 2);
        }

       
    }
}