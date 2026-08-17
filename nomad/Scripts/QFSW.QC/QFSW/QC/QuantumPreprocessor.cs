using System;
using System.Collections.Generic;
using System.Linq;

namespace QFSW.QC
{
	public class QuantumPreprocessor
	{
		private readonly IQcPreprocessor[] _preprocessors;

		public QuantumPreprocessor(IEnumerable<IQcPreprocessor> preprocessors)
		{
			_preprocessors = preprocessors.OrderByDescending((IQcPreprocessor x) => x.Priority).ToArray();
		}

		public QuantumPreprocessor()
			: this(new InjectionLoader<IQcPreprocessor>().GetInjectedInstances())
		{
		}

		public string Process(string text)
		{
			IQcPreprocessor[] preprocessors = _preprocessors;
			foreach (IQcPreprocessor qcPreprocessor in preprocessors)
			{
				try
				{
					text = qcPreprocessor.Process(text);
				}
				catch (Exception ex)
				{
					throw new Exception($"Preprocessor {qcPreprocessor} failed:\n{ex.Message}", ex);
				}
			}
			return text;
		}
	}
}
