namespace UNICEF_App.Service
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string _filePath;

        public FileLoggerProvider(string path)
        {
            _filePath = path;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_filePath);
        }

        public void Dispose()
        {
        }
    }

    public class FileLogger : ILogger
    {
        private readonly string _filePath;
        private static object _lock = new object();

        public FileLogger(string path)
        {
            _filePath = path;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= LogLevel.Information;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            lock (_lock)
            {
                File.AppendAllText(_filePath, $"{logLevel}: {formatter(state, exception)}{Environment.NewLine}");
            }
        }
    }
}
