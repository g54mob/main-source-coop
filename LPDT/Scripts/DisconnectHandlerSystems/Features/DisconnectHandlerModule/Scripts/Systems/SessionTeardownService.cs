using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using Features.DisconnectHandlerModule.Scripts.Data;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using RSG.Muffin.SceneLoaderSubmodule.SceneLoaderModule.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Features.DisconnectHandlerModule.Scripts.Systems
{
	public class SessionTeardownService : ISessionTeardownService
	{
		private const string ANCHOR_SCENE_NAME = "__SessionTeardownAnchor";

		private static readonly TimeSpan _shutdownTimeout = TimeSpan.FromSeconds(5.0);

		private static readonly TimeSpan _unloadTimeout = TimeSpan.FromSeconds(10.0);

		private readonly IMultiplayerService _multiplayerService;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly ISceneLoaderService _sceneLoaderService;

		public SessionTeardownService(IMultiplayerService multiplayerService, MultiplayerModel multiplayerModel, ISceneLoaderService sceneLoaderService)
		{
			_multiplayerService = multiplayerService;
			_multiplayerModel = multiplayerModel;
			_sceneLoaderService = sceneLoaderService;
		}

		public async UniTask TearDownAsync()
		{
			await ShutdownRunnerAsync();
			if (SceneLoadGuard.CanPerformSceneOperations)
			{
				if (ProjectContext.Instance != null)
				{
					UnityEngine.Object.Destroy(ProjectContext.Instance.gameObject);
				}
				Scene anchor = SceneManager.CreateScene("__SessionTeardownAnchor");
				SceneManager.SetActiveScene(anchor);
				CreateAnchorCamera();
				await UnloadAllExceptAsync(anchor);
				VerifyTornDown(anchor);
				await ReloadBootstrapAsync();
			}
		}

		private async UniTask ShutdownRunnerAsync()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return;
			}
			try
			{
				await _multiplayerService.Shutdown(networkRunner, ShutdownReason.Ok).Timeout(_shutdownTimeout, DelayType.Realtime);
			}
			catch (Exception arg)
			{
				Debug.LogError($"[SessionTeardown] runner shutdown did not complete within {_shutdownTimeout.TotalSeconds:0}s — " + $"continuing teardown; the explicit scene sweep will backstop whatever Fusion left behind. {arg}");
			}
		}

		private void CreateAnchorCamera()
		{
			Camera camera = new GameObject("__TeardownCamera").AddComponent<Camera>();
			camera.clearFlags = CameraClearFlags.Color;
			camera.backgroundColor = Color.black;
			camera.cullingMask = 0;
		}

		private async UniTask UnloadAllExceptAsync(Scene keep)
		{
			List<Scene> list = new List<Scene>();
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				if (sceneAt.handle != keep.handle && sceneAt.IsValid() && sceneAt.isLoaded)
				{
					list.Add(sceneAt);
				}
			}
			foreach (Scene scene in list)
			{
				Debug.Log($"[SessionTeardown] unloading residual scene '{scene.name}' (roots={scene.GetRootGameObjects().Length}).");
				try
				{
					await SceneManager.UnloadSceneAsync(scene).ToUniTask().Timeout(_unloadTimeout, DelayType.Realtime);
				}
				catch (Exception arg)
				{
					Debug.LogError($"[SessionTeardown] FAILED to unload residual scene '{scene.name}' — it is leaking outside the session. {arg}");
				}
			}
		}

		private void VerifyTornDown(Scene anchor)
		{
			int sceneCount = SceneManager.sceneCount;
			bool num = sceneCount == 1 && SceneManager.GetActiveScene().handle == anchor.handle;
			SceneContext[] array = UnityEngine.Object.FindObjectsByType<SceneContext>(FindObjectsSortMode.None);
			NetworkObject[] array2 = UnityEngine.Object.FindObjectsByType<NetworkObject>(FindObjectsSortMode.None);
			if (num && array.Length == 0 && array2.Length == 0)
			{
				Debug.Log("[SessionTeardown] VERIFY OK — only the anchor scene remains, every SceneContext was disposed with its scene, and no networked objects survive.");
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				stringBuilder.Append($"\n  scene '{sceneAt.name}' (roots={sceneAt.GetRootGameObjects().Length})");
			}
			for (int j = 0; j < array.Length; j++)
			{
				stringBuilder.Append("\n  live SceneContext on scene '" + array[j].gameObject.scene.name + "'");
			}
			int num2 = Math.Min(array2.Length, 30);
			for (int k = 0; k < num2; k++)
			{
				stringBuilder.Append("\n  live NetworkObject '" + array2[k].name + "' in scene '" + array2[k].gameObject.scene.name + "'");
			}
			if (array2.Length > num2)
			{
				stringBuilder.Append($"\n  … and {array2.Length - num2} more NetworkObject(s)");
			}
			Debug.LogError($"[SessionTeardown] VERIFY FAILED — teardown left state behind: {sceneCount} scene(s) loaded " + $"(expected 1: the anchor), {array.Length} SceneContext(s) alive (expected 0), " + $"{array2.Length} NetworkObject(s) alive (expected 0).{stringBuilder}");
		}

		private async UniTask ReloadBootstrapAsync()
		{
			try
			{
				if (DisconnectTeardownTestHooks.ForceReloadFault)
				{
					throw new InvalidOperationException("[SessionTeardown] forced reload fault (E2E hook): bootstrap-scene reload aborted.");
				}
				await _sceneLoaderService.LoadSceneAsync("BootstrapScene", unloadRedundant: true);
				Debug.Log("[SessionTeardown] bootstrap-scene reload OK — only Bootstrap remains, teardown complete.");
			}
			catch (Exception) when (!SceneLoadGuard.CanPerformSceneOperations)
			{
			}
			catch (Exception arg)
			{
				Debug.LogError("[SessionTeardown] bootstrap-scene reload FAILED — but the session scenes were already unloaded before the " + $"load, so no broken session state leaked; the app is parked on the teardown anchor. {arg}");
			}
		}
	}
}
