using MauiMvvmSample.GameMauiObjects;
using MauiMvvmSample.ViewModels;
namespace MauiMvvmSample.Views;

public partial class GamePage : ContentPage
{
	public GamePage(GameViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        this.SizeChanged += UpdateSizes;
        this.Loaded += UpdateSizes;
	}

    private async void UpdateSizes(object? sender, EventArgs e)
    {
        await Task.Delay(200);
        gameplayVisual.WidthRequest = gameplayVisual.Height; //we want for the drawing area to be a square
        //we want for the buttons to be square so for their containers to be square
        movementGrid.HeightRequest = movementGrid.Width;
        shootingGrid.HeightRequest = shootingGrid.Width;
    }
}