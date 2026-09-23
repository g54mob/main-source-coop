using System.Collections.Generic;
using System.Threading.Tasks;
using Mimicraft.Customization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mimicraft.Networking
{
	public static class SessionScenes
	{
		private const float SettleTimeoutSeconds = 5f;

		public static async Task ReleaseExtras(Scene keep)
		{
			if (!keep.IsValid() || !keep.isLoaded)
			{
				return;
			}
			PortraitStudios.CloseAll();
			List<Scene> list = new List<Scene>();
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				if (sceneAt.buildIndex >= 0 && !(sceneAt.handle == keep.handle))
				{
					list.Add(sceneAt);
				}
			}
			if (list.Count > 0)
			{
				Debug.Log($"[Oturum] '{keep.name}' disinda {list.Count} sahne bosaltiliyor: " + string.Join(", ", list.ConvertAll((Scene scene2) => scene2.name)));
			}
			foreach (Scene scene in list)
			{
				if (!scene.IsValid())
				{
					continue;
				}
				float until = Time.realtimeSinceStartup + 5f;
				while (!scene.isLoaded && Time.realtimeSinceStartup < until)
				{
					await Task.Yield();
				}
				if (!scene.isLoaded)
				{
					Debug.LogWarning("[Oturum] '" + scene.name + "' sahnesi yuklenmemis halde takili ve bosaltilamiyor. Editorde Hierarchy'de duruyorsa sag tik > Remove Scene ile kaldir - Netcode oturum baslarken onu da sahiplenip bosaltmaya calisiyor.");
					continue;
				}
				AsyncOperation unload = SceneManager.UnloadSceneAsync(scene);
				if (unload != null)
				{
					while (!unload.isDone)
					{
						await Task.Yield();
					}
				}
			}
		}
	}
}
