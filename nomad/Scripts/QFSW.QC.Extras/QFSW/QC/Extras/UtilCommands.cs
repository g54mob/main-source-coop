using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QFSW.QC.Pooling;
using QFSW.QC.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QFSW.QC.Extras
{
	public static class UtilCommands
	{
		private static readonly ConcurrentStringBuilderPool _builderPool = new ConcurrentStringBuilderPool();

		[Command("get-object-info", "Finds the specified GameObject and displays its transform and component data", Platform.AllPlatforms, MonoTargetType.Single)]
		private static string ExtractObjectInfo(GameObject target)
		{
			StringBuilder stringBuilder = _builderPool.GetStringBuilder();
			stringBuilder.AppendLine("Extracted info for object '" + target.name + "'");
			stringBuilder.AppendLine("Transform data:");
			stringBuilder.AppendLine($"   - position: {target.transform.position}");
			stringBuilder.AppendLine($"   - rotation: {target.transform.localRotation}");
			stringBuilder.AppendLine($"   - scale: {target.transform.localScale}");
			if (target.transform.childCount > 0)
			{
				stringBuilder.AppendLine($"   - child count: {target.transform.childCount}");
			}
			if ((bool)target.transform.parent)
			{
				stringBuilder.AppendLine("   - parent: " + target.transform.parent.name);
			}
			Component[] array = (from x in target.GetComponents<Component>()
				orderby x.GetType().Name
				select x).ToArray();
			if (array.Length != 0)
			{
				stringBuilder.AppendLine("Component data:");
				for (int num = 0; num < array.Length; num++)
				{
					int num2 = 1;
					Type type = array[num].GetType();
					stringBuilder.AppendLine("   - " + type.Name);
					for (; num + 1 < array.Length && array[num + 1].GetType() == type; num++)
					{
						num2++;
					}
					if (num2 > 1)
					{
						stringBuilder.Append($" ({num2})");
					}
				}
			}
			if (target.transform.childCount > 0)
			{
				stringBuilder.AppendLine("Children:");
				int childCount = target.transform.childCount;
				for (int num3 = 0; num3 < childCount; num3++)
				{
					stringBuilder.AppendLine("   - " + target.transform.GetChild(num3).name);
				}
			}
			return _builderPool.ReleaseAndToString(stringBuilder);
		}

		[Command("get-scene-hierarchy", "Renders the GameObject hierarchy of the currently open scenes", Platform.AllPlatforms, MonoTargetType.Single)]
		private static string GetSceneHierarchy()
		{
			List<GameObject> list = new List<GameObject>();
			StringBuilder stringBuilder = _builderPool.GetStringBuilder();
			foreach (Scene loadedScene in SceneUtilities.GetLoadedScenes())
			{
				list.Clear();
				loadedScene.GetRootGameObjects(list);
				stringBuilder.AppendLine(loadedScene.name);
				GetSceneHierarchy(list.Select((GameObject x) => x.transform).ToArray(), 0, stringBuilder, new List<bool>());
			}
			return _builderPool.ReleaseAndToString(stringBuilder);
		}

		private static IEnumerable<Transform> GetChildren(this Transform transform)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				yield return transform.GetChild(i);
			}
		}

		private static void GetSceneHierarchy(IList<Transform> roots, int depth, StringBuilder buffer, IList<bool> drawVertical)
		{
			for (int i = 0; i < roots.Count; i++)
			{
				Transform transform = roots[i];
				for (int j = 0; j < depth; j++)
				{
					buffer.Append(drawVertical[j] ? '|' : ' ');
					buffer.Append(' ', 2);
				}
				bool flag = i == roots.Count - 1;
				drawVertical.Add(!flag);
				buffer.Append(flag ? '|' : '|');
				buffer.Append('-', 2);
				buffer.AppendLine(transform.name);
				GetSceneHierarchy(transform.GetChildren().ToList(), depth + 1, buffer, drawVertical);
				drawVertical.RemoveAt(drawVertical.Count - 1);
			}
		}

		[Command("add-component", "Adds a component of type T to the specified GameObject", Platform.AllPlatforms, MonoTargetType.Single)]
		private static void AddComponent<T>(GameObject target) where T : Component
		{
			target.AddComponent<T>();
		}

		[Command("destroy-component", "Destroys the component of type T on the specified GameObject", Platform.AllPlatforms, MonoTargetType.Single)]
		private static void DestroyComponent<T>(T target) where T : Component
		{
			UnityEngine.Object.Destroy(target);
		}

		[Command("destroy", "Destroys a GameObject", Platform.AllPlatforms, MonoTargetType.Single)]
		private static void DestroyGO(GameObject target)
		{
			UnityEngine.Object.Destroy(target);
		}

		[Command("instantiate", "Instantiates a GameObject", Platform.AllPlatforms, MonoTargetType.Single)]
		private static void InstantiateGO([CommandParameterDescription("The original GameObject to instantiate a copy of.")] GameObject original, [CommandParameterDescription("The position of the instantiated GameObject.")] Vector3 position, [CommandParameterDescription("The rotation of the instantiated GameObject.")] Quaternion rotation)
		{
			UnityEngine.Object.Instantiate(original, position, rotation);
		}

		[Command("instantiate", "Instantiates a GameObject", Platform.AllPlatforms, MonoTargetType.Single)]
		private static void InstantiateGO(GameObject original, Vector3 position)
		{
			UnityEngine.Object.Instantiate(original).transform.position = position;
		}

		[Command("instantiate", "Instantiates a GameObject", Platform.AllPlatforms, MonoTargetType.Single)]
		private static void InstantiateGO(GameObject original)
		{
			UnityEngine.Object.Instantiate(original);
		}

		[Command("teleport", "Teleports a GameObject", Platform.AllPlatforms, MonoTargetType.Single)]
		private static void TeleportGO(GameObject target, Vector3 position)
		{
			target.transform.position = position;
		}

		[Command("teleport-relative", "Teleports a GameObject by a relative offset to its current position", Platform.AllPlatforms, MonoTargetType.Single)]
		private static void TeleportRelativeGO(GameObject target, Vector3 offset)
		{
			target.transform.Translate(offset);
		}

		[Command("rotate", "Rotates a GameObject", Platform.AllPlatforms, MonoTargetType.Single)]
		private static void RotateGO(GameObject target, Quaternion rotation)
		{
			target.transform.Rotate(rotation.eulerAngles);
		}

		[Command("set-active", "Activates/deactivates a GameObject", Platform.AllPlatforms, MonoTargetType.Single)]
		private static void SetGOActive(GameObject target, bool active)
		{
			target.SetActive(active);
		}

		[Command("set-parent", "Sets the parent of the targert transform.", Platform.AllPlatforms, MonoTargetType.Single)]
		private static void SetGOParent(Transform target, Transform parentTarget)
		{
			target.SetParent(parentTarget);
		}

		[Command("send-message", "Calls the method named 'methodName' on every MonoBehaviour in the target GameObject", Platform.AllPlatforms, MonoTargetType.Single)]
		private static void SendGOMessage(GameObject target, string methodName)
		{
			target.SendMessage(methodName);
		}
	}
}
