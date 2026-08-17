using System;
using System.Collections.Generic;
using UnityEngine;

namespace PaintCore
{
	[Serializable]
	public class CwLineConnector
	{
		private class Link
		{
			public object Owner;

			public Vector3 Position;

			public Vector3 EndPosition;

			public float Age;

			public bool Preview;
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

		public void SubmitLine(GameObject gameObject, bool preview, int priority, float pressure, Vector3 position, Vector3 endPosition, Quaternion rotation, object owner)
		{
			if (owner != null)
			{
				Link link = null;
				if (TryGetLink(owner, ref link))
				{
					if (link.Preview == preview)
					{
						if (hitSpacing > 0f)
						{
							Vector3 vector = link.Position;
							Vector3 vector2 = link.EndPosition;
							float num = Vector3.Distance(link.Position, position);
							float num2 = Vector3.Distance(link.EndPosition, endPosition);
							int num3 = Mathf.FloorToInt(num / hitSpacing);
							int num4 = Mathf.FloorToInt(num2 / hitSpacing);
							if (num3 <= 0 && num4 <= 0)
							{
								return;
							}
							int num5 = Mathf.Max(num3, num4);
							float num6 = hitSpacing;
							float num7 = hitSpacing;
							if (num5 > hitLimit)
							{
								num5 = hitLimit;
							}
							if (num3 > num4)
							{
								num7 = num2 * (num / ((float)num3 * num6)) / (float)num3;
							}
							else
							{
								num6 = num * (num2 / ((float)num4 * num7)) / (float)num4;
							}
							for (int i = 0; i < num5; i++)
							{
								vector = Vector3.MoveTowards(vector, position, num6);
								vector2 = Vector3.MoveTowards(vector2, endPosition, num7);
								if (connectHits)
								{
									hitCache.InvokeQuad(gameObject, preview, priority, pressure, link.Position, link.EndPosition, vector, vector2, rotation, clipConnected);
								}
								else
								{
									hitCache.InvokeLine(gameObject, preview, priority, pressure, vector, vector2, rotation, clip: false);
								}
								link.Position = vector;
								link.EndPosition = vector2;
							}
							return;
						}
						if (connectHits)
						{
							hitCache.InvokeQuad(gameObject, preview, priority, pressure, link.Position, link.EndPosition, position, endPosition, rotation, clipConnected);
						}
						else
						{
							hitCache.InvokeLine(gameObject, preview, priority, pressure, position, endPosition, rotation, clip: false);
						}
					}
					else
					{
						hitCache.InvokeLine(gameObject, preview, priority, pressure, position, endPosition, rotation, clip: false);
					}
				}
				else
				{
					link = ((linkPool.Count > 0) ? linkPool.Pop() : new Link());
					link.Owner = owner;
					links.Add(link);
					hitCache.InvokeLine(gameObject, preview, priority, pressure, position, endPosition, rotation, clip: false);
				}
				link.Position = position;
				link.EndPosition = endPosition;
				link.Preview = preview;
			}
			else
			{
				hitCache.InvokeLine(gameObject, preview, priority, pressure, position, endPosition, rotation, clip: false);
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

		private bool TryGetLink(object owner, ref Link link)
		{
			for (int num = links.Count - 1; num >= 0; num--)
			{
				link = links[num];
				link.Age = 0f;
				if (link.Owner == owner)
				{
					return true;
				}
			}
			return false;
		}
	}
}
