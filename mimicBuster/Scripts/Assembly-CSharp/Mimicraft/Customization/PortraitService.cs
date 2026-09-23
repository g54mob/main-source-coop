using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mimicraft.Customization
{
	public static class PortraitService<TStudio, TSubject> where TStudio : MonoBehaviour, IPortraitStudio<TSubject>
	{
		private readonly struct Job
		{
			public readonly string FilePath;

			public readonly TSubject Subject;

			public Job(string filePath, TSubject subject)
			{
				FilePath = filePath;
				Subject = subject;
			}
		}

		public static string StudioSceneName = "";

		public static float KeepLoadedSeconds = 20f;

		public static Vector3 StudioOffset = new Vector3(0f, -500f, 0f);

		private static readonly Queue<Job> pending = new Queue<Job>();

		private static readonly HashSet<string> queued = new HashSet<string>();

		private static PortraitDriver driver;

		private static bool warnedAboutScene;

		private static Scene openStudio;

		private static bool openedStudio;

		public static int Pending => pending.Count;

		public static event Action<string> PortraitWritten;

		public static void Request(string filePath, TSubject subject)
		{
			if (!string.IsNullOrEmpty(filePath) && subject != null && queued.Add(filePath))
			{
				pending.Enqueue(new Job(filePath, subject));
				EnsureDriver();
			}
		}

		private static void EnsureDriver()
		{
			if (!(driver != null))
			{
				Debug.Log("[Portre] Kuyruk basladi - '" + StudioSceneName + "' sahnesi ilk istekte yuklenecek.");
				GameObject gameObject = new GameObject("PortraitService (" + typeof(TStudio).Name + ")");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				driver = gameObject.AddComponent<PortraitDriver>();
				driver.StartCoroutine(Run());
				PortraitStudios.Register(Close);
			}
		}

		public static void Close()
		{
			pending.Clear();
			queued.Clear();
			if (driver != null)
			{
				UnityEngine.Object.Destroy(driver.gameObject);
				driver = null;
			}
			if (openedStudio && openStudio.IsValid() && openStudio.isLoaded)
			{
				SceneManager.UnloadSceneAsync(openStudio);
			}
			openedStudio = false;
			openStudio = default(Scene);
		}

		private static void MoveOutOfTheWay(Scene scene)
		{
			if (!(StudioOffset == Vector3.zero) && scene.IsValid() && scene.isLoaded)
			{
				bool flag = false;
				GameObject[] rootGameObjects = scene.GetRootGameObjects();
				foreach (GameObject gameObject in rootGameObjects)
				{
					gameObject.transform.position += StudioOffset;
					flag |= HasStaticRenderer(gameObject);
				}
				if (flag)
				{
					Debug.LogWarning($"[Portre] '{StudioSceneName}' static mesh iceriyor ama {StudioOffset} " + "kadar tasiniyor - static batching kose noktalarini DUNYA uzayinda birlestirdigi icin transform artik cizimi surmuyor. Studyoda hicbir seyi static isaretleme, ya da sahneyi bastan uzak bir yerde kur ve StudioOffset'i sifirla.");
				}
			}
		}

		private static TStudio FindStage()
		{
			return UnityEngine.Object.FindFirstObjectByType<TStudio>(FindObjectsInactive.Include);
		}

		private static bool Alive(TStudio stage)
		{
			return stage != null;
		}

		private static bool HasStaticRenderer(GameObject root)
		{
			Renderer[] componentsInChildren = root.GetComponentsInChildren<Renderer>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].gameObject.isStatic)
				{
					return true;
				}
			}
			return false;
		}

		private static IEnumerator Run()
		{
			while (true)
			{
				if (pending.Count == 0)
				{
					yield return null;
					continue;
				}
				Scene studio = default(Scene);
				bool loadedHere = false;
				TStudio stage = FindStage();
				if (!Alive(stage))
				{
					if (!Application.CanStreamedLevelBeLoaded(StudioSceneName))
					{
						if (!warnedAboutScene)
						{
							warnedAboutScene = true;
							Debug.LogWarning("[Portre] '" + StudioSceneName + "' sahnesi yok ya da Build Settings'e eklenmemis - gorseller olusturulamayacak.");
						}
						pending.Clear();
						queued.Clear();
						continue;
					}
					yield return SceneManager.LoadSceneAsync(StudioSceneName, LoadSceneMode.Additive);
					studio = SceneManager.GetSceneByName(StudioSceneName);
					loadedHere = true;
					openStudio = studio;
					openedStudio = true;
					MoveOutOfTheWay(studio);
					yield return null;
					stage = FindStage();
				}
				if (!Alive(stage))
				{
					Debug.LogWarning("[Portre] '" + StudioSceneName + "' yuklendi ama icinde " + typeof(TStudio).Name + " yok.");
					pending.Clear();
					queued.Clear();
				}
				while (Alive(stage) && pending.Count > 0)
				{
					Job job = pending.Dequeue();
					queued.Remove(job.FilePath);
					stage.Dress(job.Subject);
					yield return null;
					yield return new WaitForEndOfFrame();
					Texture2D texture2D = stage.Shoot();
					if (!(texture2D == null))
					{
						SavedThumbnails.Write(job.FilePath, texture2D);
						UnityEngine.Object.Destroy(texture2D);
						Debug.Log("[Portre] Gorsel yazildi: " + SavedThumbnails.PathFor(job.FilePath));
						PortraitService<TStudio, TSubject>.PortraitWritten?.Invoke(job.FilePath);
					}
				}
				float until = Time.unscaledTime + Mathf.Max(0f, KeepLoadedSeconds);
				while (Time.unscaledTime < until && pending.Count == 0)
				{
					yield return null;
				}
				if (pending.Count <= 0)
				{
					if (loadedHere && studio.IsValid() && studio.isLoaded)
					{
						yield return SceneManager.UnloadSceneAsync(studio);
					}
					openedStudio = false;
					openStudio = default(Scene);
				}
			}
		}
	}
}
