using System.ComponentModel.DataAnnotations;

namespace BattleCards.Data.Models
{
    public class UserCard
    {
        /* •	UserId – a string
           •	User – a User object
           •	CardId – an int
           •	Card – a Card object
        */
        [Required]
        public string UserId { get; init; }

        public User User { get; init; }

        [Required]
        public string CardId { get; init; }

        public Card Card { get; init; }
    }
}