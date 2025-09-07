using System.Collections.Generic;
using App.Domain.Models;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Transformation;
using Microsoft.VisualBasic;

namespace App.Windows;

public partial class MainWindow : Window
{
    public List<Chips> Chips { get; set; } =
    [
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
        new()
    ];

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button { Content: Panel panel } ) return;
        
        panel.ZIndex = 100;
        panel.Children[1].RenderTransform = TransformOperations.Parse("translate(0px, 200px)");
    }
}