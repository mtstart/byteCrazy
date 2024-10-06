using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Web.Configuration;

namespace byteCrazy.Models
{
    public class Users
    {
        [Key] public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string StudentNumber { get; set; }
        public string Location { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }

        public Users() { }

        public Users(string userID, string firstName, string lastName, string studentNumber, string location, String password, DateTime createdDate)
        {
            UserID = userID;
            FirstName = firstName;
            LastName = lastName;
            StudentNumber = studentNumber;
            Location = location;
            Password = password;
            CreatedDate = createdDate;
        }
    }

    public class UserList
    {
        public List<Users> Users { get; set; }
    }

    public class BtyeCrazy : DbContext
    {
        public BtyeCrazy()
        {
            // change the datasource from webconfig to your own if necessary
            this.Database.Connection.ConnectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public DbSet<Users> Users{ get; set; }
    }
}