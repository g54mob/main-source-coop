using System.Collections.Generic;
using CW.Common;
using UnityEngine;

namespace PaintCore
{
	[ExecuteInEditMode]
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwMask")]
	[AddComponentMenu("CW/Paint Core/CW Mask")]
	public class CwMask : MonoBehaviour
	{
		[SerializeField]
		private Texture texture;

		[SerializeField]
		private CwChannel channel = CwChannel.Alpha;

		[SerializeField]
		private bool invert;

		[SerializeField]
		private Vector2 stretch = Vector2.one;

		private static LinkedList<CwMask> instances = new LinkedList<CwMask>();

		private LinkedListNode<CwMask> instancesNode;

		public Texture Texture
		{
			get
			{
				return texture;
			}
			set
			{
				texture = value;
			}
		}

		public CwChannel Channel
		{
			get
			{
				return channel;
			}
			set
			{
				channel = value;
			}
		}

		public bool Invert
		{
			get
			{
				return invert;
			}
			set
			{
				invert = value;
			}
		}

		public Vector2 Stretch
		{
			get
			{
				return stretch;
			}
			set
			{
				stretch = value;
			}
		}

		public static LinkedList<CwMask> Instances => instances;

		public virtual Matrix4x4 Matrix => base.transform.worldToLocalMatrix;

		public static CwMask Find(Vector3 position, LayerMask layers)
		{
			CwMask result = null;
			float num = 1f / 0f;
			foreach (CwMask instance in instances)
			{
				if (CwHelper.IndexInMask(instance.gameObject.layer, layers))
				{
					float num2 = Vector3.SqrMagnitude(position - instance.transform.position);
					if (num2 < num)
					{
						num = num2;
						result = instance;
					}
				}
			}
			return result;
		}

		protected virtual void OnEnable()
		{
			instancesNode = instances.AddLast(this);
		}

		protected virtual void OnDisable()
		{
			instances.Remove(instancesNode);
			instancesNode = null;
		}
	}
}
