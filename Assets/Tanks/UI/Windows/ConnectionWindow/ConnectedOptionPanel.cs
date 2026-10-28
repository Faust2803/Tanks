using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class ConnectedOptionPanel : MonoBehaviour
    {
        [SerializeField] private Slider _countPlayer;
        [SerializeField] private List<Toggle>  _mapIdToggle;
        
        [SerializeField] private List<Toggle>  _typeMyltyplayerToggle;
        [SerializeField] private TextMeshProUGUI  _fieldForCode;
        
        [SerializeField] private List<GameObject>  _forCreateGame;
        [SerializeField] private List<GameObject>  _forJoinGame;
        
        public int MapId { get; private set; }
        public float CountPlayer => _countPlayer.value;
        public string FieldForCode => _fieldForCode.text;
        public int TypeMyltyplayer { get; private set; }
        private void OnEnable()
        {
            for (var i = 0; i < _mapIdToggle.Count; i++)
            {
                _mapIdToggle[i].onValueChanged.AddListener(OnToggleClicked);
            }
            
            for (var i = 0; i < _typeMyltyplayerToggle.Count; i++)
            {
                _typeMyltyplayerToggle[i].onValueChanged.AddListener(OnMultyplayerToggleClicked);
            }

            SwitchConnectionType();
        }

        private void OnDisable()
        {
            for (var i = 0; i < _mapIdToggle.Count; i++)
            {
                _mapIdToggle[i].onValueChanged.RemoveListener(OnToggleClicked);
            }
            
            for (var i = 0; i < _typeMyltyplayerToggle.Count; i++)
            {
                _typeMyltyplayerToggle[i].onValueChanged.RemoveListener(OnMultyplayerToggleClicked);
            }
        }

        private void OnMultyplayerToggleClicked(bool isOn)
        {
            if (isOn)
            {
                for (var i = 0; i < _typeMyltyplayerToggle.Count; i++)
                {
                    if (_typeMyltyplayerToggle[i].isOn)
                    {
                        TypeMyltyplayer = i;
                    }
                }
                SwitchConnectionType();
            }
        }

        private void SwitchConnectionType()
        {
            if(_forCreateGame == null || _forJoinGame == null)return;
            
            var forCreate = false;
            var forJoin = false;
                
            switch (TypeMyltyplayer)
            {
                case 0:
                    forCreate = true;
                    break;
                case 1:
                    forJoin = true;
                    break;
            }

            for (var i = 0; i < _forCreateGame.Count; i++)
            {
                _forCreateGame[i].gameObject.SetActive(forCreate);
            }
                
            for (var i = 0; i < _forJoinGame.Count; i++)
            {
                _forJoinGame[i].gameObject.SetActive(forJoin);
            }
        }

        private void OnToggleClicked(bool isOn)
        {
            if (isOn)
            {
                for (var i = 0; i < _mapIdToggle.Count; i++)
                {
                    if (_mapIdToggle[i].isOn)
                    {
                        MapId = i;
                    }
                }
            }
        }
    }
}