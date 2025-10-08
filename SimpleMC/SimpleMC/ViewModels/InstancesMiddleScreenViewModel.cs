using SimpleMC.Models;

namespace SimpleMC.ViewModels;

internal sealed class InstancesMiddleScreenViewModel(IEnumerable<Instance> instances) : ViewModelBase {
    public ViewModelBase Screen { get; } = instances.Any() ? new InstancesScreenViewModel(instances) : new NoInstancesScreenViewModel();
}
