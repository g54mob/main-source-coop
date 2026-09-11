using UnityEngine;

public class FootstepHandler : MonoBehaviour
{
	public float overrideStepNoiseDistance = 15f;

	public float noiseSprintMultiplier = 1f;

	public float noiseWalkMultiplier = 0.66f;

	public float noiseCrouchMultiplier = 0.33f;

	public bool isPlayerFootSteps;

	public bool careForSurface;

	public int stepInt;

	public Transform main;

	private Player player;

	public bool step;

	public SFXInstanceCollection[] stepSound;

	public SFX_Instance[] extra;

	private Bodypart hip;

	private bool t;

	private void Start()
	{
		player = main.GetComponent<Player>();
		hip = player.refs.ragdoll.GetBodypart(BodypartType.Hip);
	}

	private void LateUpdate()
	{
		if (!player)
		{
			return;
		}
		base.transform.position = hip.rig.transform.position;
		if (stepSound.Length == 0)
		{
			return;
		}
		stepInt = 0;
		if (careForSurface)
		{
			SetInt();
		}
		if (!step)
		{
			t = false;
		}
		if (step && !t)
		{
			float stepNoiseMultiplier = noiseWalkMultiplier;
			int alerts = 1;
			if (player.data.isCrouching)
			{
				stepNoiseMultiplier = noiseCrouchMultiplier;
			}
			else if (player.data.isSprinting)
			{
				alerts = 2;
				stepNoiseMultiplier = noiseSprintMultiplier;
			}
			for (int i = 0; i < stepSound[stepInt].instances.Length; i++)
			{
				SFX_Player.instance.PlaySFX(stepSound[stepInt].instances[i], base.transform.position, null, null, 1f, loop: false, local: false, isPlayerFootSteps, stepNoiseMultiplier, alerts);
			}
			for (int j = 0; j < extra.Length; j++)
			{
				SFX_Player.instance.PlaySFX(extra[j], base.transform.position, null, null, 1f, loop: false, local: false, isPlayerFootSteps, stepNoiseMultiplier, alerts);
			}
			t = true;
		}
	}

	private void SetInt()
	{
		if (player.data.groundTag == "Stone 1")
		{
			stepInt = 0;
		}
		if (player.data.groundTag == "Stone 2")
		{
			stepInt = 1;
		}
		if (player.data.groundTag == "Metal 1")
		{
			stepInt = 2;
		}
		if (player.data.groundTag == "Metal 2")
		{
			stepInt = 3;
		}
		if (player.data.groundTag == "Metal 3")
		{
			stepInt = 4;
		}
		if (player.data.groundTag == "Metal 4")
		{
			stepInt = 5;
		}
		if (player.data.groundTag == "Metal 5")
		{
			stepInt = 6;
		}
		if (player.data.groundTag == "Wood 1")
		{
			stepInt = 7;
		}
		if (player.data.groundTag == "Wood 2")
		{
			stepInt = 8;
		}
		if (player.data.groundTag == "Wood 3")
		{
			stepInt = 9;
		}
		if (player.data.groundTag == "Grass")
		{
			stepInt = 10;
		}
		if (player.data.groundTag == "Dirt")
		{
			stepInt = 11;
		}
		if (player.data.groundTag == "Gravel")
		{
			stepInt = 12;
		}
		if (player.data.groundTag == "Sand")
		{
			stepInt = 13;
		}
		if (player.data.groundTag == "Cloth")
		{
			stepInt = 14;
		}
	}
}
