using UnityEngine;
using UnityEngine.Serialization;

namespace Features.AerohockeyBeachInteractableModule.Scripts
{
	public class AerohockeyGoalTrigger : MonoBehaviour
	{
		[FormerlySerializedAs("_scoringSide")]
		[SerializeField]
		private AerohockeySide _goalSide;

		[SerializeField]
		private BoxCollider _trigger;

		public AerohockeySide GoalSide => _goalSide;

		public BoxCollider Trigger => _trigger;
	}
}
