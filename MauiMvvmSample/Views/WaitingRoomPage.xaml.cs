using MauiMvvmSample.ViewModels;

namespace MauiMvvmSample.Views;

public partial class WaitingRoomPage : ContentPage
{
	public WaitingRoomPage(WaitingRoomViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

        this.NavigatedFrom += async (sender, args) =>
        {
            await vm.OnNavigatedFromAsync();
        };
    }
}