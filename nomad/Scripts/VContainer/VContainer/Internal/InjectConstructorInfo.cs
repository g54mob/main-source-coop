using System.Reflection;

namespace VContainer.Internal
{
	internal sealed class InjectConstructorInfo
	{
		public readonly ConstructorInfo ConstructorInfo;

		public readonly ParameterInfo[] ParameterInfos;

		public InjectConstructorInfo(ConstructorInfo constructorInfo)
		{
			ConstructorInfo = constructorInfo;
			ParameterInfos = constructorInfo.GetParameters();
		}

		public InjectConstructorInfo(ConstructorInfo constructorInfo, ParameterInfo[] parameterInfos)
		{
			ConstructorInfo = constructorInfo;
			ParameterInfos = parameterInfos;
		}
	}
}
