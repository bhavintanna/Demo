using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;

namespace SignalRDbUpdates.UnitOfWork
{
    public interface SignleRUnity
    {

        int ID { get; set; }

        string CampaignName { get; set; }

        DateTime MessageDate { get; set; }

        int Clicks { get; set; }

        int Conversions { get; set; }

        int Impressions { get; set; }

        string AffiliateName { get; set; }

        string Insert();

        string Update();

        List<SignleRUnity> Load();

        string Status1 { get; set; }
    }

    public class SignleRUOW<T> where T : SignleRUnity
    {
        private List<T> Changed = new List<T>();
        private List<T> New = new List<T>();

        public void Add(T obj)
        {
            if (obj.ID == 0)
            {
                New.Add(obj);
            }
            else
            {
                Changed.Add(obj);
            }
        }

        public void Committ()
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (T o in Changed)
                    {
                        o.Update();
                        o.Status1 = "Success";
                    }
                    foreach (T o in New)
                    {
                        o.Insert();
                        o.Status1 = "Success";
                    }
                    scope.Complete();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            

        }
        public void Load(SignleRUnity o)
        {

            Changed = o.Load() as List<T>;
        }

        public void Add(SignleRUnity obj)
        {
            throw new NotImplementedException();
        }
    }
}
