using VContainer;

namespace EvilCore.DI.Core
{
	public static class LazyLoaderExtensions
	{
		public static IContainerBuilder RegisterLazy<T>(this IContainerBuilder builder) where T : class
		{
			return (IContainerBuilder)builder.Register<LazyLoader<T>>(Lifetime.Singleton);
		}
	}
}
