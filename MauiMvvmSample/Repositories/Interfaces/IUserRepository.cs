using MauiMvvmSample.Models;

namespace MauiMvvmSample.Repositories.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByUsernameAsync(string username);

    Task UpdateAsync(User user);
}

