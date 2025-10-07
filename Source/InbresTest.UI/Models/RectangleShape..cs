using System;
using Avalonia;
using ReactiveUI.SourceGenerators;

namespace InbresTest.Models;

public class RectangleShape : ShapeBase
{
    [Reactive] public double X { get; set; } 
    [Reactive] public double Y { get; set; }
    [Reactive] public double Width { get; set; }
    [Reactive] public double Height { get; set; }
    [Reactive] public string Fill { get; set; }

    public RectangleShape()
    {
        X = 100;
        Y = 100;
        Width = 50;
        Height = 50;
        Fill = "Red";
    }
}