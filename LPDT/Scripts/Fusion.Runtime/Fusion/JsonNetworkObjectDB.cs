using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Fusion
{
	[Serializable]
	public static class JsonNetworkObjectDB
	{
		[Serializable]
		public class ObjectDataBase
		{
			public string TypeName;

			public string Id;

			[SerializeReference]
			public object UserData;
		}

		[Serializable]
		[DebuggerDisplay("{TypeName}")]
		public class NetworkBehaviourData : ObjectDataBase
		{
			public int WordCount;

			public string Data;
		}

		[Serializable]
		[DebuggerDisplay("{Name}")]
		public class NetworkObjectData : ObjectDataBase
		{
			public string Name;

			public List<NetworkBehaviourData> NetworkedBehaviours = new List<NetworkBehaviourData>();

			public NetworkObjectFlags Flags;
		}

		[Serializable]
		[DebuggerDisplay("{Name}")]
		public class PrefabNetworkObjectData : NetworkObjectData
		{
			public int NestedObjectCount;
		}

		[Serializable]
		[DebuggerDisplay("{ScenePath}")]
		public class SceneData
		{
			public string UnityAssetGuid;

			public string SceneRef;

			public string ScenePath;

			public NetworkObjectData[] Objects;

			[SerializeReference]
			public object UserData;
		}

		[Serializable]
		[DebuggerDisplay("{UnityAssetGuid} ({UnityAssetPath})")]
		public class PrefabData
		{
			public string UnityAssetPath;

			public string UnityAssetGuid;

			public PrefabNetworkObjectData[] Objects;

			[SerializeReference]
			public object UserData;
		}

		[Serializable]
		[DebuggerDisplay("{UnityAssetGuid}-{UnityFileId} ({UnityAssetPath})")]
		public class ScriptableObjectData
		{
			public string UnityAssetPath;

			public string UnityAssetGuid;

			public long UnityFileId;

			public string Id;

			public string TypeName;

			public string Name;

			public string Data;

			[SerializeReference]
			public object UserData;
		}
	}
}
