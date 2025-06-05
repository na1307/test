using System.Diagnostics;
using UefiReboot;

Console.Write(Environment.NewLine + Environment.NewLine + Environment.NewLine);

Console.WriteLine("현재 다음과 같은 부팅 항목들이 있습니다:");

Console.WriteLine();

var bes = await
#if WINDOWS
BcdEdit.GetFirmwareEntries();
#else
OnLinux.GetFirmwareEntries();
#endif

foreach (var be in bes.Select(async (e, i) =>
#if WINDOWS
$"{i + 1}. {await BcdEdit.GetDescription(e)}"
#else
$"{i + 1}. {await OnLinux.GetDescription(e)}"
#endif
#pragma warning disable SA1111
         )) {
#pragma warning restore SA1111
    Console.WriteLine(await be);
}

Console.Write(Environment.NewLine + Environment.NewLine + Environment.NewLine);

Console.Write("원하시는 항목을 선택하세요: ");

var success = ushort.TryParse(Console.ReadLine(), out var choice);
var length = bes.Length;

if (!success || length < choice) {
    return;
}

await
#if WINDOWS
BcdEdit.SetFirmwareBootNext(bes[choice - 1]);
#else
OnLinux.Reboot(bes[choice - 1]);
#endif

#if WINDOWS
Process.Start("shutdown.exe", " /r /t 0 /f");
#else
Process.Start("systemctl", "reboot");
#endif
