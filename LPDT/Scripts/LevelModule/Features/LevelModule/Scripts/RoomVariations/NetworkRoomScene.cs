using System;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.LevelModule.Scripts.RoomVariations
{
	[NetworkBehaviourWeaved(1)]
	public class NetworkRoomScene : NetworkBehaviour
	{
		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("OwnerLevel", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private LevelType _OwnerLevel;

		private LevelsConfiguration _levelsConfiguration;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe LevelType OwnerLevel
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing NetworkRoomScene.OwnerLevel. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(LevelType*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing NetworkRoomScene.OwnerLevel. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(LevelType*)((byte*)Ptr + 0) = value;
			}
		}

		[Inject]
		public void InjectDependencies(LevelsConfiguration levelsConfiguration)
		{
			_levelsConfiguration = levelsConfiguration;
		}

		public override void Spawned()
		{
			if (_levelsConfiguration.LevelsPool.TryGetValue(OwnerLevel, out var value))
			{
				SceneRef sceneRef = base.Runner.SceneManager.GetSceneRef(value);
				base.Runner.MoveGameObjectToScene(base.gameObject, sceneRef);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			OwnerLevel = _OwnerLevel;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_OwnerLevel = OwnerLevel;
		}
	}
}
