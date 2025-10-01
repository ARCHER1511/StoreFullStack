using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {   
        Task<User?> GetByUserNameAsync(string userName);
        Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate);
    }
}
