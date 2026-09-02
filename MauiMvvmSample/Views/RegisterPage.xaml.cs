using MauiMvvmSample.ViewModels;

namespace MauiMvvmSample.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel vm)
    {
		InitializeComponent();
        BindingContext = vm;
    }
}