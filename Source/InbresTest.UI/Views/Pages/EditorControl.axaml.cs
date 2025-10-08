using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using InbresTest.Models;
using InbresTest.ViewModels;

namespace InbresTest.Views;

public partial class EditorControl : UserControl
{
    public EditorControl()
    {
        InitializeComponent();
    }

    private void ClickCanvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is not EditorViewModel vm) return;

        var clickCanvas = this.FindControl<Canvas>("ClickCanvas");
        if (clickCanvas == null) return;

        var pos = e.GetPosition(clickCanvas);
        System.Diagnostics.Debug.WriteLine($"🖱️ Click at: {pos.X}, {pos.Y}");
        
        vm.CanvasClickCommand.Execute(pos).Subscribe();
    }
   
}