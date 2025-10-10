using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace InbresTest.Models;

using Avalonia.Media;


public abstract class ShapeBaseModel: ReactiveObject
{
    private bool _isSelected;
    private double _x;
    private double _y;
    
    public double X
    {
        get => _x;
        set => this.RaiseAndSetIfChanged(ref _x, value);
    }
    public double Y
    {
        get => _y;
        set => this.RaiseAndSetIfChanged(ref _y, value);
    }
    
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
