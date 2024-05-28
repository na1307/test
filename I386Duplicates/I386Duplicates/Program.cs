try {
    var dir = args.Length switch {
        0 => Directory.GetCurrentDirectory(),
        1 => args[0],
        _ => throw new ArgumentException("Too Many Arguments."),
    };

    output(dir + "\\I386");

    if (Directory.Exists(dir + "\\AMD64")) {
        output(dir + "\\AMD64");
    }
} catch (Exception ex) {
    Console.WriteLine(ex.Message);
    Environment.Exit(ex.HResult);
}

static void output(string dirname) {
    dirname = Path.GetFullPath(dirname);

    if (!Directory.Exists(dirname)) {
        throw new DirectoryNotFoundException($"\"{dirname}\" Directory not found.");
    }

    Console.WriteLine("Duplicate files in " + dirname + " directory : ");

    var files = Directory.GetFiles(dirname);
    var duplicates = files.Where(predicate).Select(selector).ToList();

    duplicates.ForEach(Console.WriteLine);

    Console.WriteLine();

    Console.WriteLine("Total number of files : " + files.Length);
    Console.WriteLine("Number of duplicate files : " + duplicates.Count);
}

static string cfn(string fn) => !string.IsNullOrEmpty(Path.GetExtension(fn)) ? fn[..^1] + "_" : fn + "._";
static bool predicate(string fn) => fn[^1] != '_' && File.Exists(cfn(fn));
static string selector(string fn) => Path.GetFileName(cfn(fn));
