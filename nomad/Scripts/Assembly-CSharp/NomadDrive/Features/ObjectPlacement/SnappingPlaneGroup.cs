using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	public class SnappingPlaneGroup : MonoBehaviour, ISnappingPlaneShaderGroup
	{
		[SerializeField]
		private SnappingPlane[] members;

		private SnappingPlane _activeSource;

		private float _cachedGroupRadius;

		public SnappingPlane[] Members
		{
			get
			{
				return members;
			}
			set
			{
				members = value;
			}
		}

		public Vector3 SharedOrigin
		{
			get
			{
				if (members == null || members.Length == 0 || members[0] == null)
				{
					return base.transform.position;
				}
				return members[0].transform.position;
			}
		}

		private void OnEnable()
		{
			RegisterGroup();
		}

		private void OnDisable()
		{
			UnregisterGroup();
		}

		private void RegisterGroup()
		{
			if (members == null)
			{
				return;
			}
			SnappingPlane[] array = members;
			foreach (SnappingPlane snappingPlane in array)
			{
				if (snappingPlane != null)
				{
					snappingPlane.ShaderGroup = this;
				}
			}
		}

		private void UnregisterGroup()
		{
			if (members == null)
			{
				return;
			}
			SnappingPlane[] array = members;
			foreach (SnappingPlane snappingPlane in array)
			{
				if (snappingPlane != null)
				{
					snappingPlane.ShaderGroup = null;
				}
			}
		}

		public void OnMemberShaderActivated(SnappingPlane source, Vector3 centerPos, float radius)
		{
			_activeSource = source;
			_cachedGroupRadius = CalculateGroupRadius(centerPos, radius);
			Vector3 sharedOrigin = SharedOrigin;
			SnappingPlane[] array = members;
			foreach (SnappingPlane snappingPlane in array)
			{
				if (!(snappingPlane == null) && !(snappingPlane == source))
				{
					snappingPlane.ActivatePlacementShaderForGroup(centerPos, _cachedGroupRadius, sharedOrigin);
				}
			}
			source.ActivatePlacementShaderForGroup(centerPos, _cachedGroupRadius, sharedOrigin);
		}

		public void OnMemberShaderUpdated(SnappingPlane source, Vector3 centerPos)
		{
			Vector3 sharedOrigin = SharedOrigin;
			SnappingPlane[] array = members;
			foreach (SnappingPlane snappingPlane in array)
			{
				if (!(snappingPlane == null) && !(snappingPlane == source))
				{
					snappingPlane.UpdateShaderForGroup(centerPos, sharedOrigin);
				}
			}
		}

		public void OnMemberShaderDeactivated(SnappingPlane source)
		{
			if (source != _activeSource)
			{
				return;
			}
			_activeSource = null;
			SnappingPlane[] array = members;
			foreach (SnappingPlane snappingPlane in array)
			{
				if (!(snappingPlane == null) && !(snappingPlane == source))
				{
					snappingPlane.DeactivatePlacementShader();
				}
			}
		}

		private float CalculateGroupRadius(Vector3 centerPos, float objectRadius)
		{
			if (members == null || members.Length <= 1)
			{
				return objectRadius;
			}
			Vector3 sharedOrigin = SharedOrigin;
			SnappingPlane snappingPlane = members[0];
			if (snappingPlane == null)
			{
				return objectRadius;
			}
			Vector3 right = snappingPlane.transform.right;
			Vector3 forward = snappingPlane.transform.forward;
			Vector3 lhs = centerPos - sharedOrigin;
			Vector2 a = new Vector2(Vector3.Dot(lhs, right), Vector3.Dot(lhs, forward));
			float num = 0f;
			SnappingPlane[] array = members;
			foreach (SnappingPlane snappingPlane2 in array)
			{
				if (snappingPlane2 == null)
				{
					continue;
				}
				MeshFilter component = snappingPlane2.GetComponent<MeshFilter>();
				if (component == null || component.sharedMesh == null)
				{
					continue;
				}
				Bounds bounds = component.sharedMesh.bounds;
				Transform transform = snappingPlane2.transform;
				for (int j = 0; j < 8; j++)
				{
					Vector3 position = bounds.center + Vector3.Scale(bounds.extents, new Vector3(((j & 1) == 0) ? (-1f) : 1f, ((j & 2) == 0) ? (-1f) : 1f, ((j & 4) == 0) ? (-1f) : 1f));
					Vector3 lhs2 = transform.TransformPoint(position) - sharedOrigin;
					Vector2 b = new Vector2(Vector3.Dot(lhs2, right), Vector3.Dot(lhs2, forward));
					float num2 = Vector2.Distance(a, b);
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			return Mathf.Max(objectRadius, num + 0.5f);
		}
	}
}
