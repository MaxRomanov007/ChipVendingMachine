using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using App.Domain.Models;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.Immutable;
using Avalonia.Media.Transformation;
using Avalonia.Platform;
using Microsoft.VisualBasic;
using Path = System.IO.Path;

namespace App.Windows;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    private const double DoorY = 485;
    private const string CreditCardImagePath = "credit-card.png";

    public List<Chips> Chips { get; set; } =
    [
        new("salt-and-vinegar.png", 5, 200),
        new("salt-and-vinegar.png", 5, 200),
        new("salt-and-vinegar.png", 5, 200),
        new("salt-and-vinegar.png", 5, 200),
        new("salt-and-vinegar.png", 5, 200),
        new("salt-and-vinegar.png", 5, 200),
        new("salt-and-vinegar.png", 5, 200),
        new("salt-and-vinegar.png", 5, 200),
        new("salt-and-vinegar.png", 5, 200),
        new("salt-and-vinegar.png", 5, 200),
        new("salt-and-vinegar.png", 5, 200),
        new("salt-and-vinegar.png", 5, 200),
        new("salt-and-vinegar.png", 5, 200),
        new("salt-and-vinegar.png", 5, 200),
        new("salt-and-vinegar.png", 5, 200),
        new()
    ];

    private int _inserted = 1000;

    public int Inserted
    {
        get => _inserted;
        set => SetField(ref _inserted, value);
    }

    private int _balance = 1000;

    public int Balance
    {
        get => _balance;
        set => SetField(ref _balance, value);
    }

    private Button? _selectedChipsButton;

    public Button? SelectedChipsButton
    {
        get => _selectedChipsButton;
        set => SetField(ref _selectedChipsButton, value);
    }

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void DisplayTextBlock_OnLoaded(object? sender, RoutedEventArgs e)
    {
        await ShowMessage("Внесите средства!");
    }

    private async void ChipsButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button { DataContext: Chips chips } button) return;
        if (chips.Image is null) return;

        if (SelectedChipsButton is null)
        {
            SelectedChipsButton = button;
            return;
        }

        if (Inserted < chips.Price)
        {
            await ShowMessage("Недостаточно средств!", TimeSpan.FromSeconds(1));
            return;
        }

        Inserted -= chips.Price;

        await AnimateChipsFalling(sender);
        await ShowMessage("Сброшено!", TimeSpan.FromSeconds(1));
    }

    private async void CashBackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SelectedChipsButton = null;
        await AnimateCashBack();
    }

    private async void CoinButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button
            {
                Tag: string nominalString,
                Content: Viewbox { Child: Image image },
                Parent: StackPanel { Parent: FlyoutPresenter { Parent: Popup popup } }
            }) return;
        
        if (!int.TryParse(nominalString, out var nominal)) return;
        Balance -= nominal;
        Inserted += nominal;

        popup.Close();
        await AnimateCoin(image.Source);
    }

    private async void BanknoteButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button
            {
                Tag: string nominalString,
                Content: Viewbox { Child: Image image },
                Parent: StackPanel { Parent: FlyoutPresenter { Parent: Popup popup } }
            }) return;
        if (!int.TryParse(nominalString, out var nominal)) return;

        Balance -= nominal;
        Inserted += nominal;

        popup.Close();
        await AnimateBanknote(image.Source);
    }

    private async void CardButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await AnimateCard();
    }

    private void AddMoneyButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Balance += 100;
    }

    private static async Task AnimateChipsFalling(object? sender)
    {
        if (sender is not Button { Content: Panel panel, Parent: ContentPresenter contentPresenter } ||
            panel.Children.Count < 3 ||
            panel.Children[2] is not Viewbox viewbox ||
            panel.Children[1] is not Canvas place) return;

        place.RenderTransform = TransformOperations.Parse("rotate(360deg)");

        await Task.Delay(400);

        contentPresenter.ZIndex = 100;
        viewbox.RenderTransform = TransformOperations.Parse(
            $"translateY({((DoorY - contentPresenter.Bounds.Y) * 1.2).ToString("0.00", CultureInfo.InvariantCulture)}px)");

        await Task.Delay(100);

        viewbox.RenderTransform = TransformOperations.Parse(
            $"translateY({((DoorY - contentPresenter.Bounds.Y + viewbox.Bounds.Height / 5) * 1.2).ToString("0.00", CultureInfo.InvariantCulture)}px) scaleY(0.8)");

        await Task.Delay(200);

        contentPresenter.ZIndex = 0;
    }

    private async Task AnimateBanknote(IImage? image)
    {
        var viewBox = new Viewbox
        {
            RenderTransform = TransformOperations.Parse("rotate(-90deg)"),
            Transitions =
            [
                new DoubleTransition
                {
                    Property = Canvas.TopProperty,
                    Duration = TimeSpan.FromMilliseconds(1000),
                    Easing = new LinearEasing()
                },
                new DoubleTransition
                {
                    Property = Canvas.LeftProperty,
                    Duration = TimeSpan.FromMilliseconds(1000),
                    Easing = new LinearEasing()
                },
                new DoubleTransition
                {
                    Property = WidthProperty,
                    Duration = TimeSpan.FromMilliseconds(1000),
                    Easing = new LinearEasing()
                },
            ],
            Stretch = Stretch.None,
            Child = new Image
            {
                Source = image,
                MaxHeight = 100,
                MaxWidth = 200,
            },
            Width = 200,
        };
        Canvas.SetLeft(viewBox, 382);
        Canvas.SetTop(viewBox, 245);

        AnimationCanvas.Children.Add(viewBox);

        Canvas.SetLeft(viewBox, 482);
        Canvas.SetTop(viewBox, 145);
        viewBox.Width = 0;
        await Task.Delay(1000);

        AnimationCanvas.Children.Remove(viewBox);
    }

    private async Task AnimateCoin(IImage? coinImage)
    {
        var image = new Image
        {
            Source = coinImage,
            MaxHeight = 40,
            MaxWidth = 40,
            Transitions = [
            new TransformOperationsTransition
            {
                Property = RenderTransformProperty,
                Duration = TimeSpan.FromMilliseconds(500),
                Easing = new CubicEaseInOut()
            }]
        };

        var viewBox = new Viewbox
        {
            RenderTransform = TransformOperations.Parse("rotate(180deg)"),
            Transitions =
            [
                new DoubleTransition
                {
                    Property = Canvas.LeftProperty,
                    Duration = TimeSpan.FromMilliseconds(500),
                    Easing = new CubicEaseInOut()
                },
                new DoubleTransition
                {
                    Property = WidthProperty,
                    Duration = TimeSpan.FromMilliseconds(500),
                    Easing = new CubicEaseInOut()
                },
            ],
            Stretch = Stretch.None,
            Child = image,
            Width = 40,
        };
        Canvas.SetLeft(viewBox, 443);
        Canvas.SetTop(viewBox, 209);

        AnimationCanvas.Children.Add(viewBox);

        Canvas.SetLeft(viewBox, 436);
        image.RenderTransform = TransformOperations.Parse("rotate(-90deg)");
        viewBox.Width = 0;
        await Task.Delay(500);

        AnimationCanvas.Children.Remove(viewBox);
    }

    private async Task AnimateCard()
    {
        using var cardImage = new Bitmap(AssetLoader.Open(new Uri(Path.Combine("avares://App/Assets/Cards", CreditCardImagePath))));
        
        var viewBox = new Viewbox
        {
            Transitions =
            [
                new DoubleTransition
                {
                    Property = Canvas.LeftProperty,
                    Duration = TimeSpan.FromMilliseconds(300),
                    Easing = new QuadraticEaseInOut()
                },
                new DoubleTransition
                {
                    Property = OpacityProperty,
                    Duration = TimeSpan.FromMilliseconds(300),
                    Easing = new LinearEasing()
                },
            ],
            Opacity = 0,
            Stretch = Stretch.None,
            Child = new Image
            {
                Source = cardImage,
                MaxHeight = 60,
                MaxWidth = 90,
            },
        };
        Canvas.SetLeft(viewBox, 556);
        Canvas.SetTop(viewBox, 117);

        AnimationCanvas.Children.Add(viewBox);
        
        viewBox.Opacity = 1;
        await Task.Delay(300);
        Canvas.SetLeft(viewBox, 436);
        await Task.Delay(800);
        Canvas.SetLeft(viewBox, 556);
        await Task.Delay(300);
        viewBox.Opacity = 0;
        await Task.Delay(300);

        AnimationCanvas.Children.Remove(viewBox);
    }

    public async Task AnimateCashBack()
    {
        var coins = new[]
        {
            CreateFictiveCoins(new Point(440, 260)), 
            CreateFictiveCoins(new Point(469, 260)), 
            CreateFictiveCoins(new Point(497, 260))
        };
        AnimationCanvas.Children.AddRange(coins);
        var rectangle = new Rectangle
        {
            Width = 100,
            Height = 50,
            Fill = new ImmutableSolidColorBrush(Colors.Aquamarine)
        };
        Canvas.SetLeft(rectangle, 431.5);
        Canvas.SetTop(rectangle, 259);
        AnimationCanvas.Children.Add(rectangle);
        
        Canvas.SetTop(coins[0], 315);
        Canvas.SetTop(coins[1], 329);
        Canvas.SetTop(coins[2], 318);
        await Task.Delay(800);
        coins[0].Opacity = 0;
        coins[1].Opacity = 0;
        coins[2].Opacity = 0;
        await Task.Delay(300);
        
        AnimationCanvas.Children.Remove(coins[0]);
        AnimationCanvas.Children.Remove(coins[1]);
        AnimationCanvas.Children.Remove(coins[2]);
        AnimationCanvas.Children.Remove(rectangle);
    }

    public static Ellipse CreateFictiveCoins(Point position = default)
    {
        var coin = new Ellipse
        {
            Width = 25,
            Height = 25,
            Fill = new ImmutableSolidColorBrush(Colors.Gold),
            Stroke = new ImmutableSolidColorBrush(Colors.LightGoldenrodYellow),
            StrokeThickness = 1,
            Transitions =
            [
                new DoubleTransition
                {
                    Property = Canvas.TopProperty,
                    Duration = TimeSpan.FromMilliseconds(300),
                    Easing = new QuadraticEaseInOut()
                },
                new DoubleTransition
                {
                    Property = OpacityProperty,
                    Duration = TimeSpan.FromMilliseconds(300),
                    Easing = new LinearEasing()
                },
            ],
        };
        Canvas.SetLeft(coin, position.X);
        Canvas.SetTop(coin, position.Y);
        
        return coin;
    }

    private async Task ShowMessage(string? message, TimeSpan? time = null, string? textAfterEnd = null)
    {
        var previousText = textAfterEnd ?? DisplayTextBlock.Text;

        DisplayTextBlock.Text = message;

        if (time is null) return;

        await Task.Delay(time.Value);

        DisplayTextBlock.Text = previousText;
    }

    public new event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}