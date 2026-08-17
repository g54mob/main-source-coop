using System;
using System.Collections.Generic;
using System.Linq;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[AddComponentMenu("Pathfinding/Link")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_node_link.php")]
	public class NodeLink : GraphModifier
	{
		public static List<NodeLink> validNodeLinks = new List<NodeLink>();

		public Vector3 midPosition = Vector3.zero;

		public Transform end;

		public float costFactor = 1f;

		public bool oneWay;

		public bool deleteConnection;

		public Transform Start => base.transform;

		public Vector2 Start2D => new Vector2(Start.position.x, Start.position.z);

		public Transform End => end;

		public Vector2 End2D => new Vector2(End.position.x, End.position.z);

		protected override void Awake()
		{
			base.Awake();
			if (end != null)
			{
				midPosition = (Start.position + end.position) / 2f;
				validNodeLinks.Remove(this);
				validNodeLinks.Add(this);
			}
		}

		public override void OnPostScan()
		{
			if (AstarPath.active.isScanning)
			{
				InternalOnPostScan();
				return;
			}
			AstarPath.active.AddWorkItem(new AstarWorkItem((Func<bool, bool>)delegate
			{
				InternalOnPostScan();
				return true;
			}));
		}

		public void InternalOnPostScan()
		{
			Apply();
		}

		public override void OnGraphsPostUpdate()
		{
			if (!AstarPath.active.isScanning)
			{
				AstarPath.active.AddWorkItem(new AstarWorkItem((Func<bool, bool>)delegate
				{
					InternalOnPostScan();
					return true;
				}));
			}
		}

		public virtual void Apply()
		{
			if (Start == null || End == null || AstarPath.active == null)
			{
				return;
			}
			GraphNode node = AstarPath.active.GetNearest(Start.position).node;
			GraphNode node2 = AstarPath.active.GetNearest(End.position).node;
			if (node == null || node2 == null)
			{
				return;
			}
			if (deleteConnection)
			{
				node.RemoveConnection(node2);
				node.RemoveLooseConnection(node2);
				node2.RemoveLooseConnection(node);
				if (!oneWay)
				{
					node2.RemoveConnection(node);
				}
				return;
			}
			uint cost = (uint)Math.Round((float)(node.position - node2.position).costMagnitude * costFactor);
			node.AddConnection(node2, cost);
			node.AddLooseConnection(node2, cost);
			node2.AddLooseConnection(node, cost);
			if (!oneWay)
			{
				node2.AddConnection(node, cost);
			}
		}

		public Vector3 GetClosestPoint(Vector3 point)
		{
			Vector3 vector = end.position - Start.position;
			float magnitude = vector.magnitude;
			vector.Normalize();
			float num = Mathf.Clamp(Vector3.Dot(point - Start.position, vector), 0f, magnitude);
			return Start.position + vector * num;
		}

		public static List<NodeLink> GetClosestLinks(Vector3 point)
		{
			List<NodeLink> list = new List<NodeLink>();
			list.AddRange(validNodeLinks);
			return list.OrderBy((NodeLink x) => Vector3.Distance(x.GetClosestPoint(point), point)).ToList();
		}

		public void OnDrawGizmos()
		{
			if (!(Start == null) && !(End == null))
			{
				Draw.Gizmos.Bezier(Start.position, End.position, deleteConnection ? Color.red : Color.green);
			}
		}
	}
}
