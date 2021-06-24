using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static BattleCards.Data.DatabaseConstants;

namespace BattleCards.Data.Models
{
    public class User
    {
        /*•	Has an Id – a string, Primary Key
          •	Username – a string with min length 5 and max length 20 (required)
          •	Email - a string (required)
          •	Password – a string with min length 6 and max length 20  - hashed in the database   (required)
          •	UserCard collection
          */
        [Key]
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(UsernameMaxLength)]
        public string Username { get; set; }

        [Required]
        [MaxLength(PasswordMaxLength)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public ICollection<UserCard> UserCards { get; set; } = new HashSet<UserCard>();
    }
}
