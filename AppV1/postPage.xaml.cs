using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

//using AppV3.Core.Models;
using AppV1.Core.Services;

using Microsoft.Toolkit.Uwp.UI.Controls;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;



namespace AppV1
{
    public sealed partial class postPage : Page
    {

        List<string> channels = new List<string>();
        int channelCount;
        public postPage()
        {
            this.InitializeComponent();
            channelCount = 1;
            Loaded += PostPage_Loaded;
        }
        private void PostPage_Loaded(object sender, RoutedEventArgs e)
        {
            IEnumerable<string> data = AppV1.DataAccess.GetChannels();//await AppV1.DataService.GetChannelAsync();
            foreach (var item in data)
            {
                channels.Add(item);
            }
            foreach (string name in channels)
            {
                Button b = new Button();
                b.Height = 50;
                b.VerticalAlignment = VerticalAlignment.Top;
                b.HorizontalAlignment = HorizontalAlignment.Stretch;
                b.Margin = new Thickness(0, 50 * (channelCount) + 60, 0, 0);
                b.Content = name;
                //b.Click += ChangeChannel;
                b.FontSize = 20;
                b.RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);
                b.Background = new SolidColorBrush(Windows.UI.ColorHelper.FromArgb(255, 48, 179, 221));
                grid.Children.Add(b);
                channelCount++;
            }
        }

        public void setAttributes(List<string> channelList)
        {
            channels = channelList;
        }

    }
}
