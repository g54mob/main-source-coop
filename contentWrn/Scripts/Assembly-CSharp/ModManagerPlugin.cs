public class ModManagerPlugin
{
	public PluginLocal PluginLocal;

	public PluginSubscribed PluginSubscribed;

	public PluginPublished PluginPublished;

	public PluginExternal PluginExternal;

	private Plugin originalPlugin;

	public Plugin OriginalPlugin => originalPlugin;

	public string DisplayName
	{
		get
		{
			if (PluginPublished != null)
			{
				return PluginPublished.DisplayName;
			}
			if (PluginSubscribed != null)
			{
				return PluginSubscribed.DisplayName;
			}
			if (PluginLocal != null)
			{
				return PluginLocal.DisplayName;
			}
			if (PluginExternal != null)
			{
				return PluginExternal.DisplayName;
			}
			return "Unknown";
		}
	}

	public string Version
	{
		get
		{
			if (PluginLocal != null && PluginLocal.InfoFromAssembly.HasValue)
			{
				return PluginLocal.InfoFromAssembly.Value.version;
			}
			if (PluginPublished != null && PluginPublished.InfoFromAssembly.HasValue)
			{
				return PluginPublished.InfoFromAssembly.Value.version;
			}
			if (PluginSubscribed != null && PluginSubscribed.InfoFromAssembly.HasValue)
			{
				return PluginSubscribed.InfoFromAssembly.Value.version;
			}
			if (PluginExternal != null && PluginExternal.InfoFromAssembly.HasValue)
			{
				return PluginExternal.InfoFromAssembly.Value.version;
			}
			return "Unknown";
		}
	}

	public ModManagerPlugin(Plugin plugin)
	{
		originalPlugin = plugin;
		Update(plugin);
	}

	public void Update(Plugin plugin)
	{
		if (plugin is PluginLocal pluginLocal)
		{
			PluginLocal = pluginLocal;
		}
		else if (plugin is PluginSubscribed pluginSubscribed)
		{
			PluginSubscribed = pluginSubscribed;
		}
		else if (plugin is PluginPublished pluginPublished)
		{
			PluginPublished = pluginPublished;
		}
		else if (plugin is PluginExternal pluginExternal)
		{
			PluginExternal = pluginExternal;
		}
	}
}
