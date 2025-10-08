using Avalonia.Input;
using SimpleMC.Models;
using SimpleMC.ViewModels;
using System.Diagnostics;

namespace SimpleMC.Views;

public sealed partial class InstancesScreen : Screen {
    public InstancesScreen() => InitializeComponent();

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e) {
        // 클릭한 곳이 ListBoxItem이 아닌 빈 공간인지 확인
        var source = e.Source as Control;

        // ListBoxItem을 클릭한 경우가 아니라면 선택 해제
        while (source is not null) {
            if (source is ListBoxItem) {
                return; // ListBoxItem 클릭이므로 아무것도 하지 않음
            }

            source = source.Parent as Control;
        }

        // 빈 공간 클릭 - 선택 해제
        if (sender is ListBox listBox) {
            listBox.SelectedItem = null;
        }
    }

    private async void InputElement_OnDoubleTapped(object? sender, TappedEventArgs e) {
        if (sender is ListBox { SelectedItem: Instance instance }) {
            var vm = (InstancesScreenViewModel)DataContext!;

            Trace.Assert(ReferenceEquals(vm.SelectedInstance, instance));

            await vm.Launch();
        }
    }
}
