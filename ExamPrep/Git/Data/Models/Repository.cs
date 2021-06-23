using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Git.Data.DataConstants;

namespace Git.Data.Models
{
    public class Repository
    {
        /*•	Has an Id – a string, Primary Key
          •	Has a Name – a string with min length 3 and max length 10 (required)
          •	Has a CreatedOn – a datetime (required)
          •	Has a IsPublic – bool (required)
          •	Has a OwnerId – a string
          •	Has a Owner – a User object
          •	Has Commits collection – a Commit type
            */
        public Repository()
        {
            this.Commits = new HashSet<Commit>();
        }

        [Key]
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(RepositoryMaxLength)]
        public string Name { get; set; }

        public DateTime CreatedOn { get; init; }

        public bool IsPublic { get; set; }

        public string OwnerId { get; set; }

        public User Owner { get; set; }

        public ICollection<Commit> Commits { get; init; }

    }
}