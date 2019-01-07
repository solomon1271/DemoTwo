using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using System.Web.Http.Cors;
using EmployeeDataAccess;

namespace EmployeeService.Controllers
{
    //[EnableCorsAttribute("*", "*", "*")]
    public class EmployeesController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage LoadAllEmployees(string gender = "All")
        {
            using (EmployeeDB db = new EmployeeDB())

            {
                switch (gender.ToLower())
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK, db.Employees.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK, db.Employees.Where(x => x.Gender == gender).ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK, db.Employees.Where(x => x.Gender == gender).ToList());
                    default:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid gender detected!");
                }
            }
        }
        [HttpGet]
        public HttpResponseMessage LoadEmployeeWithID(int id)
        {
            try
            {
                using (EmployeeDB db = new EmployeeDB())
                {
                    var entity = db.Employees.FirstOrDefault(x => x.ID == id);
                    if (entity != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The Employee with ID " + id.ToString() + " does not exist in the database.");
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public HttpResponseMessage Post([FromBody] Employee emp)
        {
            try
            {
                using (EmployeeDB db = new EmployeeDB())
                {
                    db.Employees.Add(emp);
                    db.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, emp);
                    message.Headers.Location = new Uri(Request.RequestUri + "/" + emp.ID.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (EmployeeDB db = new EmployeeDB())
                {
                    var emp = db.Employees.FirstOrDefault(x => x.ID == id);
                    if (emp != null)
                    {
                        db.Employees.Remove(emp);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "The Employee with ID " + id.ToString() + " is deleted from the database.");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The Employee with ID " + id.ToString() + " does not exist in the database");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Put([FromBody] Employee emp)
        {
            try
            {
                using (EmployeeDB db = new EmployeeDB())
                {
                    var employee = db.Employees.FirstOrDefault(x => x.ID == emp.ID);
                    if (employee == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The Employee is not found");
                    }
                    else
                    {
                        employee.FirstName = emp.FirstName;
                        employee.LastName = emp.LastName;
                        employee.Gender = emp.Gender;
                        employee.Salary = emp.Salary;

                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "The Employee with ID " + emp.ID.ToString() + " is modified successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex); ;
            }
        }
    }
}
