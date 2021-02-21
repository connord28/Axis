using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
//using System.Data.SqlClient;
using System.Text.RegularExpressions;


namespace AppV1.Core.Services
{
    // This class holds sample data used by some generated pages to show how they can be used.
    // More information on using and configuring this service can be found at https://github.com/Microsoft/WindowsTemplateStudio/blob/release/docs/UWP/services/sql-server-data-service.md
    // TODO WTS: Change your code to use this instead of the SampleDataService.
    public static class SqlServerDataService
    {
        // TODO WTS: Specify the connection string in a config file or below.
        private static string GetConnectionString()
        {
            return "postgres://connor:78CIdY5t7mZVIVRW@free-tier.gcp-us-central1.cockroachlabs.cloud:26257/defaultdb?sslmode=verify-full&sslrootcert=<your_certs_directory>/cc-ca.crt&options=--cluster=clever-vole-790";
        }

        public static void WriteUserName(string name)
        {

        }

        //checks if user is in the data base
        //returns 0 if true, -1 if not
        public static int CheckUser(string name)
        {
            
            return 0;
        }

        public static string TestRetrieve(string test)
        {
            string connectionString = GetConnectionString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT password FROM users WHERE username = @test";
                        cmd.Parameters.Add(new SqlParameter("@test", System.Data.SqlDbType.Text));
                        cmd.Parameters["@orderID"].Value = test;

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