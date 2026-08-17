using System;
using System.Collections.Generic;
using UnityEngine;

namespace Den.Tools
{
	[Serializable]
	public class PosTab : ICloneable
	{
		[Serializable]
		public struct Cell
		{
			public CoordRect rect;

			public Transition[] poses;

			public int count;

			public int GetPosNum(float x, float z)
			{
				for (int i = 0; i < count; i++)
				{
					float num = poses[i].pos.x - x;
					if (num < 0f)
					{
						num = 0f - num;
					}
					float num2 = poses[i].pos.z - z;
					if (num2 < 0f)
					{
						num2 = 0f - num2;
					}
					if (num < 0.0001f && num2 < 0.0001f)
					{
						return i;
					}
				}
				return -1;
			}
		}

		public readonly CoordRect rect;

		public readonly Vector3 pos;

		public readonly Vector3 size;

		public Matrix2D<Cell> cells;

		public readonly int resolution;

		public readonly Coord cellSize;

		public int totalCount;

		public int idCounter = 1;

		public PosTab(Vector3 pos, Vector3 size, int resolution)
		{
			this.resolution = resolution;
			rect = new CoordRect(Coord.Round(pos), Coord.Round(size));
			this.pos = pos;
			this.size = size;
			cells = new Matrix2D<Cell>(resolution, resolution);
			cellSize = new Coord(Mathf.CeilToInt(1f * (float)rect.size.x / (float)resolution), Mathf.CeilToInt(1f * (float)rect.size.z / (float)resolution));
			for (int i = 0; i < resolution; i++)
			{
				for (int j = 0; j < resolution; j++)
				{
					Cell value = default(Cell);
					value.rect = new CoordRect(i * cellSize.x + rect.offset.x, j * cellSize.z + rect.offset.z, Mathf.Min(cellSize.x, Mathf.Max(0, rect.size.x - i * cellSize.x)), Mathf.Min(cellSize.z, Mathf.Max(0, rect.size.z - j * cellSize.z)));
					value.rect.offset.x = Mathf.Min(value.rect.offset.x, rect.offset.x + rect.size.x);
					value.rect.offset.z = Mathf.Min(value.rect.offset.z, rect.offset.z + rect.size.z);
					cells[i, j] = value;
				}
			}
		}

		public PosTab Copy()
		{
			PosTab posTab = new PosTab(pos, size, resolution);
			for (int i = 0; i < cells.arr.Length; i++)
			{
				posTab.cells.arr[i].count = cells.arr[i].count;
				posTab.cells.arr[i].rect = cells.arr[i].rect;
				if (cells.arr[i].poses != null)
				{
					posTab.cells.arr[i].poses = new Transition[cells.arr[i].poses.Length];
					Array.Copy(cells.arr[i].poses, posTab.cells.arr[i].poses, cells.arr[i].poses.Length);
				}
			}
			posTab.totalCount = totalCount;
			posTab.idCounter = idCounter;
			return posTab;
		}

		public object Clone()
		{
			return Copy();
		}

		private Coord GetCellCoord(float x, float z, bool throwExceptions = true)
		{
			int num = (int)((x - (float)rect.offset.x) / (float)cellSize.x);
			int num2 = (int)((z - (float)rect.offset.z) / (float)cellSize.z);
			if (throwExceptions && (num > cells.rect.size.x || num2 > cells.rect.size.z))
			{
				throw new Exception("Out of cells range " + num + "," + num2);
			}
			return new Coord(num, num2);
		}

		private int GetCellNum(float x, float z, bool throwExceptions = true)
		{
			int num = (int)((x - (float)rect.offset.x) / (float)cellSize.x);
			int num2 = (int)((z - (float)rect.offset.z) / (float)cellSize.z);
			if (throwExceptions && (num > cells.rect.size.x || num2 > cells.rect.size.z))
			{
				throw new Exception("Out of cells range " + num + "," + num2);
			}
			int num3 = num2 * cells.rect.size.x + num;
			if (throwExceptions && num3 < 0)
			{
				throw new Exception("Could not find object at coord " + x + "," + z);
			}
			return num3;
		}

