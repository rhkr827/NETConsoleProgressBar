namespace CSharpConsole
{
  public class Properties
  {
    public int StartOnLine { get; set; } = 0;
    public int LinesAvailable { get; set; } = 2;
    public ConsoleColor PBarColor { get; set; } = ConsoleColor.Green;
    public ConsoleColor TextColor { get; set; } = ConsoleColor.DarkCyan;
    public ConsoleColor PercentageColor { get; set; } = ConsoleColor.DarkYellow;
    public string LineIndicator { get; set; } = "> ";
    public char ProgressBarCharacter { get; set; } = 'o';
    public bool PercentageVisible { get; set; } = true;
    public bool ShowEmptyLineAbovePercentage { get; set; } = true;
    public string PercentageChaserText { get; set; } = "% Complete";
    public string TruncatedIndicator { get; set; } = "...";
    public int TotalCount { get; set; } = 100;
  }

  public class ConsoleProgressBar
  {
    public Properties Properties { get; private set; }
    public double TotalProgress { get; private set; } = 0.0;
    public double UpdateProgress { get; private set; } = 0.0;
    private List<Message> Messages = new List<Message>();
    private string MessagePercentage = string.Empty;

    private bool _disposed = false;

    public ConsoleProgressBar(Properties properties)
    {
      Properties = properties ?? new Properties();
      CreateInitialMessages();
    }

    public void ShowProgressBar()
    {
      SetProgress(true);
    }

    public void UpdateTotalCount(int total)
    {
      Properties.TotalCount = total;
      UpdateProgress = Math.Round(100 * (double)1 / Properties.TotalCount, 3);
    }

    public void SetProgress(bool initial = false)
    {
      if (initial)
        TotalProgress = 0;
      else
        TotalProgress += UpdateProgress;

      if (TotalProgress >= 99.99)
        TotalProgress = 100;
      else
        TotalProgress = Math.Round(TotalProgress, 3);

      if (Properties.PercentageVisible)
      {
        MessagePercentage = $"{TotalProgress}{Properties.PercentageChaserText}";
      }

      RenderConsoleProgress();
    }
    public void SetMainMessage(string message)
    {
      Messages[0].Value = $"{Properties.LineIndicator}{message}";
    }
    public void SetMessage(string message)
    {
      var msg = Messages.Where(e => e.Value == Properties.LineIndicator).FirstOrDefault();
      if (msg != null)
      {
        Messages[msg.Index].Value = $"{Properties.LineIndicator}{message}";
      }
      else
      {
        for (int i = 1; i < Properties.LinesAvailable; i++)
        {
          if (i + 1 == Properties.LinesAvailable)
            Messages[i].Value = $"{Properties.LineIndicator}{message}";
          else
            Messages[i].Value = Messages[i + 1].Value;

        }
      }

      RenderConsoleProgress();
    }

    public void EndConsoleOutput()
    {
      var numberOfLines = Messages.Count;
      if (Properties.PercentageVisible)
      {
        numberOfLines++;
      }
      if (Properties.ShowEmptyLineAbovePercentage)
      {
        numberOfLines++;
      }
      numberOfLines++;
      for (int i = 0; i < numberOfLines; i++)
      {
        Console.WriteLine(string.Empty);
      }
    }

    public void Dispose()
    {
      if (_disposed == false)
      {
        EndConsoleOutput();
        _disposed = true;
      }
    }

    private void RenderConsoleProgress()
    {
      Console.CursorVisible = false;
      var originalColor = Console.ForegroundColor;
      Console.ForegroundColor = Properties.TextColor;

      Console.CursorTop = Properties.StartOnLine;

      for (var i = 0; i < Messages.Count; i++)
      {
        OverwriteConsoleMessage(Messages[i].Value);
        Console.CursorTop++;
      }

      if (Properties.ShowEmptyLineAbovePercentage)
      {
        Console.CursorTop++;
      }

      if (Properties.PercentageVisible)
      {
        Console.ForegroundColor = Properties.PercentageColor;
        OverwriteConsoleMessage(MessagePercentage);
        Console.CursorTop++;
      }

      Console.ForegroundColor = Properties.PBarColor;
      Console.CursorLeft = 0;
      var width = Console.WindowWidth - 1;
      var newWidth = (int)((width * TotalProgress) / 100d);
      var progBar = new string(Properties.ProgressBarCharacter, newWidth) + new string(' ', width - newWidth);
      Console.Write(progBar);

      Console.CursorTop = Properties.StartOnLine;
      Console.ForegroundColor = originalColor;
      Console.CursorVisible = true;
    }

    private void OverwriteConsoleMessage(string message)
    {
      Console.CursorLeft = 0;
      var maxCharacterWidth = Console.WindowWidth - 1;
      if (message.Length > maxCharacterWidth)
      {
        message = $"{message.Substring(0, maxCharacterWidth - Properties.TruncatedIndicator.Length)}{Properties.TruncatedIndicator}";
      }

      message = $"{message}{new string(' ', maxCharacterWidth - message.Length)}";
      Console.Write(message);
    }

    private void CreateInitialMessages()
    {
      for (int i = 0; i < Properties.LinesAvailable; i++)
      {
        Messages.Add(new Message(i, Properties.LineIndicator));
      }
    }
  }


  public class Message
  {
    public int Index { get; private set; }
    public string Value { get; set; }

    public Message(int index, string msg)
    {
      Index = index;
      Value = msg;
    }
  }
}

