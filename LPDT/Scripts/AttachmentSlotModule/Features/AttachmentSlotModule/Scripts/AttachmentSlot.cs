using System;
using System.Collections.Generic;
using Features.GrabModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AttachmentSlotModule.Scripts
{
	public class AttachmentSlot : MonoBehaviour
	{
		[SerializeField]
		private NetworkObject _bearer;

		[SerializeField]
		private Transform _anchor;

		[SerializeField]
		private AttachmentCategory _acceptedCategories;

		[SerializeField]
		private float _radius = 0.6f;

		[SerializeField]
		private float _grabProxyRadius = 0.25f;

		private const string GRAB_PROXY_NAME = "GrabProxy";

		private static readonly List<AttachmentSlot> _all = new List<AttachmentSlot>();

		private PointGrabableContainer _grabProxyContainer;

		private SphereCollider _grabProxyCollider;

		public static IReadOnlyList<AttachmentSlot> All => _all;

		public Transform Anchor
		{
			get
			{
				if (!(_anchor != null))
				{
					return base.transform;
				}
				return _anchor;
			}
		}

		public NetworkObject Bearer
		{
			get
			{
				if (!(_bearer != null))
				{
					return GetComponentInParent<NetworkObject>();
				}
				return _bearer;
			}
		}

		public bool IsOccupied => SlottableItem.IsSlotClaimed(this);

		public float Radius => _radius;

		public int SlotIndex
		{
			get
			{
				NetworkObject bearer = Bearer;
				if (bearer == null)
				{
					return 0;
				}
				AttachmentSlot[] componentsInChildren = bearer.GetComponentsInChildren<AttachmentSlot>(includeInactive: true);
				return Math.Max(0, Array.IndexOf(componentsInChildren, this));
			}
		}

		public bool Accepts(AttachmentCategory category)
		{
			return (_acceptedCategories & category) != 0;
		}

		public bool IsInRange(Vector3 point)
		{
			return Vector3.Distance(point, Anchor.position) <= _radius;
		}

		public void SetDockedGrabable(SimplePointGrabable grabable)
		{
			if (!(_grabProxyContainer == null))
			{
				_grabProxyContainer.SetPointGrabable(grabable);
				_grabProxyCollider.enabled = grabable != null;
			}
		}

		public void ClearDockedGrabable(SimplePointGrabable grabable)
		{
			if (!(_grabProxyContainer == null) && _grabProxyContainer.PointGrabable == grabable)
			{
				SetDockedGrabable(null);
			}
		}

		private void Awake()
		{
			CreateGrabProxy();
		}

		private void CreateGrabProxy()
		{
			if (!(_grabProxyRadius <= 0f))
			{
				GameObject gameObject = new GameObject("GrabProxy");
				gameObject.layer = base.gameObject.layer;
				gameObject.transform.SetParent(Anchor, worldPositionStays: false);
				_grabProxyCollider = gameObject.AddComponent<SphereCollider>();
				_grabProxyCollider.isTrigger = true;
				_grabProxyCollider.excludeLayers = -1;
				_grabProxyCollider.radius = _grabProxyRadius / Mathf.Max(Anchor.lossyScale.x, 0.0001f);
				_grabProxyCollider.enabled = false;
				_grabProxyContainer = gameObject.AddComponent<PointGrabableContainer>();
			}
		}

		private void OnEnable()
		{
			if (!_all.Contains(this))
			{
				_all.Add(this);
			}
		}

		private void OnDisable()
		{
			_all.Remove(this);
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = (IsOccupied ? new Color(1f, 0.5f, 0f, 0.9f) : new Color(0f, 1f, 1f, 0.6f));
			Gizmos.DrawWireSphere(Anchor.position, _radius);
			Gizmos.color = new Color(1f, 1f, 0f, 0.8f);
			Gizmos.DrawWireSphere(Anchor.position, _grabProxyRadius);
		}

		private void Reset()
		{
			_bearer = GetComponentInParent<NetworkObject>();
			_anchor = base.transform;
		}
	}
}
