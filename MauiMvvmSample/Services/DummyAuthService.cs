using MauiMvvmSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMvvmSample.Services
{
    public class DummyAuthService : IAuthService // Dummy implementation for testing only
    {
        public Task<User?> LoginAsync(string u, string p)
            => Task.FromResult<User?>(null);

        public Task RegisterAsync(string u, string e, string ph, string p)
            => Task.CompletedTask;

        
    }

}