		public void Add(PosTab tab)
		{
			foreach (Transition item in tab.All())
			{
				Add(item);
			}
		}

		public void Add(float x, float z)
		{
			Transition trs = new Transition(x, z);
			Add(trs);
		}

		public void Add(Transition trs)
		{
			if (rect.Contains(trs.pos))
			{
				idCounter++;
				trs.id = idCounter;
				if (trs.hash == 0)
				{
					trs.hash = trs.id;
				}
				int cellNum = GetCellNum(trs.pos.x, trs.pos.z);
				if (cells.arr[cellNum].poses == null)
				{
					cells.arr[cellNum].poses = new Transition[1];
				}
				if (cells.arr[cellNum].poses.Length == cells.arr[cellNum].count)
				{
					Transition[] array = new Transition[cells.arr[cellNum].count * 4];
					Array.Copy(cells.arr[cellNum].poses, array, cells.arr[cellNum].count);
					cells.arr[cellNum].poses = array;
				}
				cells.arr[cellNum].poses[cells.arr[cellNum].count] = trs;
				cells.arr[cellNum].count++;
				totalCount++;
			}
		}

		public void Add(List<Transition> transitions)
		{
			for (int i = 0; i < transitions.Count; i++)
			{
				Add(transitions[i]);
			}
		}

		public void Add(TransitionsList transitions)
		{
			for (int i = 0; i < transitions.count; i++)
			{
				Add(transitions.arr[i]);
			}
		}

		public void Remove(int cellNum, int posNum)
		{
			cells.arr[cellNum].poses[posNum] = cells.arr[cellNum].poses[cells.arr[cellNum].count - 1];
			cells.arr[cellNum].poses[cells.arr[cellNum].count - 1].hash = 0;
			cells.arr[cellNum].count--;
			totalCount--;
			if (cells.arr[cellNum].count == 0)
			{
				cells.arr[cellNum].poses = null;
			}
			else if (cells.arr[cellNum].count < cells.arr[cellNum].poses.Length / 2)
			{
				Transition[] array = new Transition[cells.arr[cellNum].count];
				Array.Copy(cells.arr[cellNum].poses, array, cells.arr[cellNum].count);
				cells.arr[cellNum].poses = array;
			}
		}

		public void RemoveAt(float x, float z)
		{
			int cellNum = GetCellNum(x, z);
			int posNum = cells.arr[cellNum].GetPosNum(x, z);
			if (posNum != -1)
			{
				Remove(cellNum, posNum);
			}
		}

		public void Move(int cellNum, int posNum, float newX, float newZ)
		{
			if (GetCellNum(newX, newZ) == cellNum)
			{
				cells.arr[cellNum].poses[posNum].pos.x = newX;
				cells.arr[cellNum].poses[posNum].pos.z = newZ;
				if (!(newX >= (float)rect.offset.x) || !(newZ >= (float)rect.offset.z) || !(newX < (float)(rect.offset.x + rect.size.x)) || !(newZ < (float)(rect.offset.z + rect.size.z)))
				{
					Remove(cellNum, posNum);
				}
			}
			else
			{
				Transition trs = cells.arr[cellNum].poses[posNum];
				Remove(cellNum, posNum);
				trs.pos.x = newX;
				trs.pos.z = newZ;
				Add(trs);
			}
		}

		public void GetAndMove(float oldX, float oldZ, float newX, float newZ)
		{
			int cellNum = GetCellNum(oldX, oldZ);
			int posNum = cells.arr[cellNum].GetPosNum(oldX, oldZ);
			if (posNum < 0)
			{
				throw new Exception("Could not find object at coord " + oldX + "," + oldZ + " cell num:" + cellNum);
			}
			Move(cellNum, posNum, newX, newZ);
		}

