using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppV1
{
    class DataService
    {
        private static IEnumerable<channel> _allChannels;

        private static IEnumerable<channel> AllChannels()
        {
            List<string> names = AppV1.DataAccess.GetChannels();
            List<channel> channels = new List<channel>();
            int i = 0;
            //System.Diagnostics.Debug.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            foreach (string name in names)
            {
                System.Diagnostics.Debug.WriteLine(name);
                channel temp = new channel();
                temp.name = name;
                channels.Insert(i,temp);
            }
            //var companies = AllCompanies();
            return channels;//companies.SelectMany(c => c.Orders);
        }

        public static async Task<IEnumerable<channel>> GetMasterDetailDataAsync()
        {
            await Task.CompletedTask;
            return AllChannels();
        }

        public static async Task<IEnumerable<channel>> GetChannelAsync()
        {
            await Task.CompletedTask;
            return AllChannels();
        }
    }
}
