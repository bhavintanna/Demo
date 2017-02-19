using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitOfWork;
using SignalRDbUpdates.UnitOfWork;
using System.Data.SqlClient;
namespace SignalRDbUpdates.Models
{
    public class Messages : SignleRUnity
    {
        public int ID { get; set; }

        public string CampaignName { get; set; }

        public DateTime MessageDate { get; set; }

        public int Clicks { get; set; }

        public int Conversions { get; set; }
        
        public int Impressions { get; set; }
        
        public string AffiliateName { get; set; }

        public string Status1 { get; set; }

        public string Insert()
        {
            DataAccess obj = new DataAccess();
            Status1 = obj.InsertMessages(CampaignName, MessageDate, Clicks, Conversions, Impressions, AffiliateName);
            return Status1;
        }

        public List<SignleRUnity> Load()
        {
            DataAccess obj = new DataAccess();
            Messages o = new Messages();
            //SqlDataReader ds = obj.GetCustomer(ID);
            //while (ds.Read())
            //{
            //    o.CampaignName = ds["campainName"].ToString();
            //}
            List<SignleRUnity> Li = (new List<Messages>()).ToList<SignleRUnity>();
            Li.Add((SignleRUnity)o);
            return Li;
        }


        public string Update()
        {
            DataAccess obj = new DataAccess();
            Status1 = obj.UpdateMessages(ID, CampaignName, MessageDate, Clicks, Conversions, Impressions, AffiliateName);
            return Status1;
        }
    }
}
