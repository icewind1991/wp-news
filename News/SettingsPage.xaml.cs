using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Diagnostics;

namespace News
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsMenuEnabled = true;
            ApplicationBar.IsVisible = true;
            ApplicationBar.Opacity = 1.0;

            ApplicationBarIconButton doneButton = new ApplicationBarIconButton(new Uri("/Images/check.png", UriKind.Relative));
            doneButton.Text = "done";
            doneButton.Click += new EventHandler(doneButton_Click);

            ApplicationBarIconButton cancelButton = new ApplicationBarIconButton(new Uri("/Images/cancel.png", UriKind.Relative));
            cancelButton.Text = "cancel";
            cancelButton.Click += new EventHandler(cancelButton_Click);

            ApplicationBar.Buttons.Add(doneButton);
            ApplicationBar.Buttons.Add(cancelButton);
        }

        void doneButton_Click(object sender, EventArgs e)
        {
            AppSettings settings = new AppSettings();
            settings.UsernameSetting = textBoxUsername.Text;
            settings.PasswordSetting = passwordBoxPassword.Password;
            settings.UrlSetting = textBoxUrl.Text;
            App.ViewModel.applySettings(settings);
            NavigationService.GoBack();
        }

        void cancelButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}