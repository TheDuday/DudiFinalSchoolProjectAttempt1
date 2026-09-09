using CommunityToolkit.Mvvm.ComponentModel;

namespace MyApp.ViewModels.Base;

public abstract partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    bool isBusy;
}