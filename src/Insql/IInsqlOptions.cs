namespace Insql
{
    public interface IInsqlOptions
    {
        TExtension FindExtension<TExtension>() where TExtension : class, IInsqlOptionsExtension;
    }
}