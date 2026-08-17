using System;
using System.Reflection;
using UnityEngine;

namespace Den.Tools
{
	public static class ScriptableAssetExtensions
	{
		public static class ObjectSelectorWrapper
		{
		}

		private static Type objectSelectorType;

		public static T SaveAsset<T>(this T asset, string savePath = null, string filename = "Data", string type = "asset", string caption = "Save Data as Unity Asset") where T : UnityEngine.Object
		{
			return null;
		}

		public static void SaveRawBytes(this byte[] bytes, string savePath = null, string filename = "Data", string type = "asset")
		{
		}

		public static void SaveTexture(this Texture2D tex, string savePath = null, string filename = "Texutre", string type = "png", string caption = "Save Texture as PNG")
		{
		}

		public static T ReleaseAsset<T>(this T asset, string savePath = null) where T : ScriptableObject, ISerializationCallbackReceiver
		{
			return asset;
		}

		public static T LoadAsset<T>(string label = "Load Unity Asset", string[] filters = null) where T : UnityEngine.Object
		{
			return null;
		}

		public static object GetObjectSelector()
		{
			if (objectSelectorType == null)
			{
				objectSelectorType = Type.GetType("UnityEditor.ObjectSelector,UnityEditor");
			}
			return objectSelectorType.GetProperty("get", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
		}

		public static int GetObjectSelectorId()
		{
			if (objectSelectorType == null)
			{
				objectSelectorType = Type.GetType("UnityEditor.ObjectSelector,UnityEditor");
			}
			object objectSelector = GetObjectSelector();
			return (int)objectSelectorType.GetField("objectSelectorID", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(objectSelector);
		}

		public static object GetObjectSelectorObject()
		{
			if (objectSelectorType == null)
			{
				objectSelectorType = Type.GetType("UnityEditor.ObjectSelector,UnityEditor");
			}
			GetObjectSelector();
			MethodInfo method = objectSelectorType.GetMethod("GetCurrentObject", BindingFlags.Static | BindingFlags.Public);
			Debug.Log(method.Invoke(null, null));
			return method.Invoke(null, null);
		}

		public static void ShowObjectSelector(Type objType, int id = 12345, bool allowSceneObjects = false, Action<UnityEngine.Object> onClosed = null, Action<UnityEngine.Object> onUpdated = null)
		{
		}
	}
}
