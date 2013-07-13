using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using News.ViewModels;
using LinqToVisualTree;

namespace News
{
    public class HtmlStringBinding : DependencyObject
    {
        public static readonly DependencyProperty HtmlStringProperty =
            DependencyProperty.RegisterAttached(
            "HtmlString",
            typeof(string),
            typeof(HtmlStringBinding),
            new PropertyMetadata(OnHtmlStringPropertyChanged));

        private static void OnHtmlStringPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                WebBrowser wb = (WebBrowser)d;
                MainViewModel model = (MainViewModel)wb.DataContext;
                ItemViewModel item = model.ActiveItem;
                string prefix = "<meta name = \"viewport\" content = \"width=device-width\"/>";
                prefix += "<style>body{background-color: black; color: white; width:100%;overflow:hidden}a{color: white;}h2>a{text-decoration:none;}img{width:100%;height:auto !important;}a,a img{outline:none;border:none}</style>";
                prefix += "<h2><a href=\"" + item.Url + "\">" + item.Title + "</a></h2>";
                string postfix = "";
                wb.NavigateToString(prefix + (string)e.NewValue + postfix);
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