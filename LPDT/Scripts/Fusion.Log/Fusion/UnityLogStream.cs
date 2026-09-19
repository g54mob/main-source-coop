using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using UnityEngine;

namespace Fusion
{
	internal sealed class UnityLogStream : LogStream
	{
		private readonly FusionUnityLoggerBase _logger;

		private readonly LogLevel _logLevel;

		private readonly string _prefix;

		private readonly LogFlags _flags;

		public UnityLogStream(FusionUnityLoggerBase logger, LogLevel logLevel, TraceChannels channel, LogFlags flags)
		{
			_prefix = ((channel == (TraceChannels)0 || channel == TraceChannels.Global) ? "" : channel.ToString());
			_logLevel = logLevel;
			_logger = logger;
			_flags = flags;
		}

		[HideInCallstack]
		public override void Log(ILogSource source, string message)
		{
			var (text, context) = _logger.CreateMessage(new FusionUnityLoggerBase.LogContext(message, _prefix, source, _flags));
			if (text != null)
			{
				switch (_logLevel)
				{
				case LogLevel.Error:
					Debug.LogError(text, context);
					break;
				case LogLevel.Warn:
					Debug.LogWarning(text, context);
					break;
				default:
					Debug.Log(text, context);
					break;
				}
			}
		}

		[HideInCallstack]
		public override void Log(string message)
		{
			var (text, context) = _logger.CreateMessage(new FusionUnityLoggerBase.LogContext(message, _prefix, null, _flags));
			if (text != null)
			{
				switch (_logLevel)
				{
				case LogLevel.Error:
					Debug.LogError(text, context);
					break;
				case LogLevel.Warn:
					Debug.LogWarning(text, context);
					break;
				default:
					Debug.Log(text, context);
					break;
				}
			}
		}

		[HideInCallstack]
		public override void Log(ILogSource source, string message, Exception error)
		{
			var (text, obj) = _logger.CreateMessage(new FusionUnityLoggerBase.LogContext((message ?? error.GetType().FullName) + " <i>See next error log entry for details.</i>", null, source, (LogFlags)0));
			if (text == null)
			{
				return;
			}
			Debug.LogWarning(text, obj);
			if (Application.isEditor)
			{
				ExceptionDispatchInfo edi = ExceptionDispatchInfo.Capture(error);
				Thread thread = new Thread((ThreadStart)delegate
				{
					edi.Throw();
				});
				thread.Start();
				thread.Join();
			}
			else if ((bool)obj)
			{
				Debug.LogException(error, obj);
			}
			else
			{
				Debug.LogException(error);
			}
		}

		[HideInCallstack]
		public override void Log(string message, Exception error)
		{
			var (text, obj) = _logger.CreateMessage(new FusionUnityLoggerBase.LogContext((message ?? error.GetType().FullName) + " <i>See next error log entry for details.</i>", null, null, (LogFlags)0));
			if (text == null)
			{
				return;
			}
			Debug.LogWarning(text, obj);
			if (Application.isEditor)
			{
				ExceptionDispatchInfo edi = ExceptionDispatchInfo.Capture(error);
				Thread thread = new Thread((ThreadStart)delegate
				{
					edi.Throw();
				});
				thread.Start();
				thread.Join();
			}
			else if ((bool)obj)
			{
				Debug.LogException(error, obj);
			}
			else
			{
				Debug.LogException(error);
			}
		}

		[HideInCallstack]
		public override void Log(ILogSource source, Exception error)
		{
			var (text, obj) = _logger.CreateMessage(new FusionUnityLoggerBase.LogContext(error.GetType().FullName + " <i>See next error log entry for details.</i>", null, source, (LogFlags)0));
			if (text == null)
			{
				return;
			}
			Debug.LogWarning(text, obj);
			if (Application.isEditor)
			{
				ExceptionDispatchInfo edi = ExceptionDispatchInfo.Capture(error);
				Thread thread = new Thread((ThreadStart)delegate
				{
					edi.Throw();
				});
				thread.Start();
				thread.Join();
			}
			else if ((bool)obj)
			{
				Debug.LogException(error, obj);
			}
			else
			{
				Debug.LogException(error);
			}
		}

		[HideInCallstack]
		public override void Log(Exception error)
		{
			var (text, obj) = _logger.CreateMessage(new FusionUnityLoggerBase.LogContext(error.GetType().FullName + " <i>See next error log entry for details.</i>", null, null, (LogFlags)0));
			if (text == null)
			{
				return;
			}
			Debug.LogWarning(text, obj);
			if (Application.isEditor)
			{
				ExceptionDispatchInfo edi = ExceptionDispatchInfo.Capture(error);
				Thread thread = new Thread((ThreadStart)delegate
				{
					edi.Throw();
				});
				thread.Start();
				thread.Join();
			}
			else if ((bool)obj)
			{
				Debug.LogException(error, obj);
			}
			else
			{
				Debug.LogException(error);
			}
		}
	}
}