		public bool Exists(float x, float z)
		{
			int cellNum = GetCellNum(x, z, throwExceptions: false);
			if (cellNum > cells.arr.Length)
			{
				return false;
			}
			return cells.arr[cellNum].GetPosNum(x, z) >= 0;
		}

		public void Flush()
		{
			for (int i = 0; i < cells.arr.Length; i++)
			{
				if (cells.arr[i].poses != null)
				{
					if (cells.arr[i].count == 0)
					{
						cells.arr[i].poses = null;
					}
					if (cells.arr[i].poses.Length > cells.arr[i].count)
					{
						Transition[] array = new Transition[cells.arr[i].count];
						Array.Copy(cells.arr[i].poses, array, cells.arr[i].count);
						cells.arr[i].poses = array;
					}
				}
			}
		}

		public Transition Closest(float x, float z, float minDist = 0f, float maxDist = 2.147E+09f, Predicate<Transition> filterFn = null)
		{
			float minDistSq = maxDist * maxDist;
			Transition closestPos = new Transition
			{
				hash = 0
			};
			Coord cellCoord = GetCellCoord(x, z);
			int num = (int)(maxDist / (float)cellSize.x * 1.42f + 1f);
			int num2 = ((cellCoord.x > cellCoord.z) ? cellCoord.x : cellCoord.z);
			if (cells.rect.size.x - cellCoord.x > num2)
			{
				num2 = cells.rect.size.x - cellCoord.x;
			}
			if (cells.rect.size.z - cellCoord.z > num2)
			{
				num2 = cells.rect.size.z - cellCoord.z;
			}
			if (num > num2)
			{
				num = num2;
			}
			num++;
			_ = minDist / (float)cellSize.x;
			for (int i = 0; i < num; i++)
			{
				ClosestInPerimeter(ref minDistSq, ref closestPos, cellCoord, i, x, z, minDist, maxDist, filterFn);
				if (closestPos.hash != 0)
				{
					int num3 = (int)(Mathf.Sqrt(minDistSq) / (float)cellSize.x * 0.3f);
					if (num3 < 2)
					{
						num3 = 2;
					}
					for (int j = 0; j <= num3; j++)
					{
						ClosestInPerimeter(ref minDistSq, ref closestPos, cellCoord, i + j, x, z, minDist, maxDist, filterFn);
					}
					break;
				}
			}
			return closestPos;
		}

		public void ClosestInPerimeter(ref float minDistSq, ref Transition closestPos, Coord center, int perimSize, float x, float z, float minDist, float maxDist, Predicate<Transition> filterFn = null)
		{
			if (perimSize == 0)
			{
				Cell cell = cells[center];
				for (int i = 0; i < cell.count; i++)
				{
					float num = (cell.poses[i].pos.x - x) * (cell.poses[i].pos.x - x) + (cell.poses[i].pos.z - z) * (cell.poses[i].pos.z - z);
					if (num < minDistSq && num >= minDist * minDist && (filterFn == null || filterFn(cell.poses[i])))
					{
						minDistSq = num;
						closestPos = cell.poses[i];
					}
				}
				return;
			}
			for (int j = 0; j < perimSize; j++)
			{
				foreach (Coord item in center.DistanceStep(j, perimSize))
				{
					if (item.x < cells.rect.offset.x || item.x >= cells.rect.offset.x + cells.rect.size.x || item.z < cells.rect.offset.z || item.z >= cells.rect.offset.z + cells.rect.size.z)
					{
						continue;
					}
					Cell cell2 = cells[item];
					for (int k = 0; k < cell2.count; k++)
					{
						float num2 = (cell2.poses[k].pos.x - x) * (cell2.poses[k].pos.x - x) + (cell2.poses[k].pos.z - z) * (cell2.poses[k].pos.z - z);
						if (num2 < minDistSq && num2 >= minDist * minDist && (filterFn == null || filterFn(cell2.poses[k])))
						{
							minDistSq = num2;
							closestPos = cell2.poses[k];
						}
					}
				}
			}
		}

