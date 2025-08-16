using Microsoft.UI.Xaml;
using Microsoft.Windows.Storage.Pickers;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace OcrTest;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow {
    public MainWindow() => InitializeComponent();

    private async void TheButton_OnClick(object sender, RoutedEventArgs e) {
        try {
            Language ko = new("ko");

            var engine = OcrEngine.TryCreateFromLanguage(ko);

            if (!OcrEngine.IsLanguageSupported(ko) || engine is null) {
                TheTextBlock.Text = "OCR 한국어 미지원";

                return;
            }

            FileOpenPicker fop = new(AppWindow.Id);

            var file = await fop.PickSingleFileAsync();

            if (file is null) {
                return;
            }

            await using FileStream fs = new(file.Path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var ras = fs.AsRandomAccessStream();
            var decoder = await BitmapDecoder.CreateAsync(ras);
            var bitmap = await decoder.GetSoftwareBitmapAsync();
            var ocrResult = await engine.RecognizeAsync(bitmap);
            TheTextBlock.Text = ocrResult.Text;
        } catch (Exception ex) {
            TheTextBlock.Text = ex.ToString();
        }
    }
}
