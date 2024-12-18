using ElmSharp;
using Tizen.Applications;

namespace SampleTVApp;

internal sealed class App : CoreUIApplication {
    protected override void OnCreate() {
        base.OnCreate();
        Initialize();
    }

    private void Initialize() {
        Window window = new("SampleTVApp") {
            AvailableRotations = DisplayRotation.Degree_0
        };
        window.BackButtonPressed += (s, e) => {
            Exit();
        };
        window.Show();

        Box box = new(window) {
            AlignmentX = -1,
            AlignmentY = -1,
            WeightX = 1,
            WeightY = 1,
        };
        box.Show();

        Background bg = new(window) {
            Color = Color.White
        };
        bg.SetContent(box);

        Conformant conformant = new(window);
        conformant.Show();
        conformant.SetContent(bg);

        Label label = new(window) {
            Text = "Hello, Tizen",
            Color = Color.Black
        };
        label.Show();
        box.PackEnd(label);

        Button button = new(window) {
            Text = "Click Me",
            MinimumWidth = 300,
            Color = Color.Aqua
        };

        button.Clicked += (s, e) => {
            Popup popup = new(window) {
                Text = DateTime.Now.ToString("yyyy년 MMMM d일 tt h:mm:s"),
                Timeout = 5
            };

            popup.Show();
        };

        button.Show();
        box.PackEnd(button);
    }

    private static void Main(string[] args) {
        Elementary.Initialize();
        Elementary.ThemeOverlay();
        App app = new();
        app.Run(args);
    }
}
