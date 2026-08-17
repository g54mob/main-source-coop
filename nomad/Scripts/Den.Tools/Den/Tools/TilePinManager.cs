using System;
using System.Collections.Generic;
using UnityEngine;

namespace Den.Tools
{
	public class TilePinManager<T> : TileManager<T> where T : IPinTile, IEquatable<T>
	{
		private Dictionary<Coord, T> pinned = new Dictionary<Coord, T>();

		public void Pin(Coord coord, MonoBehaviour holder = null)
		{
			grid.TryGetValue(coord, out var value);
			if (value == null)
			{
				value = ConstructTile(holder);
				grid.Add(coord, value);
				value.Pin();
				float dist = ((camCoords != null) ? TileManager<T>.GetRemoteness(coord, camCoords) : 0f);
				value.Move(coord, dist);
			}
			else
			{
				value.Pin();
			}
			if (pinned.ContainsKey(coord))
			{
				pinned.Add(coord, value);
			}
		}

		public void Unpin(Coord coord)
		{
			if (pinned.ContainsKey(coord))
			{
				pinned.Remove(coord);
				if (camCoords != null)
				{
					Deploy(camCoords, pinned);
					return;
				}
				grid[coord].Remove();
				grid.Remove(coord);
			}
		}

		public void Deploy(Coord[] camCoords, MonoBehaviour holder = null)
		{
			Deploy(camCoords, pinned, holder);
		}
	}
}
