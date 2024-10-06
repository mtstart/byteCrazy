using byteCrazy.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using static byteCrazy.Controllers.ManageController;

public class UsersController : Controller
{
    private BtyeCrazy db = new BtyeCrazy();

    // GET: /Users/
    public ActionResult Index()
    {

        ViewData["Greeting"] = "Hello";

        return View();
    }

    // returns a RedirectToActionResult that redirects incoming request to the Index() action
    // Sample 1 for getting data by DbContext
    // http://localhost:44344/Users/Details?id=U0001
    public ActionResult Details(string id)
    {
        if (id != null)
        {
            Users user2 = (from item in db.Users where item.UserID == id select item).FirstOrDefault();

            return View(user2);
        } else
        {
            return View("Error");
        }
    }

    private List<Users> GetUsers()
    {
        // change the datasource from webconfig to your own if necessary
        string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connectionString))
        {

            List<Users> users = new List<Users>();
            string query = "select * from Users";

            // for insert/ update/ delete
            //using (SqlCommand cmd = new SqlCommand(query))
            //{
            //    cmd.Connection = con;
            //    con.Open();

            //    // if with parameters
            //    cmd.Parameters.AddWithValue("@firstName", "put value here");

            //    string status = (cmd.ExecuteNonQuery() >= 1) ? "get failed" : "get successed";
            //}

            // for select
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = con;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        users.Add(
                            new Users(
                            reader["userID"].ToString(), 
                            reader["FirstName"].ToString(), 
                            reader["LastName"].ToString(), 
                            reader["StudentNumber"].ToString(), 
                            reader["location"].ToString(), 
                            reader["Password"].ToString(), 
                            (DateTime)reader["createdDate"]
                            )
                        );
                    }

                }

            };


            // closing
            con.Close();

            ViewData["UserList"] = users;
            return users;

        }

    }

    // Sample 2 for getting data by new connection
    public ActionResult SampleUser()
    {
        return View(GetUsers().FirstOrDefault());
    }

}