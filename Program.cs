// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

Console.WriteLine("Hello, World!");


Process p = new Process
{
    StartInfo =
    {
        FileName = "netsh",
        //WorkingDirectory = @"C:\myproject",
        Arguments = "wlan show profiles"
    }
};

p.StartInfo.RedirectStandardOutput = true;
p.StartInfo.RedirectStandardError = true;
p.StartInfo.UseShellExecute = false;
p.Start();

string info = "@" + p.StandardOutput.ReadToEnd();
//Console.WriteLine(info);
string pattern = @" *: (.*)";
RegexOptions options = RegexOptions.Multiline;
List<string> allWifi = new();
foreach (Match m in Regex.Matches(info, pattern, options))
{
    string wifiName = m.Value.Split(":")[1].Trim().Replace("\n", "");
    //Console.WriteLine(wifiName);
    allWifi.Add(wifiName);
}

foreach(string wifiName in allWifi)
{
        Console.WriteLine(wifiName);
    //Console.WriteLine($"wlan show profiles {wifiName} key=clear");
        Process pwdInfo = new Process
        {
            StartInfo =
                {
                    FileName = "cmd.exe",
                    //WorkingDirectory = @"C:\myproject",
                    Arguments = $"/C chcp 437 && netsh wlan show profiles {wifiName} key=clear"
                }
        };

        /*pwdInfo.StartInfo.RedirectStandardOutput = true;
        pwdInfo.StartInfo.RedirectStandardError = true;
        pwdInfo.StartInfo.UseShellExecute = false;*/
        pwdInfo.Start();
        pwdInfo.WaitForExit();
}