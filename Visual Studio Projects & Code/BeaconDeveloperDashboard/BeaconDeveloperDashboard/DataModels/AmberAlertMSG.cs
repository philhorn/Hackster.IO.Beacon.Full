using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaconDeveloperDashboard.DataModels
{
    public class AmberAlertMSG
    {
        public string Title { get; set; }

        public string Details { get; set; }

        public AmberAlertMSG()
        {
            Title = "Title";
            Details = "Something Here";
        }


    }
}
