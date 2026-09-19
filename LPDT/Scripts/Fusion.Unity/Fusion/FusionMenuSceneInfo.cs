using System.IO;
using UnityEngine;

namespace Fusion
{
	[CreateAssetMenu(menuName = "Fusion/Menu/Menu Scene Info")]
	public class FusionMenuSceneInfo : FusionScriptableObject
	{
		public string Name;

		[ScenePath]
		public string ScenePath;

		public Sprite Preview;

		public bool IsHidden;

		public string NameOrSceneName
		{
			get
			{
				if (!string.IsNullOrEmpty(Name))
				{
					return Name;
				}
				return SceneName;
			}
		}

		public string SceneName
		{
			get
			{
				if (ScenePath != null)
				{
					return Path.GetFileNameWithoutExtension(ScenePath);
				}
				return null;
			}
		}
	}
}
