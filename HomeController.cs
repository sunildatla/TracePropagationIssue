using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAppForLogs.Models;

namespace WebAppForLogs.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var activity = new Activity("StartingIindex");
            activity.Start();
            _logger.LogInformation("I am in {viewname} View and {trace}", "Index",Activity.Current?.Id);

            HttpClient httpClient = new HttpClient();
           HttpResponseMessage responseMessage= httpClient.GetAsync("https://api.publicapis.org/entries").GetAwaiter().GetResult();
            var response=responseMessage.Content.ReadAsStringAsync().Result;

            return RedirectToAction("Privacy");
            activity.Stop();
    
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation("I am in {viewname} View", "Privacy");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}