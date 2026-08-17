using System;
using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.Rope
{
	[ExecuteAlways]
	[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
	public class Rope : MonoBehaviour
	{
		public Transform startAttach;

		public Transform endAttach;

		public float length = 5f;

		public float widthStart;

		public float widthEnd;

		public float swingAngle = 0.5f;

		public float swingFreq = 2f;

		public float swingOffset;

		[Range(0f, 1f)]
		public float alpha;

		public RopeEnd startConnect;

		public Transform startObj;

		public RopeEnd endConnect;

		public Transform endObj;

		public float ropeForce;

		public float stretch = 8f;

		[Range(0f, 1f)]
		public float ropeStart;

		[Range(0f, 1f)]
		public float ropeEnd;

		[Range(0f, 1f)]
		public float ropeStartAttach;

		[Range(0f, 1f)]
		public float ropeEndAttach;

		[Range(-0.2f, 0.2f)]
		public float adjustZ;

		[Range(-0.2f, 0.2f)]
		public float adjustY;

		[Range(-0.2f, 0.2f)]
		public float adjustZEnd;

		[Range(-0.2f, 0.2f)]
		public float adjustYEnd;

		public RopeAttach[] attached;

		public List<RopeAttach> attachedObjects = new List<RopeAttach>();

		private bool dirty = true;

		private MaterialPropertyBlock pblock;

		private MeshRenderer mr;

		private float lastLength;

		[SerializeField]
		[HideInInspector]
		private Vector3 p0;

		[SerializeField]
		[HideInInspector]
		private Vector3 p1;

		[SerializeField]
		[HideInInspector]
		private float a;

		[SerializeField]
		[HideInInspector]
		private float p;

		[SerializeField]
		[HideInInspector]
		private float q;

		[SerializeField]
		[HideInInspector]
		private float arcl;

		[SerializeField]
		[HideInInspector]
		private float flipx;

		private Vector4 adjust;

		private Vector4 width;

		private Rigidbody rbs;

		private Rigidbody rbe;

		private Rope ropeS;

		private Rope ropeE;

		public Vector3 endPos = new Vector3(0f, 0f, 1f);

		public int startIndex;

		public int endIndex;

		public int ropeMeshIndex;

		public bool showStartConnect;

		public bool showEndConnect;

		[Range(4f, 128f)]
		public static int iterations = 32;

		[Range(4f, 128f)]
		public static int minIterations = 32;

		private float dist;

		private Vector3 dir;

		private Vector3 forceDir;

		private Vector3 rbsOffset;

		private Vector3 rbeOffset;

		public bool isPrefab;

		public Vector3 startLscl = Vector3.one;

		public Vector3 endLscl = Vector3.one;

		private bool visible = true;

		public void SetDirty(bool _dirty = true)
		{
			dirty = _dirty;
		}

		public float GetArcLength()
		{
			return arcl;
		}

		public float GetDist()
		{
			return dist;
		}

		public Vector3 GetForceDir()
		{
			return forceDir;
		}

		public void SetLength(float _len)
		{
			length = _len;
			SetDirty();
		}

		public void SetEndObject(RopeEnd end, bool start = true)
		{
			if (start)
			{
				if ((bool)startObj)
				{
					startLscl = startObj.transform.localScale;
					UnityEngine.Object.DestroyImmediate(startObj.gameObject);
					startObj = null;
				}
				if ((bool)end && (bool)end.prefab)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(end.prefab);
					gameObject.transform.SetParent(base.transform);
					startConnect = end;
					startObj = gameObject.transform;
					startObj.transform.localScale = startLscl;
				}
				else
				{
					startConnect = null;
				}
			}
			else
			{
				if ((bool)endObj)
				{
					endLscl = endObj.transform.localScale;
					UnityEngine.Object.DestroyImmediate(endObj.gameObject);
					endObj = null;
				}
				if ((bool)end && (bool)end.prefab)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate(end.prefab);
					gameObject2.transform.SetParent(base.transform);
					endConnect = end;
					endObj = gameObject2.transform;
					endObj.transform.localScale = endLscl;
				}
				else
				{
					endConnect = null;
				}
			}
			SetDirty();
		}

		private void Awake()
		{
			isPrefab = true;
		}

		private void Start()
		{
			swingOffset = UnityEngine.Random.Range(0f, 4f);
			pblock = new MaterialPropertyBlock();
			mr = GetComponent<MeshRenderer>();
			UpdateBounds();
			if ((bool)endAttach)
			{
				rbe = endAttach.GetComponentInParent<Rigidbody>();
				ropeE = endAttach.GetComponent<Rope>();
				if ((bool)rbe)
				{
					rbeOffset = rbe.transform.InverseTransformPoint(endAttach.position);
				}
			}
			if (!startAttach)
			{
				startAttach = base.transform;
			}
			else if (startAttach != base.transform)
			{
				ropeS = startAttach.GetComponent<Rope>();
			}
			if ((bool)startAttach)
			{
				rbs = startAttach.GetComponentInParent<Rigidbody>();
				if ((bool)rbs)
				{
					rbsOffset = rbs.transform.InverseTransformPoint(startAttach.position);
				}
			}
			SetDirty();
		}

		public void SetLength()
		{
			length = Vector3.Distance(base.transform.position, endPos);
			stretch = length;
		}

		public void SetStartAttach(Transform obj)
		{
			startAttach = obj;
			if ((bool)startAttach)
			{
				ropeS = startAttach.GetComponent<Rope>();
			}
			else
			{
				ropeS = null;
			}
		}

		public void SetEndAttach(Transform obj)
		{
			endAttach = obj;
			if ((bool)endAttach)
			{
				ropeE = endAttach.GetComponent<Rope>();
			}
			else
			{
				ropeE = null;
			}
		}

		public void UpdateBounds()
		{
			if (mr == null)
			{
				mr = GetComponent<MeshRenderer>();
			}
			if (mr != null)
			{
				Vector3 vector = ((endAttach != null) ? base.transform.InverseTransformPoint(endAttach.position) : endPos);
				Vector3 center = vector * 0.5f;
				Vector3 size = new Vector3(Mathf.Abs(vector.x) + 40f, Mathf.Abs(vector.y) + 80f, Mathf.Abs(vector.z) + 40f);
				mr.localBounds = new Bounds(center, size);
			}
		}

		private void Update()
		{
			if (isPrefab)
			{
				endPos = base.transform.position + Vector3.forward * 2f;
			}
			if (!visible)
			{
				return;
			}
			UpdateCurve();
			if (attached == null)
			{
				return;
			}
			for (int i = 0; i < attached.Length; i++)
			{
				if ((bool)attached[i].obj)
				{
					Vector3 up;
					Vector3 position = CalcPos(attached[i].alpha, out up);
					Vector3 position2 = CalcPos(attached[i].alpha + 0.01f);
					attached[i].obj.position = base.transform.TransformPoint(CalcPos(attached[i].alpha));
					Quaternion quaternion = Quaternion.LookRotation(base.transform.TransformPoint(position) - base.transform.TransformPoint(position2), up);
					attached[i].obj.rotation = quaternion * Quaternion.Euler(attached[i].rot);
				}
			}
		}

		public void RopeUpdate()
		{
			if (!visible)
			{
				return;
			}
			UpdateCurve();
			if (attached == null)
			{
				return;
			}
			for (int i = 0; i < attached.Length; i++)
			{
				if ((bool)attached[i].obj)
				{
					Vector3 up;
					Vector3 position = CalcPos(attached[i].alpha, out up);
					Vector3 position2 = CalcPos(attached[i].alpha + 0.01f);
					attached[i].obj.position = base.transform.TransformPoint(CalcPos(attached[i].alpha));
					Quaternion quaternion = Quaternion.LookRotation(base.transform.TransformPoint(position) - base.transform.TransformPoint(position2), up);
					attached[i].obj.rotation = quaternion * Quaternion.Euler(attached[i].rot);
				}
			}
		}

		public void UpdateCurve()
		{
			if (pblock == null)
			{
				pblock = new MaterialPropertyBlock();
			}
			Vector3 zero = Vector3.zero;
			if ((bool)ropeS)
			{
				base.transform.position = ropeS.transform.TransformPoint(ropeS.CalcPos(ropeStart));
			}
			else if ((bool)startAttach)
			{
				base.transform.position = startAttach.position;
			}
			Vector3 vector;
			Vector3 vector2;
			if ((bool)endAttach && (bool)ropeE)
			{
				vector = (endPos = ropeE.transform.TransformPoint(ropeE.CalcPos(ropeEnd)));
				vector2 = base.transform.InverseTransformPoint(vector);
			}
			else if ((bool)endAttach)
			{
				vector = (endPos = endAttach.position);
				vector2 = base.transform.InverseTransformPoint(vector);
			}
			else
			{
				vector = endPos;
				vector2 = base.transform.InverseTransformPoint(endPos);
			}
			forceDir = (base.transform.position - vector).normalized;
			bool flag = vector2.y < zero.y;
			p0 = (flag ? vector2 : zero);
			p1 = (flag ? zero : vector2);
			Vector3 vector3 = p1 - p0;
			dist = vector3.magnitude;
			dir = vector3.normalized;
			if (stretch > length)
			{
				stretch = length;
			}
			if (dist != lastLength || dirty)
			{
				dirty = false;
				lastLength = dist;
				arcl = Mathf.Max(dist * 1.0001f, length);
				if (dist < length)
				{
					float num = (length - dist * 1.0001f) / length;
					arcl -= num * stretch;
				}
				float num2 = Mathf.Sqrt(vector3.x * vector3.x + vector3.z * vector3.z);
				float y = vector3.y;
				float num3 = Mathf.Sqrt(arcl * arcl - y * y);
				if (num2 == 0f)
				{
					return;
				}
				float num4 = 0f;
				float num5 = 1f;
				int num6 = 0;
				while (num6 < iterations && (double)num3 < (double)(2f * num5) * Math.Sinh(num2 / (2f * num5)))
				{
					num6++;
					num4 = num5;
					num5 *= 2f;
				}
				num6 += minIterations;
				a = 0f;
				while (num6 > 0)
				{
					num6--;
					a = (num4 + num5) * 0.5f;
					if ((double)num3 < (double)(2f * a) * Math.Sinh(num2 / (2f * a)))
					{
						num4 = a;
					}
					else
					{
						num5 = a;
					}
				}
				p = (num2 - a * Mathf.Log((arcl + y) / (arcl - y))) / 2f;
				q = (y - arcl * (1f / (float)Math.Tanh(num2 / (2f * a)))) / 2f;
				flipx = (flag ? 1f : 0f);
				float num7 = 0f;
				float num8 = 0f;
				adjust.x = adjustZ;
				adjust.y = adjustZEnd;
				adjust.w = adjustY;
				adjust.z = adjustYEnd;
				width.x = 1f + widthStart;
				width.y = 1f + widthEnd;
				width.z = length;
				if ((bool)startConnect && (bool)startObj)
				{
					num7 = startConnect.ropeStart;
					adjust.x += startConnect.adjustZ;
					adjust.z += startConnect.adjustY;
					width.x = startConnect.width + widthStart;
				}
				if ((bool)endConnect && (bool)endObj)
				{
					num8 = endConnect.ropeStart;
					adjust.y += endConnect.adjustZ;
					adjust.w += endConnect.adjustY;
					width.y = endConnect.width + widthEnd;
				}
				num7 += ropeStartAttach;
				num8 += ropeEndAttach;
				if (dist <= length)
				{
					pblock.SetVector("_MinMax", new Vector4((0f - num7) / length, 1f + num8 / length, 0f, 0f));
				}
				else
				{
					pblock.SetVector("_MinMax", new Vector4((0f - num7) / arcl, 1f + num8 / arcl, 0f, 0f));
				}
				pblock.SetVector("_P0", p0);
				pblock.SetVector("_P1", p1);
				pblock.SetVector("_Apq", new Vector3(a, p, q));
				pblock.SetFloat("_ArcLength", arcl);
				pblock.SetFloat("_FlipX", flipx);
				pblock.SetVector("_Width", width);
				pblock.SetVector("_Adjust", adjust);
				pblock.SetFloat("_SwingAngle", swingAngle);
				pblock.SetFloat("_SwingFreq", swingFreq);
				pblock.SetFloat("_SwingOffset", swingOffset);
				mr.SetPropertyBlock(pblock);
				UpdateBounds();
			}
			if ((bool)startConnect)
			{
				Vector3 up;
				Vector3 position = CalcPosFromDist(startConnect.tangent, start: true, out up);
				if ((bool)startObj)
				{
					startObj.transform.position = base.transform.position;
					Quaternion quaternion = Quaternion.Euler(0f, 0f, 0f);
					Quaternion rotation = Quaternion.LookRotation(base.transform.position - base.transform.TransformPoint(position), up) * quaternion;
					startObj.transform.rotation = rotation;
				}
			}
			if ((bool)endConnect)
			{
				Vector3 up2;
				Vector3 position2 = CalcPosFromDist(endConnect.tangent, start: false, out up2);
				if ((bool)endObj)
				{
					endObj.transform.position = base.transform.TransformPoint(vector2);
					Quaternion quaternion2 = Quaternion.Euler(0f, 0f, 0f);
					Quaternion rotation2 = Quaternion.LookRotation(base.transform.TransformPoint(vector2) - base.transform.TransformPoint(position2), up2) * quaternion2;
					endObj.transform.rotation = rotation2;
				}
			}
		}

		private void FixedUpdate()
		{
			float num = Mathf.Max((dist - length) * ropeForce, 0f);
			if (num > 0f)
			{
				if ((bool)rbs)
				{
					rbs.AddForceAtPosition(num * -forceDir, rbs.transform.TransformPoint(rbsOffset));
				}
				if ((bool)rbe)
				{
					rbe.AddForceAtPosition(num * forceDir, rbe.transform.TransformPoint(rbeOffset));
				}
			}
		}

		public void RopeFixedUpdate()
		{
			float num = Mathf.Max((dist - length) * ropeForce, 0f);
			if (num > 0f)
			{
				if ((bool)rbs)
				{
					rbs.AddForceAtPosition(num * -forceDir, rbs.transform.TransformPoint(rbsOffset));
				}
				if ((bool)rbe)
				{
					rbe.AddForceAtPosition(num * forceDir, rbe.transform.TransformPoint(rbeOffset));
				}
			}
		}

		private float asinh(float x)
		{
			return Mathf.Log(x + Mathf.Sqrt(x * x + 1f));
		}

		public Vector3 CalcPos(float alpha)
		{
			Vector3 up;
			return CalcPos(alpha, out up);
		}

		public Vector3 CalcPos(float alpha, out Vector3 up)
		{
			Vector3 vector = p1 - p0;
			Vector3 normalized = new Vector3(0f - vector.z, 0f, vector.x).normalized;
			float num = Mathf.Sqrt(vector.x * vector.x + vector.z * vector.z);
			float num2 = Mathf.Lerp(alpha, 1f - alpha, flipx);
			float num3 = (a * asinh(num2 * arcl / a - (float)Math.Sinh(p / a)) + p) / num;
			float num4 = num3 + 0.01f;
			float num5 = a * (float)Math.Cosh((num3 * num - p) / a) + q;
			float y = a * (float)Math.Cosh((num4 * num - p) / a) + q;
			float num6 = num5 - vector.y * num3;
			float timeSinceLevelLoad = Time.timeSinceLevelLoad;
			float num7 = Mathf.Sin((float)Math.PI * swingFreq * timeSinceLevelLoad + swingOffset);
			if (flipx > 0f)
			{
				num7 = 0f - num7;
			}
			float num8 = swingAngle;
			float num9 = Mathf.Sin(num7 * num8 * 0.5f) * num6;
			float y2 = Mathf.Cos(num7 * num8 * 0.5f) * num6;
			Vector3 vector2 = new Vector3(num9 * normalized.x, y2, num9 * normalized.z);
			Vector3 vector3 = new Vector3(vector.x * num3, num5, vector.z * num3);
			Vector3 normalized2 = (new Vector3(vector.x * num4, y, vector.z * num4) - vector3).normalized;
			up = Vector3.Cross(normalized, normalized2);
			return vector3 + p0 + vector2;
		}

		public Vector3 CalcPosNoUp(float alpha)
		{
			Vector3 vector = p1 - p0;
			Vector3 normalized = new Vector3(0f - vector.z, 0f, vector.x).normalized;
			float num = Mathf.Sqrt(vector.x * vector.x + vector.z * vector.z);
			float num2 = Mathf.Lerp(alpha, 1f - alpha, flipx);
			float num3 = (a * asinh(num2 * arcl / a - (float)Math.Sinh(p / a)) + p) / num;
			float num4 = num3 + 0.01f;
			float num5 = a * (float)Math.Cosh((num3 * num - p) / a) + q;
			_ = a;
			Math.Cosh((num4 * num - p) / a);
			_ = q;
			float num6 = num5 - vector.y * num3;
			float timeSinceLevelLoad = Time.timeSinceLevelLoad;
			float num7 = Mathf.Sin((float)Math.PI * swingFreq * timeSinceLevelLoad + swingOffset);
			if (flipx > 0f)
			{
				num7 = 0f - num7;
			}
			float num8 = swingAngle;
			float num9 = Mathf.Sin(num7 * num8 * 0.5f) * num6;
			float y = Mathf.Cos(num7 * num8 * 0.5f) * num6;
			Vector3 vector2 = new Vector3(num9 * normalized.x, y, num9 * normalized.z);
			return new Vector3(vector.x * num3, num5, vector.z * num3) + p0 + vector2;
		}

		public Vector3 CalcPosFromDist(float distance, bool start, out Vector3 up)
		{
			if (start)
			{
				float num = distance / arcl;
				float magnitude = CalcPosNoUp(num).magnitude;
				num /= magnitude / distance;
				return CalcPos(num, out up);
			}
			float num2 = distance / arcl;
			Vector3 vector = CalcPosNoUp(1f - num2);
			float magnitude2 = (base.transform.InverseTransformPoint(endPos) - vector).magnitude;
			num2 /= magnitude2 / distance;
			return CalcPos(1f - num2, out up);
		}

		public Vector3 CalcPosSimple(float alpha, out Vector3 up)
		{
			Vector3 vector = p1 - p0;
			Vector3 normalized = new Vector3(0f - vector.z, 0f, vector.x).normalized;
			float num = Mathf.Sqrt(vector.x * vector.x + vector.z * vector.z);
			Vector3 vector2 = new Vector3(alpha, 0f, 0f);
			float num2 = Mathf.Lerp(vector2.x, 1f - vector2.x, flipx);
			float num3 = (a * asinh(num2 * arcl / a - (float)Math.Sinh(p / a)) + p) / num;
			float num4 = num3 + 0.01f;
			float num5 = a * (float)Math.Cosh((num3 * num - p) / a) + q;
			float y = a * (float)Math.Cosh((num4 * num - p) / a) + q;
			float num6 = num5 - vector.y * num3;
			float timeSinceLevelLoad = Time.timeSinceLevelLoad;
			float num7 = Mathf.Sin((float)Math.PI * swingFreq * timeSinceLevelLoad + swingOffset);
			if (flipx > 0f)
			{
				num7 = 0f - num7;
			}
			float num8 = swingAngle;
			float num9 = Mathf.Sin(num7 * num8 * 0.5f) * num6;
			float y2 = Mathf.Cos(num7 * num8 * 0.5f) * num6;
			Vector3 vector3 = new Vector3(num9 * normalized.x, y2, num9 * normalized.z);
			Vector3 vector4 = new Vector3(vector.x * num3, num5, vector.z * num3);
			Vector3 normalized2 = (new Vector3(vector.x * num4, y, vector.z * num4) - vector4).normalized;
			up = Vector3.Cross(normalized, normalized2);
			float x = normalized.x * vector2.z * 1f * (1f - flipx * 2f);
			float z = normalized.z * vector2.z * 1f * (1f - flipx * 2f);
			return vector4 + p0 + vector3 + new Vector3(x, 0f, z) + up * vector2.y * 1f;
		}

		private void OnBecameVisible()
		{
			visible = true;
		}

		private void OnBecameInvisible()
		{
			visible = false;
		}
	}
}
