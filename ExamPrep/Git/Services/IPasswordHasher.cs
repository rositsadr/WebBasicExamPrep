namespace Git.Services
{
    public interface IPasswordHasher
    {
        string HashedPassword(string password);
    }
}
