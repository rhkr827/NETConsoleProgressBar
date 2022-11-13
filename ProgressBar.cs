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
    private List<MessageIndex> _messages = new List<MessageIndex>();

    private const int MessagePERCENTAGE = -2;
    private const int MessageNOTFOUND = -1;

    private readonly MessageIndex _MIPercentage = new MessageIndex(string.Empty, MessagePERCENTAGE);

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

      if (TotalProgress > 100)
        TotalProgress = 100;
      else
        TotalProgress = Math.Round(TotalProgress, 3);

      if (Properties.PercentageVisible)
      {
        SetMessage(_MIPercentage, $"{TotalProgress}{Properties.PercentageChaserText}");
      }

      RenderConsoleProgress();
    }
    public bool SetMessage(string mKey, string message)
    {
      var key = GetMessageIndexValue(mKey);
      if (key != MessageNOTFOUND)
      {
        SetMessage(_messages[key], message);
        return true;
      }
      return false;
    }

    public void EndConsoleOutput()
    {
      var numberOfLines = _messages.Count;
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

      for (var i = 0; i < _messages.Count; i++)
      {
        OverwriteConsoleMessage(_messages[i].Message);
        Console.CursorTop++;
      }

      if (Properties.ShowEmptyLineAbovePercentage)
      {
        Console.CursorTop++;
      }

      if (Properties.PercentageVisible)
      {
        Console.ForegroundColor = Properties.PercentageColor;
        OverwriteConsoleMessage(_MIPercentage.Message);
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

    private void SetMessage(MessageIndex messageIndex, string message)
    {
      messageIndex.Message = $"{Properties.LineIndicator}{message}";
      RenderConsoleProgress();
    }

    private int GetMessageIndexValue(string key)
    {
      key = key.ToLower();

      for (int index = 0; index < _messages.Count; index++)
      {
        var messageIndex = _messages[index];
        if (messageIndex.Line.Equals(key))
        {
          return index;
        }
      }
      return MessageNOTFOUND;
    }

    private void OverwriteConsoleMessage(string message)
    {
      Console.CursorLeft = 0;
      var maxCharacterWidth = Console.WindowWidth - 1;
      if (message.Length > maxCharacterWidth)
      {
        message = $"{message.Substring(0, maxCharacterWidth - Properties.TruncatedIndicator.Length)}{Properties.TruncatedIndicator}";
      }
      //Pads the message out to reach the maximum character width of the console
      message = $"{message}{new string(' ', maxCharacterWidth - message.Length)}";
      Console.Write(message);
    }

    private void CreateInitialMessages()
    {
      for (int i = 0; i < Properties.LinesAvailable; i++)
      {
        _messages.Add(new MessageIndex(Properties.LineIndicator, i + 1));
      }
    }
  }


  public class MessageIndex
  {
    public string Message { get; set; }

    public string Line { get; private set; }

    public MessageIndex(string msg, int index)
    {
      Message = msg;
      Line = $"line{index}";
    }
  }
}

