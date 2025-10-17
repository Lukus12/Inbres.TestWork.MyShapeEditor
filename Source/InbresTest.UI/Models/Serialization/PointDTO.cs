namespace InbresTest.Models.Serialization;

public class PointDto
{
    public double X { get; set; }
    public double Y { get; set; }

    public PointDto(Avalonia.Point p)
    {
        X = p.X;
        Y = p.Y;
    }

    public PointDto() { }

    public Avalonia.Point ToAvaloniaPoint() => new (X, Y);
}