using CommunityToolkit.Maui;
using MauiMvvmSample.Repositories;
using MauiMvvmSample.Repositories.Interfaces;
using MauiMvvmSample.Services;
using MauiMvvmSample.ViewModels;
using MauiMvvmSample.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui;
namespace MauiMvvmSample
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {

          

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.UseMauiApp<App>().UseMauiCommunityToolkit();

            // Register pages and viewmodels in DI
            
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginViewModel>();

            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<RegisterViewModel>();

            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<HomeViewModel>();

           
            
            builder.Services.AddSingleton<IAuthService, FirebaseAuthService>();
            
            builder.Services.AddSingleton<IUserRepository, FirebaseUserRepository>();   

            builder.Services.AddSingleton<HttpClient>();
           




            return builder.Build();
        }
    }
}
