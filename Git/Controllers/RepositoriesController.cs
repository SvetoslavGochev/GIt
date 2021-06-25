namespace Git.Controllers
{
    using Git.Data;
    using Git.Data.Models;
    using Git.Models.Repositories;
    using Git.Services;
    using MyWebServer.Controllers;
    using MyWebServer.Http;
    using System.Linq;
    using static Data.DataConstants;
    public class RepositoriesController : Controller
    {
        private readonly GitDbContext data;
        private readonly IValidator validator;

        public RepositoriesController(GitDbContext data, IValidator validator)
        {
            this.data = data;
            this.validator = validator;
        }

        public HttpResponse All()
        {
            var repositories = this.data
                .Repositories
                .Where(r => r.IsPublic)
                .Select(r => new RepositoryListingViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Owner = r.Owner.Username,
                    CreatedOn = r.CreatedOn.ToLocalTime().ToString("R"),
                    Commits = r.Commits.Count()

                })
                .ToList();//vzima vsi4ki pubkli4ni


            return View(repositories);
            //da vijda vsi4ki + negovite private
            //var repositoriesQuery = this.data.Repositories.AsQueryable();


            //if (this.User.IsAuthenticated)//ako e 
            //{
            //    repositoriesQuery = repositoriesQuery
            //        .Where(r => r.IsPublic || r.OwnerId == this.User.Id);
            //    //ili e publi4no ili ownera savpada
            //}
            //else
            //{
            //    repositoriesQuery = repositoriesQuery.Where(r => r.IsPublic);
            //    //vzimame vsi4ki publi4ni
            //}

        }
        [Authorize]
        public HttpResponse Create() => View();

        [HttpPost]
        [Authorize]
        public HttpResponse Create(CreateRepositoryFormModel model)
        {
            var modelErrors = this.validator.ValidateRepository(model);

            if (modelErrors.Any())
            {
                return Error(modelErrors);
            }

            var repository = new Repository
            {
                Name = model.Name,
                IsPublic = model.RepositoryType == RepositoryPublicType,
                OwnerId = this.User.Id
            };

            this.data.Repositories.Add(repository);

            this.data.SaveChanges();

            return Redirect("/Repositories/All");
        }


    }
}
