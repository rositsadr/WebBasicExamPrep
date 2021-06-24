namespace BattleCards.Services
{
    public interface IHashPassword
    {
        string Hash(string password);
    }
}
