using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Git.Data.DataConstants;

namespace Git.Data.Models
{
    public class User
    {
        /*•	Id – a string, Primary Key
          •	Username – a string with min length 5 and max length 20 (required)
          •	Has an Email - a string (required)
          •	Has a Password – a string with min length 6 and max length 20  - hashed in the database(required)
          •	Has Repositories collection – a Repository type
          •	Has Commits collection – a Commit type
        */
        public User()
        {
            this.Repositories = new HashSet<Repository>();
            this.Commits = new HashSet<Commit>();
        }

        [Key]
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(UserMaxLength)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public ICollection<Repository> Repositories { get; init; }

        public ICollection<Commit> Commits { get; init; }
    }
}
