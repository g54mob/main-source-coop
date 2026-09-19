using System.Collections.Generic;
using UnityEngine;

namespace RSG.Muffin.SceneLoaderSubmodule.SceneLoaderModule.Scripts
{
	[CreateAssetMenu(fileName = "SceneLoaderConfig_Default", menuName = "Configurations/SceneLoader/SceneLoaderConfig")]
	public class SceneLoaderConfig : ScriptableObject
	{
		[field: SerializeField]
		public List<string> SceneAddressablesInGroup { get; set; }
	}
}
