using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UpgradeYourself.Windows.Views
{
    public sealed partial class PageHeader : UserControl
    {
        public PageHeader()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public string PageTitleText
        {
            get
            {
                return (string)GetValue(PageTitleTextProperty);
            }

            set
            {
                SetValue(PageTitleTextProperty, value);

            }
        }

        public static readonly DependencyProperty PageTitleTextProperty =
            DependencyProperty.Register("PageTitleText", typeof(string), typeof(PageHeader),
            new PropertyMetadata(null));
    }
}
