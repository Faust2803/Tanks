using System;
using System.Collections.Generic;
using Common.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

namespace UI.Panels
{
    public class LobbyMainPanelView : BasePanelView
    {
        [Space]
        [SerializeField] private Button  _startButton;
        [SerializeField] private Button  _leftButton;
        [SerializeField] private Button  _rightButton;
        [SerializeField] private Button  _rotateButton;
        [SerializeField] private ButtonListener  _rotateButtonListener;
        [SerializeField] private List<Button>  _colorButtons;
        [SerializeField] private List<Toggle>  _colorLevelToggle;
        
        [SerializeField] private Button  _colorButton;
        [SerializeField] private Transform  _colorButtonConteyner;
        
        [SerializeField] private Button  _shopButton;
        [SerializeField] private TextMeshProUGUI  _currencyText;
        [SerializeField] private Button  _bankButton;
        
        public Button  ShopButton => _shopButton;
        public Button  BankButton => _bankButton;
        public TextMeshProUGUI  CurrencyText => _currencyText;
        public Button  RotateButton => _rotateButton;
        public List<Toggle>  ColorLevelToggle => _colorLevelToggle;

        public List<Button>  ColorButtons => _colorButtons;
        public ButtonListener  RotateButtonListener => _rotateButtonListener;
        public Button  StartButton => _startButton;
        public Button  LeftButton => _leftButton;
        public Button  RightButton => _rightButton;
        
        protected override BaseMediator CreateMediator()
        {
            _mediator = new LobbyMainPanelMediator();
            return _mediator;
        }

        public Button CreateColorButton(Color color)
        {
            var item = Instantiate(_colorButton, _colorButtonConteyner);
            item.transform.SetAsFirstSibling();
            item.image.color = color;
            _colorButtons.Add(item);
            return item;
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