using System.Collections.Generic;

namespace PlayEveryWare.EpicOnlineServices
{
	public class LogLevelConfig : Config
	{
		public List<LogCategoryLevelPair> LogCategoryLevelPairs;

		static LogLevelConfig()
		{
			Config.RegisterFactory(() => new LogLevelConfig());
		}

		protected LogLevelConfig()
			: base("log_level_config.json")
		{
		}
	}
}
