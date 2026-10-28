using Managers;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels
{
    public abstract class BasePanelView : BaseView
    {
        [Space]
        [SerializeField] private Button  _moveButton;
        [SerializeField] private GameObject  _movePanel;
        [SerializeField] private float  _offset;
        [SerializeField] private PanelMoveDirection  _moveDirection;
        

        public GameObject Panel => gameObject;
        public Button  MoveButton => _moveButton;
        public GameObject  MovePanel => _movePanel;
        public float  Offset => _offset;
        public PanelMoveDirection  MoveDirection => _moveDirection;
        
    }
    
    public enum PanelMoveDirection
    {
        Top,
        Bottom,
        Left,
        Right
    }
}