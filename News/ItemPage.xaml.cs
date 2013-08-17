using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using News.ViewModels;
using LinqToVisualTree;
using Windows.ApplicationModel;
using System.Threading.Tasks;
using Windows.Storage;

namespace News
{
    public class HtmlStringBinding : DependencyObject
    {
        static private string template;

        static async private Task<string> getTemplate()
        {
            template = "";
            string folder = Package.Current.InstalledLocation.Path;
            string path = string.Format(@"{0}\Assets\template.html", folder);
            StorageFile storageFile = await StorageFile.GetFileFromPathAsync(path);
            Stream stream = await storageFile.OpenStreamForReadAsync();
            StreamReader reader = new StreamReader(stream);
            template = reader.ReadToEnd();

            return template;
        }

        public static readonly DependencyProperty HtmlStringProperty =
            DependencyProperty.RegisterAttached(
            "HtmlString",
            typeof(string),
            typeof(HtmlStringBinding),
            new PropertyMetadata(OnHtmlStringPropertyChanged));

        private async static void OnHtmlStringPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                WebBrowser wb = (WebBrowser)d;
                MainViewModel model = (MainViewModel)wb.DataContext;
                ItemViewModel item = model.ActiveItem;
                string body = await getTemplate();
                body = body.Replace("%body%", (string)e.NewValue);
                body = body.Replace("%url%", item.Url);
                body = body.Replace("%title%", item.Title);
                body = body.Replace("<img ", "<img max-width=\"100%\" ");
                wb.NavigateToString(body);
            }
        }

        public static void SetHtmlString(DependencyObject obj, string html)
        {
            obj.SetValue(HtmlStringProperty, html);
        }

        public static string GetHtmlString(DependencyObject obj)
        {
            return (string)obj.GetValue(HtmlStringProperty);
        }
    }

    public partial class ItemPage : PhoneApplicationPage
    {
        public ItemPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            WebBrowser wb = (WebBrowser)sender;
            var border = wb.Descendants<Border>().Last() as Border;

            border.ManipulationDelta += Border_ManipulationDelta;
            border.ManipulationCompleted += Border_ManipulationCompleted;
        }

        private void Border_ManipulationCompleted(object sender,
                                              ManipulationCompletedEventArgs e)
        {
            // suppress zoom
            if (e.FinalVelocities.ExpansionVelocity.X != 0.0 ||
                e.FinalVelocities.ExpansionVelocity.Y != 0.0)
                e.Handled = true;
        }

        private void Border_ManipulationDelta(object sender,
                                              ManipulationDeltaEventArgs e)
        {
            //Debug.WriteLine("man");
            //// suppress zoom
            //if (e.DeltaManipulation.Scale.X != 0.0 ||
            //    e.DeltaManipulation.Scale.Y != 0.0)
            //    e.Handled = true;

            //// optionally suppress scrolling
            //if (e.DeltaManipulation.Translation.X != 0.0 ||
            //  e.DeltaManipulation.Translation.Y != 0.0)
            //    e.Handled = true;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
    }

}