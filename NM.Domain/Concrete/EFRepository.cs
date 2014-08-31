namespace NM.Domain.Concrete {
    public abstract class EFRepository {
        internal static readonly EFDbContext Context = new EFDbContext();
    }
}