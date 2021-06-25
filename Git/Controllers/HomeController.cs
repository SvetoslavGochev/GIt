namespace Git.Controllers
{
    using MyWebServer.Controllers;
    using MyWebServer.Http;

    public class HomeController : Controller
    {
        public HttpResponse Index()
        {
            if (true)
            {
                if (this.User.IsAuthenticated)
                {
                    return Redirect("/Repositories/All");
                }
            }

            return View();

        }
    }
}
