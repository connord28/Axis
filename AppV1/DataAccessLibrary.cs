using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;


namespace AppV1
{
    public static class DataAccess
    {
        public async static void InitializeDatabase()
        {
            await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("sqliteSample.db", Windows.Storage.CreationCollisionOption.OpenIfExists);
            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (Microsoft.Data.Sqlite.SqliteConnection db =
               new Microsoft.Data.Sqlite.SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS users (username TEXT, " +
                    "password TEXT)";

                Microsoft.Data.Sqlite.SqliteCommand createTable = new Microsoft.Data.Sqlite.SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();

                String tableCommand2 = "CREATE TABLE IF NOT " +
                    "EXISTS channels (name TEXT UNIQUE)";
                System.Diagnostics.Debug.WriteLine(tableCommand);

                Microsoft.Data.Sqlite.SqliteCommand createTable2 = new Microsoft.Data.Sqlite.SqliteCommand(tableCommand2, db);

                createTable2.ExecuteReader();
            }
        }

        public async static void ResetChannels()
        {
            await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("sqliteSample.db", Windows.Storage.CreationCollisionOption.OpenIfExists);
            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (Microsoft.Data.Sqlite.SqliteConnection db =
               new Microsoft.Data.Sqlite.SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String tableCommand = "DROP TABLE IF EXISTS channels";

                Microsoft.Data.Sqlite.SqliteCommand dropTable = new Microsoft.Data.Sqlite.SqliteCommand(tableCommand, db);

                dropTable.ExecuteReader();
                String tableCommand2 = "CREATE TABLE IF NOT " +
                    "EXISTS channels (name TEXT UNIQUE)";
                //System.Diagnostics.Debug.WriteLine(tableCommand);

                Microsoft.Data.Sqlite.SqliteCommand createTable2 = new Microsoft.Data.Sqlite.SqliteCommand(tableCommand2, db);

                createTable2.ExecuteReader();
            }
            AddChannel("General");
        }
        public static void AddChannel(string name)
        {
            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (Microsoft.Data.Sqlite.SqliteConnection db =
              new Microsoft.Data.Sqlite.SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                Microsoft.Data.Sqlite.SqliteCommand insertCommand = new Microsoft.Data.Sqlite.SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO channels VALUES (@name);";
                insertCommand.Parameters.AddWithValue("@name", name);
                System.Diagnostics.Debug.WriteLine("-------------------------------------" + insertCommand.CommandText.ToString());
                try
                {
                    insertCommand.ExecuteReader();
                }
                catch (Exception)
                {
                    ContentDialog dialog = new ContentDialog();
                    dialog.Title = "That Channel Already Exists";
                    dialog.IsSecondaryButtonEnabled = false;
                    dialog.PrimaryButtonText = "Continue";
                    //dialog.SecondaryButtonText = "Cancel";
                }
                string trimmed = String.Concat(name.Where(c => !Char.IsWhiteSpace(c)));
                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS " + trimmed + " (title TEXT, content TEXT)";
                //System.Diagnostics.Debug.WriteLine(tableCommand);
                System.Diagnostics.Debug.WriteLine("-------------------------------------" + tableCommand);

                Microsoft.Data.Sqlite.SqliteCommand createTable = new Microsoft.Data.Sqlite.SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        public static void Post(string channel, string title, string content)
        {
            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (Microsoft.Data.Sqlite.SqliteConnection db =
              new Microsoft.Data.Sqlite.SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                Microsoft.Data.Sqlite.SqliteCommand insertCommand = new Microsoft.Data.Sqlite.SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO @channel VALUES (@title, @content);";
                insertCommand.Parameters.AddWithValue("@channel", channel);
                insertCommand.Parameters.AddWithValue("@title", title);
                insertCommand.Parameters.AddWithValue("@content", content);
                //System.Diagnostics.Debug.WriteLine("-------------------------------------" + insertCommand.CommandText.ToString());
                //System.Diagnostics.Debug.WriteLine("-------------------------------------" + insertCommand.Parameters[0].Value.ToString());
                // System.Diagnostics.Debug.WriteLine("-------------------------------------" + insertCommand.Parameters[1].Value.ToString());
                //System.Diagnostics.Debug.WriteLine("-------------------------------------" + insertCommand.Parameters[2].Value.ToString());
                title = title.Trim();
                content = content.Trim();
                String insertCommand2 = "INSERT INTO " + channel + " VALUES ('" +title+"', '" + content + "');";
                Microsoft.Data.Sqlite.SqliteCommand selection = new Microsoft.Data.Sqlite.SqliteCommand(insertCommand2, db);
                System.Diagnostics.Debug.WriteLine("-------------------------------------" + insertCommand2);
                selection.ExecuteReader();

                //insertCommand.ExecuteReader();

                db.Close();
            }
        }

        public static void AddUser(string name, string pass)
        {
            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (Microsoft.Data.Sqlite.SqliteConnection db =
              new Microsoft.Data.Sqlite.SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                Microsoft.Data.Sqlite.SqliteCommand insertCommand = new Microsoft.Data.Sqlite.SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO users VALUES (@user, @pass);";
                insertCommand.Parameters.AddWithValue("@user", name);
                insertCommand.Parameters.AddWithValue("@pass", pass);

                insertCommand.ExecuteReader();

                db.Close();
            }
        }

        public static List<String> TestGet(string name)
        {
            List<String> entries = new List<string>();
            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (Microsoft.Data.Sqlite.SqliteConnection db =
               new Microsoft.Data.Sqlite.SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                Microsoft.Data.Sqlite.SqliteCommand selectCommand = new Microsoft.Data.Sqlite.SqliteCommand
                    ("SELECT password from users WHERE username = 'connor'", db);

                Microsoft.Data.Sqlite.SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(query.GetString(0));
                }

                db.Close();
            }
            return entries;
        }

        public static List<String> GetChannels()
        {
            List<String> entries = new List<string>();
            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (Microsoft.Data.Sqlite.SqliteConnection db =
               new Microsoft.Data.Sqlite.SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                Microsoft.Data.Sqlite.SqliteCommand selectCommand = new Microsoft.Data.Sqlite.SqliteCommand
                    ("SELECT name from channels", db);

                Microsoft.Data.Sqlite.SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(query.GetString(0));
                }
                /*foreach(var entry in entries)
                {
                    System.Diagnostics.Debug.WriteLine("------------------------------------"+entry);
                }
                //System.Diagnostics.Debug.WriteLine("++++++++++++++++++++++" + entries[2]);*/
                db.Close();
            }
            return entries;
        }

        public static List<post> GetPostsTest()
        {
            List<post> entries = new List<post>();
            for (int i = 0; i < 3; i++)
            {
                post p = new post();
                p.title = "post_" + i;
                p.content = "this is the content for the post #" + i;
                entries.Add(p);
            }
            return entries;
        }

        public static List<post> GetPosts(string channel)
        {
            List<post> posts = new List<post>();
            List<string> contents = new List<string>();
            List<string> titles = new List<string>();
            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (Microsoft.Data.Sqlite.SqliteConnection db =
               new Microsoft.Data.Sqlite.SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                Microsoft.Data.Sqlite.SqliteCommand selectCommand = new Microsoft.Data.Sqlite.SqliteCommand
                    ("SELECT title from General", db);
                //selectCommand.Parameters.AddWithValue("@channel", channel);


                String selCommand = "SELECT title from " + channel;
                Microsoft.Data.Sqlite.SqliteCommand selection = new Microsoft.Data.Sqlite.SqliteCommand(selCommand, db);


                System.Diagnostics.Debug.WriteLine("---------------------" + channel);
                Microsoft.Data.Sqlite.SqliteDataReader query = selection.ExecuteReader();
                
                while (query.Read())
                {
                    titles.Add(query.GetString(0));
                }
                System.Diagnostics.Debug.WriteLine("++++++++++++++++++++++" + titles.Count());
                System.Diagnostics.Debug.WriteLine("========================" + titles[0]);

                Microsoft.Data.Sqlite.SqliteCommand selectCommand2 = new Microsoft.Data.Sqlite.SqliteCommand
                    ("SELECT content from General", db);
                //selectCommand2.Parameters.AddWithValue("@channel", channel);

                String selCommand2 = "SELECT content from " + channel;
                Microsoft.Data.Sqlite.SqliteCommand  selection2 = new Microsoft.Data.Sqlite.SqliteCommand(selCommand2, db);

                Microsoft.Data.Sqlite.SqliteDataReader query2 = selection2.ExecuteReader();
                while (query2.Read())
                {
                    contents.Add(query2.GetString(0));
                }
                db.Close();
            }
            System.Diagnostics.Debug.WriteLine("++++++++++++++++++++++" + titles.Count());
            int k = 0;
            for (int i = 0; i < titles.Count(); i++)
            {
                post p = new post();
                p.content = contents[i];
                p.title = titles[i];
                posts.Add(p);
            }

            return posts;
        }

    }
}
