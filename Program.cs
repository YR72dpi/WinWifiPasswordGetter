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
RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;
List<string> allWifi = new();
foreach (Match m in Regex.Matches(info, pattern, options))
{
    string wifiName = m.Value.Replace("\n", "").Remove(0,2).Trim();
    //Console.WriteLine(wifiName);
    allWifi.Add(wifiName);
}
foreach(string wifiName in allWifi)
{
        //Console.WriteLine(wifiName);
        //Console.WriteLine($"wlan show profiles {wifiName} key=clear");
        Process pwdInfo = new Process
        {
            StartInfo =
                {
                    FileName = "cmd.exe",
                    //WorkingDirectory = @"C:\myproject",
                    Arguments = $"/C netsh wlan show profiles {wifiName} key=clear"
                }
        };

        pwdInfo.StartInfo.RedirectStandardOutput = true;
        pwdInfo.StartInfo.RedirectStandardError = true;
        pwdInfo.StartInfo.UseShellExecute = false;
        pwdInfo.Start();
        pwdInfo.WaitForExit();

        string RawSecurityInfo = "@" + pwdInfo.StandardOutput.ReadToEnd();
        //Console.WriteLine(RawSecurityInfo);

        string pattern2 = @" *Contenu de la clé *: (.*)";

        foreach (Match m in Regex.Matches(RawSecurityInfo, pattern2, options))
        {
        string mdp = m.Value.Split(":")[1];
            Console.WriteLine($"{wifiName}\t\t{mdp}");
        }
}
Console.ReadLine();