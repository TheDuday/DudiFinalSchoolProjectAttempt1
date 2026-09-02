using Microsoft.Maui.Controls;

namespace MauiMvvmSample;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm; // Set ViewModel from DI (we register it in MauiProgram) הסבר בהמשך
    }
}

