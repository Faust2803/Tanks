using Tanks.Common.Scripts.Player;
using Zenject;

namespace UI.Windows
{
    public class BankWindowMediator :BaseWindowMediator <BankWindowView, UIData>
    {
        
        private PlayerManager _playerManager;
        
        [Inject]
        private void Construct(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }
        
        private void OnCurrencvCange()
        {
            Target.CurrencyText.text =_playerManager.Data.Currency.ToString();
        }
		
        protected override void ShowStart()
        {
            base.ShowStart();
            
            OnCurrencvCange();
        }

        
        
        protected override void CloseStart()
        { 
           
            base.CloseStart();
        }
    }
}