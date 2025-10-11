using Avalonia;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace InbresTest.Models;

using Avalonia.Media;


public abstract partial class ShapeBaseModel: ReactiveObject
{
    private bool _isSelected;

    [Reactive] public partial double X { get; set; }
    [Reactive] public partial double Y { get; set; }


    [Reactive] public partial double Width { get; set; } = 50;
    [Reactive] public partial double Height { get; set; } = 50;
    [Reactive] public partial string Fill { get; set; } = "Red";
    
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


    public void MoveTo(Point delta)
    {
        if (X >= 0 && Y >= 0)
        {
            X += delta.X;
            Y += delta.Y;
        }

        if (X < 0) X = 0;
        if (Y < 0) Y = 0;
    }

}
