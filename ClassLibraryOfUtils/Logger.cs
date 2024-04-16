using Serilog;
namespace ClassLibraryOfUtils;

public static class Logger
{
    /// <summary>
    /// The method specifies the parameters for logger.
    /// </summary>
    public static void CreateLogger()
    {
        string logDirectory = @"..\..\..\..\var";
        logDirectory = logDirectory.Replace('\\', Path.DirectorySeparatorChar);
        Directory.CreateDirectory(logDirectory);
        Log.Logger = new LoggerConfiguration().WriteTo.
            File(Path.Combine(logDirectory, "messages.log"), 
            rollingInterval: RollingInterval.Day).CreateLogger();
    }
}