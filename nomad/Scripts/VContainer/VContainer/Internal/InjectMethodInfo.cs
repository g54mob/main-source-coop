using System.Reflection;

namespace VContainer.Internal
{
	internal sealed class InjectMethodInfo
	{
		public readonly MethodInfo MethodInfo;

		public readonly ParameterInfo[] ParameterInfos;

		public InjectMethodInfo(MethodInfo methodInfo)
		{
			MethodInfo = methodInfo;
			ParameterInfos = methodInfo.GetParameters();
		}
	}
}
