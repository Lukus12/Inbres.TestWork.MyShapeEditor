using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using InbresTest.Models;

namespace InbresTest;
public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
            return new TextBlock { Text = "Null data" };

        var modelType = data.GetType();
        
        // Формируем имя View: заменяем пространство имен и суффикс
        var viewTypeName = modelType.FullName!
            .Replace("InbresTest.Models", "InbresTest.Views.Shape") // меняем пространство имен
            .Replace("Model", "View"); // меняем суффикс
        
        var viewType = Type.GetType(viewTypeName);

        if (viewType != null && viewType.IsSubclassOf(typeof(Control)))
        {
            return (Control)Activator.CreateInstance(viewType)!;
        }

        // Если View не найдена — возвращаем заглушку с информацией
        return new TextBlock 
        { 
            Text = $"No view for {modelType.Name}. Looked for: {viewTypeName}",
            Background = Avalonia.Media.Brushes.LightPink 
        };
    }

    public bool Match(object? data) => data is ShapeBaseModel;
}