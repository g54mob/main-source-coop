using RootMotion.FinalIK;
using UnityEngine;

public class TestInteractionsStarter : MonoBehaviour
{
	public InteractionObject leftHandinteractionObject;

	public InteractionObject rightHandinteractionObject;

	private InteractionSystem interactionSystem;

	public FullBodyBipedEffector[] effectors;

	private void Awake()
	{
		interactionSystem = GetComponent<InteractionSystem>();
	}

	public void StartInteraction()
	{
		FullBodyBipedEffector[] array = effectors;
		foreach (FullBodyBipedEffector fullBodyBipedEffector in array)
		{
			switch (fullBodyBipedEffector)
			{
			case FullBodyBipedEffector.RightHand:
				interactionSystem.StartInteraction(fullBodyBipedEffector, rightHandinteractionObject, interrupt: true);
				break;
			case FullBodyBipedEffector.LeftHand:
				interactionSystem.StartInteraction(fullBodyBipedEffector, leftHandinteractionObject, interrupt: true);
				break;
			}
		}
	}
}
