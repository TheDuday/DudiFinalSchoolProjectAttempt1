using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;

namespace MauiMvvmSample;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string userInput;

    [ObservableProperty]
    private string resultText;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private int counter;

    // CanSubmit נבנה כש־property חישובי או כתוצאה משינוי
    public bool CanSubmit => !string.IsNullOrWhiteSpace(UserInput) && !IsBusy;

    public MainViewModel()  //פעולה בונה
    {
        Counter = 0;
        UserInput = string.Empty;
        ResultText = "Ready";
        // כשמשתנה UserInput או IsBusy אנחנו רוצים להודיע על שינוי של CanSubmit
        // אפשר להאזין ל־PropertyChanged כדי להפעיל את NotifyPropertyChanged של CanSubmit
        PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(UserInput) || e.PropertyName == nameof(IsBusy))
            {
                OnPropertyChanged(nameof(CanSubmit));
            }
        };
    }

    // RelayCommand יוצר ICommand אוטומטית
    [RelayCommand]
    private async Task SubmitAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        try
        {
            // סימולציה של פעולה אסינכרונית (לדוגמה: קריאת API)
            await Task.Delay(1000);

            ResultText = $"נשלח: {UserInput} (זמן: {DateTime.Now:T})";

            // איפוס קלט
            UserInput = string.Empty;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void Increment()
    {
        Counter++;
    }
}
