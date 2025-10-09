using System;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using InbresTest.Models;
using InbresTest.ViewModels;

namespace InbresTest.Views;

public partial class EditorControl : UserControl
{
    public EditorControl()
    {
        InitializeComponent();
    }

    private void ClickItem_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is not EditorViewModel vm) return;

        //var clickCanvas = this.FindControl<Canvas>("ClickCanvas");
        var itemsControl = sender as ItemsControl;
        if (itemsControl == null) return;

        var pos = e.GetPosition(itemsControl);
        
        System.Diagnostics.Debug.WriteLine($"🖱️ Click at: {pos.X}, {pos.Y}");

        //var hitControl = clickCanvas.InputHitTest(pos); // получаем верхний элемент по коорд-ам
        //System.Diagnostics.Debug.WriteLine($"Hit control: {hitControl?.GetType().Name}");

        //ShapeBaseModel? modelShape = FindShapeAtPositionSimple(pos);
        
        if (e.Source is Path path && path.DataContext is ShapeBaseModel shape)
        {
            System.Diagnostics.Debug.WriteLine($"Shape found at: {pos.X}, {pos.Y}");
            vm.SelectedShapeCommand.Execute(shape).Subscribe();
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("No shape found.");
            vm.CanvasClickCommand.Execute(pos).Subscribe();
        }
        
    }
    
    // Вспомогательный метод для поиска ShapeBaseModel из Canvas
    /*private ShapeBaseModel? FindShapeAtPositionSimple(Point position)
    {
        if (DataContext is not EditorViewModel vm) return null;
        
        System.Diagnostics.Debug.WriteLine($"Checking {vm.Shapes.Count} shapes");
        
        // Идем в обратном порядке (от верхних к нижним)
        for (int i = vm.Shapes.Count - 1; i >= 0; i--)
        {
            var shape = vm.Shapes[i];
            var shapeRect = new Rect(shape.X, shape.Y, shape.Width, shape.Height);
            
            System.Diagnostics.Debug.WriteLine($"Shape {i}: {shape.GetType().Name} at ({shape.X}, {shape.Y}) size {shape.Width}x{shape.Height}");
            
            if (shapeRect.Contains(position))
            {
                System.Diagnostics.Debug.WriteLine($"Hit shape {i}!");
                return shape;
            }
        }
        
        return null;
    }*/

    
}