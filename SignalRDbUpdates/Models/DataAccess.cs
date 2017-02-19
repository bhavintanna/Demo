using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace UnitOfWork
{
    public class DataAccess
    {
        //private string ConnectionString = @"Data Source=WIN-0A9O9Q2K0N9\SQL2008;Initial Catalog=Customer;Integrated Security=True";
        readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public string InsertMessages(string CampaignName, DateTime MessageDate, int Clicks, int Conversions, int Impressions, string AffiliateName)
        {
            string messages = string.Empty;
            var connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand("sp_Insert_Messages", connection);

            try
            {

                connection.Open();
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@campainName", CampaignName);
                command.Parameters.Add("@date", MessageDate);
                command.Parameters.Add("@click", Clicks);
                command.Parameters.Add("@conversions", Conversions);
                command.Parameters.Add("@impression", Impressions);
                command.Parameters.Add("@affiliateName", AffiliateName);
                int reader = command.ExecuteNonQuery();
                if (reader >= 1)
                {
                    messages = "Success";
                }
                else
                {
                    messages = "Failed";
                }
            }
            catch (Exception)
            {

                messages = "Failed";
            }
            return messages;
        }

        public string UpdateMessages(int Id, string CampaignName, DateTime MessageDate, int Clicks, int Conversions, int Impressions, string AffiliateName)
        {
            string messages = string.Empty;
            try
            {
                var connection = new SqlConnection(ConnectionString);
                SqlCommand command = new SqlCommand("sp_Update_Messages", connection);
                connection.Open();

                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Id", Id);
                command.Parameters.Add("@MessageDate", MessageDate);
                command.Parameters.Add("@campainName", CampaignName);
                command.Parameters.Add("@click", Clicks);
                command.Parameters.Add("@conversions", Conversions);
                command.Parameters.Add("@impression", Impressions);
                command.Parameters.Add("@affiliateName", AffiliateName);

                int reader = command.ExecuteNonQuery();
                if (reader >= 1)
                {
                    messages = "Success";
                }
                else
                {
                    messages = "Failed";
                }
            }
            catch (Exception)
            {

                messages = "Failed";
            }
            return messages;
        }

    }
}
