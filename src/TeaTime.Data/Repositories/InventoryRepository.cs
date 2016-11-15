namespace TeaTime.Data.Repositories
{
    using Contracts.Data;
    using Contracts.Data.Repository;

    public class InventoryRepository : BaseRepository, IInventoryRepository
    {
        public InventoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}