		public Transition ClosestDebug(float x, float z, float minDist = 0f, float maxDist = 2E+10f)
		{
			float num = maxDist;
			Transition result = new Transition
			{
				hash = 0
			};
			for (int i = 0; i < cells.arr.Length; i++)
			{
				Cell cell = cells.arr[i];
				for (int j = 0; j < cell.count; j++)
				{
					float num2 = (cell.poses[j].pos.x - x) * (cell.poses[j].pos.x - x) + (cell.poses[j].pos.z - z) * (cell.poses[j].pos.z - z);
					if (num2 < num && num2 >= minDist * minDist)
					{
						num = num2;
						result = cell.poses[j];
					}
				}
			}
			return result;
		}

		public void TwoClosest(out Transition minTrs1, out Transition minTrs2)
		{
			float num = 3.4028235E+38f;
			minTrs1 = default(Transition);
			minTrs2 = default(Transition);
			foreach (Transition item in All())
			{
				Transition transition = Closest(item.pos.x, item.pos.z, 0.0001f);
				float sqrMagnitude = (item.pos - transition.pos).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					minTrs1 = item;
					minTrs2 = transition;
				}
			}
		}

		public static PosTab Combine(params PosTab[] posTabs)
		{
			if (posTabs.Length == 0)
			{
				return null;
			}
			PosTab posTab = posTabs.Any();
			if (posTab == null)
			{
				return null;
			}
			PosTab posTab2 = new PosTab(posTab.pos, posTab.size, posTab.resolution);
			foreach (PosTab posTab3 in posTabs)
			{
				if (posTab3 == null)
				{
					continue;
				}
				for (int j = 0; j < posTab3.cells.arr.Length; j++)
				{
					Cell cell = posTab3.cells.arr[j];
					for (int k = 0; k < cell.count; k++)
					{
						posTab2.Add(cell.poses[k]);
					}
				}
			}
			return posTab2;
		}

		public IEnumerable<Transition> All()
		{
			for (int c = 0; c < cells.arr.Length; c++)
			{
				Cell cell = cells.arr[c];
				for (int i = 0; i < cell.count; i++)
				{
					yield return cell.poses[i];
				}
			}
		}

		public Transition Any()
		{
			for (int i = 0; i < cells.arr.Length; i++)
			{
				Cell cell = cells.arr[i];
				if (cell.count != 0)
				{
					return cell.poses[0];
				}
			}
			return default(Transition);
		}

		public int GetCountInRect(Vector3 rectPos, Vector3 rectSize)
		{
			int num = 0;
			for (int i = 0; i < cells.arr.Length; i++)
			{
				Cell cell = cells.arr[i];
				for (int j = 0; j < cell.count; j++)
				{
					if (!(cell.poses[j].pos.x < rectPos.x) && !(cell.poses[j].pos.x > rectPos.x + rectSize.x) && !(cell.poses[j].pos.z < rectPos.z) && !(cell.poses[j].pos.z > rectPos.z + rectSize.z))
					{
						num++;
					}
				}
			}
			return num;
		}

		public IEnumerable<int> CellNumsInRect(Vector2 min, Vector2 max, bool inCenter = true)
		{
			int minX = (int)((min.x - (float)rect.offset.x) / (float)cellSize.x);
			int minY = (int)((min.y - (float)rect.offset.z) / (float)cellSize.z);
			int maxX = (int)((max.x - (float)rect.offset.x) / (float)cellSize.x);
			int maxY = (int)((max.y - (float)rect.offset.z) / (float)cellSize.z);
			minX = Mathf.Max(0, minX);
			minY = Mathf.Max(0, minY);
			maxX = Mathf.Min(resolution - 1, maxX);
			maxY = Mathf.Min(resolution - 1, maxY);
			if (inCenter)
			{
				for (int x = minX; x <= maxX; x++)
				{
					for (int y = minY; y <= maxY; y++)
					{
						yield return y * resolution + x;
					}
				}
				yield break;
			}
			for (int x = minX; x <= maxX; x++)
			{
				yield return minY * resolution + x;
				yield return maxY * resolution + x;
			}
			for (int x = minY; x <= maxY; x++)
			{
				yield return x * resolution + minX;
				yield return x * resolution + maxX;
			}
		}

