using MauiMvvmSample.ViewModels;

namespace MauiMvvmSample.Views;

public partial class TutorialPage : ContentPage
{
	public TutorialPage(TutorialViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}