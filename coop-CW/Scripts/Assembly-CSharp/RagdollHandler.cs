using System.Collections.Generic;
using UnityEngine;
using Zorro.Core;

[DefaultExecutionOrder(-1)]
public class RagdollHandler : Singleton<RagdollHandler>
{
	private List<PlayerRagdoll> ragdolls = new List<PlayerRagdoll>();

	public static void RegisterRagdoll(PlayerRagdoll playerRagdoll)
	{
		Singleton<RagdollHandler>.Instance.ragdolls.Add(playerRagdoll);
	}

	public static void UnregisterRagdoll(PlayerRagdoll playerRagdoll)
	{
		if (!(Singleton<RagdollHandler>.Instance == null))
		{
			Singleton<RagdollHandler>.Instance.ragdolls.Remove(playerRagdoll);
		}
	}

	private void FixedUpdate()
	{
		for (int i = 0; i < ragdolls.Count; i++)
		{
			ragdolls[i].FixedUpdateHandled();
		}
	}
}
