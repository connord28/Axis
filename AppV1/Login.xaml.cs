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
using System.Data.SqlClient;

using Microsoft.Data.Sqlite;
using System.Collections.Generic;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AppV1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
        }
        private async void OnLogIn(object sender, RoutedEventArgs e)
        {
            string title = "Username";
            TextBox inputTextBox = new TextBox();
            inputTextBox.AcceptsReturn = false;
            inputTextBox.Height = 32;
            ContentDialog dialog = new ContentDialog();
            dialog.Content = inputTextBox;
            dialog.Title = title;
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Ok";
            dialog.SecondaryButtonText = "Cancel";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                OnUser(inputTextBox.Text);
            }
        }

        private async void OnUser(string user)
        {
            PasswordBox inputPassBox = new PasswordBox();
            inputPassBox.Height = 32;
            ContentDialog dialog = new ContentDialog();
            dialog.Content = inputPassBox;
            dialog.Title = "Password";
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Ok";
            dialog.SecondaryButtonText = "Cancel";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                if (user == "connor" && inputPassBox.Password == "password") //need to make this work with database
                {
                    this.Frame.Navigate(typeof(BlankPage2));
                }
                else
                {
                    OnUser(user);
                }
            }
        }

        private async void Test_Click(object sender, RoutedEventArgs e)
        {
            /*SqlServerDataService sqlServerDataService = new SqlServerDataService();
            string testText = sqlServerDataService.TestRetrieve("connor");*/
            //List<string> testText = AppV1.DataAccess.TestGet("connor");
            //textBlock1.Text = testText[0];
            AppV1.DataAccess.ResetChannels();
        }
    }

    public class SqlServerDataService
    {
        // TODO WTS: Specify the connection string in a config file or below.
        private string GetConnectionString()
        {
            return "postgres://connor@free-tier.gcp-us-central1.cockroachlabs.cloud:26257/defaultdb?sslmode=verify-full&sslrootcert=C:/Users/Connor/Downloads/cc-ca.crt&options=--cluster=clever-vole-790";
        }

        public void WriteUserName(string name)
        {
            
        }

        //checks if user is in the data base
        //returns 0 if true, -1 if not
        public int CheckUser(string name)
        {

            return 0;
        }

        public string TestRetrieve(string test)
        {

            string connectionString = "postgres://connor@free-tier.gcp-us-central1.cockroachlabs.cloud:26257/defaultdb?sslmode=verify-full&sslrootcert=<your_certs_directory>/cc-ca.crt&options=--cluster=clever-vole-790";//GetConnectionString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT password FROM users WHERE username = @test";
                        cmd.Parameters.Add(new SqlParameter("@test", System.Data.SqlDbType.Text));
                        cmd.Parameters["@test"].Value = test;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            return (string)reader[0];
                        }
                    }

                }
            }
            return "Failed";
        }
    }

}

