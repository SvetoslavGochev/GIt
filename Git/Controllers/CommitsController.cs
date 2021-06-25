namespace Git.Controllers
{
    using Git.Data;
    using Git.Data.Models;
    using Git.Models.Commits;
    using MyWebServer.Controllers;
    using MyWebServer.Http;
    using System.Linq;
    using static Data.DataConstants;
    public class CommitsController : Controller
    {
        private readonly GitDbContext data;

        public CommitsController(GitDbContext data)
        {
            this.data = data;
        }

        [Authorize]
        public HttpResponse Create(string Id)
        {
            //id na repositorito

            var repository = this.data
               .Repositories
               .Where(r => r.Id == Id)
               .Select(r => new CommitToRepositoryViewModel
               {
                   Id = r.Id,
                   Name = r.Name
               })
               .FirstOrDefault();

            if (repository == null)
            {
                return BadRequest();
            }

            return View(repository);

        }
        [Authorize]
        public HttpResponse All()
        {
            var commits = this.data
                .Commits
                .Where(c => c.CreatorId == this.User.Id)
                .OrderByDescending(c => c.CreatedOn)
                .Select(c => new CommitListigViewModel
                {
                    Id = c.Id,
                    Description = c.Desscription,
                    CreatedOn = c.CreatedOn.ToLocalTime().ToString("F"),
                    Repository = c.Repository.Name
                })
                .ToList();

            return View(commits);
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Create(CreateCommitFormModel model)
        {

            

            if (!this.data.Repositories.Any(r => r.Id == model.Id))
            {
                return BadRequest();
            }

            if (model.Description.Length < CommitMinDescription)
            {
                return Error("Commit length is wrong.");
            }


            var commit = new Commit
            {
                
                RepositoryId = model.Id,
                Desscription = model.Description,
                CreatorId = this.User.Id
            };

            this.data.Commits.Add(commit);
            this.data.SaveChanges();

            return Redirect("/Repositories/All");
        }

        [Authorize]
        public HttpResponse Delete(string Id)
        {
            var commit = this.data.Commits.Find(Id);

            if (commit == null || commit.CreatorId != this.User.Id)
            {
                return Error("nULL");
            }


            this.data.Commits.Remove(commit);
            this.data.SaveChanges();

            return Redirect("/Commits/All");

        }

    }
}
