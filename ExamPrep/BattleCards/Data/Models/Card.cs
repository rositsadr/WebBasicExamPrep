using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static BattleCards.Data.DatabaseConstants;


namespace BattleCards.Data.Models
{
    public class Card
    {
        /*•	Id – an int, Primary Key
          •	Name – a string (required); min. length: 5, max. length: 15
          •	ImageUrl – a string (required)
          •	Keyword – a string (required)
          •	Attack – an int (required); cannot be negative
          •	Health – an int (required); cannot be negative
          •	Description – a string with max length 200 (required)
          •	UserCard collection
          */
        [Key]
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(cardNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Keyword { get; set; }

        [Required]
        public int Attack { get; set; }

        [Required]
        public int Health { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public ICollection<UserCard> CardUsers { get; set; } = new HashSet<UserCard>();
    }
}
