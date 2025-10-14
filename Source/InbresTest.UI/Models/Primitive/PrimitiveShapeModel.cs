using Avalonia;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace InbresTest.Models.Primitive;

public abstract partial class PrimitiveShapeModel: ShapeBaseModel
{
    private double _width = 50;
    private double _height = 50;

    private int _indexColor = 1;
    private static string[] _color =  { "Red", "Green", "Yellow", "Black", "White" };
    
    public override double Width
    {
        get => _width;
        set
        {
            this.RaiseAndSetIfChanged(ref _width, value);
            this.RaisePropertyChanged(nameof(Geometry));
        } 
    }

    public override double Height
    {
        get => _height;
        set
        {
            this.RaiseAndSetIfChanged(ref _height, value);
            this.RaisePropertyChanged(nameof(Geometry));
        } 
    }
    
    [Reactive] public override partial string? Fill { get; set; } = _color[0];
    
    public override void ResizeShape(string type, Point delta)
    {
        if(Height < 0)
        {
            Height = 0;
            Y -= delta.Y;
            return;
        }

        if (Width < 0)
        {
            Width = 0;
            X -= delta.X;
            return;
        }
        
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

    public override void ChangeColor()
    {
        Fill = _color[_indexColor++ % _color.Length];
    }
}