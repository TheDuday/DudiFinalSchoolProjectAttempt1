using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMvvmSample.Models;
using MauiMvvmSample.Repositories.Interfaces;
using MauiMvvmSample.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMvvmSample.ViewModels;

public partial class TutorialViewModel : ObservableObject
{
    public TutorialViewModel()
    {

    }
    [ObservableProperty]
    private string tutorialString = "\nrules of the game:" +
        "\n\n 1. the game takes place on a board. each turn you may move using the 8 movement buttons or you may shoot bullets that move each turn with speeds 1 or 2 in a certain direction using the 16 shooting buttons." +
        "\n\n 2. the host chooses the game settings, in which an amount of walls is chosen, you may not shoot through walls or walk into them." +
        "\n\n 3. each player starts with a certain amount of health according to the game settings chosen, the winner is the one whos opponent reaches 0 health first." +
        "\n\n 4. the host player will be identified on the game board by his blue shirt and yellow pants, and the other player will be identified by his red shirt and green pants";
}
