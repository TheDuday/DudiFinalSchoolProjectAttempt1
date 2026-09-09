using MauiMvvmSample.Models;

namespace MauiMvvmSample.Repositories.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByIdAsync(string id);
    Task UpdateAsync(User user);
}

