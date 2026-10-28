
using System;
using Cysharp.Threading.Tasks;

namespace UI.Windows
{
    public class MessageWindowMediator :BaseWindowMediator <MessageWindowView, MessageWindowData>
    {
        protected override void ShowStart()
        {
            base.ShowStart();
            Target.TitleText.text = Data.Title;
            
            
            Target.CancelButton.gameObject.SetActive(Data.HasCancelButton);
            Target.OkButton.gameObject.SetActive(Data.HasOKButton);
            
            Target.CancelButton.onClick.AddListener(OnCancelButtonClicked);
            Target.OkButton.onClick.AddListener(OnOkButtonClicked);
            Target.CloseButton.onClick.AddListener(OnCancelButtonClicked);

            if (Data.OkButtonText != null)
            {
                Target.OkButtonText.text = Data.OkButtonText;
            }

            if (Data.CancelButtonText != null)
            {
                Target.CancelButtonText.text = Data.CancelButtonText;
            }
            
            if (Data.TextAnimation)
            {
                TextAnimation().Forget();
            }
            else
            {
                Target.DescriptionText.text = Data.Description;
            }
            
        }

        private void OnCancelButtonClicked()
        {
            if (Data.CancelAction != null)
            {
                Data.CancelAction.Invoke();
            }
            CloseSelf();
        }

        private void OnOkButtonClicked()
        {
            if (Data.OKAction != null)
            {
                Data.OKAction.Invoke();
            }
            CloseSelf();
        }


        private async UniTask TextAnimation()
        {
            var dots = 0;
            while (Data.TextAnimation)
            {
                string suffix = dots switch
                {
                    0 => "   ",
                    1 => ".  ",
                    2 => ".. ",
                    3 => "...",
                    _ => "   "
                };

                Target.DescriptionText.text = Data.Description + suffix;
                dots++;
                if (dots > 3)
                    dots = 0;

                Target.CancelButton.gameObject.SetActive(Data.HasCancelButton);
                Target.OkButton.gameObject.SetActive(Data.HasOKButton);
                await UniTask.Delay(TimeSpan.FromSeconds(1));
            }
            if (Target)
            {
                Target.CancelButton.gameObject.SetActive(Data.HasCancelButton);
                Target.OkButton.gameObject.SetActive(Data.HasOKButton);
                Target.DescriptionText.text = Data.Description;
            }
        }

        protected override void CloseStart()
        {
            Data.TextAnimation = false;
            Target.CancelButton.onClick.RemoveListener(OnCancelButtonClicked);
            Target.OkButton.onClick.RemoveListener(OnOkButtonClicked);
            Target.CloseButton.onClick.RemoveListener(OnCancelButtonClicked);
            base.CloseStart();
        }
        
       
    }
}