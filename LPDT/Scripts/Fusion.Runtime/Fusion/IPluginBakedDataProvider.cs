namespace Fusion
{
	public interface IPluginBakedDataProvider
	{
		object Bake(in PluginBakedDataContext context);
	}
	public interface IPluginBakedDataProvider<T> : IPluginBakedDataProvider
	{
		new T Bake(in PluginBakedDataContext context);

		object IPluginBakedDataProvider.Bake(in PluginBakedDataContext context)
		{
			return Bake(in context);
		}
	}
}
