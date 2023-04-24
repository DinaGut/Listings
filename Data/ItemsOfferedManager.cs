using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ItemsOfferedManager
    {
        public string _connectionString;
        public ItemsOfferedManager(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<ItemOffered> GetAllItems()
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT * FROM ItemInfo ORDER BY DatePosted DESC";
            var items = new List<ItemOffered>();
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new()
                {
                    Id = (int)reader["Id"],
                    Name = reader.GetOrNull<string>("Name"),
                    PhoneNumber = (int)reader["PhoneNumber"],
                    DatePosted = (DateTime)reader["DatePosted"],
                    Description = (string)reader["Description"],
                    UserId =reader.GetOrNull<int>("UserId")
                });
            }

            return items;
        }
        public void AddItem(ItemOffered item, int userId,string name)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
          
      
               

                cmd.CommandText = @"INSERT into ItemInfo(Name, DatePosted, Description,PhoneNumber, UserId)
                                values(@name, @datePosted, @description, @phoneNumber, @userId)";
            
        cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@description", item.Description);
            cmd.Parameters.AddWithValue("@phonenumber", item.PhoneNumber);
            cmd.Parameters.AddWithValue("@dateposted", DateTime.Now);
            cmd.Parameters.AddWithValue("@userId", userId);
            connection.Open();

            cmd.ExecuteNonQuery();
        }
        public List<ItemOffered> GetAdsForCurrentUser(int userId)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM ItemInfo WHERE UserId = @userId ORDER BY DatePosted DESC";
            command.Parameters.AddWithValue("@userId", userId);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            var Ads = new List<ItemOffered>();
            while (reader.Read())
            {
                Ads.Add(new ItemOffered
                {
                    Id = (int)reader["id"],
                    Description = (string)reader["description"],
                    PhoneNumber = (int)reader["phoneNumber"],
                    DatePosted = (DateTime)reader["datePosted"],
                    UserId = (int)reader["userId"]
                });
            }
            return Ads;
        }

        public void Delete(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM ItemInfo  " +
                                  "WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            cmd.ExecuteNonQuery();
        }



    }
    public static class Extensions
    {
        public static T GetOrNull<T>(this SqlDataReader reader, string column)
        {
            object value = reader[column];
            if (value == DBNull.Value)
            {
                return default(T);
            }

            return (T)value;
        }
    }

}
