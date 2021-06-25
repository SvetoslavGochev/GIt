namespace Git.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static Git.Data.DataConstants;

    public class Commit
    {
        [Key]
        [Required]
        [MaxLength(IdMaxLength)]
        public string Id { get; init; } = Guid.NewGuid().ToString();


        [Required]
        public string Desscription { get; init; }


        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;

        [Required]
        public string CreatorId { get; init; } 

        public User Creator { get; set; }

        [Required]
        public string RepositoryId { get; set; }

        public Repository Repository { get; set; }





    }
}
