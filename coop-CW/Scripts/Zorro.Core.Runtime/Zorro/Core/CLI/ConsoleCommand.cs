using System.Reflection;

namespace Zorro.Core.CLI
{
	public struct ConsoleCommand
	{
		public string Command;

		public string DomainName;

		public MethodInfo MethodInfo;

		public ParameterInfo[] ParameterInfo;

		public ConsoleCommand(MethodInfo methodInfo)
		{
			Command = methodInfo.Name;
			DomainName = methodInfo.DeclaringType.Name;
			ConsoleClassCustomizerAttribute customAttribute = methodInfo.DeclaringType.GetCustomAttribute<ConsoleClassCustomizerAttribute>();
			if (customAttribute != null)
			{
				DomainName = customAttribute.NewDomainName;
			}
			ParameterInfo = methodInfo.GetParameters();
			MethodInfo = methodInfo;
		}
	}
}
