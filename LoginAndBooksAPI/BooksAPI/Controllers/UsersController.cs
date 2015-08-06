using BooksAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Linq;

namespace BooksAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        // GET api/values
        public IEnumerable<User> Get()
        {
            using (SampleDbEntities entities = new SampleDbEntities())
            {
                return entities.Users.ToList<User>();
            }
        }

        // GET api/values/
        public HttpResponseMessage Get(string username, string password)
        {
            try
            {

                using (SampleDbEntities entities = new SampleDbEntities())
                {
                    List<User> UsersList = entities.Users.ToList<User>();
                    User loginUser = UsersList.Where(u => u.Username == username && u.Password == password).FirstOrDefault();
                    if (loginUser == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Username or password is incorrect");
                    }

                    return Request.CreateResponse(HttpStatusCode.OK,"Success");
                }
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
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
                        if (CheckForAlreadyExists(value.Username, entities) == false)
                        {
                            entities.Users.Add(value);
                            entities.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, "Success");
                        }
                        else
                        {
                            string error = string.Format("User with {0} username already exist", value.Username);
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
                        }
                    }   
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Model state is invalid");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private bool CheckForAlreadyExists(string username, SampleDbEntities entities)
        {

            bool isExists = false;
            try
            {
                List<User> UsersList = entities.Users.ToList<User>();
                User loginUser = UsersList.Where(u => u.Username == username).FirstOrDefault();

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
