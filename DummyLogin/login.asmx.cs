using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Services;
using System.Net.Mail;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using MySql.Data.MySqlClient;

namespace DummyLogin
{
    /// <summary>
    /// Summary description for login
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/webmethods")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class login : System.Web.Services.WebService
    {
        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string getUser(user login)
        {
            Response response = new Response();

            var cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spUserLogin", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter parUsername = new SqlParameter();
            parUsername.ParameterName = "@Username";
            parUsername.Value = login.Username;
            cmd.Parameters.Add(parUsername);

            SqlParameter parPassword = new SqlParameter();
            parPassword.ParameterName = "@Password";
            parPassword.Value = login.Password;
            cmd.Parameters.Add(parPassword);

            SqlDataReader sdr = cmd.ExecuteReader();
            var harRows = sdr.HasRows;

            if (harRows)
            {
                response.Code = 200;
                response.Message = "execution was successful";
                response.Success = true;                
            }                
            else
            {
                response.Code = 400;
                response.Message = "execution was not successful";
                response.Success = false;                
            }
            return new JavaScriptSerializer().Serialize(response);
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]        
        public void sendMail(string password)
        {
            try
            {
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                smtpServer.Port = 587;
                smtpServer.Credentials = new System.Net.NetworkCredential("phahle.koketso", "200306351");
                smtpServer.EnableSsl = true;

                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("julias@delter.co.za");
                mail.To.Add("phahle.koketso@gmail.com");
                mail.Subject = "Requested Password";
                mail.Body = "Your password is: " + password;
                smtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                var errMess = ex.Message;
             
            }            
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string getPassword(user login)
        {
            Response response = new Response();

            var cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            SqlCommand cmd = new SqlCommand("spGetPassword", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter parUsername = new SqlParameter();
            parUsername.ParameterName = "@Username";
            parUsername.Value = login.Username;
            cmd.Parameters.Add(parUsername);

            SqlDataReader sdr = cmd.ExecuteReader();

            string pswd = null;
            while (sdr.Read())
            {
                pswd = sdr["Password"].ToString();
            }
            
            if (pswd != null)
            {
                response.Code = 200;
                response.Message = pswd;
                response.Success = true;

                sendMail(pswd);
            }
            else
            {
                response.Code = 400;
                response.Message = "Please provide valid username";
                response.Success = false;
            }
            
            return new JavaScriptSerializer().Serialize(response);
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void connectDB()
        {
            DBConnect connect = new DBConnect();
            connect.Insert();
        }
    }
}
