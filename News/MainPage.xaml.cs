using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Diagnostics;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;
using News.Resources;
using News.ViewModels;

namespace News
{
    public class UnreadColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                bool unread = (bool)value;
                return new SolidColorBrush((unread) ? Colors.White : Colors.Gray);
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsConfigured)
            {
                NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
            }
            ProgressIndicator indicator = SystemTray.ProgressIndicator;
            if (indicator == null)
            {
                indicator = new ProgressIndicator();

                SystemTray.SetProgressIndicator(this, indicator);
                Binding binding = new Binding("IsLoading") { Source = App.ViewModel };
                BindingOperations.SetBinding(indicator, ProgressIndicator.IsVisibleProperty, binding);
                binding = new Binding("IsLoading") { Source = App.ViewModel };
                BindingOperations.SetBinding(indicator, ProgressIndicator.IsIndeterminateProperty, binding);
                indicator.Text = "Loading items...";
            }
        }

        // Load data for the ViewModel Items
        async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                if (!await App.ViewModel.LoadData())
                {
                    NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
                }
                else
                {
                    Debug.WriteLine("configured");
                }
            }
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

        public void OnLoadingPivotItem(object sender, PivotItemEventArgs e)
        {
            App.ViewModel.loadFolder(FolderPivot.SelectedIndex, 0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ItemViewModel item = (ItemViewModel)button.DataContext;
            item.Unread = false;
            App.ViewModel.ActiveItems = App.ViewModel.Folders[FolderPivot.SelectedIndex].Items;
            int index = App.ViewModel.ActiveItems.IndexOf(item);
            App.ViewModel.ActiveItem = item;
            NavigationService.Navigate(new Uri("/ItemPage.xaml?index=" + index, UriKind.Relative));
        }

        private void LongListSelector_ItemRealized(object sender, ItemRealizationEventArgs e)
        {
            LongListSelector list = (LongListSelector)sender;
            if ((list.ItemsSource as ObservableCollection<ItemViewModel>) == null)
            {
                return;
            }
            ItemViewModel item = e.Container.Content as ItemViewModel;

            ObservableCollection<ItemViewModel> items = list.ItemsSource as ObservableCollection<ItemViewModel>;
            int index = items.IndexOf(item);
            if (! App.ViewModel.IsLoading && (items.Count - index) < 5)
            {
                Debug.WriteLine(items.Last().Id);
                App.ViewModel.loadFolder(FolderPivot.SelectedIndex, items.Last().Id);
            }

        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            App.ViewModel.loadFolder(FolderPivot.SelectedIndex, 0);
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

    }

}