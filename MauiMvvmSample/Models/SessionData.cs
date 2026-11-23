using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace MauiMvvmSample.Models;

public class SessionData
{
    public User? user { get; set; }
    public GameData? gameData { get; set; }
}