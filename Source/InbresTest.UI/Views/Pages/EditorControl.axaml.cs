using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using InbresTest.Models;
using InbresTest.ViewModels;

namespace InbresTest.Views.Pages;

public partial class EditorControl : UserControl
{
    private Point _lastPointerPosition;
    private ShapeBaseModel? _draggedShape;
    
    private string? _activeHandleType;
    
    public EditorControl()
    {
        InitializeComponent();
    }

    private void ClickItem_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is not EditorViewModel vm) return;
        
        var itemsControl = sender as ItemsControl;
        if (itemsControl == null) return;

        var pos = e.GetPosition(itemsControl);
        _lastPointerPosition = pos;
        
        System.Diagnostics.Debug.WriteLine($"🖱️ Click at: {pos.X}, {pos.Y}");
        
        // логика безье
        if (vm.CurrentCreationMode != EditorViewModel.CreationMode.None)
        {
            if (e.GetCurrentPoint(this).Properties.PointerUpdateKind == PointerUpdateKind.RightButtonPressed) 
                vm.IsEnding = true;
            vm.CanvasClickCommand.Execute(pos).Subscribe();
            e.Handled = true;
            return;
        }
        
        // логика маркеров
        if (e.Source is Border handleBorder && handleBorder.DataContext is ShapeBaseModel shapeBorder)
        {
            // Проверяем, что это действительно маркер, а не часть Path
            if (handleBorder.Parent is Canvas container && container.DataContext == shapeBorder)
            {
                System.Diagnostics.Debug.WriteLine("Marker found.");
                
                _draggedShape = shapeBorder;
                _activeHandleType = handleBorder.Tag?.ToString(); 
            
                e.Pointer.Capture(itemsControl); 
                e.Handled = true;
                return;
            }
        }
        
        // логика нахождения фигуры
        if (e.Source is Path path && path.DataContext is ShapeBaseModel shape)
        {
            System.Diagnostics.Debug.WriteLine($"Shape found at: {pos.X}, {pos.Y}");
            vm.SelectedShapeCommand.Execute(shape).Subscribe();
            
            _draggedShape = shape;
            _activeHandleType = null;
            
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

        if (_activeHandleType != null)
        {
            vm.ResizedShapeCommand.Execute(new object[] { _activeHandleType, delta}).Subscribe();
        }
        else vm.MovedShapeCommand.Execute(delta).Subscribe();

        _lastPointerPosition = currentPosition;
        e.Handled = true;
    }

    private void ClickItem_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _draggedShape = null;
        _activeHandleType = null;
        e.Pointer.Capture(null);
        e.Handled = true;
    }

    private void ClickItem_DoubleTapped(object? sender, TappedEventArgs e)
    {
        if (DataContext is not EditorViewModel vm) return;
        
        var itemsControl = sender as ItemsControl;
        if (itemsControl == null) return;
        
        vm.ChangeColorCommand.Execute().Subscribe();
    }
}