using BooksAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BooksAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        // GET api/values
        public string Get(int userid)
        {
            using (SampleDbEntities entities = new SampleDbEntities())
            {
                User activateUser = entities.Users.Where(a => a.Id == userid).FirstOrDefault();
                if(activateUser != null)
                {
                    activateUser.Active = "y";                   
                    entities.SaveChanges();
                }
            }

            return string.Empty;
        }


        // GET api/values/
        public HttpResponseMessage Get(string email, string password)
        {
            try
            {

                using (SampleDbEntities entities = new SampleDbEntities())
                {
                
                    List<User> UsersList = entities.Users.ToList<User>();
                    User loginUser = UsersList.Where(u => u.Email == email && u.Password == password).FirstOrDefault();
                    if (loginUser == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Username or password is incorrect");
                    }
                    else if (loginUser.Active.Trim() == "y")
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "Success-" + loginUser.Username);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Please confirm your email. ");
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        
        
        }


        // POST api/values
        public HttpResponseMessage Post(User value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (SampleDbEntities entities = new SampleDbEntities())
                    {
                        if (CheckForUsernameAlreadyExists(value, entities))
                        {
                            string error = string.Format("User with {0} username already exist", value.Username);
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
                        }
                        else if (CheckForEmailAlreadyExists(value, entities))
                        {
                            string error = string.Format("User with {0} email already exist", value.Email);
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
                        }
                        else
                        {
                            value.Active = "n";
                            entities.Users.Add(value);
                            entities.SaveChanges();
                            SendActiveLinkToUser(value);
                            return Request.CreateResponse(HttpStatusCode.OK, "Success");
                        
                        }
                    }   
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Model state is invalid");
                }
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Some error occrued in registration" );
            }
        }

        private bool CheckForUsernameAlreadyExists(User user, SampleDbEntities entities)
        {

            bool isExists = false;
            try
            {
                List<User> UsersList = entities.Users.ToList<User>();
                User loginUser = UsersList.Where(u => u.Username == user.Username).FirstOrDefault();

                if (loginUser == null)
                    isExists = false;
                else
                    isExists = true;

            }
            catch (Exception)
            {
                isExists = true;
            }

            return isExists;
        }

        private bool CheckForEmailAlreadyExists(User user, SampleDbEntities entities)
        {

            bool isExists = false;
            try
            {
                List<User> UsersList = entities.Users.ToList<User>();
                User loginUser = UsersList.Where(u => u.Email == user.Email).FirstOrDefault();

                if (loginUser == null)
                    isExists = false;
                else
                    isExists = true;

            }
            catch (Exception)
            {
                isExists = true;
            }

            return isExists;
        }


        private void SendActiveLinkToUser(User user)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("test123rao@gmail.com");
            mail.To.Add(user.Email);
            mail.Subject = "Email Confirmation";
            mail.Body = GetMessageBody(user.Username,user.Id);
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpServer.Port = 587;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential("test123rao@gmail.com", "p@ssw0rd000");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }
        
        private string GetMessageBody(string username,int userid)
        {
            string body = string.Empty;

            string url = "http://localhost:63342/AngularLogin-Rao/index.html#/activate/user:" + userid;

            body = "<html>\n" +
                "\n" +
                "<h2>Hi " + username + "\n" +
                "\t<body>\n" +
                "\t\tPlease click on below link to activate email\n" +
                "\t\t<a href=\'" + url + "'>click here</a>\n" +
                "\t</body>\n" +
                "</html>";
            return body;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
