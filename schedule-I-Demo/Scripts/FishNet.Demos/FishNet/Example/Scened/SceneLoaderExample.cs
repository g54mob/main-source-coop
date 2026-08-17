using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Managing.Logging;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;

namespace FishNet.Example.Scened
{
	public class SceneLoaderExample : MonoBehaviour
	{
		[Tooltip("True to move the triggering object.")]
		[SerializeField]
		private bool _moveObject = true;

		[Tooltip("True to move all connection objects (clients).")]
		[SerializeField]
		private bool _moveAllObjects;

		[Tooltip("True to replace current scenes with new scenes. First scene loaded will become active scene.")]
		[SerializeField]
		private ReplaceOption _replaceOption = ReplaceOption.None;

		[Tooltip("Scenes to load.")]
		[SerializeField]
		private string[] _scenes = new string[0];

		[Tooltip("True to only unload for the connectioning causing the trigger.")]
		[SerializeField]
		private bool _connectionOnly;

		[Tooltip("True to automatically unload the loaded scenes when no more connections are using them.")]
		[SerializeField]
		private bool _automaticallyUnload = true;

		[Tooltip("True to fire when entering the trigger. False to fire when exiting the trigger.")]
		[SerializeField]
		private bool _onTriggerEnter = true;

		private Dictionary<NetworkConnection, float> _triggeredTimes = new Dictionary<NetworkConnection, float>();

		[Server(Logging = LoggingType.Off)]
		private void OnTriggerEnter(Collider other)
		{
			if (InstanceFinder.IsServer && _onTriggerEnter)
			{
				LoadScene(other.GetComponent<NetworkObject>());
			}
		}

		[Server(Logging = LoggingType.Off)]
		private void OnTriggerExit(Collider other)
		{
			if (InstanceFinder.IsServer && !_onTriggerEnter)
			{
				LoadScene(other.GetComponent<NetworkObject>());
			}
		}

		private void LoadScene(NetworkObject triggeringIdentity)
		{
			if (!InstanceFinder.NetworkManager.IsServer || triggeringIdentity == null || (_triggeredTimes.TryGetValue(triggeringIdentity.Owner, out var value) && Time.time - value < 0.5f))
			{
				return;
			}
			_triggeredTimes[triggeringIdentity.Owner] = Time.time;
			List<NetworkObject> list = new List<NetworkObject>();
			if (_moveAllObjects)
			{
				foreach (NetworkConnection value2 in InstanceFinder.ServerManager.Clients.Values)
				{
					foreach (NetworkObject @object in value2.Objects)
					{
						list.Add(@object);
					}
				}
			}
			else if (_moveObject)
			{
				list.Add(triggeringIdentity);
			}
			LoadOptions options = new LoadOptions
			{
				AutomaticallyUnload = _automaticallyUnload
			};
			SceneLoadData sceneLoadData = new SceneLoadData(_scenes);
			sceneLoadData.PreferredActiveScene = sceneLoadData.SceneLookupDatas[0];
			sceneLoadData.ReplaceScenes = _replaceOption;
			sceneLoadData.Options = options;
			sceneLoadData.MovedNetworkObjects = list.ToArray();
			if (_connectionOnly)
			{
				InstanceFinder.SceneManager.LoadConnectionScenes(triggeringIdentity.Owner, sceneLoadData);
			}
			else
			{
				InstanceFinder.SceneManager.LoadGlobalScenes(sceneLoadData);
			}
		}
	}
}
