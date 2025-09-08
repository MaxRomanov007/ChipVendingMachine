using System;
using System.IO;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace App.Domain.Models;

public class Chips : IDisposable
{
    public Bitmap? Image { get; set; }
    public int Quantity { get; set; }
    public int Price { get; set; }

    public Chips()
    {
    }

    public Chips(string assetPath, int quantity, int price)
    {
        Quantity = quantity;
        Price = price;
        Image = new Bitmap(AssetLoader.Open(new Uri(Path.Combine("avares://App/Assets/Chips", assetPath))));
    }

    public void Dispose()
    {
        Image?.Dispose();
        Image = null;
        Quantity = 0;
        Price = 0;
    }
}