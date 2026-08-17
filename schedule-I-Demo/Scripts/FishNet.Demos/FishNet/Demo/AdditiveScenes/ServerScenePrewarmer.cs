using FishNet.Managing.Scened;
using FishNet.Object;
using GameKit.Utilities.Types;
using UnityEngine;

namespace FishNet.Demo.AdditiveScenes
{
	public class ServerScenePrewarmer : NetworkBehaviour
	{
		[SerializeField]
		[Scene]
		private string[] _scenes = new string[0];

		private bool NetworkInitialize___EarlyFishNet_002EDemo_002EAdditiveScenes_002EServerScenePrewarmerFishNet_002EDemos_002Edll_Excuted;

		private bool NetworkInitialize__LateFishNet_002EDemo_002EAdditiveScenes_002EServerScenePrewarmerFishNet_002EDemos_002Edll_Excuted;

		public override void OnStartServer()
		{
			string[] scenes = _scenes;
			for (int i = 0; i < scenes.Length; i++)
			{
				SceneLoadData sceneLoadData = new SceneLoadData(new SceneLookupData(scenes[i]))
				{
					Options = new LoadOptions
					{
						AutomaticallyUnload = false
					}
				};
				base.SceneManager.LoadConnectionScenes(sceneLoadData);
			}
		}

		public virtual void NetworkInitialize___Early()
		{
			if (!NetworkInitialize___EarlyFishNet_002EDemo_002EAdditiveScenes_002EServerScenePrewarmerFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize___EarlyFishNet_002EDemo_002EAdditiveScenes_002EServerScenePrewarmerFishNet_002EDemos_002Edll_Excuted = true;
			}
		}

		public virtual void NetworkInitialize__Late()
		{
			if (!NetworkInitialize__LateFishNet_002EDemo_002EAdditiveScenes_002EServerScenePrewarmerFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize__LateFishNet_002EDemo_002EAdditiveScenes_002EServerScenePrewarmerFishNet_002EDemos_002Edll_Excuted = true;
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
