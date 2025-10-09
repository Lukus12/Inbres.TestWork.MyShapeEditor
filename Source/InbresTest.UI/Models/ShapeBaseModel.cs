using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace InbresTest.Models;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Avalonia.Media;


public abstract class ShapeBaseModel: ReactiveObject
{
    private bool _isSelected;
    [Reactive] public double X { get; set; }
    [Reactive] public double Y { get; set; }
    [Reactive] public double Width { get; set; } = 40;
    [Reactive] public double Height { get; set; } = 40;
    [Reactive] public string Fill { get; set; } = "Red";
    
    public abstract Geometry Geometry { get; }
    
    public string Stroke => IsSelected ? "MediumBlue" : "Black";
    public double StrokeThickness => IsSelected ? 3 : 1;

    [Reactive]
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            this.RaiseAndSetIfChanged(ref _isSelected, value);
            // Уведомляем об изменении Stroke и StrokeThickness
            this.RaisePropertyChanged(nameof(Stroke));
            this.RaisePropertyChanged(nameof(StrokeThickness));
        }
    }

}
