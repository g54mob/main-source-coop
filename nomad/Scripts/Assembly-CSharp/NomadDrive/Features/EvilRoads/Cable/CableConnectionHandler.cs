using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Extensions;
using NomadDrive.Features.Rope;
using UnityEngine;

namespace NomadDrive.Features.EvilRoads.Cable
{
	public static class CableConnectionHandler
	{
		public static List<GameObject> CreateCableConnections(List<GameObject> placedObjects, CableConnectionConfig config, ICableManager cableManager = null, Transform parent = null)
		{
			List<GameObject> list = new List<GameObject>();
			if (config == null)
			{
				return list;
			}
			if (config.RopePrefab == null)
			{
				return list;
			}
			if (placedObjects == null || placedObjects.Count < 2)
			{
				return list;
			}
			for (int i = 0; i < placedObjects.Count - 1; i++)
			{
				GameObject gameObject = placedObjects[i];
				GameObject gameObject2 = placedObjects[i + 1];
				if (!(gameObject == null) && !(gameObject2 == null))
				{
					CableConnectionPoints component = gameObject.GetComponent<CableConnectionPoints>();
					CableConnectionPoints component2 = gameObject2.GetComponent<CableConnectionPoints>();
					if (!(component == null) && !(component2 == null))
					{
						List<GameObject> collection = CreateRopesBetween(component.ExitPoints, component2.EntryPoints, config, cableManager, parent);
						list.AddRange(collection);
					}
				}
			}
			if (placedObjects.Count > 0 && config.StartBehavior != CableEndBehavior.NoConnection)
			{
				GameObject gameObject3 = placedObjects[0];
				if (gameObject3 != null)
				{
					CableConnectionPoints component3 = gameObject3.GetComponent<CableConnectionPoints>();
					if (component3 != null)
					{
						List<GameObject> collection2 = HandleEndBehavior(component3, config, isEnd: false, placedObjects, cableManager, parent);
						list.AddRange(collection2);
					}
				}
			}
			if (placedObjects.Count > 0 && config.EndBehavior != CableEndBehavior.NoConnection)
			{
				GameObject gameObject4 = placedObjects[placedObjects.Count - 1];
				if (gameObject4 != null)
				{
					CableConnectionPoints component4 = gameObject4.GetComponent<CableConnectionPoints>();
					if (component4 != null)
					{
						List<GameObject> collection3 = HandleEndBehavior(component4, config, isEnd: true, placedObjects, cableManager, parent);
						list.AddRange(collection3);
					}
				}
			}
			return list;
		}

		public static async UniTask<List<GameObject>> CreateCableConnectionsAsync(List<GameObject> placedObjects, CableConnectionConfig config, ICableManager cableManager = null, Transform parent = null)
		{
			List<GameObject> createdRopes = new List<GameObject>();
			if (config == null)
			{
				return createdRopes;
			}
			if (config.RopePrefab == null)
			{
				return createdRopes;
			}
			if (placedObjects == null || placedObjects.Count < 2)
			{
				return createdRopes;
			}
			for (int i = 0; i < placedObjects.Count - 1; i++)
			{
				GameObject gameObject = placedObjects[i];
				GameObject gameObject2 = placedObjects[i + 1];
				if (!(gameObject == null) && !(gameObject2 == null))
				{
					CableConnectionPoints component = gameObject.GetComponent<CableConnectionPoints>();
					CableConnectionPoints component2 = gameObject2.GetComponent<CableConnectionPoints>();
					if (!(component == null) && !(component2 == null))
					{
						createdRopes.AddRange(await CreateRopesBetweenAsync(component.ExitPoints, component2.EntryPoints, config, cableManager, parent));
					}
				}
			}
			if (placedObjects.Count > 0 && config.StartBehavior != CableEndBehavior.NoConnection)
			{
				GameObject gameObject3 = placedObjects[0];
				if (gameObject3 != null)
				{
					CableConnectionPoints component3 = gameObject3.GetComponent<CableConnectionPoints>();
					if (component3 != null)
					{
						createdRopes.AddRange(await HandleEndBehaviorAsync(component3, config, isEnd: false, placedObjects, cableManager, parent));
					}
				}
			}
			if (placedObjects.Count > 0 && config.EndBehavior != CableEndBehavior.NoConnection)
			{
				GameObject gameObject4 = placedObjects[placedObjects.Count - 1];
				if (gameObject4 != null)
				{
					CableConnectionPoints component4 = gameObject4.GetComponent<CableConnectionPoints>();
					if (component4 != null)
					{
						createdRopes.AddRange(await HandleEndBehaviorAsync(component4, config, isEnd: true, placedObjects, cableManager, parent));
					}
				}
			}
			return createdRopes;
		}

		private static async UniTask<List<GameObject>> CreateRopesBetweenAsync(Transform[] exitPoints, Transform[] entryPoints, CableConnectionConfig config, ICableManager cableManager, Transform parent)
		{
			List<GameObject> ropes = new List<GameObject>();
			if (exitPoints == null || entryPoints == null)
			{
				return ropes;
			}
			int connectionCount = Mathf.Min(exitPoints.Length, entryPoints.Length);
			for (int i = 0; i < connectionCount; i++)
			{
				Transform exitPoint = exitPoints[i];
				Transform entryPoint = entryPoints[i];
				if (!(exitPoint == null) && !(entryPoint == null))
				{
					GameObject gameObject = await MainThreadWorkBudget.RunInstantiate(() => CreateRope(exitPoint, entryPoint, config, cableManager, parent));
					if (gameObject != null)
					{
						ropes.Add(gameObject);
					}
				}
			}
			return ropes;
		}

