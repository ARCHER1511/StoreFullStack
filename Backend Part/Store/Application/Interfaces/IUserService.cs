using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetByUserNameAsync(string userName);
    }
}
