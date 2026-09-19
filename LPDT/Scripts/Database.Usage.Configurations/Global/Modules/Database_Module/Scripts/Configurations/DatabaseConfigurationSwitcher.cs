using UnityEngine;

namespace Global.Modules.Database_Module.Scripts.Configurations
{
	[CreateAssetMenu(menuName = "Configurations/Database/DatabaseConfigurationSwitcher", fileName = "DatabaseConfigurationSwitcher_Default", order = 0)]
	public class DatabaseConfigurationSwitcher : ScriptableObject
	{
		[field: SerializeField]
		public bool IsOnlineDatabase { get; private set; }
	}
}
