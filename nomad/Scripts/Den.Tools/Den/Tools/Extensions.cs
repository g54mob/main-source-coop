using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Den.Tools
{
	public static class Extensions
	{
		public class WeakRefComparer<T> : IEqualityComparer<WeakReference<T>> where T : class
		{
			public bool Equals(WeakReference<T> wr2, T t1)
			{
				if (!wr2.TryGetTarget(out var target))
				{
					return false;
				}
				return t1 == target;
			}

			public bool Equals(T t1, WeakReference<T> wr2)
			{
				if (!wr2.TryGetTarget(out var target))
				{
					return false;
				}
				return t1 == target;
			}

			public bool Equals(WeakReference<T> wr1, WeakReference<T> wr2)
			{
				if (!wr1.TryGetTarget(out var target))
				{
					return false;
				}
				if (!wr2.TryGetTarget(out var target2))
				{
					return false;
				}
				return target == target2;
			}

			public int GetHashCode(WeakReference<T> obj)
			{
				if (!obj.TryGetTarget(out var target))
				{
					return 0;
				}
				return target.GetHashCode();
			}
		}

		public static bool isPlaying => true;

		public static void RemoveChildren(this Transform tfm)
		{
			for (int num = tfm.childCount - 1; num >= 0; num--)
			{
				UnityEngine.Object.Destroy(tfm.GetChild(num).gameObject);
			}
		}

		public static Transform FindChildRecursive(this Transform tfm, string name)
		{
			int childCount = tfm.childCount;
			for (int i = 0; i < childCount; i++)
			{
				if (tfm.GetChild(i).name == name)
				{
					return tfm.GetChild(i);
				}
			}
			for (int j = 0; j < childCount; j++)
			{
				Transform transform = tfm.GetChild(j).FindChildRecursive(name);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		public static void ToggleDisplayWireframe(this Transform tfm, bool show)
		{
		}

		public static int ToInt(this Coord coord)
		{
			int num = ((coord.x < 0) ? (-coord.x) : coord.x);
			int num2 = ((coord.z < 0) ? (-coord.z) : coord.z);
			return (((coord.z < 0) ? 1000000000 : 0) + num * 30000 + num2) * ((coord.x >= 0) ? 1 : (-1));
		}

		public static Coord ToCoord(this int hash)
		{
			int num = ((hash < 0) ? (-hash) : hash);
			int num2 = num / 1000000000 * 1000000000;
			int num3 = (num - num2) / 30000;
			int num4 = num - num2 - num3 * 30000;
			return new Coord((hash < 0) ? (-num3) : num3, (num2 == 0) ? num4 : (-num4));
		}

		public static TValue[] ToArray<TKey, TValue>(this Dictionary<TKey, TValue>.ValueCollection values)
		{
			TValue[] array = new TValue[values.Count];
			values.CopyTo(array, 0);
			return array;
		}

		public static TKey[] ToArray<TKey, TValue>(this Dictionary<TKey, TValue>.KeyCollection keys)
		{
			TKey[] array = new TKey[keys.Count];
			keys.CopyTo(array, 0);
			return array;
		}

		public static T[] ToArray<T>(this ICollection<T> col)
		{
			T[] array = new T[col.Count];
			col.CopyTo(array, 0);
			return array;
		}

		public static T[] ToArray<T>(this HashSet<T> vals)
		{
			T[] array = new T[vals.Count];
			vals.CopyTo(array, 0);
			return array;
		}

		public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey[] keys, TValue[] values)
		{
			for (int i = 0; i < keys.Length; i++)
			{
				dict.Add(keys[i], values[i]);
			}
		}

		public static void TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
		{
			if (!dict.ContainsKey(key))
			{
				dict.Add(key, value);
			}
		}

		public static void ForceAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
		{
			if (dict.ContainsKey(key))
			{
				dict[key] = value;
			}
			else
			{
				dict.Add(key, value);
			}
		}

		public static void TryRemove<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
		{
			if (dict.ContainsKey(key))
			{
				dict.Remove(key);
			}
		}

		public static void RemoveNotContained<TKey, TValue>(this Dictionary<TKey, TValue> dict, HashSet<TKey> contained)
		{
			List<TKey> list = null;
			foreach (TKey key in dict.Keys)
			{
				if (!contained.Contains(key))
				{
					if (list == null)
					{
						list = new List<TKey>();
					}
					list.Add(key);
				}
			}
			if (list == null)
			{
				return;
			}
			foreach (TKey item in list)
			{
				dict.Remove(item);
			}
		}

		public static void RemoveWhere<TKey, TValue>(this Dictionary<TKey, TValue> dict, Predicate<TKey> predicate)
		{
			List<TKey> list = null;
			foreach (TKey key in dict.Keys)
			{
				if (predicate(key))
				{
					if (list == null)
					{
						list = new List<TKey>();
					}
					list.Add(key);
				}
			}
			if (list == null)
			{
				return;
			}
			foreach (TKey item in list)
			{
				dict.Remove(item);
			}
		}

		public static TValue CheckGet<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
		{
			if (dict.ContainsKey(key))
			{
				return dict[key];
			}
			return default(TValue);
		}

		public static TKey AnyKey<TKey, TValue>(this Dictionary<TKey, TValue> dict)
		{
			using (Dictionary<TKey, TValue>.Enumerator enumerator = dict.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current.Key;
				}
			}
			return default(TKey);
		}

		public static TValue AnyValue<TKey, TValue>(this Dictionary<TKey, TValue> dict)
		{
			using (Dictionary<TKey, TValue>.Enumerator enumerator = dict.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current.Value;
				}
			}
			return default(TValue);
		}

		public static T Any<T>(this HashSet<T> hashSet)
		{
			using (HashSet<T>.Enumerator enumerator = hashSet.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return default(T);
		}

		public static void RemoveAfter<T>(this List<T> list, int num)
		{
			list.RemoveRange(num + 1, list.Count - num - 1);
		}

		public static T ElementOfType<T>(this IEnumerable arr) where T : class
		{
			foreach (object item in arr)
			{
				if (item is T result)
				{
					return result;
				}
			}
			return null;
		}

		public static bool MinDist<T1, T2>(this Dictionary<T1, T2> dict, Func<T1, float> distEvaluator, out float minDist, out T2 minValue)
		{
			bool result = false;
			minDist = 2.1474836E+09f;
			minValue = default(T2);
			foreach (KeyValuePair<T1, T2> item in dict)
			{
				float num = distEvaluator(item.Key);
				if (num < minDist)
				{
					minDist = num;
					result = true;
					minValue = item.Value;
				}
			}
			return result;
		}

		public static void AddRange<T>(this HashSet<T> set, T[] objs)
		{
			for (int i = 0; i < objs.Length; i++)
			{
				set.Add(objs[i]);
			}
		}

		public static void CheckAdd<T>(this HashSet<T> set, T obj)
		{
			if (!set.Contains(obj))
			{
				set.Add(obj);
			}
		}

		public static void CheckRemove<T>(this HashSet<T> set, T obj)
		{
			if (set.Contains(obj))
			{
				set.Remove(obj);
			}
		}

		public static void SetState<T>(this HashSet<T> set, T obj, bool state)
		{
			if (state && !set.Contains(obj))
			{
				set.Add(obj);
			}
			if (!state && set.Contains(obj))
			{
				set.Remove(obj);
			}
		}

		public static void Normalize(this float[,,] array, int pinnedLayer)
		{
			int length = array.GetLength(0);
			int length2 = array.GetLength(1);
			int length3 = array.GetLength(2);
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					float num = 0f;
					for (int k = 0; k < length3; k++)
					{
						if (k != pinnedLayer)
						{
							num += array[i, j, k];
						}
					}
					float num2 = array[i, j, pinnedLayer];
					if (num2 > 1f)
					{
						num2 = 1f;
						array[i, j, pinnedLayer] = 1f;
					}
					if (num2 < 0f)
					{
						num2 = 0f;
						array[i, j, pinnedLayer] = 0f;
					}
					float num3 = 1f - num2;
					float num4 = ((num > 0f) ? (num3 / num) : 0f);
					for (int l = 0; l < length3; l++)
					{
						if (l != pinnedLayer)
						{
							array[i, j, l] *= num4;
						}
					}
				}
			}
		}

		public static void DrawDebug(this Vector3 pos, float range = 1f, Color color = default(Color))
		{
			if (color.a < 0.001f)
			{
				color = Color.white;
			}
			Debug.DrawLine(pos + new Vector3(-1f, 0f, 1f) * range, pos + new Vector3(1f, 0f, 1f) * range, color);
			Debug.DrawLine(pos + new Vector3(1f, 0f, 1f) * range, pos + new Vector3(1f, 0f, -1f) * range, color);
			Debug.DrawLine(pos + new Vector3(1f, 0f, -1f) * range, pos + new Vector3(-1f, 0f, -1f) * range, color);
			Debug.DrawLine(pos + new Vector3(-1f, 0f, -1f) * range, pos + new Vector3(-1f, 0f, 1f) * range, color);
		}

		public static void DrawDebug(this Rect rect, Color color = default(Color))
		{
			if (color.a < 0.001f)
			{
				color = Color.white;
			}
			Debug.DrawLine(new Vector3(rect.x, 0f, rect.y), new Vector3(rect.x + rect.width, 0f, rect.y), color);
			Debug.DrawLine(new Vector3(rect.x + rect.width, 0f, rect.y), new Vector3(rect.x + rect.width, 0f, rect.y + rect.height), color);
			Debug.DrawLine(new Vector3(rect.x + rect.width, 0f, rect.y + rect.height), new Vector3(rect.x, 0f, rect.y + rect.height), color);
			Debug.DrawLine(new Vector3(rect.x, 0f, rect.y + rect.height), new Vector3(rect.x, 0f, rect.y), color);
		}

		public static Transform AddChild(this Transform tfm, string name = "", Vector3 offset = default(Vector3))
		{
			GameObject gameObject = new GameObject();
			gameObject.name = name;
			gameObject.transform.parent = tfm;
			gameObject.transform.localPosition = offset;
			return gameObject.transform;
		}

		public static T CreateObjectWithComponent<T>(string name = "", Transform parent = null, Vector3 offset = default(Vector3)) where T : MonoBehaviour
		{
			GameObject gameObject = new GameObject();
			if (name != null && parent != null)
			{
				gameObject.transform.parent = parent.transform;
			}
			gameObject.transform.localPosition = offset;
			return gameObject.AddComponent<T>();
		}

		public static float EvaluateMultithreaded(this AnimationCurve curve, float time)
		{
			int num = curve.keys.Length;
			if (time <= curve.keys[0].time)
			{
				return curve.keys[0].value;
			}
			if (time >= curve.keys[num - 1].time)
			{
				return curve.keys[num - 1].value;
			}
			int num2 = 0;
			for (int i = 0; i < num - 1; i++)
			{
				if (curve.keys[num2 + 1].time > time)
				{
					break;
				}
				num2++;
			}
			float num3 = curve.keys[num2 + 1].time - curve.keys[num2].time;
			float num4 = (time - curve.keys[num2].time) / num3;
			float num5 = num4 * num4;
			float num6 = num5 * num4;
			float num7 = 2f * num6 - 3f * num5 + 1f;
			float num8 = num6 - 2f * num5 + num4;
			float num9 = num6 - num5;
			float num10 = -2f * num6 + 3f * num5;
			return num7 * curve.keys[num2].value + num8 * curve.keys[num2].outTangent * num3 + num9 * curve.keys[num2 + 1].inTangent * num3 + num10 * curve.keys[num2 + 1].value;
		}

		public static bool IdenticalTo(this AnimationCurve c1, AnimationCurve c2)
		{
			if (c1 == null || c2 == null)
			{
				return false;
			}
			if (c1.keys.Length != c2.keys.Length)
			{
				return false;
			}
			int num = c1.keys.Length;
			for (int i = 0; i < num; i++)
			{
				if (c1.keys[i].time != c2.keys[i].time || c1.keys[i].value != c2.keys[i].value || c1.keys[i].inTangent != c2.keys[i].inTangent || c1.keys[i].outTangent != c2.keys[i].outTangent)
				{
					return false;
				}
			}
			return true;
		}

		public static Keyframe[] Copy(this Keyframe[] src)
		{
			Keyframe[] array = new Keyframe[src.Length];
			for (int i = 0; i < src.Length; i++)
			{
				array[i].value = src[i].value;
				array[i].time = src[i].time;
				array[i].inTangent = src[i].inTangent;
				array[i].outTangent = src[i].outTangent;
			}
			return array;
		}

		public static AnimationCurve Copy(this AnimationCurve src)
		{
			return new AnimationCurve
			{
				keys = src.keys.Copy()
			};
		}

		public static object Parse(this string s, Type t)
		{
			if (s.Contains("="))
			{
				s = s.Remove(0, s.IndexOf('=') + 1);
			}
			object obj = null;
			if (t == typeof(float))
			{
				return float.Parse(s);
			}
			if (t == typeof(int))
			{
				return int.Parse(s);
			}
			if (t == typeof(bool))
			{
				return bool.Parse(s);
			}
			if (t == typeof(string))
			{
				return s;
			}
			if (t == typeof(byte))
			{
				return byte.Parse(s);
			}
			if (t == typeof(short))
			{
				return short.Parse(s);
			}
			if (t == typeof(long))
			{
				return long.Parse(s);
			}
			if (t == typeof(double))
			{
				return double.Parse(s);
			}
			if (t == typeof(char))
			{
				return char.Parse(s);
			}
			if (t == typeof(decimal))
			{
				return decimal.Parse(s);
			}
			if (t == typeof(sbyte))
			{
				return sbyte.Parse(s);
			}
			if (t == typeof(uint))
			{
				return uint.Parse(s);
			}
			if (t == typeof(ulong))
			{
				return ulong.Parse(s);
			}
			return null;
		}

		public static bool IsEditor()
		{
			return false;
		}

		public static bool IsSelected(Transform transform)
		{
			return false;
		}

		public static Camera GetMainCamera()
		{
			if (IsEditor())
			{
				return null;
			}
			Camera camera = Camera.main;
			if (camera == null)
			{
				camera = UnityEngine.Object.FindObjectOfType<Camera>();
			}
			return camera;
		}

		public static Vector3[] GetCamPoses(bool genAroundMainCam = true, string genAroundTag = null, Vector3[] camPoses = null)
		{
			if (IsEditor())
			{
				camPoses = new Vector3[1];
			}
			else
			{
				GameObject[] array = null;
				if (genAroundTag != null && genAroundTag.Length != 0)
				{
					array = GameObject.FindGameObjectsWithTag(genAroundTag);
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
				if (num == 0)
				{
					Debug.LogError("No Main Camera to deploy");
					return new Vector3[0];
				}
				if (camPoses == null || num != camPoses.Length)
				{
					camPoses = new Vector3[num];
				}
				int num2 = 0;
				if (genAroundMainCam)
				{
					Camera camera = Camera.main;
					if (camera == null)
					{
						camera = UnityEngine.Object.FindObjectOfType<Camera>();
					}
					camPoses[0] = camera.transform.position;
					num2++;
				}
				if (array != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						camPoses[i + num2] = array[i].transform.position;
					}
				}
			}
			return camPoses;
		}

		public static Vector2 GetMousePosition()
		{
			IsEditor();
			return Input.mousePosition;
		}

		public static void GizmosDrawFrame(Vector3 center, Vector3 size, int resolution, float level = 30f)
		{
			Vector3 vector = center - size / 2f;
			Vector3 vector2 = Vector3.zero;
			Vector3 vector3 = Vector3.zero;
			for (float num = 0f; num < size.x + 0.0001f; num += 1f * size.x / (float)resolution)
			{
				RaycastHit hitInfo = default(RaycastHit);
				Vector3 vector4 = new Vector3(vector.x + num, 10000f, vector.z);
				if (Physics.Raycast(new Ray(vector4, Vector3.down * 20000f), out hitInfo, 20000f))
				{
					vector4.y = hitInfo.point.y;
				}
				else if (Physics.Raycast(new Ray(vector4 + new Vector3(1f, 0f, 0f), Vector3.down * 20000f), out hitInfo, 20000f))
				{
					vector4.y = hitInfo.point.y;
				}
				else if (Physics.Raycast(new Ray(vector4 + new Vector3(-1f, 0f, 0f), Vector3.down * 20000f), out hitInfo, 20000f))
				{
					vector4.y = hitInfo.point.y;
				}
				else if (Physics.Raycast(new Ray(vector4 + new Vector3(0f, 0f, 1f), Vector3.down * 20000f), out hitInfo, 20000f))
				{
					vector4.y = hitInfo.point.y;
				}
				else if (Physics.Raycast(new Ray(vector4 + new Vector3(0f, 0f, -1f), Vector3.down * 20000f), out hitInfo, 20000f))
				{
					vector4.y = hitInfo.point.y;
				}
				else
				{
					vector4.y = level;
				}
				if (num > 0.0001f)
				{
					Gizmos.DrawLine(vector2, vector4);
				}
				vector2 = vector4;
				Vector3 vector5 = new Vector3(vector.x + num, 10000f, vector.z + size.z);
				if (Physics.Raycast(new Ray(vector5, Vector3.down * 20000f), out hitInfo, 20000f))
				{
					vector5.y = hitInfo.point.y;
				}
				else if (Physics.Raycast(new Ray(vector5 + new Vector3(1f, 0f, 0f), Vector3.down * 20000f), out hitInfo, 20000f))
				{
					vector5.y = hitInfo.point.y;
				}
				else if (Physics.Raycast(new Ray(vector5 + new Vector3(-1f, 0f, 0f), Vector3.down * 20000f), out hitInfo, 20000f))
				{
					vector5.y = hitInfo.point.y;
				}
				else if (Physics.Raycast(new Ray(vector5 + new Vector3(0f, 0f, 1f), Vector3.down * 20000f), out hitInfo, 20000f))
				{
					vector5.y = hitInfo.point.y;
				}
				else if (Physics.Raycast(new Ray(vector5 + new Vector3(0f, 0f, -1f), Vector3.down * 20000f), out hitInfo, 20000f))
				{
					vector5.y = hitInfo.point.y;
				}
				else
				{
					vector5.y = level;
				}
				if (num > 0.0001f)
				{
					Gizmos.DrawLine(vector3, vector5);
				}
				vector3 = vector5;
			}
			for (float num2 = 0f; num2 < size.z + 0.0001f; num2 += 1f * size.z / (float)resolution)
			{
				RaycastHit hitInfo2 = default(RaycastHit);
				Vector3 vector6 = new Vector3(vector.x, 10000f, vector.z + num2);
				if (Physics.Raycast(new Ray(vector6, Vector3.down * 20000f), out hitInfo2, 20000f))
				{
					vector6.y = hitInfo2.point.y;
				}
				else if (Physics.Raycast(new Ray(vector6 + new Vector3(1f, 0f, 0f), Vector3.down * 20000f), out hitInfo2, 20000f))
				{
					vector6.y = hitInfo2.point.y;
				}
				else if (Physics.Raycast(new Ray(vector6 + new Vector3(-1f, 0f, 0f), Vector3.down * 20000f), out hitInfo2, 20000f))
				{
					vector6.y = hitInfo2.point.y;
				}
				else if (Physics.Raycast(new Ray(vector6 + new Vector3(0f, 0f, 1f), Vector3.down * 20000f), out hitInfo2, 20000f))
				{
					vector6.y = hitInfo2.point.y;
				}
				else if (Physics.Raycast(new Ray(vector6 + new Vector3(0f, 0f, -1f), Vector3.down * 20000f), out hitInfo2, 20000f))
				{
					vector6.y = hitInfo2.point.y;
				}
				else
				{
					vector6.y = level;
				}
				if (num2 > 0.0001f)
				{
					Gizmos.DrawLine(vector2, vector6);
				}
				vector2 = vector6;
				Vector3 vector7 = new Vector3(vector.x + size.x, 10000f, vector.z + num2);
				if (Physics.Raycast(new Ray(vector7, Vector3.down * 20000f), out hitInfo2, 20000f))
				{
					vector7.y = hitInfo2.point.y;
				}
				else if (Physics.Raycast(new Ray(vector7 + new Vector3(1f, 0f, 0f), Vector3.down * 20000f), out hitInfo2, 20000f))
				{
					vector7.y = hitInfo2.point.y;
				}
				else if (Physics.Raycast(new Ray(vector7 + new Vector3(-1f, 0f, 0f), Vector3.down * 20000f), out hitInfo2, 20000f))
				{
					vector7.y = hitInfo2.point.y;
				}
				else if (Physics.Raycast(new Ray(vector7 + new Vector3(0f, 0f, 1f), Vector3.down * 20000f), out hitInfo2, 20000f))
				{
					vector7.y = hitInfo2.point.y;
				}
				else if (Physics.Raycast(new Ray(vector7 + new Vector3(0f, 0f, -1f), Vector3.down * 20000f), out hitInfo2, 20000f))
				{
					vector7.y = hitInfo2.point.y;
				}
				else
				{
					vector7.y = level;
				}
				if (num2 > 0.0001f)
				{
					Gizmos.DrawLine(vector3, vector7);
				}
				vector3 = vector7;
			}
		}

		public static void Planar(this Mesh mesh, float size, int resolution)
		{
			float num = size / (float)resolution;
			Vector3[] array = new Vector3[(resolution + 1) * (resolution + 1)];
			Vector2[] array2 = new Vector2[array.Length];
			int[] array3 = new int[resolution * resolution * 2 * 3];
			int num2 = 0;
			int num3 = 0;
			for (float num4 = 0f; num4 < size + 0.001f; num4 += num)
			{
				for (float num5 = 0f; num5 < size + 0.001f; num5 += num)
				{
					array[num2] = new Vector3(num4, 0f, num5);
					array2[num2] = new Vector2(num4 / size, num5 / size);
					if (num4 > 0.001f && num5 > 0.001f)
					{
						array3[num3] = num2 - (resolution + 1);
						array3[num3 + 1] = num2 - 1;
						array3[num3 + 2] = num2 - resolution - 2;
						array3[num3 + 3] = num2 - 1;
						array3[num3 + 4] = num2 - (resolution + 1);
						array3[num3 + 5] = num2;
						num3 += 6;
					}
					num2++;
				}
			}
			mesh.Clear();
			mesh.vertices = array;
			mesh.uv = array2;
			mesh.triangles = array3;
		}

		public static string LogBinary(this int src)
		{
			string text = "";
			for (int i = 0; i < 32; i++)
			{
				if (i % 4 == 0)
				{
					text = " " + text;
				}
				text = (src & 1) + text;
				src >>= 1;
			}
			return text;
		}

		public static string ToStringArray<T>(this T[] array)
		{
			string text = "";
			for (int i = 0; i < array.Length; i++)
			{
				text += array[i].ToString();
				if (i != array.Length - 1)
				{
					text += ",";
				}
			}
			return text;
		}

		public static Color[] ToColors(this Vector4[] src)
		{
			Color[] array = new Color[src.Length];
			for (int i = 0; i < src.Length; i++)
			{
				array[i] = src[i];
			}
			return array;
		}

		public static Texture2D GetBiggestTexture(this Texture2D[] textures)
		{
			int num = 0;
			int num2 = -1;
			for (int i = 0; i < textures.Length; i++)
			{
				if (!(textures[i] == null))
				{
					if (textures[i].width > num)
					{
						num = textures[i].width;
						num2 = i;
					}
					if (textures[i].height > num)
					{
						num = textures[i].height;
						num2 = i;
					}
				}
			}
			if (num2 >= 0)
			{
				return textures[num2];
			}
			return null;
		}

		public static string ToStringMemberwise<T>(this IEnumerable list, Func<T, string> toStringFn = null)
		{
			string text = "";
			foreach (T item in list)
			{
				text = ((toStringFn != null) ? (text + toStringFn(item)) : (text + item.ToString()));
				text += ", ";
			}
			if (text.Length >= 2)
			{
				text = text.Substring(0, text.Length - 2);
			}
			return text;
		}

		public static string Nicify(this string camelCase)
		{
			string text = Regex.Replace(camelCase, "(\\B[A-Z])", " $1");
			return text.Substring(0, 1).ToUpper() + text.Substring(1);
		}

		public static void CheckSetInt(this Material mat, string name, int val)
		{
			if (mat.HasProperty(name))
			{
				mat.SetInt(name, val);
			}
		}

		public static void CheckSetFloat(this Material mat, string name, float val)
		{
			if (mat.HasProperty(name))
			{
				mat.SetFloat(name, val);
			}
		}

		public static void CheckSetTexture(this Material mat, string name, Texture tex)
		{
			if (mat.HasProperty(name))
			{
				mat.SetTexture(name, tex);
			}
		}

		public static void CheckSetVector(this Material mat, string name, Vector4 val)
		{
			if (mat.HasProperty(name))
			{
				mat.SetVector(name, val);
			}
		}

		public static void CheckSetColor(this Material mat, string name, Color val)
		{
			if (mat.HasProperty(name))
			{
				mat.SetColor(name, val);
			}
		}

		public static int Max<T>(this IEnumerable<T> enumerable, Func<T, int> getNumFn)
		{
			int num = -2147483648;
			foreach (T item in enumerable)
			{
				int num2 = getNumFn(item);
				if (num2 > num)
				{
					num = num2;
				}
			}
			return num;
		}

		public static uint Max<T>(this IEnumerable<T> enumerable, Func<T, uint> getNumFn)
		{
			uint num = 0u;
			foreach (T item in enumerable)
			{
				uint num2 = getNumFn(item);
				if (num2 > num)
				{
					num = num2;
				}
			}
			return num;
		}

		public static bool Contains<T>(this List<T> list, T val, out int index)
		{
			index = list.IndexOf(val);
			return index >= 0;
		}

		public static T GetInRange<T>(this List<T> list, int index) where T : class
		{
			if (index > 0 && index < list.Count)
			{
				return list[index];
			}
			return null;
		}

		public static void ExtendSet<T>(this List<T> list, T val, int index)
		{
			if (list.Count - 1 < index)
			{
				list.AddRange(new T[index - (list.Count - 1)]);
			}
			list[index] = val;
		}
	}
}