		public CoordRect CellsWithinBounds(Vector2 min, Vector2 max)
		{
			CoordRect c = default(CoordRect);
			min.x -= rect.offset.x;
			if (min.x < 0f)
			{
				min.x -= 1f;
			}
			c.offset.x = (int)(min.x / (float)cellSize.x);
			min.y -= rect.offset.z;
			if (min.y < 0f)
			{
				min.y -= 1f;
			}
			c.offset.z = (int)(min.y / (float)cellSize.z);
			max.x -= rect.offset.x;
			if (max.x < 0f)
			{
				max.x -= 1f;
			}
			c.MaxX = (int)(max.x / (float)cellSize.x + 1f);
			max.y -= rect.offset.z;
			if (max.y < 0f)
			{
				max.y -= 1f;
			}
			c.MaxZ = (int)(max.y / (float)cellSize.z + 1f);
			c = CoordRect.Intersected(c, new CoordRect(0, 0, resolution, resolution));
			c.Clamp(new Coord(0, 0), new Coord(resolution, resolution));
			return c;
		}

		public void RemoveObjsInRange(float posX, float posZ, float range)
		{
			Rect r = new Rect(posX - range, posZ - range, range * 2f, range * 2f);
			r = CoordinatesExtensions.Intersect(r, rect);
			foreach (int item in CellNumsInRect(r.min, r.max))
			{
				for (int num = cells.arr[item].count - 1; num >= 0; num--)
				{
					if ((cells.arr[item].poses[num].pos.x - posX) * (cells.arr[item].poses[num].pos.x - posX) + (cells.arr[item].poses[num].pos.z - posZ) * (cells.arr[item].poses[num].pos.z - posZ) < range * range)
					{
						Remove(item, num);
					}
				}
			}
		}

		public bool IsAnyObjInRange(float posX, float posZ, float range)
		{
			Vector2 min = new Vector2(posX - range, posZ - range);
			Vector2 max = new Vector2(posX + range, posZ + range);
			CoordRect coordRect = CellsWithinBounds(min, max);
			Coord min2 = coordRect.Min;
			Coord max2 = coordRect.Max;
			for (int i = min2.x; i < max2.x; i++)
			{
				for (int j = min2.z; j < max2.z; j++)
				{
					int num = j * resolution + i;
					for (int num2 = cells.arr[num].count - 1; num2 >= 0; num2--)
					{
						if ((cells.arr[num].poses[num2].pos.x - posX) * (cells.arr[num].poses[num2].pos.x - posX) + (cells.arr[num].poses[num2].pos.z - posZ) * (cells.arr[num].poses[num2].pos.z - posZ) < range * range)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public List<Transition> ToList()
		{
			List<Transition> list = new List<Transition>();
			for (int i = 0; i < cells.arr.Length; i++)
			{
				Cell cell = cells.arr[i];
				list.AddRange(cell.poses);
			}
			return list;
		}

		public TransitionsList ToTransitionsList()
		{
			TransitionsList transitionsList = new TransitionsList();
			for (int i = 0; i < cells.arr.Length; i++)
			{
				Cell cell = cells.arr[i];
				for (int j = 0; j < cell.count; j++)
				{
					transitionsList.Add(cells.arr[i].poses[j]);
				}
			}
			return transitionsList;
		}
	}
}
