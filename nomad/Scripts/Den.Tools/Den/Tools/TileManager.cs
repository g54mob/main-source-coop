using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Den.Tools
{
	public class TileManager<T> : ISerializationCallbackReceiver where T : ITile
	{
		public Dictionary<Coord, T> grid = new Dictionary<Coord, T>();

		public object gridLocker = new object();

		public bool allowMove;

		public bool generateLimited = true;

		public Vector2D generateCenter;

		public bool generateInfinite = true;

		public int generateRange = 2;

		public int retainMargin = 1;

		public bool genAroundMainCam = true;

		public bool genAroundObjsTag;

		public string genAroundTag;

		public bool genAroundTfms;

		public Transform[] genAroundTfmsList = new Transform[0];

		public bool genAroundCoordinates;

		public Coord[] genCoordinates = new Coord[0];

		[NonSerialized]
		protected Coord[] camCoords;

		public T[] serializedTiles;

		public Coord[] serializedCoords;

		public T this[Coord coord]
		{
			get
			{
				if (grid.TryGetValue(coord, out var value))
				{
					return value;
				}
				return default(T);
			}
		}

		public T this[int x, int z] => this[new Coord(x, z)];

		public bool Contains(Coord coord)
		{
			return grid.ContainsKey(coord);
		}

		protected T ConstructTile(MonoBehaviour holder)
		{
			return (T)typeof(T).GetMethod("Construct", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[1] { holder });
		}

		public IEnumerable<T> Tiles()
		{
			foreach (KeyValuePair<Coord, T> item in grid)
			{
				yield return item.Value;
			}
		}

		public virtual T Closest()
		{
			float num = 2.1474836E+09f;
			T result = default(T);
			foreach (KeyValuePair<Coord, T> item in grid)
			{
				if (camCoords == null)
				{
					return item.Value;
				}
				float remoteness = GetRemoteness(item.Key, camCoords);
				if (remoteness < num)
				{
					num = remoteness;
					result = item.Value;
				}
			}
			return result;
		}

		public void Update(Vector3 tileSize, Dictionary<Coord, T> pinned = null, MonoBehaviour holder = null, bool distsOnly = false)
		{
			RemoveNulls();
			if (RefreshCamCoords(tileSize.x, holder) && camCoords.Length != 0)
			{
				if (!distsOnly && generateInfinite)
				{
					Deploy(camCoords, pinned, holder);
				}
				ChangeDists(camCoords);
			}
		}

		public void ReDeploy(Vector3 tileSize, Dictionary<Coord, T> pinned = null, MonoBehaviour holder = null)
		{
			RemoveNulls();
			RefreshCamCoords(tileSize.x);
			Deploy(camCoords, pinned, holder);
			ChangeDists(camCoords);
		}

		private bool RefreshCamCoords(float tileSize, MonoBehaviour holder = null)
		{
			bool result = false;
			GameObject[] array = null;
			if (genAroundObjsTag)
			{
				array = GameObject.FindGameObjectsWithTag(genAroundTag);
			}
			Transform[] array2 = null;
			if (genAroundTfms)
			{
				array2 = genAroundTfmsList.RemoveNulls();
			}
			int num = 0;
			if (genAroundMainCam)
			{
				num++;
			}
			if (array != null)
			{
				num += array.Length;
			}
			if (array2 != null)
			{
				num += array2.Length;
			}
			if (genAroundCoordinates)
			{
				num += genCoordinates.Length;
			}
			if (camCoords == null || num != camCoords.Length)
			{
				camCoords = new Coord[num];
				result = true;
			}
			if (num == 0)
			{
				return result;
			}
			int num2 = 0;
			if (genAroundMainCam)
			{
				Camera camera = Camera.main;
				if (camera == null)
				{
					camera = UnityEngine.Object.FindObjectOfType<Camera>();
				}
				if (camera != null)
				{
					Vector3 position = camera.transform.position;
					if (holder != null)
					{
						position = holder.transform.InverseTransformPoint(position);
					}
					Coord coord = Coord.Floor(position.x / tileSize, position.z / tileSize);
					if (camCoords[0] != coord)
					{
						camCoords[0] = coord;
						result = true;
					}
					num2++;
				}
			}
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					Vector3 position2 = array[i].transform.position;
					if (holder != null)
					{
						position2 = holder.transform.InverseTransformPoint(position2);
					}
					Coord coord2 = Coord.Floor(position2.x / tileSize, position2.z / tileSize);
					if (camCoords[i + num2] != coord2)
					{
						camCoords[i + num2] = coord2;
						result = true;
					}
				}
			}
			if (array2 != null)
			{
				for (int j = 0; j < array2.Length; j++)
				{
					Vector3 position3 = array2[j].position;
					if (holder != null)
					{
						position3 = holder.transform.InverseTransformPoint(position3);
					}
					Coord coord3 = Coord.Floor(position3.x / tileSize, position3.z / tileSize);
					if (camCoords[j + num2] != coord3)
					{
						camCoords[j + num2] = coord3;
						result = true;
					}
				}
			}
			if (genAroundCoordinates)
			{
				for (int k = 0; k < genCoordinates.Length; k++)
				{
					Coord coord4 = genCoordinates[k];
					if (camCoords[k + num2] != coord4)
					{
						camCoords[k + num2] = coord4;
						result = true;
					}
				}
			}
			return result;
		}

		public virtual void ChangeDists(Coord[] camCoords)
		{
			foreach (KeyValuePair<Coord, T> item in grid)
			{
				Coord key = item.Key;
				item.Value.Dist(GetRemoteness(key, camCoords));
			}
		}

		public virtual void Deploy(Coord[] camCoords, Dictionary<Coord, T> pinned = null, MonoBehaviour holder = null)
		{
			CoordRect[] deployRects = GetDeployRects(camCoords, generateRange);
			Dictionary<Coord, T> dictionary = new Dictionary<Coord, T>();
			Dictionary<Coord, T> dictionary2 = new Dictionary<Coord, T>(grid);
			if (pinned != null)
			{
				foreach (KeyValuePair<Coord, T> item4 in pinned)
				{
					Coord key = item4.Key;
					T value = item4.Value;
					dictionary2.Remove(key);
					dictionary.Add(key, value);
					value.Dist(GetRemoteness(key, camCoords));
				}
			}
			for (int i = 0; i < deployRects.Length; i++)
			{
				CoordRect coordRect = deployRects[i];
				Coord coord = coordRect.Min - retainMargin;
				Coord coord2 = coordRect.Max + retainMargin;
				for (int j = coord.x; j < coord2.x; j++)
				{
					for (int k = coord.z; k < coord2.z; k++)
					{
						Coord coord3 = new Coord(j, k);
						if (dictionary2.TryGetValue(coord3, out var value2))
						{
							dictionary2.Remove(coord3);
							dictionary.Add(coord3, value2);
							float remoteness = GetRemoteness(coord3, camCoords);
							value2.Dist(remoteness);
						}
					}
				}
			}
			Queue<T> queue = new Queue<T>(dictionary2.Values);
			List<(T, Coord, float)> list = new List<(T, Coord, float)>();
			for (int l = 0; l < deployRects.Length; l++)
			{
				CoordRect coordRect2 = deployRects[l];
				Coord min = coordRect2.Min;
				Coord max = coordRect2.Max;
				for (int m = min.x; m < max.x; m++)
				{
					for (int n = min.z; n < max.z; n++)
					{
						Coord coord4 = new Coord(m, n);
						if (!dictionary.ContainsKey(coord4))
						{
							T val = ((queue.Count == 0 || !allowMove) ? ConstructTile(holder) : queue.Dequeue());
							dictionary.Add(coord4, val);
							list.Add((val, coord4, GetRemoteness(coord4, camCoords)));
						}
					}
				}
			}
			while (queue.Count != 0)
			{
				queue.Dequeue().Remove();
			}
			list.Sort(delegate((T tile, Coord coord, float dist) x, (T tile, Coord coord, float dist) y)
			{
				float num2 = x.dist - y.dist;
				if (num2 > 1E-05f)
				{
					return 1;
				}
				return (num2 < -1E-06f) ? (-1) : 0;
			});
			lock (gridLocker)
			{
				grid = dictionary;
			}
			int count = list.Count;
			for (int num = 0; num < count; num++)
			{
				(T, Coord, float) tuple = list[num];
				ref T item = ref tuple.Item1;
				Coord item2 = list[num].Item2;
				float item3 = list[num].Item3;
				item.Move(item2, item3);
			}
		}

		private static CoordRect[] GetDeployRects(Coord[] camCoords, int range)
		{
			CoordRect[] array = new CoordRect[camCoords.Length];
			for (int i = 0; i < camCoords.Length; i++)
			{
				array[i] = new CoordRect(camCoords[i].x - range, camCoords[i].z - range, range * 2 + 1, range * 2 + 1);
			}
			return array;
		}

		protected static float GetRemoteness(Coord coord, Coord[] camCoords)
		{
			float num = 3.4028235E+38f;
			if (camCoords == null)
			{
				return num;
			}
			for (int i = 0; i < camCoords.Length; i++)
			{
				float num2 = Coord.DistanceAxisPriority(camCoords[i], coord);
				if (num2 < num)
				{
					num = num2;
				}
			}
			return num;
		}

		public virtual void RemoveNulls()
		{
			List<Coord> list = null;
			foreach (KeyValuePair<Coord, T> item in grid)
			{
				T value = item.Value;
				if (value == null || value.IsNull)
				{
					if (list == null)
					{
						list = new List<Coord>();
					}
					list.Add(item.Key);
				}
			}
			if (list == null)
			{
				return;
			}
			foreach (Coord item2 in list)
			{
				grid.Remove(item2);
			}
		}

		[Obsolete]
		private CoordRect WorldToChunksRect(CoordRect wrect, int size)
		{
			Coord coord = new Coord((wrect.offset.x >= 0) ? (wrect.Min.x / size) : ((wrect.Min.x + 1) / size - 1), (wrect.offset.z >= 0) ? (wrect.Min.z / size) : ((wrect.Min.z + 1) / size - 1));
			Coord coord2 = new Coord((wrect.offset.x + wrect.size.x > 0) ? ((wrect.offset.x + wrect.size.x - 1) / size + 1) : ((wrect.offset.x + wrect.size.x) / size), (wrect.offset.z + wrect.size.z > 0) ? ((wrect.offset.z + wrect.size.z - 1) / size + 1) : ((wrect.offset.z + wrect.size.z) / size));
			return new CoordRect(coord, coord2 - coord);
		}

		public virtual void OnBeforeSerialize()
		{
			if (serializedTiles == null || serializedTiles.Length != grid.Count)
			{
				serializedTiles = new T[grid.Count];
			}
			if (serializedCoords == null || serializedCoords.Length != grid.Count)
			{
				serializedCoords = new Coord[grid.Count];
			}
			int num = 0;
			foreach (KeyValuePair<Coord, T> item in grid)
			{
				serializedTiles[num] = item.Value;
				serializedCoords[num] = item.Key;
				num++;
			}
		}

		public virtual void OnAfterDeserialize()
		{
			Dictionary<Coord, T> dictionary = new Dictionary<Coord, T>();
			for (int i = 0; i < serializedTiles.Length; i++)
			{
				if (serializedTiles[i] != null)
				{
					dictionary.Add(serializedCoords[i], serializedTiles[i]);
				}
			}
			lock (grid)
			{
				grid = dictionary;
			}
		}
	}
}
