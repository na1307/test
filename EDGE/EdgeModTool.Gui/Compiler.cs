using EdgeModTool.Level;
using SkiaSharp;
using System.Xml.Linq;

namespace EdgeModTool;

internal static class Compiler {
    private const string HangulCharLookup
        = "가간감갖개게경계공과금기끔나너녕노높뉴느는니님다더도동됩뒤드든디딩때래랭레려력로록료를리릭림마막만많머멀메며면명모문뮤받방버벨보북브쁘사새서설세셔셨소속수순쉽스습시십아악안않야어에예오와완요용운원위으은을음이익인임입있자작잘잠재저전점접정제좋주죽중즘지직차체총취커켬코큐크클키킹타터텐통트튼티포표프플하할합해했향화환회효희히";

    private const string CharLookup
        = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789%１２３４５６７８９,.？!：'()_-=+@／＂çβāäēëīíōöūü＞＜[]ñ¡À$ÉȇÜ｜şğÇĕčřýňšžŮďĎŐȁÚâćęźɫąśńżŚ"
        + HangulCharLookup;

    private const string DuplicateChars = "abcdefghijklmnopqrstuvwxyzÜÇĎŚ"; // fix for stupid windows FS
    private static readonly HashSet<char> CharSet = new(CharLookup);
    private static readonly HashSet<char> DuplicateCharSet = new(DuplicateChars);

    public static void Compile(string file) {
        switch (Path.GetFileName(file)) {
            case "font.bin":
                DecompileFont(file, Path.Combine(Path.GetDirectoryName(file)!, "font"));

                break;

            case "font.xml":
                var root = XHelper.Load(file).Elements().First();

                CompileFont(Path.Combine(Path.GetDirectoryName(file)!, Path.GetFileNameWithoutExtension(file)), root);

                break;
        }
    }

    private static void DecompileFont(string file, string outputPath) {
        XElement result = new("Font");

        using (var stream = File.OpenRead(file))
        using (BinaryReader reader = new(stream)) {
            result.SetAttributeValueWithDefault("SpaceWidth", reader.ReadByte(), 9);

            var line = reader.ReadByte();

            result.SetAttributeValueWithDefault("LineSpacing", line, 9);

            var count = reader.ReadUInt16();

            for (var i = 0; i < count; i++) {
                var ch = i < CharLookup.Length ? CharLookup[i] : '\0';
                var rects = new Rect8[reader.ReadByte()];
                var specifiedWidth = reader.ReadByte();
                byte width = 0;
                var height = line;

                for (var j = 0; j < rects.Length; j++) {
                    var rect = rects[j] = new(reader);

                    if (rect.Size.X == 0 || rect.Size.Y == 0) {
                        continue;
                    }

                    var t = (byte)(rect.Point.X + rect.Size.X);

                    if (t > width) {
                        width = t;
                    }

                    if ((t = (byte)(rect.Point.Y + rect.Size.Y)) > height) {
                        height = t;
                    }
                }

                if (width > 0) {
                    if (width < specifiedWidth) {
                        width = specifiedWidth;
                    }

                    using SKBitmap bitmap = new(width, height);

                    using (SKPaint paintBlack = new()) {
                        paintBlack.Color = SKColors.Black;

                        using SKCanvas graphics = new(bitmap);

                        foreach (var rect in rects) {
                            graphics.DrawRect(rect.Point.X, rect.Point.Y, rect.Size.X, rect.Size.Y, paintBlack);
                        }
                    }

                    using var ob = File.Create(GetCharPath(outputPath, ch));

                    bitmap.Encode(ob, SKEncodedImageFormat.Png, 100);
                }

                if (width == specifiedWidth) {
                    continue;
                }

                XElement element = new("Char");

                element.SetAttributeValueWithDefault("Character", ch);
                element.SetAttributeValue("Width", specifiedWidth);
                result.Add(element);
            }
        }

        File.WriteAllText(outputPath + ".xml", result.ToString());
    }

    private static string GetCharPath(string prefix, char ch) => prefix + '.' + ch + (DuplicateCharSet.Contains(ch) ? "_" : string.Empty) + ".png";

    private static void CompileFont(string filePrefix, XElement element) {
        using var stream = File.Create(filePrefix + ".bin");
        using BinaryWriter writer = new(stream);

        writer.Write(element.GetAttributeValueWithDefault<byte>("SpaceWidth", 9));
        writer.Write(element.GetAttributeValueWithDefault<byte>("LineSpacing", 9));

        var lookup = element.ElementsCaseInsensitive("Char").ToLookup(e => e.GetAttributeValueWithDefault("Character", "\0")![0]);
        var reserved = lookup.Where(pair => !CharSet.Contains(pair.Key)).ToList();

        writer.Write((ushort)(CharSet.Count + reserved.SelectMany(e => e).Count()));

        foreach (var ch in CharLookup) {
            CompileChar(writer, filePrefix, ch, lookup[ch].SingleOrDefault());
        }

        foreach (var pair in reserved) {
            foreach (var e in pair) {
                CompileChar(writer, filePrefix, pair.Key, e); // write unknown chars
            }
        }
    }

    private static void CompileChar(BinaryWriter writer, string filePrefix, char ch, XElement? element) {
        byte width = 0;
        var rects = (RectilinearPolygonSolver.Solve(GetCharPath(filePrefix, ch), ref width) ?? []).ToArray();

        writer.Write((byte)rects.Length);
        writer.Write(element?.GetAttributeValueWithDefault("Width", width) ?? width);

        foreach (var rect in rects) {
            rect.Write(writer);
        }
    }
}
