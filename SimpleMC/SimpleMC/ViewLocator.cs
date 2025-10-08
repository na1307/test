using Avalonia.Controls.Templates;
using SimpleMC.ViewModels;

namespace SimpleMC;

internal sealed class ViewLocator : IDataTemplate {
    public Control? Build(object? param) {
        if (param is null) {
            return null;
        }

        var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal)
            .Replace("ScreenView", "Screen", StringComparison.Ordinal);

        var type = Type.GetType(name);

        if (type is not null) {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock {
            Text = "Not Found: " + name
        };
    }

    public bool Match(object? data) => data is ViewModelBase;
}
