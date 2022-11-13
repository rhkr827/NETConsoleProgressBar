using System.Diagnostics;
using CSharpConsole;
using CSharpConsole.Test;

namespace CharpConsole
{
  public class Program
  {
    public static ConsoleProgressBar ProgressBar = new ConsoleProgressBar(new Properties());

    static void Main(string[] args)
    {
      Console.Write("Start Test ProcessBar");
      ProgressBar.UpdateTotalCount(82);
      ProgressBar.ShowProgressBar();
      ProgressBar.SetMessage("line1", "Start CSharpConsole App...");
      ProgressBar.SetMessage("line2", "Execute Command Prompt command 'dir'");
      StartProcess("dir");

      ProgressBar.SetProgress();
      Thread.Sleep(1000);
      UpdateProcessBar.Run(40);

      ProgressBar.SetMessage("line2", "Execute Command Prompt command 'tree D:\'");
      StartProcess("tree D:/");

      ProgressBar.SetProgress();
      Thread.Sleep(1000);

      UpdateProcessBar2.Run(40);


      Console.ReadLine();
      Console.WriteLine(Environment.NewLine);
      Console.WriteLine(Environment.NewLine);
      Console.WriteLine("Done.");
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