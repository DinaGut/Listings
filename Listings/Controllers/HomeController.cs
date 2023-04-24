using Data;
using Listings.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;

namespace Listings.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ItemsOffering;Integrated Security=true;";

        public IActionResult Index()
        {
            
            var iom = new ItemsOfferedManager(_connectionString);

            HomeViewModel hvm = new();
           
            
            var repo = new UserRepository(_connectionString);

           
         
            if (User.Identity.IsAuthenticated)
            {
                var currentUserEmail = User.Identity.Name;
                hvm.User = repo.GetByEmail(currentUserEmail);
            };

            hvm.Items = iom.GetAllItems();





            return View(hvm);
        }
        [Authorize]
        public IActionResult NewAd()
        {

            return View();
        }
        [HttpPost]
        public IActionResult NewAd(ItemOffered item)
        {
            ItemsOfferedManager iom = new(_connectionString);
            var repo = new UserRepository(_connectionString);
            var UserEmail = User.Identity.Name;
            
            item.DatePosted = DateTime.Now;
            User user = repo.GetByEmail(UserEmail);
           
           
            iom.AddItem(item, user.Id,user.Name);
           

            return Redirect("index");
        }
        [Authorize]
        public IActionResult MyAccount()
        {
          
            var iom = new ItemsOfferedManager(_connectionString);
            var repo = new UserRepository(_connectionString);
            var UserEmail = User.Identity.Name;
            User user = repo.GetByEmail(UserEmail);
            var hvm = new HomeViewModel
            {
                Items = iom.GetAdsForCurrentUser(user.Id)
            };


            return View(hvm);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var iom = new ItemsOfferedManager(_connectionString);
            iom.Delete(id);
            return Redirect("/home/MyAccount");
        }

    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonSerializer.Deserialize<T>(value);
        }
    }
}