using System.Diagnostics;
using CSharpConsole;
using CSharpConsole.Test;

namespace CharpConsole
{
  public class Program
  {
    public static ConsoleProgressBar ProgressBar = new ConsoleProgressBar(new Properties() { LinesAvailable = 2 });

    static void Main(string[] args)
    {
      Console.Write("Start Test ProcessBar");
      for (int line = 2; line <= 10; line++)
      {
        ProgressBar = new ConsoleProgressBar(new Properties() { LinesAvailable = line });
        ProgressBar.UpdateTotalCount(12);
        ProgressBar.ShowProgressBar();
        ProgressBar.SetMainMessage($"Start CSharpConsole App  LinesAvailable={line} ...");
        ProgressBar.SetMessage("Execute Command Prompt command 'dir'");
        StartProcess("dir");

        ProgressBar.SetProgress();
        Thread.Sleep(1000);
        UpdateProcessBar.Run(5);

        ProgressBar.SetMessage("Execute Command Prompt command 'tree D:\'");
        StartProcess("tree D:/Utility");

        ProgressBar.SetProgress();
        Thread.Sleep(1000);

        UpdateProcessBar2.Run(5);

        int length = ProgressBar.Properties.LinesAvailable / 2;
        if (ProgressBar.Properties.LinesAvailable % 2 == 1)
          length++;

        for (int i = 0; i < length + 2; i++)
          Console.WriteLine(Environment.NewLine);

        Console.WriteLine($"Press Any Key {length}");
        Console.ReadLine();
      }
    }

    static void StartProcess(string command)
    {
      Process proc = new Process();
      ProcessStartInfo info = new ProcessStartInfo()
      {
        FileName = "cmd.exe",
        RedirectStandardInput = true,
        UseShellExecute = false,
        WindowStyle = ProcessWindowStyle.Normal
      };

      proc.StartInfo = info;
      proc.Start();

      using (StreamWriter sw = proc.StandardInput)
      {
        if (sw.BaseStream.CanWrite)
        {
          sw.WriteLine(command);
          sw.WriteLine("cls");
        }
      }

      proc.WaitForExit();
      if (!proc.HasExited)
      {
        proc.Kill();
      }
    }
  }
}