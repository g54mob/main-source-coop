using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror.Examples.MultipleAdditiveScenes
{
	[AddComponentMenu("")]
	public class MultiSceneNetManager : NetworkManager
	{
		[Header("Spawner Setup")]
		[Tooltip("Reward Prefab for the Spawner")]
		public GameObject rewardPrefab;

		public byte poolSize = 20;

		[Header("MultiScene Setup")]
		public int instances = 3;

		[Scene]
		public string gameScene;

		private bool subscenesLoaded;

		private readonly List<Scene> subScenes = new List<Scene>();

		private int clientIndex;

		public override void OnServerAddPlayer(NetworkConnectionToClient conn)
		{
			StartCoroutine(OnServerAddPlayerDelayed(conn));
		}

		private IEnumerator OnServerAddPlayerDelayed(NetworkConnectionToClient conn)
		{
			while (!subscenesLoaded)
			{
				yield return null;
			}
			conn.Send(new SceneMessage
			{
				sceneName = gameScene,
				sceneOperation = SceneOperation.LoadAdditive
			});
			yield return new WaitForEndOfFrame();
			Transform startPosition = GetStartPosition();
			GameObject gameObject = ((startPosition != null) ? Object.Instantiate(playerPrefab, startPosition.position, startPosition.rotation) : Object.Instantiate(playerPrefab));
			gameObject.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
			PlayerScore component = gameObject.GetComponent<PlayerScore>();
			component.NetworkplayerNumber = clientIndex;
			component.NetworkscoreIndex = clientIndex / subScenes.Count;
			component.NetworkmatchIndex = clientIndex % subScenes.Count;
			if (subScenes.Count > 0)
			{
				SceneManager.MoveGameObjectToScene(gameObject, subScenes[clientIndex % subScenes.Count]);
			}
			NetworkServer.AddPlayerForConnection(conn, gameObject);
			clientIndex++;
		}

		public override void OnStartServer()
		{
			StartCoroutine(ServerLoadSubScenes());
		}

		private IEnumerator ServerLoadSubScenes()
		{
			for (int index = 1; index <= instances; index++)
			{
				yield return SceneManager.LoadSceneAsync(gameScene, new LoadSceneParameters
				{
					loadSceneMode = LoadSceneMode.Additive,
					localPhysicsMode = LocalPhysicsMode.Physics3D
				});
				Scene sceneAt = SceneManager.GetSceneAt(index);
				subScenes.Add(sceneAt);
			}
			Spawner.InitializePool(rewardPrefab, poolSize);
			foreach (Scene subScene in subScenes)
			{
				if (subScene.IsValid())
				{
					Spawner.InitialSpawn(subScene);
				}
			}
			subscenesLoaded = true;
		}

		public override void OnStopServer()
		{
			NetworkServer.SendToAll(new SceneMessage
			{
				sceneName = gameScene,
				sceneOperation = SceneOperation.UnloadAdditive
			});
			if (base.gameObject.activeSelf)
			{
				StartCoroutine(ServerUnloadSubScenes());
			}
			Spawner.ClearPool();
			clientIndex = 0;
		}

		private IEnumerator ServerUnloadSubScenes()
		{
			for (int index = 0; index < subScenes.Count; index++)
			{
				if (subScenes[index].IsValid())
				{
					yield return SceneManager.UnloadSceneAsync(subScenes[index]);
				}
			}
			subScenes.Clear();
			subscenesLoaded = false;
			yield return Resources.UnloadUnusedAssets();
		}

		public override void OnClientSceneChanged()
		{
			if (!NetworkServer.active && SceneManager.sceneCount > 1)
			{
				Spawner.InitializePool(rewardPrefab, poolSize);
			}
			base.OnClientSceneChanged();
		}

		private IEnumerator ClientUnloadSubScenes()
		{
			for (int index = 0; index < SceneManager.sceneCount; index++)
			{
				if (SceneManager.GetSceneAt(index) != SceneManager.GetActiveScene())
				{
					yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(index));
				}
			}
		}

		public override void OnStopClient()
		{
			if (!NetworkServer.active)
			{
				Spawner.ClearPool();
			}
			if (base.mode == NetworkManagerMode.Offline && base.gameObject.activeSelf)
			{
				StartCoroutine(ClientUnloadSubScenes());
			}
		}
	}
}
