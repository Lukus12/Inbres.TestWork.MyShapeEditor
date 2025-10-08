using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace InbresTest.Models;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Avalonia.Media;


public abstract class ShapeBaseModel: ReactiveObject
{
    [Reactive] public double X { get; set; }
    [Reactive] public double Y { get; set; }
    [Reactive] public double Width { get; set; } = 40;
    [Reactive] public double Height { get; set; } = 40;
    [Reactive] public string Fill { get; set; } = "Red";
    [Reactive] public string Stroke { get; set; } = "Black";
    [Reactive] public double StrokeThickness { get; set; } = 1;

    // Геометрия фигуры - будет использоваться в Path.Data
    public abstract Geometry Geometry { get; }

}
