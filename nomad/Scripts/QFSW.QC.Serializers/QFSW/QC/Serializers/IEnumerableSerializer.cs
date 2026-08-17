using System;
using System.Collections;
using System.Text;
using QFSW.QC.Pooling;

namespace QFSW.QC.Serializers
{
	public class IEnumerableSerializer : IEnumerableSerializer<IEnumerable>
	{
		public override int Priority => base.Priority - 1000;

		protected override IEnumerable GetObjectStream(IEnumerable value)
		{
			return value;
		}
	}
	public abstract class IEnumerableSerializer<T> : PolymorphicQcSerializer<T> where T : class, IEnumerable
	{
		private readonly StringBuilderPool _builderPool = new StringBuilderPool();

		public override string SerializeFormatted(T value, QuantumTheme theme)
		{
			Type type = value.GetType();
			StringBuilder stringBuilder = _builderPool.GetStringBuilder();
			string leftScoper = "[";
			string seperator = ",";
			string rightScoper = "]";
			if ((bool)theme)
			{
				theme.GetCollectionFormatting(type, out leftScoper, out seperator, out rightScoper);
			}
			stringBuilder.Append(leftScoper);
			bool flag = true;
			foreach (object item in GetObjectStream(value))
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append(seperator);
				}
				stringBuilder.Append(SerializeRecursive(item, theme));
			}
			stringBuilder.Append(rightScoper);
			return _builderPool.ReleaseAndToString(stringBuilder);
		}

		protected abstract IEnumerable GetObjectStream(T value);
	}
}
