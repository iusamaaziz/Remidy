namespace Remidy.Data
{
    public class LookupRepositoryFactory(IServiceProvider serviceProvider)
    {
        public LookupRepository<T> GetRepository<T>() where T : ILookup
        {
            return serviceProvider.GetRequiredService<LookupRepository<T>>();
        }
    }
}
