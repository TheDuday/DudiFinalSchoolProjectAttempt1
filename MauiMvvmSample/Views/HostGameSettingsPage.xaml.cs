using MauiMvvmSample.ViewModels;

namespace MauiMvvmSample.Views;

public partial class HostGameSettingsPage : ContentPage
{
	public HostGameSettingsPage(HostGameSettingsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}