		private static async UniTask<List<GameObject>> HandleEndBehaviorAsync(CableConnectionPoints points, CableConnectionConfig config, bool isEnd, List<GameObject> allObjects, ICableManager cableManager, Transform parent)
		{
			List<GameObject> ropes = new List<GameObject>();
			switch (isEnd ? config.EndBehavior : config.StartBehavior)
			{
			case CableEndBehavior.LoopToEnd:
				if (allObjects.Count >= 2 && isEnd)
				{
					CableConnectionPoints cableConnectionPoints = allObjects[0]?.GetComponent<CableConnectionPoints>();
					if (cableConnectionPoints != null)
					{
						ropes.AddRange(await CreateRopesBetweenAsync(points.ExitPoints, cableConnectionPoints.EntryPoints, config, cableManager, parent));
					}
				}
				break;
			case CableEndBehavior.DanglingEnd:
			{
				Transform[] array = (isEnd ? points.ExitPoints : points.EntryPoints);
				Transform[] array2 = array;
				foreach (Transform point in array2)
				{
					if (!(point == null))
					{
						GameObject gameObject = await MainThreadWorkBudget.RunInstantiate(() => CreateRope(point, null, config, cableManager, parent));
						if (gameObject != null)
						{
							ropes.Add(gameObject);
						}
					}
				}
				break;
			}
			}
			return ropes;
		}

		private static List<GameObject> CreateRopesBetween(Transform[] exitPoints, Transform[] entryPoints, CableConnectionConfig config, ICableManager cableManager, Transform parent)
		{
			List<GameObject> list = new List<GameObject>();
			if (exitPoints == null || entryPoints == null)
			{
				return list;
			}
			int num = Mathf.Min(exitPoints.Length, entryPoints.Length);
			for (int i = 0; i < num; i++)
			{
				Transform transform = exitPoints[i];
				Transform transform2 = entryPoints[i];
				if (!(transform == null) && !(transform2 == null))
				{
					GameObject gameObject = CreateRope(transform, transform2, config, cableManager, parent);
					if (gameObject != null)
					{
						list.Add(gameObject);
					}
				}
			}
			return list;
		}

		public static GameObject CreateSingleRope(Transform startPoint, Transform endPoint, CableConnectionConfig config, ICableManager cableManager, Transform parent)
		{
			return CreateRope(startPoint, endPoint, config, cableManager, parent);
		}

		private static GameObject CreateRope(Transform startPoint, Transform endPoint, CableConnectionConfig config, ICableManager cableManager, Transform parent)
		{
			if (startPoint == null)
			{
				return null;
			}
			GameObject gameObject = Object.Instantiate(config.RopePrefab, startPoint.position, Quaternion.identity);
			if (parent != null)
			{
				gameObject.transform.SetParent(parent);
			}
			NomadDrive.Features.Rope.Rope component = gameObject.GetComponent<NomadDrive.Features.Rope.Rope>();
			if (component == null)
			{
				EvilLogger.LogError("[CableConnectionHandler] Instantiated prefab does not have Rope component", "CreateRope", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\EvilRoads\\Scripts\\Cable\\CableConnectionHandler.cs", 386);
				Object.Destroy(gameObject);
				return null;
			}
			component.startAttach = startPoint;
			component.endAttach = endPoint;
			float num = ((endPoint != null) ? Vector3.Distance(startPoint.position, endPoint.position) : 5f);
			component.length = num + config.Slack;
			component.stretch = config.Stretch;
			component.swingAngle = config.SwingAngle;
			component.swingFreq = config.SwingFreq;
			component.widthStart = config.WidthStart;
			component.widthEnd = config.WidthEnd;
			if (config.RopeType != null)
			{
				MeshFilter component2 = gameObject.GetComponent<MeshFilter>();
				MeshRenderer component3 = gameObject.GetComponent<MeshRenderer>();
				if (component2 != null && config.RopeType.mesh != null)
				{
					component2.mesh = config.RopeType.mesh;
				}
				if (component3 != null && config.RopeType.material != null)
				{
					component3.material = config.RopeType.material;
				}
			}
			if (config.StartEnd != null)
			{
				component.SetEndObject(config.StartEnd);
			}
			if (config.EndEnd != null)
			{
				component.SetEndObject(config.EndEnd, start: false);
			}
			component.SetDirty();
			cableManager?.RegisterCable(component, config);
			return gameObject;
		}

		private static List<GameObject> HandleEndBehavior(CableConnectionPoints points, CableConnectionConfig config, bool isEnd, List<GameObject> allObjects, ICableManager cableManager, Transform parent)
		{
			List<GameObject> list = new List<GameObject>();
			switch (isEnd ? config.EndBehavior : config.StartBehavior)
			{
			case CableEndBehavior.LoopToEnd:
				if (allObjects.Count >= 2 && isEnd)
				{
					CableConnectionPoints cableConnectionPoints = allObjects[0]?.GetComponent<CableConnectionPoints>();
					if (cableConnectionPoints != null)
					{
						List<GameObject> collection = CreateRopesBetween(points.ExitPoints, cableConnectionPoints.EntryPoints, config, cableManager, parent);
						list.AddRange(collection);
					}
				}
				break;
			case CableEndBehavior.DanglingEnd:
			{
				Transform[] array = (isEnd ? points.ExitPoints : points.EntryPoints);
				foreach (Transform transform in array)
				{
					if (!(transform == null))
					{
						GameObject gameObject = CreateRope(transform, null, config, cableManager, parent);
						if (gameObject != null)
						{
							list.Add(gameObject);
						}
					}
				}
				break;
			}
			}
			return list;
		}
	}
}
