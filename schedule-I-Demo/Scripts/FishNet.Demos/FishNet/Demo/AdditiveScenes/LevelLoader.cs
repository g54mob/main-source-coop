using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;

namespace FishNet.Demo.AdditiveScenes
{
	public class LevelLoader : NetworkBehaviour
	{
		private bool NetworkInitialize___EarlyFishNet_002EDemo_002EAdditiveScenes_002ELevelLoaderFishNet_002EDemos_002Edll_Excuted;

		private bool NetworkInitialize__LateFishNet_002EDemo_002EAdditiveScenes_002ELevelLoaderFishNet_002EDemos_002Edll_Excuted;

		private void OnTriggerEnter(Collider other)
		{
			if (base.IsServer)
			{
				Player playerOwnedObject = GetPlayerOwnedObject(other);
				if (!(playerOwnedObject == null))
				{
					SceneLookupData sceneLookupData = new SceneLookupData(base.gameObject.scene);
					SceneLoadData sceneLoadData = new SceneLoadData(sceneLookupData);
					sceneLoadData.Options = new LoadOptions
					{
						AutomaticallyUnload = false
					};
					sceneLoadData.MovedNetworkObjects = new NetworkObject[1] { playerOwnedObject.NetworkObject };
					sceneLoadData.ReplaceScenes = ReplaceOption.None;
					sceneLoadData.PreferredActiveScene = sceneLookupData;
					SceneLoadData sceneLoadData2 = sceneLoadData;
					base.SceneManager.LoadConnectionScenes(playerOwnedObject.Owner, sceneLoadData2);
				}
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (base.IsServer)
			{
				Player playerOwnedObject = GetPlayerOwnedObject(other);
				if (!(playerOwnedObject == null))
				{
					SceneUnloadData sceneUnloadData = new SceneUnloadData(new SceneLookupData(base.gameObject.scene))
					{
						Options = new UnloadOptions
						{
							Mode = UnloadOptions.ServerUnloadMode.KeepUnused
						}
					};
					base.SceneManager.UnloadConnectionScenes(playerOwnedObject.Owner, sceneUnloadData);
				}
			}
		}

		private Player GetPlayerOwnedObject(Collider other)
		{
			Player component = other.GetComponent<Player>();
			if (component == null)
			{
				return null;
			}
			if (!component.Owner.IsActive)
			{
				return null;
			}
			return component;
		}

		public virtual void NetworkInitialize___Early()
		{
			if (!NetworkInitialize___EarlyFishNet_002EDemo_002EAdditiveScenes_002ELevelLoaderFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize___EarlyFishNet_002EDemo_002EAdditiveScenes_002ELevelLoaderFishNet_002EDemos_002Edll_Excuted = true;
			}
		}

		public virtual void NetworkInitialize__Late()
		{
			if (!NetworkInitialize__LateFishNet_002EDemo_002EAdditiveScenes_002ELevelLoaderFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize__LateFishNet_002EDemo_002EAdditiveScenes_002ELevelLoaderFishNet_002EDemos_002Edll_Excuted = true;
			}
		}

		public override void NetworkInitializeIfDisabled()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}

		public virtual void Awake()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}
	}
}
