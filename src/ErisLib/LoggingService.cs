using System.Diagnostics;
using System.Runtime.CompilerServices;
using ErisLib.Enums;

namespace ErisLib;

public sealed class LoggingService
{
	private readonly LoggingOutputType _debugType;

	private readonly LoggingFilterSeverity _loggingFilterSeverity;

	private readonly string? _logPath;

	public LoggingService(LoggingOutputType loggingOutputType, LoggingFilterSeverity loggingFilterSeverity)
	{
		_debugType = loggingOutputType;
		_loggingFilterSeverity = loggingFilterSeverity;
	}

	public LoggingService(LoggingOutputType loggingOutputType, LoggingFilterSeverity loggingFilterSeverity, string logPath)
	{
		_debugType = loggingOutputType;
		_logPath = logPath;
		_loggingFilterSeverity = loggingFilterSeverity;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
	public Task DebugAsync(string message, [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
	{
		return LogAsync(LoggingSeverity.Debug, message, caller, file, line);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
	public Task InfoAsync(string message, [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
	{
		return LogAsync(LoggingSeverity.Info, message, caller, file, line);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
	public Task WarningAsync(string message, [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
	{
		return LogAsync(LoggingSeverity.Warning, message, caller, file, line);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
	public Task ErrorAsync(Exception? ex, bool EnsureFilePropertys = false)
	{
		if ( ex == null )
			return Task.CompletedTask;

		var st = new StackTrace(ex, true);
		var sf = EnsureFilePropertys ? st.GetFrames().FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.GetFileName()) && x.GetFileColumnNumber() != 0 && x.GetFileLineNumber() != 0) : st.GetFrame(st.FrameCount - 1);

		return LogAsync(LoggingSeverity.Error, $"{ex.GetType().FullName} - {ex.Message}{Environment.NewLine}{ex.StackTrace}", sf!.GetMethod()!.Name, sf.GetFileName()!, sf.GetFileLineNumber());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
	private static bool ShouldLog(in LoggingSeverity loggingSeverity, in LoggingFilterSeverity loggingFilterSeverity)
	{
		return loggingFilterSeverity switch
		{
			LoggingFilterSeverity.All        => true,
			LoggingFilterSeverity.NoDebug    => loggingSeverity is not LoggingSeverity.Debug,
			LoggingFilterSeverity.Extended   => loggingSeverity is LoggingSeverity.Warning or LoggingSeverity.Error,
			LoggingFilterSeverity.Production => loggingSeverity is LoggingSeverity.Error,
			LoggingFilterSeverity.None       => false,
			_                                => throw new ArgumentOutOfRangeException(nameof(loggingFilterSeverity), loggingFilterSeverity, null)
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
	private Task LogAsync(LoggingSeverity loggingSeverity, string message = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
	{
		if ( string.IsNullOrEmpty(message) || _debugType == LoggingOutputType.None || !ShouldLog(in loggingSeverity, in _loggingFilterSeverity) )
			return Task.CompletedTask;

		if ( _debugType is LoggingOutputType.Console or LoggingOutputType.All )
		{
			Console.ForegroundColor = (ConsoleColor)loggingSeverity;
			Console.Write($@"{DateTime.Now.ToLongTimeString()} [{Path.GetFileNameWithoutExtension(file)}->{caller} L{line}] ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write($@"{message}{Environment.NewLine}");
		}

		if ( _logPath != null && _debugType is LoggingOutputType.LogFile or LoggingOutputType.All )
			File.WriteAllText(_logPath, $@"[{Path.GetFileNameWithoutExtension(file)}->{caller} L{line}] {message}");

		return Task.CompletedTask;
	}
}
