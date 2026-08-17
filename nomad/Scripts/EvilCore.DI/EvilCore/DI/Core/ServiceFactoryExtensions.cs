using VContainer;

namespace EvilCore.DI.Core
{
	public static class ServiceFactoryExtensions
	{
		public static IContainerBuilder RegisterFactory<TProduct>(this IContainerBuilder builder) where TProduct : class
		{
			return (IContainerBuilder)builder.Register<ServiceFactory<TProduct>>(Lifetime.Singleton);
		}
	}
}
