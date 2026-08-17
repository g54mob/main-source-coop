using VContainer;

namespace EvilCore.DI.Core
{
	public static class TaggedServiceRegistryExtensions
	{
		public static IContainerBuilder RegisterTaggedServiceRegistry<TService>(this IContainerBuilder builder) where TService : class
		{
			return (IContainerBuilder)builder.Register<TaggedServiceRegistry<TService>>(Lifetime.Singleton);
		}
	}
}
