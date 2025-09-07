using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using App.Domain.Models;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Transformation;
using Microsoft.VisualBasic;

namespace App.Windows;

public partial class MainWindow : Window
{
    private const double DoorY = 485;
    public List<Chips> Chips { get; set; } =
    [
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
        new("SaltAndVinegar.png", 5, 200),
    ];

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button { Content: Panel panel, Parent: ContentPresenter contentPresenter } || 
            panel.Children.Count < 3 || 
            panel.Children[2] is not Viewbox viewbox || 
            panel.Children[1] is not Canvas place) return;

        place.RenderTransform = TransformOperations.Parse("rotate(360deg)");
        
        await Task.Delay(400);

        contentPresenter.ZIndex = 100;
        viewbox.RenderTransform = TransformOperations.Parse($"translateY({((DoorY - contentPresenter.Bounds.Y) * 1.2).ToString("0.00", CultureInfo.InvariantCulture)}px)");

        await Task.Delay(100);
        
        viewbox.RenderTransform = TransformOperations.Parse($"translateY({((DoorY - contentPresenter.Bounds.Y + viewbox.Bounds.Height / 5) * 1.2).ToString("0.00", CultureInfo.InvariantCulture)}px) scaleY(0.8)");
        
        await Task.Delay(200);
        
        contentPresenter.ZIndex = 0;
    }
}