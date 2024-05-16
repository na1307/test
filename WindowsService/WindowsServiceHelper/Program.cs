using System.IO.MemoryMappedFiles;
using System.Text;

using var mmf = MemoryMappedFile.OpenExisting("Global\\TestServiceMMF");
var ewh = EventWaitHandle.OpenExisting("Global\\TestServiceEWH");

while (true) {
    using StreamWriter sw = new(mmf.CreateViewStream(), Encoding.UTF8);

    sw.WriteLine(Console.ReadLine());
    ewh.Set();
}
