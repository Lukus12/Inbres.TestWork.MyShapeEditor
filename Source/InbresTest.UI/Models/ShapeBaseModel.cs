using Avalonia;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace InbresTest.Models;

using Avalonia.Media;


public abstract partial class ShapeBaseModel: ReactiveObject
{
    private bool _isSelected;
    private double _width = 50;
    private double _height = 50;
    
    [Reactive] public partial double X { get; set; }
    [Reactive] public partial double Y { get; set; }

    public double Width
    {
        get => _width;
        set
        {
            this.RaiseAndSetIfChanged(ref _width, value);
            this.RaisePropertyChanged(nameof(Geometry));
        } 
    }

    public double Height
    {
        get => _height;
        set
        {
            this.RaiseAndSetIfChanged(ref _height, value);
            this.RaisePropertyChanged(nameof(Geometry));
        } 
    }
    
    
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
            // Уведомляем об изменении фигуры
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

    public void ResizeShape(string type, Point delta)
    {
        switch (type)
        {
            case "TopCenter":
                
                Y += delta.Y;
                Height -= delta.Y;
                
                break;
            
            case "LeftCenter":
                
                X += delta.X;
                Width -= delta.X;
                
                break;
            
            case "TopLeft":
                
                X += delta.X;
                Y += delta.Y;
                
                Width -= delta.X;
                Height -= delta.Y;
                
                break;
        }
    }

}
