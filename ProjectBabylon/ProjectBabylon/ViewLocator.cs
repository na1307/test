using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ProjectBabylon.ViewModels;

namespace ProjectBabylon;

public sealed class ViewLocator : IDataTemplate {
    public Control? Build(object? param) {
        if (param is not ViewModelBase) {
            return null;
        }

        var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null) {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock {
            Text = "Not Found: " + name
        };
    }

    public bool Match(object? data) => data is ViewModelBase;
}
