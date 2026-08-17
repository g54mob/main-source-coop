using System.Collections;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Networking;
using Mirror;
using NomadDrive.Sandbox.Test;
using UnityEngine;

namespace NomadDrive.Features.WorldGeneration
{
	public class TestNetworkManager : NetworkManager
	{
		[SerializeField]
		private float playerSpawnHeight = 10f;

		public override void Update()
		{
			if (Input.GetKeyDown(KeyCode.H))
			{
				StartHost();
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				StartClient();
			}
		}

		public override void OnServerAddPlayer(NetworkConnectionToClient conn)
		{
			GameObject player = Object.Instantiate(position: new Vector3(0f, playerSpawnHeight, 0f), original: playerPrefab, rotation: Quaternion.identity);
			NetworkServer.AddPlayerForConnection(conn, player);
			StartCoroutine(RelocatePlayerToStarterHouse(player));
		}

		private IEnumerator RelocatePlayerToStarterHouse(GameObject player)
		{
			float timeout = 10f;
			float elapsed = 0f;
			TestDummyPlayer playerComponent = player.GetComponent<TestDummyPlayer>();
			if (playerComponent == null)
			{
				EvilLogger.LogError("<color=red>[TestNetworkManager]</color> TestDummyPlayer component not found on spawned object!", "RelocatePlayerToStarterHouse", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\WorldGeneration\\Scripts\\_Core\\TestNetworkManager.cs", 53);
				yield break;
			}
			while (elapsed < timeout)
			{
				if (NetworkSingleton<WorldGenerator>.Instance != null)
				{
					Vector3 playerSpawnPoint = NetworkSingleton<WorldGenerator>.Instance.PlayerSpawnPoint;
					playerComponent.RpcTeleport(playerSpawnPoint);
					break;
				}
				elapsed += Time.deltaTime;
				yield return null;
			}
		}
	}
}
