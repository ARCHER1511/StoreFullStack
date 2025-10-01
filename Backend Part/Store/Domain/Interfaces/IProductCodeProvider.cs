namespace Domain.Interfaces
{
    public interface IProductCodeProvider
    {
        Task<int> GetCurrentMaxNumberAsync();
        Task<int?> GetNextAvailableNumberAsync();
    }
}
