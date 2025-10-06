using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using InbresTest.ViewModels;
using System.Diagnostics.CodeAnalysis;

namespace InbresTest;

public class ViewLocator : IDataTemplate
{
    [UnconditionalSuppressMessage(
        "ReflectionAnalysis", 
        "IL2057:UnrecognizedValue", 
        Justification = "All view types are preserved via [DynamicDependency]")]
    [DynamicDependency(
        DynamicallyAccessedMemberTypes.PublicConstructors, 
        "InbresTest.Views.Pages.*", 
        "InbresTest.UI")]
    public Control? Build(object? param)
    {
        if (param is null)
            return null;

        var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}