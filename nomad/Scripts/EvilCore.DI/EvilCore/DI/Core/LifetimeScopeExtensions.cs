using VContainer.Unity;

namespace EvilCore.DI.Core
{
	public static class LifetimeScopeExtensions
	{
		public static void RegisterWithContextManager(this LifetimeScope scope, string contextName, bool activateImmediately = false)
		{
			MonoSingleton<DIContextManager>.Instance.RegisterContext(contextName, scope);
			if (activateImmediately)
			{
				MonoSingleton<DIContextManager>.Instance.ActivateContext(contextName);
			}
		}
	}
}
