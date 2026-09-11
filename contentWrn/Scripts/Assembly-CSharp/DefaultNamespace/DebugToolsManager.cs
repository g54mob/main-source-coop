using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using Zorro.Core;

namespace DefaultNamespace
{
	public class DebugToolsManager : RetrievableResourceSingleton<DebugToolsManager>
	{
		[Header("Debugging")]
		public string targetScene = string.Empty;

		[Header("Monster Spawning")]
		public string[] monsterNames;

		public InputActionReference[] monsterInput;

		private bool m_shouldToggleLights;

		private bool m_lightEnabled;

		public bool LightsEnabled => m_lightEnabled;

		protected override void OnCreated()
		{
			base.OnCreated();
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			Debug.Log("Created ToolsManager");
		}

		public void Initialize(Func<bool> isSteamDeck)
		{
			m_lightEnabled = true;
			m_shouldToggleLights = false;
			Debug.Log("Initialized ToolsManager");
		}

		private void LateUpdate()
		{
		}

		private void TeleportToDiveBell()
		{
			DivingBell divingBell = UnityEngine.Object.FindObjectOfType<DivingBell>();
			if (divingBell == null)
			{
				return;
			}
			Transform transform = divingBell.transform.Find("Spawns");
			if (transform == null)
			{
				return;
			}
			List<Player> players = PlayerHandler.instance.players;
			for (int i = 0; i < players.Count; i++)
			{
				if (players[i].IsLocal)
				{
					Transform child = transform.GetChild(0);
					players[i].Teleport(child.position, child.forward);
					Debug.Log("Teleporting Player to Diving Bell");
					break;
				}
			}
		}

		private void SpawnMonster()
		{
			string text = null;
			for (int i = 0; i < monsterInput.Length; i++)
			{
				if (monsterInput.Length > i && monsterNames.Length > i && monsterInput[i].action.WasPressedThisFrame())
				{
					text = monsterNames[i];
					break;
				}
			}
			if (!string.IsNullOrEmpty(text) && PhotonNetwork.IsMasterClient)
			{
				Debug.Log("Spawning Monster " + text);
				MonsterSpawner.SpawnMonster(text);
			}
		}

		private void ToggleLights()
		{
			Debug.Log("Toggle Lights");
			m_lightEnabled = !m_lightEnabled;
			Debug.Log("Lights enabled: " + m_lightEnabled);
			Light[] array = UnityEngine.Object.FindObjectsOfType<Light>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = m_lightEnabled;
			}
		}

		private void ToggleShadows()
		{
			Debug.Log("Toggle Shadows");
			UniversalAdditionalCameraData universalAdditionalCameraData = Camera.main.GetUniversalAdditionalCameraData();
			universalAdditionalCameraData.renderShadows = !universalAdditionalCameraData.renderShadows;
		}
	}
}
