using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;

namespace Fusion
{
	[Preserve]
	public class FusionGlobalScriptableObjectResourceAttribute : FusionGlobalScriptableObjectSourceAttribute
	{
		public string ResourcePath { get; }

		public bool InstantiateIfLoadedInEditor { get; set; } = true;

		public FusionGlobalScriptableObjectResourceAttribute(Type objectType, string resourcePath = "")
			: base(objectType)
		{
			ResourcePath = resourcePath;
		}

		public override FusionGlobalScriptableObjectLoadResult Load(Type type)
		{
			FusionGlobalScriptableObjectAttribute customAttribute = type.GetCustomAttribute<FusionGlobalScriptableObjectAttribute>();
			string resourcePath = GetResourcePath(type, customAttribute);
			if (resourcePath == null)
			{
				return default(FusionGlobalScriptableObjectLoadResult);
			}
			UnityEngine.Object obj = Resources.Load(resourcePath, type);
			if (!obj)
			{
				return default(FusionGlobalScriptableObjectLoadResult);
			}
			return HandleInstance(obj);
		}

		public override Task<FusionGlobalScriptableObjectLoadResult> LoadAsync(Type type)
		{
			FusionGlobalScriptableObjectAttribute customAttribute = type.GetCustomAttribute<FusionGlobalScriptableObjectAttribute>();
			TaskCompletionSource<FusionGlobalScriptableObjectLoadResult> tcs = new TaskCompletionSource<FusionGlobalScriptableObjectLoadResult>();
			string resourcePath = GetResourcePath(type, customAttribute);
			if (resourcePath == null)
			{
				tcs.SetResult(default(FusionGlobalScriptableObjectLoadResult));
			}
			else
			{
				ResourceRequest resourceRequest = Resources.LoadAsync(resourcePath, type);
				if (resourceRequest == null)
				{
					tcs.SetResult(default(FusionGlobalScriptableObjectLoadResult));
				}
				else
				{
					resourceRequest.completed += delegate(AsyncOperation op)
					{
						UnityEngine.Object asset = ((ResourceRequest)op).asset;
						if ((bool)asset)
						{
							tcs.SetResult(HandleInstance(asset));
						}
						else
						{
							tcs.SetResult(default(FusionGlobalScriptableObjectLoadResult));
						}
					};
				}
			}
			return tcs.Task;
		}

		private string GetResourcePath(Type type, FusionGlobalScriptableObjectAttribute attribute)
		{
			if (string.IsNullOrEmpty(ResourcePath))
			{
				string defaultPath = attribute.DefaultPath;
				int num = defaultPath.LastIndexOf("/Resources/", StringComparison.OrdinalIgnoreCase);
				if (num < 0)
				{
					return null;
				}
				string text = defaultPath.Substring(num + "/Resources/".Length);
				if (Path.HasExtension(text))
				{
					return text.Substring(0, text.LastIndexOf('.'));
				}
				return text;
			}
			return ResourcePath;
		}

		private FusionGlobalScriptableObjectLoadResult HandleInstance(UnityEngine.Object instance)
		{
			if (InstantiateIfLoadedInEditor && Application.isEditor)
			{
				UnityEngine.Object clone = UnityEngine.Object.Instantiate(instance);
				return new FusionGlobalScriptableObjectLoadResult((FusionGlobalScriptableObject)clone, delegate
				{
					UnityEngine.Object.Destroy(clone);
				});
			}
			return new FusionGlobalScriptableObjectLoadResult((FusionGlobalScriptableObject)instance, delegate
			{
				Resources.UnloadAsset(instance);
			});
		}
	}
}
