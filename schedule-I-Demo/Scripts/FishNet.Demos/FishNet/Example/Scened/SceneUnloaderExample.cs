using FishNet.Managing.Logging;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;

namespace FishNet.Example.Scened
{
	public class SceneUnloaderExample : MonoBehaviour
	{
		[Tooltip("Scenes to unload.")]
		[SerializeField]
		private string[] _scenes = new string[0];

		[Tooltip("True to only unload for the connectioning causing the trigger.")]
		[SerializeField]
		private bool _connectionOnly;

		[Tooltip("True to unload unused scenes.")]
		[SerializeField]
		private bool _unloadUnused = true;

		[Tooltip("True to fire when entering the trigger. False to fire when exiting the trigger.")]
		[SerializeField]
		private bool _onTriggerEnter = true;

		[Server(Logging = LoggingType.Off)]
		private void OnTriggerEnter(Collider other)
		{
			if (InstanceFinder.IsServer && _onTriggerEnter)
			{
				UnloadScenes(other.gameObject.GetComponent<NetworkObject>());
			}
		}

		[Server(Logging = LoggingType.Off)]
		private void OnTriggerExit(Collider other)
		{
			if (InstanceFinder.IsServer && !_onTriggerEnter)
			{
				UnloadScenes(other.gameObject.GetComponent<NetworkObject>());
			}
		}

		private void UnloadScenes(NetworkObject triggeringIdentity)
		{
			if (InstanceFinder.NetworkManager.IsServer && !(triggeringIdentity == null))
			{
				UnloadOptions options = new UnloadOptions
				{
					Mode = ((!_unloadUnused) ? UnloadOptions.ServerUnloadMode.KeepUnused : UnloadOptions.ServerUnloadMode.UnloadUnused)
				};
				SceneUnloadData sceneUnloadData = new SceneUnloadData(_scenes);
				sceneUnloadData.Options = options;
				if (_connectionOnly)
				{
					InstanceFinder.SceneManager.UnloadConnectionScenes(triggeringIdentity.Owner, sceneUnloadData);
				}
				else
				{
					InstanceFinder.SceneManager.UnloadGlobalScenes(sceneUnloadData);
				}
			}
		}
	}
}
