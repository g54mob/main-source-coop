using FishNet.Connection;
using FishNet.Observing;
using UnityEngine;

namespace FishNet.Component.Observing
{
	[CreateAssetMenu(menuName = "FishNet/Observers/Scene Condition", fileName = "New Scene Condition")]
	public class SceneCondition : ObserverCondition
	{
		public void ConditionConstructor()
		{
		}

		public override bool ConditionMet(NetworkConnection connection, bool currentlyAdded, out bool notProcessed)
		{
			notProcessed = false;
			return connection.Scenes.Contains(NetworkObject.gameObject.scene);
		}

		public override ObserverConditionType GetConditionType()
		{
			return ObserverConditionType.Normal;
		}

		public override ObserverCondition Clone()
		{
			SceneCondition sceneCondition = ScriptableObject.CreateInstance<SceneCondition>();
			sceneCondition.ConditionConstructor();
			return sceneCondition;
		}
	}
}
