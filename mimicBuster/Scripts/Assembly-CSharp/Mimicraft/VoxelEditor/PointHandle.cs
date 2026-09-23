using System;
using Mimicraft.VoxelEditor.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Mimicraft.VoxelEditor
{
	public class PointHandle : MonoBehaviour
	{
		private const float GrabPixels = 14f;

		private static readonly Color XColor = new Color(0.9f, 0.3f, 0.3f, 0.9f);

		private static readonly Color YColor = new Color(0.4f, 0.85f, 0.4f, 0.9f);

		private static readonly Color ZColor = new Color(0.35f, 0.55f, 0.95f, 0.9f);

		private static readonly Color HoverColor = new Color(1f, 0.85f, 0.2f, 1f);

		private ArrowGizmo[] arrows;

		private Transform marker;

		private SpriteRenderer icon;

		private float iconSize;

		private const float HoverIconScale = 1.2f;

		private Transform space;

		private TextMeshPro label;

		private string displayName = "";

		public Action<Vector3> Moved;

		private int draggingAxis = -1;

		private int hoveredAxis = -1;

		private static PointHandle dragOwner;

		private static PointHandle hoverWinner;

		private static PointHandle hoverCandidate;

		private static float hoverCandidateDistance;

		private static int hoverPassFrame = -1;

		private float dragOffset;

		private float armLength = 0.2f;

		private float thickness = 1f;

		private float markerSize = 0.02f;

		private float labelSize = 0.02f;

		private float snap;

		private Vector3 snapOrigin;

		private Color tint = Color.white;

		private Vector3 boundsMin;

		private Vector3 boundsMax;

		private bool hasBounds;

		public Vector3 LocalPoint { get; private set; }

		public bool IsDragging => draggingAxis >= 0;

		private bool HasIcon
		{
			get
			{
				if (icon != null && icon.sprite != null)
				{
					return iconSize > 0f;
				}
				return false;
			}
		}

		private float ScaleCompensation
		{
			get
			{
				float num = ((space != null) ? space.lossyScale.x : 1f);
				if (!(Mathf.Abs(num) > 0.0001f))
				{
					return 1f;
				}
				return 1f / num;
			}
		}

		public void SetSnapOrigin(Vector3 origin)
		{
			snapOrigin = origin;
		}

		public Vector3 Snapped(Vector3 point)
		{
			if (snap <= 0f)
			{
				return point;
			}
			return new Vector3(SnapAxis(point.x, snapOrigin.x), SnapAxis(point.y, snapOrigin.y), SnapAxis(point.z, snapOrigin.z));
		}

		private float SnapAxis(float value, float origin)
		{
			return origin + Mathf.Round((value - origin) / snap) * snap;
		}

		public static PointHandle Create(Transform space, string name, Color tint, float armLength, float thickness, float markerSize, float labelSize, float snap, TMP_FontAsset font = null)
		{
			GameObject gameObject = new GameObject("PointHandle_" + name);
			gameObject.transform.SetParent(space, worldPositionStays: false);
			PointHandle pointHandle = gameObject.AddComponent<PointHandle>();
			pointHandle.space = space;
			pointHandle.tint = tint;
			pointHandle.armLength = Mathf.Max(0.001f, armLength);
			pointHandle.thickness = Mathf.Max(0.001f, thickness);
			pointHandle.markerSize = Mathf.Max(0f, markerSize);
			pointHandle.labelSize = Mathf.Max(0f, labelSize);
			pointHandle.snap = Mathf.Max(0f, snap);
			pointHandle.arrows = new ArrowGizmo[3]
			{
				ArrowGizmo.Create(gameObject.transform),
				ArrowGizmo.Create(gameObject.transform),
				ArrowGizmo.Create(gameObject.transform)
			};
			pointHandle.marker = CreateMarker(gameObject.transform, tint);
			pointHandle.icon = CreateIcon(gameObject.transform);
			pointHandle.displayName = name;
			pointHandle.label = WorldLabel.Create(gameObject.transform, "Label", 4f, tint, font);
			pointHandle.label.gameObject.SetActive(value: false);
			return pointHandle;
		}

		public void SetIcon(Sprite sprite, float size)
		{
			iconSize = Mathf.Max(0f, size);
			if (icon != null)
			{
				icon.sprite = sprite;
			}
		}

		private static SpriteRenderer CreateIcon(Transform parent)
		{
			GameObject obj = new GameObject("Icon");
			obj.transform.SetParent(parent, worldPositionStays: false);
			SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>();
			Shader shader = Shader.Find("Mimicraft/GizmoSprite");
			if (shader != null)
			{
				spriteRenderer.sharedMaterial = new Material(shader);
			}
			obj.SetActive(value: false);
			return spriteRenderer;
		}

		private static Transform CreateMarker(Transform parent, Color colour)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			gameObject.name = "Point";
			Collider component = gameObject.GetComponent<Collider>();
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
			gameObject.transform.SetParent(parent, worldPositionStays: false);
			Shader shader = Shader.Find("Mimicraft/HighlightUnlit");
			if (shader != null)
			{
				gameObject.GetComponent<MeshRenderer>().sharedMaterial = new Material(shader)
				{
					color = colour
				};
			}
			return gameObject.transform;
		}

		public void SetPoint(Vector3 localPoint)
		{
			LocalPoint = Clamp(Snapped(localPoint));
		}

		public void SetBounds(Vector3 min, Vector3 max)
		{
			boundsMin = Vector3.Min(min, max);
			boundsMax = Vector3.Max(min, max);
			hasBounds = true;
			LocalPoint = Clamp(LocalPoint);
		}

		private Vector3 Clamp(Vector3 point)
		{
			if (!hasBounds)
			{
				return point;
			}
			return new Vector3(ClampAxis(point.x, boundsMin.x, boundsMax.x, snapOrigin.x), ClampAxis(point.y, boundsMin.y, boundsMax.y, snapOrigin.y), ClampAxis(point.z, boundsMin.z, boundsMax.z, snapOrigin.z));
		}

		private float ClampAxis(float value, float min, float max, float origin)
		{
			float num = Mathf.Clamp(value, min, max);
			if (snap <= 0f)
			{
				return num;
			}
			float num2 = SnapAxis(num, origin);
			if (num2 < min - 1E-05f)
			{
				num2 += Mathf.Ceil((min - num2) / snap) * snap;
			}
			else if (num2 > max + 1E-05f)
			{
				num2 -= Mathf.Ceil((num2 - max) / snap) * snap;
			}
			if (!(num2 >= min - 1E-05f) || !(num2 <= max + 1E-05f))
			{
				return num;
			}
			return num2;
		}

		public void Hide()
		{
			ReleaseDrag();
			hoveredAxis = -1;
			if (marker != null)
			{
				marker.gameObject.SetActive(value: false);
			}
			if (icon != null)
			{
				icon.gameObject.SetActive(value: false);
			}
			if (label != null)
			{
				label.gameObject.SetActive(value: false);
			}
			if (arrows != null)
			{
				ArrowGizmo[] array = arrows;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Hide();
				}
			}
		}

		public void Tick(Camera cam)
		{
			if (arrows == null || space == null || cam == null)
			{
				return;
			}
			bool flag = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
			Vector2 position;
			bool flag2 = Mouse.current != null && PointerScreenPosition.TryGet(out position);
			Vector2 pointer = (flag2 ? PointerScreenPositionOrZero() : Vector2.zero);
			BeginHoverPass();
			if (draggingAxis >= 0)
			{
				if (!flag2 || !Mouse.current.leftButton.isPressed)
				{
					ReleaseDrag();
				}
				else
				{
					Drag(cam, pointer);
				}
			}
			else
			{
				float bestDistance;
				int num = ((flag2 && !flag) ? PickAxis(cam, pointer, out bestDistance) : Miss(out bestDistance));
				if (num >= 0 && bestDistance < hoverCandidateDistance)
				{
					hoverCandidateDistance = bestDistance;
					hoverCandidate = this;
				}
				bool flag3 = (hoverWinner == null || hoverWinner == this) && dragOwner == null;
				hoveredAxis = (flag3 ? num : (-1));
				if (hoveredAxis >= 0 && Mouse.current.leftButton.wasPressedThisFrame)
				{
					dragOwner = this;
					draggingAxis = hoveredAxis;
					dragOffset = AxisDistance(cam, pointer, draggingAxis) - Along(draggingAxis);
				}
			}
			Render(cam);
		}

		private static void BeginHoverPass()
		{
			int frameCount = Time.frameCount;
			if (hoverPassFrame != frameCount)
			{
				hoverPassFrame = frameCount;
				hoverWinner = hoverCandidate;
				hoverCandidate = null;
				hoverCandidateDistance = float.MaxValue;
			}
		}

		private void ReleaseDrag()
		{
			draggingAxis = -1;
			if (dragOwner == this)
			{
				dragOwner = null;
			}
		}

		private static int Miss(out float distance)
		{
			distance = float.MaxValue;
			return -1;
		}

		private void Drag(Camera cam, Vector2 pointer)
		{
			float value = AxisDistance(cam, pointer, draggingAxis) - dragOffset;
			if (snap > 0f)
			{
				value = SnapAxis(value, snapOrigin[draggingAxis]);
			}
			Vector3 localPoint = LocalPoint;
			localPoint[draggingAxis] = value;
			localPoint = Clamp(localPoint);
			if (!(localPoint == LocalPoint))
			{
				LocalPoint = localPoint;
				Moved?.Invoke(LocalPoint);
			}
		}

		private float Along(int axis)
		{
			return LocalPoint[axis];
		}

		private float AxisDistance(Camera cam, Vector2 pointer, int axis)
		{
			Vector3 vector = space.TransformPoint(LocalPoint - AxisVector(axis) * Along(axis));
			Vector3 lhs = space.TransformDirection(AxisVector(axis));
			float magnitude = lhs.magnitude;
			if (magnitude < 0.0001f)
			{
				return Along(axis);
			}
			lhs /= magnitude;
			Ray ray = cam.ScreenPointToRay(pointer);
			Vector3 rhs = vector - ray.origin;
			float num = Vector3.Dot(lhs, ray.direction);
			float num2 = 1f - num * num;
			if (Mathf.Abs(num2) < 0.0001f)
			{
				return Along(axis);
			}
			float num3 = Vector3.Dot(lhs, rhs);
			float num4 = Vector3.Dot(ray.direction, rhs);
			return (num * num4 - num3) / num2 / magnitude;
		}

		private int PickAxis(Camera cam, Vector2 pointer, out float bestDistance)
		{
			int result = -1;
			bestDistance = 14f;
			float num = armLength * ScaleCompensation;
			for (int i = 0; i < 3; i++)
			{
				Vector3 position = space.TransformPoint(LocalPoint);
				Vector3 position2 = space.TransformPoint(LocalPoint + AxisVector(i) * num);
				Vector2 a = cam.WorldToScreenPoint(position);
				Vector2 b = cam.WorldToScreenPoint(position2);
				float num2 = DistanceToSegment(pointer, a, b);
				if (num2 < bestDistance)
				{
					bestDistance = num2;
					result = i;
				}
			}
			return result;
		}

		private void Render(Camera cam)
		{
			float scaleCompensation = ScaleCompensation;
			for (int i = 0; i < 3; i++)
			{
				Color color = ((i == ((draggingAxis >= 0) ? draggingAxis : hoveredAxis)) ? HoverColor : (AxisColor(i) * tint));
				arrows[i].ShowCustom(LocalPoint, AxisVector(i), armLength, color, scaleCompensation, thickness);
			}
			if (!(marker == null))
			{
				marker.gameObject.SetActive(markerSize > 0f && !HasIcon);
				marker.localPosition = LocalPoint;
				marker.localScale = Vector3.one * (markerSize * scaleCompensation);
				RenderIcon(cam, scaleCompensation);
				RenderLabel(cam, scaleCompensation);
			}
		}

		private void RenderIcon(Camera cam, float compensation)
		{
			if (!(icon == null))
			{
				bool hasIcon = HasIcon;
				if (icon.gameObject.activeSelf != hasIcon)
				{
					icon.gameObject.SetActive(hasIcon);
				}
				if (hasIcon)
				{
					Transform transform = icon.transform;
					Vector3 vector = (transform.position = space.TransformPoint(LocalPoint));
					transform.rotation = Quaternion.LookRotation(vector - cam.transform.position, cam.transform.up);
					Vector3 size = icon.sprite.bounds.size;
					float num = Mathf.Max(size.x, size.y, 0.0001f);
					bool flag = hoveredAxis >= 0 || draggingAxis >= 0;
					transform.localScale = Vector3.one * (iconSize * (flag ? 1.2f : 1f) / num * compensation);
				}
			}
		}

		private void RenderLabel(Camera cam, float compensation)
		{
			if (!(label == null))
			{
				bool flag = labelSize > 0f && (hoveredAxis >= 0 || draggingAxis >= 0);
				label.gameObject.SetActive(flag);
				if (flag)
				{
					label.text = displayName;
					float num = (HasIcon ? (iconSize * 0.5f) : markerSize);
					Vector3 worldPosition = space.TransformPoint(LocalPoint) + Vector3.up * (num + labelSize * 2f);
					WorldLabel.Place(label, worldPosition, cam.transform.position, labelSize * compensation);
				}
			}
		}

		private static Vector3 AxisVector(int axis)
		{
			return axis switch
			{
				1 => Vector3.up, 
				0 => Vector3.right, 
				_ => Vector3.forward, 
			};
		}

		private static Color AxisColor(int axis)
		{
			return axis switch
			{
				1 => YColor, 
				0 => XColor, 
				_ => ZColor, 
			};
		}

		private static Vector2 PointerScreenPositionOrZero()
		{
			if (!PointerScreenPosition.TryGet(out var position))
			{
				return Vector2.zero;
			}
			return position;
		}

		private static float DistanceToSegment(Vector2 p, Vector2 a, Vector2 b)
		{
			Vector2 vector = b - a;
			float sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude < 0.0001f)
			{
				return Vector2.Distance(p, a);
			}
			float num = Mathf.Clamp01(Vector2.Dot(p - a, vector) / sqrMagnitude);
			return Vector2.Distance(p, a + vector * num);
		}
	}
}
