using MauiMvvmSample.ViewModels;

namespace MauiMvvmSample.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomeViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
