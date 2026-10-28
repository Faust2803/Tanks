using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels
{
    public class LoadingPanelView : BasePanelView
    {
        [Space]
        [SerializeField] private Button  _startButton;
        
        public Button  StartButton => _startButton;
        
        protected override BaseMediator CreateMediator()
        {
            _mediator = new LoadingPanelMediator();
            return _mediator;
        }
    }
}