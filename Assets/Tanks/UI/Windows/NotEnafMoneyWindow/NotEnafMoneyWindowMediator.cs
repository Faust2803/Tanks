
namespace UI.Windows
{
    public class NotEnafMoneyWindowMediator :BaseWindowMediator <NotEnafMoneyWindowView, NotEnafMoneyWindowData>
    {
		
        protected override void ShowStart()
        {
            base.ShowStart();

            Target.DescriptionText.text = $"You do not have {Data.Amount} coins to complete the operation. Do you want to buy them?";
            Target.BankButton.onClick.AddListener(OnBankButtonClicked);
            Target.CancelButton.onClick.AddListener(OnCancelButtonClicked);
            
        }

        protected override void CloseStart()
        {
            Target.BankButton.onClick.AddListener(OnBankButtonClicked);
            base.CloseStart();
        }
        
        private void OnBankButtonClicked()
        {
            _uiManager.CloseAllWindowsAndOpenOne(WindowType.BankWindow);
           
        }
        private void OnCancelButtonClicked()
        {
            CloseSelf();
        }
    }
}