using SignalRDbUpdates.Hubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalRDbUpdates.Hubs;
using SignalRDbUpdates.UnitOfWork;

namespace SignalRDbUpdates.Models
{
    public class MessagesRepository 
    {
        readonly string _connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
       
        public IEnumerable<Messages> GetAllMessages()
        {
            var messages = new List<Messages>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_select_All", connection))
                {
                    command.Notification = null;

                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        messages.Add(item: new Messages { ID = (int)reader["ID"], CampaignName = (string)reader["CampaignName"], MessageDate = Convert.ToDateTime(reader["Date"]), Clicks = (int)reader["Clicks"], Conversions = (int)reader["Conversions"], Impressions = (int)reader["Impressions"], AffiliateName = (string)reader["AffiliateName"] });
                    }
                }
              
            }
            return messages;
           
            
        }

        public IEnumerable<Messages> GetAllMessagesWithDelete()
        {
            var messages = new List<Messages>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_select_AllWithDelete", connection))
                {
                    command.Notification = null;

                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        messages.Add(item: new Messages { ID = (int)reader["ID"], CampaignName = (string)reader["CampaignName"], MessageDate = Convert.ToDateTime(reader["Date"]), Clicks = (int)reader["Clicks"], Conversions = (int)reader["Conversions"], Impressions = (int)reader["Impressions"], AffiliateName = (string)reader["AffiliateName"] });
                    }
                }

            }
            return messages;


        }

        public string InsertMessages(Messages objMessage)
        {
            //var messages = new List<Messages>();
            string messages = string.Empty;
           
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_Insert_Messages", connection))
                {
                    command.Notification = null;

                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);
                   SignleRUOW<Messages> objSigMessage = new SignleRUOW<Messages>();
                   objSigMessage.Add(objMessage);
                   objSigMessage.Committ();
                   if (objMessage.Status1 == "Success")
                    {
                        var hubContext = GlobalHost.ConnectionManager.GetHubContext<MessagesHub>();
                        hubContext.Clients.All.updateMessages();
                        messages = "Success";
                    }
                    else
                    {
                        messages = "Failed";
                    }
                }

            }
            return messages;

        }

        public string DeleteMessages(int GridId)
        {
            //var messages = new List<Messages>();
            string messages = string.Empty;
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_Delete_Messages", connection))
                {
                    command.Notification = null;

                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@gridId", GridId);
                    //var reader = command.ExecuteReader();
                    int reader = command.ExecuteNonQuery();
                    if (reader >= 1)
                    {
                        messages = "Success";
                        MessagesHub.SendMessages();
                        
                    }
                    else
                    {
                        messages = "Failed";
                    }
                    //while (reader.Read())
                    //{
                    //    messages.Add(item: new Messages { ID = (int)reader["ID"], CampaignName = (string)reader["CampaignName"], MessageDate = Convert.ToDateTime(reader["Date"]), Clicks = (int)reader["Clicks"], Conversions = (int)reader["Conversions"], Impressions = (int)reader["Impressions"], AffiliateName = (string)reader["AffiliateName"] });
                    //}
                }

            }
            return messages;


        }

        public Messages GetAllMessagesByID(int Id)
        {
            var messages = new Messages();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_select_All_By_Id", connection))
                {
                    command.Notification = null;

                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id", Id);
                    var reader = command.ExecuteReader();

                    //messages.Add(item: new Messages { ID = (int)reader["ID"], CampaignName = (string)reader["CampaignName"], MessageDate = Convert.ToDateTime(reader["Date"]), Clicks = (int)reader["Clicks"], Conversions = (int)reader["Conversions"], Impressions = (int)reader["Impressions"], AffiliateName = (string)reader["AffiliateName"] });
                    while (reader.Read())
                    {
                        
                        messages.ID = (int)reader["ID"];
                        messages.CampaignName = (string)reader["CampaignName"];
                        messages.MessageDate = Convert.ToDateTime(reader["Date"]);
                        messages.Clicks = (int)reader["Clicks"];
                        messages.Impressions = (int)reader["Impressions"];
                        messages.Conversions = (int)reader["Conversions"];
                        messages.AffiliateName = (string)reader["AffiliateName"];
                        MessagesHub.UpdateStatus(messages.ID, "Editing");
                    }
                }

            }
            return messages;


        }

        public string UpdateMessages(Messages objMessage)
        {
            string messages = string.Empty;
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_Update_Messages", connection))
                {
                    command.Notification = null;

                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);
                    SignleRUOW<Messages> objSigMessage = new SignleRUOW<Messages>();
                    objSigMessage.Add(objMessage);
                    objSigMessage.Committ();
                    if (objMessage.Status1 == "Success")
                    {
                        messages = "Success";
                        MessagesHub.UpdateStatus(objMessage.ID, "Updated");
                    }
                    else
                    {
                        messages = "Failed";
                    }
                }

            }
            return messages;


        }

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                MessagesHub.SendMessages();
            }
        }
    }
}