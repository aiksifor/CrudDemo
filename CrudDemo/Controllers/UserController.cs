using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrudDemo.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


// Backend Database -> UserDb with 1 table UserTable ( sqlserver localdb 2019 )
// UserDb.mdf inside App_Data
// Controller class uses ADO.NET to interact with database.

namespace CrudDemo.Controllers
{
    public class UserController : Controller
    {
        // ADO.NET components

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["userdbcon"].ConnectionString);
        SqlParameter  sqlParam;
        SqlCommand    sqlCmd;
        SqlDataReader sdr;





        // GET: User  ( default controller method )   List all user data from database
        public ActionResult Index()
        {
            var userList = new List<User>();

            sqlCmd  = new SqlCommand("select * from UserTable", sqlCon);
            sqlCon.Open();
            sdr = sqlCmd.ExecuteReader();

            while (sdr.Read())
            {
                User tempUser = new User();

                tempUser.Id = int.Parse(sdr[0].ToString());
                tempUser.Name = sdr[1].ToString();
                tempUser.Email = sdr[2].ToString();
                userList.Add(tempUser);
            }

            sdr.Close();
            sqlCon.Close();

            return View(userList);

        } // end of method Index().





        // returns the view to create new user
        public ActionResult Create()
        {
            return View();
        }  // end of method Create.


        // create new user and show all user's data list
        [HttpPost]
        public ActionResult Create(User user)
        {
            String sql;    // hold sql query string
            int newId;     // holds newly created Id. Ad Id is not auto incremented in backend database
            
            sqlCmd = new SqlCommand("", sqlCon);
            sqlCon.Open();

            // getting max id and create new one based on it ( by incrementing 1 )
            sqlCmd.CommandText = "select isnull(max(id),0) from userTable";
            newId = (int)(sqlCmd.ExecuteScalar()) + 1;


            sql = string.Format("insert into userTable values ({0},@name,@email)", newId);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@Name";
            sqlParam.Value = user.Name;
            sqlParam.SqlDbType = SqlDbType.VarChar;
            sqlParam.Size = 50;
            sqlCmd.Parameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@email";
            sqlParam.Value = user.Email;
            sqlParam.SqlDbType = SqlDbType.VarChar;
            sqlParam.Size = 50;
            sqlCmd.Parameters.Add(sqlParam);

            sqlCmd.CommandText = sql;
            sqlCmd.ExecuteNonQuery();
            sqlCon.Close();
            return RedirectToAction("Index");
        }  // end of method Create POST.





        // Returns the selected user data to show on Edit screen
        public ActionResult Edit(int Id)
        {

            sqlCmd = new SqlCommand("select * from UserTable where Id = " + Id, sqlCon);
            sqlCon.Open();
            sdr = sqlCmd.ExecuteReader();
            sdr.Read();    // select first record by default  
           
            User tempUser = new User();
            tempUser.Id = int.Parse(sdr[0].ToString());
            tempUser.Name = sdr[1].ToString();
            tempUser.Email = sdr[2].ToString();
            
            sdr.Close();
            sqlCon.Close();

            return View(tempUser);
        }  // end of method Edit().



        // update user data and show all user's data list
        [HttpPost]
        public ActionResult Edit(User user)
        {
            String sql;    // hold sql query string

            sqlCmd = new SqlCommand("", sqlCon);

            sql = string.Format("update userTable set name = @name,email = @email where Id = {0}",user.Id);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@Name";
            sqlParam.Value = user.Name;
            sqlParam.SqlDbType = SqlDbType.VarChar;
            sqlParam.Size = 50;
            sqlCmd.Parameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@email";
            sqlParam.Value = user.Email;
            sqlParam.SqlDbType = SqlDbType.VarChar;
            sqlParam.Size = 50;
            sqlCmd.Parameters.Add(sqlParam);

            sqlCmd.CommandText = sql;
            sqlCon.Open();
            sqlCmd.ExecuteNonQuery();
            sqlCon.Close();
            return RedirectToAction("Index");
        }  // end of method Edit() POST.






        // Returns the selected user data to show on Delete screen
        public ActionResult Delete(int Id)
        {
            sqlCmd = new SqlCommand("select * from UserTable where Id = " + Id, sqlCon);
            sqlCon.Open();
            sdr = sqlCmd.ExecuteReader();
            sdr.Read();

            User tempUser = new User();
            tempUser.Id = int.Parse(sdr[0].ToString());
            tempUser.Name = sdr[1].ToString();
            tempUser.Email = sdr[2].ToString();

            sdr.Close();
            sqlCon.Close();


            return View(tempUser);
        }  // end of method Delete.


        // delete user data and show all user's data list
        [HttpPost]
        public ActionResult Delete(User user)
        {
            String sql;    // hold sql query string

            sqlCmd = new SqlCommand("", sqlCon);
            
            sql = string.Format("delete from userTable where Id = {0}",user.Id);

            sqlCmd.CommandText = sql;
            sqlCon.Open();
            sqlCmd.ExecuteNonQuery();
            sqlCon.Close();
            return RedirectToAction("Index");
        }  // end of method Delete POST.






    }
}