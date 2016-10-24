using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Awlog.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Awlog.Controllers
{
    public class BlogController : Controller
    {
        // GET: /<controller>/

        private readonly IDataLayer _db;

        public BlogController(IDataLayer db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var token = GetToken();

            return View(token);
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegForm form)
        {
            try
            {
                int id = _db.Registration(form);
                Guid token = _db.GetAuthToken(id);
                SetToken(token);
                return RedirectToAction("Index", "Blog");
            }
            catch(Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public Guid? GetToken()
        {
            if (Request.Cookies["auth_token"] != null)
            {
                return Guid.Parse(Request.Cookies["auth_token"]);
            }
            return null;
        }

        public void SetToken(Guid token)
        {
            Response.Cookies.Append("auth_token", token.ToString());
        }
    }
}
