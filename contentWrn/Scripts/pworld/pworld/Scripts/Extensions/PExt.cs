using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace pworld.Scripts.Extensions
{
	public static class PExt
	{
		public static float GetTimeLength(this AnimationCurve curve)
		{
			Keyframe keyframe = curve.keys[0];
			Keyframe keyframe2 = curve.keys[curve.length - 1];
			return keyframe2.time - keyframe.time;
		}

		public static int2 UnFlatten(int index, int width)
		{
			return new int2(index % width, index / width);
		}

		public static int Flatten(int x, int y, int width)
		{
			throw new Exception();
		}

		public static int FlattenCorrect(int x, int y, int width)
		{
			throw new Exception();
		}

		public static List<T> RemoveRange<T>(this List<T> me, List<T> other)
		{
			foreach (T item in other)
			{
				me.Remove(item);
			}
			return me;
		}

		public static void MakeAssetReadable(string path)
		{
		}

		public static void SaveObj(UnityEngine.Object obj)
		{
		}

		public static Bounds ToWorld(this Bounds me, Transform transform)
		{
			Vector3 center = transform.TransformPoint(me.center);
			Vector3 extents = me.extents;
			Vector3 vector = transform.TransformVector(extents.x, 0f, 0f);
			Vector3 vector2 = transform.TransformVector(0f, extents.y, 0f);
			Vector3 vector3 = transform.TransformVector(0f, 0f, extents.z);
			extents.x = Mathf.Abs(vector.x) + Mathf.Abs(vector2.x) + Mathf.Abs(vector3.x);
			extents.y = Mathf.Abs(vector.y) + Mathf.Abs(vector2.y) + Mathf.Abs(vector3.y);
			extents.z = Mathf.Abs(vector.z) + Mathf.Abs(vector2.z) + Mathf.Abs(vector3.z);
			return new Bounds
			{
				center = center,
				extents = extents
			};
		}

		public static ConfigurableJoint AttachTo(this Rigidbody me, Rigidbody other)
		{
			ConfigurableJoint configurableJoint = me.gameObject.AddComponent<ConfigurableJoint>();
			configurableJoint.connectedBody = other;
			configurableJoint.xMotion = ConfigurableJointMotion.Locked;
			configurableJoint.yMotion = ConfigurableJointMotion.Locked;
			configurableJoint.zMotion = ConfigurableJointMotion.Locked;
			configurableJoint.angularXMotion = ConfigurableJointMotion.Locked;
			configurableJoint.angularYMotion = ConfigurableJointMotion.Locked;
			configurableJoint.angularZMotion = ConfigurableJointMotion.Locked;
			return configurableJoint;
		}

		public static Vector3[] GetPositions(this LineRenderer me)
		{
			Vector3[] array = new Vector3[me.positionCount];
			me.GetPositions(array);
			return array;
		}

		public static bool GetMouseRaycast(out RaycastHit hit, int groundMask, bool onlyMap = false)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (onlyMap)
			{
				return Physics.Raycast(ray, out hit, 10000f, groundMask);
			}
			return Physics.Raycast(ray, out hit, 10000f);
		}

		public static bool GetMouseRaycastAll(out RaycastHit[] hit, int groundMask, bool onlyMap = false)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (onlyMap)
			{
				hit = Physics.RaycastAll(ray, 10000f, groundMask);
			}
			else
			{
				hit = Physics.RaycastAll(ray, 10000f, groundMask);
			}
			return hit.Length != 0;
		}

		public static bool CanSee(Vector3 from, Vector3 to, int layermask)
		{
			Physics.Raycast(new Ray(from, to - from), out var hitInfo, Vector3.Distance(from, to), layermask);
			return hitInfo.transform == null;
		}

		public static KEY_STATE GetKeyState(this Input input, KeyCode key)
		{
			if (Input.GetKeyDown(key))
			{
				return KEY_STATE.justPressed;
			}
			if (Input.GetKey(key))
			{
				return KEY_STATE.held;
			}
			if (Input.GetKeyUp(key))
			{
				return KEY_STATE.released;
			}
			return KEY_STATE.none;
		}

		public static bool IsPlaying(this Animator me, string name, int layerIndex = 0)
		{
			return me.GetCurrentAnimatorStateInfo(layerIndex).IsName(name);
		}

		private static string GetVariableName<T>(Expression<Func<T>> expr)
		{
			return ((MemberExpression)expr.Body).Member.Name;
		}

		public static void DoIfNotNull(this object me, Action thingToDo)
		{
			if (me != null)
			{
				thingToDo?.Invoke();
			}
		}

		public static T PGetComp<T>(this T me, GameObject owner) where T : MonoBehaviour
		{
			return owner.gameObject.GetComponent<T>();
		}

		public static void PCompInit<T>(this T me, ref T alsoMe, GameObject owner) where T : MonoBehaviour
		{
			alsoMe = owner.gameObject.GetComponent<T>();
		}

		public static T PLazyFetch<T>(this T me, ref T alsoMe, GameObject owner) where T : MonoBehaviour
		{
			if (me == null)
			{
				alsoMe = owner.GetComponent<T>();
			}
			return alsoMe;
		}

		public static T PLazyFetchP<T>(this MonoBehaviour me, ref T alsoMe, GameObject owner) where T : MonoBehaviour
		{
			if (me == null)
			{
				alsoMe = owner.GetComponentInParent<T>();
			}
			return alsoMe;
		}

		public static T PLazyFetchC<T>(this MonoBehaviour me, ref T alsoMe, GameObject owner) where T : MonoBehaviour
		{
			if (me == null)
			{
				alsoMe = owner.GetComponentInChildren<T>();
			}
			return alsoMe;
		}

		public static void DoAtEndOfFrame(this MonoBehaviour me, Action doIt)
		{
			me.StartCoroutine(DoAtEndOfFrame(doIt));
		}

		public static IEnumerator DoAtEndOfFrame(Action doIt)
		{
			yield return new WaitForEndOfFrame();
			doIt?.Invoke();
		}

		public static void DoInSec(this MonoBehaviour me, float time, Action doIt)
		{
			me.StartCoroutine(DoInSec(time, doIt));
		}

		public static IEnumerator DoInSec(float time, Action doIt)
		{
			yield return new WaitForSeconds(time);
			doIt?.Invoke();
		}

		public static IEnumerator EvalCurve(AnimationCurve curve, Action<float> readCurve, Action finished, bool reverse = false)
		{
			if (curve.keys.Length == 0)
			{
				Debug.Log("Given Empy curve");
				yield break;
			}
			float deltaTime = Time.deltaTime;
			float c = curve.keys[0].time;
			float t = curve.keys[curve.keys.Length - 1].time;
			if (reverse)
			{
				c = curve.keys[curve.keys.Length - 1].time;
				deltaTime = 0f - deltaTime;
			}
			float cStart = c;
			while (Math.Abs(c - cStart) < t)
			{
				c += deltaTime;
				readCurve(curve.Evaluate(c));
				yield return null;
			}
			finished();
		}

		public static void DoTimes(int times, Action DoIt)
		{
			for (int i = 0; i < times; i++)
			{
				DoIt?.Invoke();
			}
		}

		public static T GetEither<T>(T lhs, T rhs)
		{
			if (UnityEngine.Random.Range(0, 2) != 0)
			{
				return rhs;
			}
			return lhs;
		}

		public static StoppableCoroutine StartStoppableCoroutine(this MonoBehaviour mb, IEnumerator coroutine)
		{
			return new StoppableCoroutine(mb, coroutine);
		}

		public static bool IsPointOverUIObject(this EventSystem me, Vector3 screenPosition)
		{
			PointerEventData pointerEventData = new PointerEventData(me);
			pointerEventData.position = screenPosition;
			List<RaycastResult> list = new List<RaycastResult>();
			me.RaycastAll(pointerEventData, list);
			return list.Count > 0;
		}

		public static bool GetUiUnderPos(this EventSystem me, out List<RaycastResult> hits, Vector3 screenPos)
		{
			PointerEventData eventData = new PointerEventData(me)
			{
				position = screenPos
			};
			hits = new List<RaycastResult>();
			me.RaycastAll(eventData, hits);
			return hits.Count > 0;
		}

		public static IEnumerable<T> PGetComponentsInChildrenButNotMe<T>(this GameObject me, bool includeInActive = false) where T : Component
		{
			List<T> list = me.GetComponentsInChildren<T>(includeInActive).ToList();
			list.RemoveAll((T behaviour) => behaviour.gameObject == me);
			return list;
		}
	}
}
