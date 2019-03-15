namespace Insql
{
    public interface IDbSessionFactory
    {
        IDbSession CreateSession();
    }
}
