using System;

namespace QFSW.QC
{
	public interface IQcSerializer
	{
		int Priority { get; }

		bool CanSerialize(Type type);

		string SerializeFormatted(object value, QuantumTheme theme, Func<object, QuantumTheme, string> recursiveSerializer);
	}
}
