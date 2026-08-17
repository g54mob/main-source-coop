using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Den.Tools
{
	public static class AssetsExtensions
	{
		private static PropertyInfo dirtyIdProp;

		public static string GUID(this UnityEngine.Object obj)
		{
			Debug.LogError("GUID does not work in build");
			return "";
		}

		public static T GUIDtoObj<T>(this string guid) where T : UnityEngine.Object
		{
			Debug.LogError("GUIDtObj does not work in build");
			return null;
		}

		public static string[] GetUserData(this UnityEngine.Object obj, string param)
		{
			Debug.LogError("GetUserData does not work in build");
			return null;
		}

		public static string[] GetUserData(string guid, string param)
		{
			Debug.LogError("GetUserData does not work in build");
			return null;
		}

		public static void SetUserData(this UnityEngine.Object obj, string param, string[] data, bool reload = false)
		{
			Debug.LogError("SetUserData does not work in build");
		}

		public static void SetUserData(string guid, string param, string[] data, bool reload = false)
		{
			Debug.LogError("SetUserData does not work in build");
		}

		public static void Reimport(this UnityEngine.Object obj)
		{
			Debug.LogError("Reimport does not work in build");
		}

		public static int GetDirtyId(this Scene scene)
		{
			if (dirtyIdProp == null)
			{
				dirtyIdProp = typeof(Scene).GetProperty("dirtyID", BindingFlags.Instance | BindingFlags.NonPublic);
			}
			return (int)dirtyIdProp.GetValue(scene, new object[0]);
		}

		public static int GetDirtyId()
		{
			int num = 0;
			int sceneCount = SceneManager.sceneCount;
			for (int i = 0; i < sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				num += sceneAt.GetDirtyId();
			}
			return num;
		}

		public static Texture2D GetNormalTexture(this Texture2D diffuse)
		{
			return null;
		}

		public static Texture GetMainTexture(this GameObject gameObject)
		{
			return gameObject.GetComponent<Renderer>()?.material?.mainTexture;
		}

		public static bool IsAsset(this GameObject obj)
		{
			return false;
		}

		public static bool IsNull(this UnityEngine.Object unityObj)
		{
			try
			{
				return unityObj == null;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
