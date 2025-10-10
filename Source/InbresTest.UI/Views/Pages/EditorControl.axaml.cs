using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using InbresTest.Models;
using InbresTest.ViewModels;

namespace InbresTest.Views;

public partial class EditorControl : UserControl
{
    private Point _lastPointerPosition;
    private ShapeBaseModel? _draggedShape;
    
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
        _lastPointerPosition = pos;
        
        System.Diagnostics.Debug.WriteLine($"🖱️ Click at: {pos.X}, {pos.Y}");
        
        if (e.Source is Path path && path.DataContext is ShapeBaseModel shape)
        {
            System.Diagnostics.Debug.WriteLine($"Shape found at: {pos.X}, {pos.Y}");
            vm.SelectedShapeCommand.Execute(shape).Subscribe();
            
            _draggedShape = shape;
            
            e.Pointer.Capture(itemsControl); //элемент захватывает указатель
            e.Handled = true; // текущий элемент занимается обработкой, родительские элементы не должны
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("No shape found.");
            vm.CanvasClickCommand.Execute(pos).Subscribe();
            
            _draggedShape = null;
        }
        
    }
    
    private void ClickItem_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (DataContext is not EditorViewModel vm) return;
        if (_draggedShape == null) return;
        
        var itemsControl = sender as ItemsControl;
        if (itemsControl == null) return;
        
        var currentPosition = e.GetPosition(itemsControl);
        var delta = currentPosition - _lastPointerPosition;
        
        System.Diagnostics.Debug.WriteLine($"Moving shape: delta({delta.X}, {delta.Y}), new position({_draggedShape.X + delta.X}, {_draggedShape.Y + delta.Y})");
        
        vm.MovedShapeCommand.Execute(delta).Subscribe();

        _lastPointerPosition = currentPosition;
        e.Handled = true;
    }

    private void ClickItem_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _draggedShape = null;
        e.Pointer.Capture(null);
        e.Handled = true;
    }
}