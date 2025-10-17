using Avalonia;
using InbresTest.Models.Serialization;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace InbresTest.Models;

using Avalonia.Media;


public abstract partial class ShapeBaseModel : ReactiveObject
{
    private bool _isSelected;

    [Reactive] public partial double X { get; set; }
    [Reactive] public partial double Y { get; set; }

    public virtual double Width { get; set; }

    public virtual double Height { get; set; }
    
    public virtual string? Fill {get;set;}
    
    public abstract Geometry Geometry { get; }
    
    
    public string Stroke => IsSelected ? "MediumBlue" : "Black";
    public double StrokeThickness => IsSelected ? 5 : 3;

    [Reactive]
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            this.RaiseAndSetIfChanged(ref _isSelected, value);
            // Уведомляем об изменении фигуры
            this.RaisePropertyChanged(nameof(Stroke));
            this.RaisePropertyChanged(nameof(StrokeThickness));
        }
    }

    // Для сериализации
    public abstract ShapeData CreateSerializationData();
    public abstract void RestoreFromData(ShapeData data);

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

    public virtual void ResizeShape(string type, Point delta){}
    public virtual void ChangeColor(){}
}
