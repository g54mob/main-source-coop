using System;

namespace VisualDesignCafe.Nature
{
	internal class IndentScope : IDisposable
	{
		private readonly ILog _log;

		private readonly bool _brackets;

		public IndentScope(ILog log, string message = null, bool brackets = false)
		{
			_brackets = brackets;
			_log = log;
			if (message != null)
			{
				_log?.Log(message);
			}
			if (brackets)
			{
				_log?.Log("{");
			}
			_log?.Indent();
		}

		public void Dispose()
		{
			_log?.Unindent();
			if (_brackets)
			{
				_log?.Log("}");
			}
		}
	}
}
