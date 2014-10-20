using System;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls;


namespace Acr.MvvmCross.Plugins.UserDialogs.WindowsStore
{

    public class WinStoreUserDialogService : AbstractUserDialogService
    {
        // TODO: dispatching
        public override void Login(LoginConfig config)
        {
            throw new NotImplementedException();
        }

        protected override IProgressDialog CreateDialogInstance()
        {
            throw new NotImplementedException();
        }

        public override void ActionSheet(ActionSheetConfig config)
        {
            var input = new InputDialog
            {
                ButtonsPanelOrientation = Orientation.Vertical
            };

            var buttons = config.Options
                .Select(x => x.Text)
                .ToArray();

            input
                .ShowAsync(config.Title, null, buttons)
                .ContinueWith(x =>
                    config
                        .Options
                        .Single(y => y.Text == x.Result)
                        .Action()
                );
        }


        AlertConfig _alertConfig;

        public override void Alert(AlertConfig config)
        {
            _alertConfig = config;
            AlertPopup(config);
        }

        private async void AlertPopup(AlertConfig config)
        {
            Windows.UI.Popups.MessageDialog msg = new Windows.UI.Popups.MessageDialog(config.Message, config.Title);
            msg.Commands.Add(new UICommand(config.OkText, new UICommandInvokedHandler(AlertHandler)));
            msg.DefaultCommandIndex = 1;
            await msg.ShowAsync();
        }

        private void AlertHandler(IUICommand command)
        {
            if (_alertConfig != null)
            {
                _alertConfig.OnOk();
            }
        }

        ConfirmConfig _confirmConfig;

        public override void Confirm(ConfirmConfig config)
        {
            _confirmConfig = config;
            ConfirmPopup(config);
        }

        private async void ConfirmPopup(ConfirmConfig config)
        {
            Windows.UI.Popups.MessageDialog msg = new Windows.UI.Popups.MessageDialog(config.Message, config.Title);
            msg.Commands.Add(new UICommand(config.OkText, new UICommandInvokedHandler(ConfirmHandler)));
            msg.Commands.Add(new UICommand(config.CancelText, new UICommandInvokedHandler(ConfirmHandler)));
            msg.DefaultCommandIndex = 1;
            msg.CancelCommandIndex = 1;
            await msg.ShowAsync();
        }

        private void ConfirmHandler(IUICommand command)
        {
            var commandLabel = command.Label;

            if (commandLabel == _confirmConfig.OkText)
            {
                _confirmConfig.OnConfirm(true);
            }
            else if (commandLabel == _confirmConfig.CancelText)
            {
                _confirmConfig.OnConfirm(false);
            }
        }


        public override void Prompt(PromptConfig config)
        {
            var input = new InputDialog
            {
                AcceptButton = config.OkText,
                CancelButton = config.CancelText,
                InputText = config.Placeholder
            };
            input
                .ShowAsync(config.Title, config.Message)
                .ContinueWith(x =>
                {
                    // TODO: how to get button click for this scenario?
                });
        }


        public override void Toast(string message, int timeoutSeconds, Action onClick)
        {
            //http://msdn.microsoft.com/en-us/library/windows/apps/hh465391.aspx
            //  TODO: Windows.UI.Notifications.

            //var toast = new ToastPrompt {
            //    Message = message,
            //    MillisecondsUntilHidden = timeoutSeconds * 1000
            //};
            //if (onClick != null) {
            //    toast.Tap += (sender, args) => onClick();
            //}
            //toast.Show();
        }


        //protected override WinStoreProgressDialog CreateProgressDialogInstance()
        //{
        //    return new WinStoreProgressDialog();
        //}
    }
}
