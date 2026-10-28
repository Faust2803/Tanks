using System;

namespace UI.Windows
{
    public class MessageWindowData : UIData
    {
        public string Title;
        public string Description;
        public bool HasOKButton;
        public bool HasCancelButton;
        public Action OKAction;
        public Action CancelAction;
        public bool TextAnimation;
        public string OkButtonText;
        public string CancelButtonText;
    }
}