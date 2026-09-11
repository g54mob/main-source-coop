using UnityEngine;
using UnityEngine.UIElements;

namespace Zorro.Core
{
	public class CoreGlobalDependencies : SingletonAsset<CoreGlobalDependencies>
	{
		public StyleSheet DebugPageStyleSheets;

		public GameObject ConsolePrefab;
	}
}
