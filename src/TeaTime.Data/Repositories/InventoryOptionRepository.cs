namespace TeaTime.Data.Repositories
{
    using Contracts.Data;
    using Contracts.Data.Repositories;

    public class InventoryOptionRepository : BaseRepository, IInventoryOptionRepository
    {
        public InventoryOptionRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}