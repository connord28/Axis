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
using Windows.UI.Xaml.Controls;



// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppV1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class BlankPage2 : Page
    {
        List<StackPanel> channelPanels = new List<StackPanel>();
        List<string> channels = new List<string>();
        private int channelCount;
        IEnumerable<post> posts;
        private string currChannel;
        private string nextChannel;
        public BlankPage2()
        {
            //DataAccess.InitializeDatabase();
            this.InitializeComponent();
            channelCount = 1;
            Loaded += ChannelPage_Loaded;
        }

        private void ChannelPage_Loaded(object sender, RoutedEventArgs e)
        {
            //bool isChannels = true;

            //try
            //{
            IEnumerable<string> data = AppV1.DataAccess.GetChannels();//await AppV1.DataService.GetChannelAsync();
            if(data.Count() == 0)
            {
                DataAccess.AddChannel("General");
                channels.Add("General");
            }
            foreach (var item in data)
            {
                channels.Add(item);
            }
            foreach (string name in channels)
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Windows.UI.Colors.Silver;//ColorHelper.FromArgb(255, 48, 0, 0);
                Button b = new Button();
                b.Height = 50;
                b.VerticalAlignment = VerticalAlignment.Top;
                b.HorizontalAlignment = HorizontalAlignment.Stretch;
                b.Margin = new Thickness(0, 50 * (channelCount) + 60, 0, 0);
                b.Content = name;
                b.Click += ChangeChannel;
                b.FontSize = 20;
                b.RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);
                b.BorderBrush = brush;
                b.Background = new SolidColorBrush(Windows.UI.ColorHelper.FromArgb(255, 48, 179, 221));
                grid.Children.Add(b);
                channelCount++;
            }
            currChannel = "General";
            nextChannel = "General";
            try
            {
                posts = new List<post>();
                posts = DataAccess.GetPosts(currChannel);
                System.Diagnostics.Debug.WriteLine(posts.Count());
                if (posts.Count() != 0)
                {
                    System.Diagnostics.Debug.WriteLine(posts.ElementAt(0).title);
                    postList.Items.Clear();
                    postList.ItemsSource = posts;
                }
                else
                {
                    postList.Items.Clear();
                }
            }
            catch (Exception)
            {
                //System.Diagnostics.Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                postList.Items.Clear();
            }
        }

        private void updatePosts()
        {
            try
            {
                posts = new List<post>();
                posts = DataAccess.GetPosts(currChannel);
                System.Diagnostics.Debug.WriteLine(posts.Count());
                if (posts.Count() != 0)
                {
                    System.Diagnostics.Debug.WriteLine(posts.ElementAt(0).title);
                    //postList.Items.Clear();
                    postList.ItemsSource = posts;
                }
                else
                {
                    postList.Items.Clear();
                }
            }
            catch (Exception)
            {
                //System.Diagnostics.Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                //postList.Items.Clear();
                posts = new List<post>();
                postList.ItemsSource = posts;
            }
        }

        private void ChangeChannel(object sender, RoutedEventArgs e)
        {
            currChannel = (sender as Button).Content.ToString();
            updatePosts();
        }
        private async void ChannelDialog(object sender, RoutedEventArgs e)
        {
            string title = "Enter Channel Name";
            TextBox inputTextBox = new TextBox();
            inputTextBox.AcceptsReturn = false;
            inputTextBox.Height = 32;
            ContentDialog dialog = new ContentDialog();
            dialog.Content = inputTextBox;
            dialog.Title = title;
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Create";
            dialog.SecondaryButtonText = "Cancel";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {

                AppV1.DataAccess.AddChannel(inputTextBox.Text);

                int numPanel = channelPanels.Count;
                StackPanel sp = new StackPanel
                {
                    Name = inputTextBox.Text,
                    Orientation = Orientation.Horizontal,
                    Width = 20,
                    Height = 20,
                    Margin = new Thickness(1),
                };

                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Windows.UI.Colors.Silver;
                Button b = new Button();
                b.Height = 50;
                b.VerticalAlignment = VerticalAlignment.Top;
                b.HorizontalAlignment = HorizontalAlignment.Stretch;
                b.Margin = new Thickness(0, 50* (channelCount)+60, 0, 0);
                channelCount++;
                b.Content = inputTextBox.Text;
                b.Click += ChangeChannel;
                b.FontSize = 20;
                b.RenderTransformOrigin = new Windows.Foundation.Point(0.5,0.5);
                b.BorderBrush = brush;
                b.Background = new SolidColorBrush(Windows.UI.ColorHelper.FromArgb(255, 48, 179, 221));
                grid.Children.Add(b);
                //sp.Children.Add(b);
                //channelPanels.Add(sp);
                //grid.Children.Add(sp);
            }
        }

        private void postList_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            TryGoBack();
        }

        // App.xaml.cs
        //
        // Add this method to the App class.
        public static bool TryGoBack()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
                return true;
            }
            return false;
        }

        private async void postDialog(object sender, RoutedEventArgs e)
        {
            string title = "Make your post";
            var container = new StackPanel();
            TextBox nameBox = new TextBox()
            {
                PlaceholderText = "Add a title for your post.",
                Header = "Title",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 60
            };
            TextBox contentBox = new TextBox()
            {
                PlaceholderText = "The content of your post goes here",
                //Header = "Phone",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 45, 0, 0),
                Height = 100,
                AcceptsReturn = true,
            };
            /*TextBox titleTextBox = new TextBox();
            titleTextBox.AcceptsReturn = false;
            titleTextBox.Height = 32;
            TextBox contentTextBox = new TextBox();
            contentTextBox.AcceptsReturn = true;
            contentTextBox.Height = 75;*/
            container.Children.Add(nameBox);
            container.Children.Add(contentBox);
            ContentDialog dialog = new ContentDialog();
            dialog.Content = container;
            dialog.Title = title;
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Create";
            dialog.SecondaryButtonText = "Cancel";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                DataAccess.Post(currChannel, nameBox.Text, contentBox.Text);
                updatePosts();
            }

        }
    }
}
