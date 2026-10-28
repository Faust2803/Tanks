using System.Collections.Generic;
using Tanks.Lobby.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Windows
{
    public class ConnectionWindowView : BaseView
    {
        [Space]
        [SerializeField] private List<Toggle>  _tabToggle;
        [Space]
        [SerializeField] private List<GameObject>  _tabs;
        [Space]
        [SerializeField] private Button  _play;
        [Space]
        [SerializeField] private ConnectedOptionPanel  _botsOptions;
        [SerializeField] private ConnectedOptionPanel  _randomOptions;
        [SerializeField] private ConnectedOptionPanel  _commandOptions;
       
        [Space]
        [SerializeField] private GameObject  _panelStart;
        [SerializeField] private GameObject  _panelConnect;
        
        [Space]
        [SerializeField] private TextMeshProUGUI  _titleTextConnect;
        [SerializeField] private TextMeshProUGUI  _descriptionTextConnect;
        [Space]
        [SerializeField] private Button  _okButtonConnect;
        [SerializeField] private Button  _cancelButtonConnect;
        
        [SerializeField] public ItemsLoader _itemsLoader;
        public List<Toggle>  TabToggle => _tabToggle;
        public List<GameObject>  Tabs => _tabs;
        public ItemsLoader  ItemsLoader => _itemsLoader;
        public Button  Play => _play;
        public ConnectedOptionPanel  BotsOptions => _botsOptions;
        public ConnectedOptionPanel  RandomOptions => _randomOptions;
        public ConnectedOptionPanel  CommandOptions => _commandOptions;
        
        public Button  OkButtonConnect => _okButtonConnect;
        public Button CancelButtonConnect => _cancelButtonConnect;
        public TextMeshProUGUI  TitleTextConnect => _titleTextConnect;
        public TextMeshProUGUI  DescriptionTextConnect => _descriptionTextConnect;
        
        public GameObject  PanelStart => _panelStart;
        public GameObject  PanelConnect => _panelConnect;
       
        protected override BaseMediator CreateMediator()
        {
            _mediator = new ConnectionWindowMediator();
            return _mediator;
        }
        
        public CharacterRotator Rotator { get; set; }

        private void Update()
        {
            if (Rotator != null)
            {
                Rotator.Tick(Time.deltaTime);
            }
            
        }
    }
}