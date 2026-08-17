using UnityEngine;

[CreateAssetMenu(fileName = "PenguinActions", menuName = "Actions/Idle", order = 1)]
public class IdleAction : PenguinAction
{
	public override void Initialize()
	{
		base.Initialize();
		PenguinInputController.OnEmotePlayed.AddListener(PlayEmote);
	}

	public override void Enter()
	{
		base.Enter();
		Controller.SetAnimationInt("IdleState", 0);
	}

	public void PlayEmote(int newEmote, PenguinActionsController controller)
	{
		if (!(controller != Controller))
		{
			switch (newEmote)
			{
			case 3:
				Controller.PlayEmote("SS");
				break;
			case 4:
				Controller.PlayEmote("Timer");
				break;
			default:
				Controller.SetAnimationInt("IdleState", newEmote);
				break;
			}
		}
	}

	public override void Clean()
	{
		base.Clean();
		PenguinInputController.OnEmotePlayed?.RemoveListener(PlayEmote);
	}
}
