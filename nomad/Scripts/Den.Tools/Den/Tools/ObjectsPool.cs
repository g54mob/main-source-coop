using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Den.Tools
{
	[Serializable]
	public class ObjectsPool : MonoBehaviour
	{
		[Serializable]
		public class Prototype
		{
			public GameObject prefab;

			public bool allowReposition = true;

			public bool instantiateClones;

			public bool regardPrefabRotation;

			public bool regardPrefabScale;

			public override int GetHashCode()
			{
				return prefab.GetHashCode();
			}

			public Prototype()
			{
			}

			public Prototype(Prototype src)
			{
				prefab = src.prefab;
				allowReposition = src.allowReposition;
				instantiateClones = src.instantiateClones;
				regardPrefabRotation = src.regardPrefabRotation;
				regardPrefabScale = src.regardPrefabScale;
			}
		}

		[Serializable]
		private class Pool
		{
			[SerializeField]
			public Prototype prototype;

			[SerializeField]
			public List<GameObject> instances;

			private int created;

			private int moved;

			private int deleted;

			public string stats => "created:" + created + " moved:" + moved + " deleted:" + deleted;

			public void ResetStats()
			{
				created = 0;
				moved = 0;
				deleted = 0;
			}

			public Pool(Prototype prototype)
			{
				this.prototype = new Prototype(prototype);
				instances = new List<GameObject>();
			}

			private void ClearNulls()
			{
				for (int num = instances.Count - 1; num >= 0; num--)
				{
					if (instances[num] == null)
					{
						instances.RemoveAt(num);
					}
				}
			}

			private void ClampCount(int targetCount)
			{
				int count = instances.Count;
				if (instances.Count >= targetCount)
				{
					for (int i = targetCount; i < count; i++)
					{
						UnityEngine.Object.Destroy(instances[i].gameObject);
						deleted++;
					}
					instances.RemoveRange(targetCount, count - targetCount);
				}
			}

			private void AppendCount(int targetCount, Transform parent = null)
			{
				int count = instances.Count;
				moved += count;
				for (int i = count; i < targetCount; i++)
				{
					GameObject gameObject = InstantiateObject();
					gameObject.transform.parent = parent;
					gameObject.hideFlags = parent.gameObject.hideFlags;
					instances.Add(gameObject);
					created++;
				}
			}

			private void MoveItems(List<Transition> drafts, int startNum, int endNum)
			{
				for (int i = startNum; i < endNum; i++)
				{
					Transform transform = instances[i].transform;
					Transition transition = drafts[i];
					transform.localPosition = transition.pos;
					if (prototype.regardPrefabRotation)
					{
						transform.localRotation = transition.rotation * prototype.prefab.transform.rotation;
					}
					else
					{
						transform.localRotation = transition.rotation;
					}
					if (prototype.regardPrefabScale)
					{
						transform.transform.localScale = transition.scale.x * prototype.prefab.transform.localScale;
					}
					else
					{
						transform.transform.localScale = transition.scale;
					}
				}
			}

			public void Reposition(List<Transition> drafts, Transform parent = null)
			{
				ResetStats();
				ClearNulls();
				int count = drafts.Count;
				_ = instances.Count;
				if (count < instances.Count)
				{
					ClampCount(count);
				}
				else if (count > instances.Count)
				{
					AppendCount(count, parent);
				}
				MoveItems(drafts, 0, count);
			}

			public IEnumerator RepositionRoutine(List<Transition> drafts, Transform parent = null, int objsPerIteration = 500)
			{
				ResetStats();
				ClearNulls();
				int draftsCount = drafts.Count;
				_ = instances.Count;
				int iterations = draftsCount / objsPerIteration + 1;
				if (draftsCount < instances.Count)
				{
					ClampCount(draftsCount);
				}
				for (int i = 0; i < iterations; i++)
				{
					int startNum = i * objsPerIteration;
					int num = (i + 1) * objsPerIteration;
					if (num > draftsCount)
					{
						num = draftsCount;
					}
					if (num > instances.Count)
					{
						AppendCount(num, parent);
					}
					MoveItems(drafts, startNum, num);
					yield return null;
				}
			}

			public GameObject InstantiateObject()
			{
				return UnityEngine.Object.Instantiate(prototype.prefab);
			}

			public void Clear()
			{
				if (instances != null)
				{
					int count = instances.Count;
					for (int i = 0; i < count; i++)
					{
						UnityEngine.Object.Destroy(instances[i].gameObject);
					}
					instances.Clear();
				}
			}
		}

		public class PrototypeComparer : IEqualityComparer<Prototype>
		{
			public bool Equals(Prototype p1, Prototype p2)
			{
				if (p1.prefab == p2.prefab && p1.allowReposition == p2.allowReposition)
				{
					return p1.instantiateClones == p2.instantiateClones;
				}
				return false;
			}

			public int GetHashCode(Prototype p)
			{
				if (p == null || p.prefab == null || p.prefab.Equals(null))
				{
					return 0;
				}
				return p.prefab.GetHashCode() + ((!p.allowReposition) ? 1 : 0) + ((!p.instantiateClones) ? 2 : 0);
			}
		}

		[SerializeField]
		private Pool[] pools = new Pool[0];

		public void SetPrototypes(Prototype[] prototypes)
		{
			for (int num = pools.Length - 1; num >= 0; num--)
			{
				if (pools[num] == null || pools[num].prototype == null || pools[num].prototype.prefab == null || pools[num].prototype.prefab.Equals(null))
				{
					pools[num].Clear();
					ArrayTools.RemoveAt(ref pools, num);
				}
			}
			Dictionary<Prototype, Pool> dictionary = new Dictionary<Prototype, Pool>(pools.Length);
			for (int i = 0; i < pools.Length; i++)
			{
				if (!dictionary.ContainsKey(pools[i].prototype))
				{
					dictionary.Add(pools[i].prototype, pools[i]);
				}
			}
			Pool[] array = new Pool[prototypes.Length];
			for (int j = 0; j < prototypes.Length; j++)
			{
				if (dictionary.TryGetValue(prototypes[j], out var value))
				{
					dictionary.Remove(prototypes[j]);
				}
				else
				{
					value = new Pool(prototypes[j]);
				}
				value.prototype.regardPrefabRotation = prototypes[j].regardPrefabRotation;
				value.prototype.regardPrefabScale = prototypes[j].regardPrefabScale;
				array[j] = value;
			}
			if (dictionary.Count != 0)
			{
				foreach (KeyValuePair<Prototype, Pool> item in dictionary)
				{
					item.Value.Clear();
				}
			}
			pools = array;
		}

		public void ClearPrototypes(Prototype[] prototypes)
		{
			HashSet<Prototype> hashSet = new HashSet<Prototype>(prototypes);
			List<Pool> list = new List<Pool>();
			for (int i = 0; i < pools.Length; i++)
			{
				if (hashSet.Contains(pools[i].prototype))
				{
					pools[i].Clear();
				}
				else
				{
					list.Add(pools[i]);
				}
			}
			pools = list.ToArray();
		}

		public void Reposition(Prototype[] prototypes, List<Transition>[] drafts)
		{
			for (int num = drafts.Length - 1; num > 0; num--)
			{
				if (drafts[num].Count == 0)
				{
					ArrayTools.RemoveAt(ref prototypes, num);
					ArrayTools.RemoveAt(ref drafts, num);
				}
			}
			SetPrototypes(prototypes);
			for (int i = 0; i < pools.Length; i++)
			{
				pools[i].Reposition(drafts[i], base.transform);
			}
		}

		public IEnumerator RepositionRoutine(Prototype[] prototypes, List<Transition>[] drafts, int objsPerIteration = 500)
		{
			for (int num = drafts.Length - 1; num >= 0; num--)
			{
				if (drafts[num].Count == 0)
				{
					ArrayTools.RemoveAt(ref prototypes, num);
					ArrayTools.RemoveAt(ref drafts, num);
				}
			}
			SetPrototypes(prototypes);
			for (int i = 0; i < pools.Length; i++)
			{
				IEnumerator e = pools[i].RepositionRoutine(drafts[i], base.transform, objsPerIteration);
				while (e.MoveNext())
				{
					yield return null;
				}
			}
		}

		public void Depool(GameObject obj)
		{
			for (int i = 0; i < pools.Length; i++)
			{
				pools[i].instances.Remove(obj);
			}
		}
	}
}
