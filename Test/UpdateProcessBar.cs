using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharpConsole;

namespace CSharpConsole.Test
{
  public class UpdateProcessBar
  {
    public static void Run(int number = 100)
    {
      for (int i = 0; i < number; i++)
      {
        Program.ProgressBar.SetMessage($"line2", $"[Update1] Index {i} Updated...");
        Program.ProgressBar.SetProgress();
        Thread.Sleep(100);
      }
    }
  }
}