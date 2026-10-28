using System.Collections.Generic;
using Common.Scripts.Network;
using Managers;
using UI.Panels;
using UnityEngine;
using Zenject;

namespace Tanks.Lobby.Scripts
{
    public class LobbyScenesManager : MonoBehaviour
    {
        [Inject] private UiManager UiManager;
        [Inject] private NetworkService NetworkService;
        

        [SerializeField] public ItemsLoader _itemsLoader;

        void Start()
        {
            //Debug.Log("Lobby Scene Manager");
            
            _itemsLoader.LoadAll();

            UiManager.OpenPanel(
                PanelType.LobbyMainPanel,
                new LobbyMainPanelData
                {
                    ItemsLoader = _itemsLoader,
                });

        }

    }
}
