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
using Microsoft.Phone.Tasks;

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

            var gl = GestureService.GetGestureListener(Browser);
            gl.Flick += gl_Flick;
        }

        void gl_Flick(object sender, Microsoft.Phone.Controls.FlickGestureEventArgs e)
        {
            if (Math.Abs(e.HorizontalVelocity) / 2 > Math.Abs(e.VerticalVelocity))
            {
                // User flicked towards left
                if (e.HorizontalVelocity < 0)
                {
                    Debug.WriteLine("left");
                }

                // User flicked towards right
                if (e.HorizontalVelocity > 0)
                {
                    // Load the previous image
                    Debug.WriteLine("right");
                }
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void Browser_Navigating(object sender, NavigatingEventArgs e)
        {
            if (e.Uri.ToString() != "")
            {
                e.Cancel = true;
                WebBrowserTask task = new WebBrowserTask();
                task.Uri = e.Uri;
                task.Show();
            }
        }

    }

}