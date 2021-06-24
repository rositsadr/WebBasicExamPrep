namespace BattleCards.Models.Card
{
    public class AllCardModel
    {
        public string Id { get; init; }
        public string Name { get; init; }

        public string ImageUrl { get; init; }

        public string Keyword { get; init; }

        public string Description { get; init; }

        public int Attack { get; init; }

        public int Health { get; init; }
    }
}
