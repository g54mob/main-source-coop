using VContainer;

namespace EvilCore.DI.Core
{
	public static class ScopedContainerExtensions
	{
		public static IContainerBuilder RegisterScopedContainer(this IContainerBuilder builder)
		{
			return (IContainerBuilder)builder.Register<ScopedContainer>(Lifetime.Singleton);
		}
	}
}
