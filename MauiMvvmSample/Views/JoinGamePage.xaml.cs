namespace MauiMvvmSample.Views;

public partial class JoinGamePage : ContentPage
{
	public JoinGamePage(JoinGameViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}