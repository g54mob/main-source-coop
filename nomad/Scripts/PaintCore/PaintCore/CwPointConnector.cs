using System;
using System.Collections.Generic;
using UnityEngine;

namespace PaintCore
{
	[Serializable]
	public class CwPointConnector
	{
		private class Link
		{
			public object Owner;

			public Vector3 Position;

			public float Age;

			public bool Preview;

			public Vector3 LastPosition;

			public float LastPressure;

			public int LastPriority;

			public Quaternion LastRotation;
		}

		[SerializeField]
		private float hitSpacing;

		[SerializeField]
		private int hitLimit = 30;

		[SerializeField]
		protected bool connectHits;

		[SerializeField]
		protected bool clipConnected;

		[NonSerialized]
		private List<Link> links = new List<Link>();

		[NonSerialized]
		private static Stack<Link> linkPool = new Stack<Link>();

		[NonSerialized]
		protected CwHitCache hitCache = new CwHitCache();

		public float HitSpacing
		{
			get
			{
				return hitSpacing;
			}
			set
			{
				hitSpacing = value;
			}
		}

		public int HitLimit
		{
			get
			{
				return hitLimit;
			}
			set
			{
				hitLimit = value;
			}
		}

		public bool ConnectHits
		{
			get
			{
				return connectHits;
			}
			set
			{
				connectHits = value;
			}
		}

		public bool ClipConnected
		{
			get
			{
				return clipConnected;
			}
			set
			{
				clipConnected = value;
			}
		}

		public CwHitCache HitCache => hitCache;

		public void ClearHitCache()
		{
			hitCache.Clear();
		}

		public void ResetConnections()
		{
			for (int num = links.Count - 1; num >= 0; num--)
			{
				linkPool.Push(links[num]);
			}
			links.Clear();
		}

		public void BreakHits(object owner)
		{
			for (int num = links.Count - 1; num >= 0; num--)
			{
				Link link = links[num];
				if (link.Owner == owner)
				{
					links.RemoveAt(num);
					linkPool.Push(link);
					break;
				}
			}
		}

		public void SubmitLastPoint(GameObject gameObject, bool preview, object owner)
		{
			if (owner == null)
			{
				return;
			}
			Link foundLink = null;
			if (!TryGetLink(owner, ref foundLink) || foundLink.Preview != preview || preview || !(hitSpacing > 0f))
			{
				return;
			}
			Vector3 vector = foundLink.Position;
			int num = Mathf.FloorToInt(Vector3.Distance(foundLink.Position, foundLink.LastPosition) / hitSpacing);
			if (num > hitLimit)
			{
				num = hitLimit;
			}
			for (int i = 0; i < num; i++)
			{
				vector = Vector3.MoveTowards(vector, foundLink.LastPosition, hitSpacing);
				if (connectHits)
				{
					hitCache.InvokeLine(gameObject, preview, foundLink.LastPriority, foundLink.LastPressure, foundLink.Position, vector, foundLink.LastRotation, clipConnected);
				}
				else
				{
					hitCache.InvokePoint(gameObject, preview, foundLink.LastPriority, foundLink.LastPressure, vector, foundLink.LastRotation);
				}
				foundLink.Position = vector;
			}
		}

		public void SubmitPoint(GameObject gameObject, bool preview, int priority, float pressure, Vector3 position, Quaternion rotation, object owner)
		{
			if (owner != null)
			{
				bool flag = true;
				Link foundLink = null;
				if (TryGetLink(owner, ref foundLink))
				{
					CwPaintableManager.InvokeOnBeginPainting(owner);
					if (hitSpacing > 0f && !preview)
					{
						Vector3 vector = foundLink.Position;
						int num = Mathf.FloorToInt(Vector3.Distance(foundLink.Position, position) / hitSpacing);
						if (num > hitLimit)
						{
							num = hitLimit;
						}
						for (int i = 0; i < num; i++)
						{
							vector = Vector3.MoveTowards(vector, position, hitSpacing);
							if (connectHits)
							{
								hitCache.InvokeLine(gameObject, preview, priority, pressure, foundLink.Position, vector, rotation, clipConnected);
							}
							else
							{
								hitCache.InvokePoint(gameObject, preview, priority, pressure, vector, rotation);
							}
							foundLink.Position = vector;
						}
						flag = false;
					}
					else if (connectHits)
					{
						hitCache.InvokeLine(gameObject, preview, priority, pressure, foundLink.Position, position, rotation, clipConnected);
					}
					else
					{
						hitCache.InvokePoint(gameObject, preview, priority, pressure, position, rotation);
					}
				}
				else
				{
					CwPaintableManager.LastPaintingObject = owner;
					foundLink = ((linkPool.Count > 0) ? linkPool.Pop() : new Link());
					foundLink.Owner = owner;
					links.Add(foundLink);
					hitCache.InvokePoint(gameObject, preview, priority, pressure, position, rotation);
				}
				if (flag)
				{
					foundLink.Position = position;
					foundLink.Preview = preview;
				}
				foundLink.LastPosition = position;
				foundLink.LastPressure = pressure;
				foundLink.LastPriority = priority;
				foundLink.LastRotation = rotation;
			}
			else
			{
				hitCache.InvokePoint(gameObject, preview, priority, pressure, position, rotation);
			}
		}

		public void Update()
		{
			for (int num = links.Count - 1; num >= 0; num--)
			{
				Link link = links[num];
				link.Age += Time.deltaTime;
				if (link.Age > 1f)
				{
					link.Age = 0f;
					links.RemoveAt(num);
					linkPool.Push(link);
				}
			}
		}

		private bool TryGetLink(object owner, ref Link foundLink)
		{
			for (int num = links.Count - 1; num >= 0; num--)
			{
				Link link = links[num];
				if (link.Owner == owner)
				{
					foundLink = link;
					link.Age = 0f;
					return true;
				}
			}
			return false;
		}
	}
}
