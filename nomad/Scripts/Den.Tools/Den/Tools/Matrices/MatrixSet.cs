using System;
using System.Collections.Generic;
using UnityEngine;

namespace Den.Tools.Matrices
{
	public class MatrixSet
	{
		public enum PrototypeType
		{
			Texture = 0,
			Prefab = 1,
			TerrainLayer = 2
		}

		public struct Prototype
		{
			public TerrainLayer layer;

			public Texture2D tex;

			public GameObject prefab;

			public int instanceNum;

			public UnityEngine.Object Object
			{
				get
				{
					if (layer != null)
					{
						return layer;
					}
					if (tex != null)
					{
						return tex;
					}
					return prefab;
				}
			}

			public Prototype(TerrainLayer layer)
			{
				this.layer = layer;
				tex = null;
				prefab = null;
				instanceNum = 0;
			}

			public Prototype(Texture2D texture)
			{
				layer = null;
				tex = texture;
				prefab = null;
				instanceNum = 0;
			}

			public Prototype(GameObject prefab)
			{
				layer = null;
				tex = null;
				this.prefab = prefab;
				instanceNum = 0;
			}

			public Prototype(TerrainLayer layer, int instanceNum)
			{
				this.layer = layer;
				tex = null;
				prefab = null;
				this.instanceNum = instanceNum;
			}

			public Prototype(Texture2D texture, int instanceNum)
			{
				layer = null;
				tex = texture;
				prefab = null;
				this.instanceNum = instanceNum;
			}

			public Prototype(GameObject prefab, int instanceNum)
			{
				layer = null;
				tex = null;
				this.prefab = prefab;
				this.instanceNum = instanceNum;
			}

			public Prototype(Array layers, int num)
			{
				UnityEngine.Object layerObject = GetLayerObject(layers.GetValue(num));
				if (layerObject == null)
				{
					layer = null;
					tex = null;
					prefab = null;
					instanceNum = 0;
					return;
				}
				instanceNum = 0;
				for (int i = 0; i < num; i++)
				{
					UnityEngine.Object layerObject2 = GetLayerObject(layers.GetValue(i));
					if (!(layerObject2 == null) && layerObject2 == layerObject)
					{
						instanceNum++;
					}
				}
				if (layerObject is TerrainLayer terrainLayer)
				{
					layer = terrainLayer;
				}
				else
				{
					layer = null;
				}
				if (layerObject is Texture2D texture2D)
				{
					tex = texture2D;
				}
				else
				{
					tex = null;
				}
				if (layerObject is GameObject gameObject)
				{
					prefab = gameObject;
				}
				else
				{
					prefab = null;
				}
			}

			public T[] CheckAppendLayers<T>(T[] layers) where T : class
			{
				int num = 0;
				UnityEngine.Object obj = Object;
				bool flag = false;
				for (int i = 0; i < layers.Length; i++)
				{
					UnityEngine.Object layerObject = GetLayerObject(layers.GetValue(i));
					if (!(layerObject == null) && layerObject == obj)
					{
						if (num == instanceNum)
						{
							flag = true;
							break;
						}
						num++;
					}
				}
				if (!flag)
				{
					ArrayTools.Add(ref layers, NewLayer<T>(this));
				}
				return layers;
			}

			private static UnityEngine.Object GetLayerObject(object layer)
			{
				if (!(layer is TerrainLayer result))
				{
					if (!(layer is DetailPrototype prot))
					{
						if (layer is Texture2D result2)
						{
							return result2;
						}
						return null;
					}
					return prot.Object();
				}
				return result;
			}

			public static T NewLayer<T>(Prototype prot) where T : class
			{
				if (typeof(T) == typeof(Texture2D))
				{
					if (prot.tex != null)
					{
						return (T)(object)prot.tex;
					}
					if (prot.prefab != null)
					{
						return (T)(object)prot.prefab.GetMainTexture();
					}
				}
				if (typeof(T) == typeof(TerrainLayer))
				{
					if (prot.layer != null)
					{
						return (T)(object)prot.layer;
					}
					if (prot.tex != null)
					{
						return new TerrainLayer
						{
							diffuseTexture = prot.tex,
							normalMapTexture = prot.tex.GetNormalTexture(),
							tileSize = new Vector2(20f, 20f)
						} as T;
					}
				}
				if (typeof(T) == typeof(DetailPrototype))
				{
					if (prot.tex != null)
					{
						return new DetailPrototype
						{
							renderMode = DetailRenderMode.Grass,
							dryColor = new Color(0.95f, 1f, 0.65f),
							healthyColor = new Color(0.5f, 0.65f, 0.35f),
							prototypeTexture = prot.tex
						} as T;
					}
					if (prot.prefab != null)
					{
						return new DetailPrototype
						{
							renderMode = DetailRenderMode.VertexLit,
							usePrototypeMesh = true,
							prototype = prot.prefab
						} as T;
					}
				}
				return null;
			}
		}

		public CoordRect rect;

		public Vector3 worldPos;

		public Vector3 worldSize;

		private DictionaryOrdered<Prototype, Matrix> prototypesMatrices = new DictionaryOrdered<Prototype, Matrix>();

		public Vector3 WorldMax => worldPos + worldSize;

		public Vector2D PixelSize => new Vector2D(worldSize.x / (float)(rect.size.x - 1), worldSize.z / (float)(rect.size.z - 1));

		public Matrix this[Prototype prototype]
		{
			get
			{
				if (prototypesMatrices.TryGetValue(prototype, out var value))
				{
					return value;
				}
				return null;
			}
			set
			{
				if (value.rect != rect)
				{
					throw new Exception("Trying to add a matrix of different resolution");
				}
				if (prototypesMatrices.ContainsKey(prototype))
				{
					prototypesMatrices[prototype] = value;
				}
				else
				{
					prototypesMatrices.Add(prototype, value);
				}
			}
		}

		public Matrix this[int num]
		{
			get
			{
				return prototypesMatrices[num];
			}
			set
			{
				prototypesMatrices[num] = value;
			}
		}

		public int Count => prototypesMatrices.Count;

		public ICollection<Prototype> Prototypes => prototypesMatrices.Keys;

		public ICollection<Matrix> Matrices => prototypesMatrices.Values;

		public MatrixSet(CoordRect rect, Vector3 worldPos, Vector3 worldSize)
		{
			this.rect = rect;
			this.worldPos = worldPos;
			this.worldSize = worldSize;
		}

		public MatrixSet(CoordRect rect, Vector2D worldPos, Vector2D worldSize, float height)
		{
			this.rect = rect;
			this.worldPos = new Vector3(worldPos.x, 0f, worldPos.z);
			this.worldSize = new Vector3(worldSize.x, height, worldSize.z);
		}

		public MatrixSet(MatrixSet src)
		{
			rect = src.rect;
			worldPos = src.worldPos;
			worldSize = src.worldSize;
			foreach (KeyValuePair<Prototype, Matrix> prototypesMatrix in src.prototypesMatrices)
			{
				prototypesMatrices.Add(prototypesMatrix.Key, new Matrix(prototypesMatrix.Value));
			}
		}

		public MatrixSet(CoordRect rect, Vector3 worldPos, Vector3 worldSize, ICollection<Prototype> prototypes)
		{
			this.rect = rect;
			this.worldPos = worldPos;
			this.worldSize = worldSize;
			foreach (Prototype prototype in prototypes)
			{
				prototypesMatrices.Add(prototype, new Matrix(rect));
			}
		}

		public Matrix GetMatrixByNum(int num)
		{
			return prototypesMatrices[num];
		}

		public void SetMatrixByNum(int num, Matrix value)
		{
			prototypesMatrices[num] = value;
		}

		public Prototype GetPrototypeByNum(int num)
		{
			return prototypesMatrices.GetKeyByNum(num);
		}

		public IEnumerable<T> PrototypesOfType<T>() where T : class
		{
			foreach (Prototype key in prototypesMatrices.Keys)
			{
				if ((object)key is T val)
				{
					yield return val;
				}
			}
		}

		public bool TryGetValue(Prototype prototype, out Matrix matrix)
		{
			return prototypesMatrices.TryGetValue(prototype, out matrix);
		}

		public bool TryGetValue(Predicate<object> predicate, out Matrix matrix)
		{
			foreach (KeyValuePair<Prototype, Matrix> prototypesMatrix in prototypesMatrices)
			{
				if (predicate(prototypesMatrix.Key))
				{
					matrix = prototypesMatrix.Value;
					return true;
				}
			}
			matrix = null;
			return false;
		}

		public void SetOffset(Coord newOffset)
		{
			rect.offset = newOffset;
			foreach (Matrix value in prototypesMatrices.Values)
			{
				value.rect.offset = newOffset;
			}
		}

		public void Append(Matrix matrix, Prototype prototype, bool normalized = true)
		{
			if (normalized)
			{
				foreach (Matrix value2 in prototypesMatrices.Values)
				{
					value2.MultiplyInv(matrix);
				}
			}
			if (prototypesMatrices.TryGetValue(prototype, out var value))
			{
				value.Add(matrix);
				return;
			}
			value = new Matrix(matrix);
			prototypesMatrices.Add(prototype, value);
		}

		public void SyncPrototypes(ICollection<Prototype> prototypes)
		{
			foreach (Prototype prototype in prototypes)
			{
				if (!prototypesMatrices.Contains(prototype))
				{
					Matrix value = new Matrix(rect);
					prototypesMatrices.Add(prototype, value);
				}
			}
		}

		public void Resize(CoordRect dstRect, bool interpolate = false)
		{
			DictionaryOrdered<Prototype, Matrix> dictionaryOrdered = new DictionaryOrdered<Prototype, Matrix>();
			foreach (KeyValuePair<Prototype, Matrix> prototypesMatrix in prototypesMatrices)
			{
				Matrix value = prototypesMatrix.Value;
				Matrix matrix = new Matrix(dstRect);
				ResizeMatrix(value, matrix, interpolate);
				dictionaryOrdered.Add(prototypesMatrix.Key, matrix);
			}
			prototypesMatrices = dictionaryOrdered;
			rect = dstRect;
		}

		public static void Resize(MatrixSet src, MatrixSet dst, bool interpolate = false)
		{
			foreach (KeyValuePair<Prototype, Matrix> prototypesMatrix in src.prototypesMatrices)
			{
				Matrix value = prototypesMatrix.Value;
				if (!dst.TryGetValue(prototypesMatrix.Key, out var matrix))
				{
					matrix = new Matrix(dst.rect);
					dst[prototypesMatrix.Key] = matrix;
				}
				ResizeMatrix(value, matrix, interpolate);
			}
		}

		private static void ResizeMatrix(Matrix src, Matrix dst, bool interpolate = false)
		{
			if (interpolate)
			{
				if (src.rect.size.x * 2 == dst.rect.size.x)
				{
					MatrixOps.UpscaleFast(src, dst);
				}
				else if (src.rect.size.x == dst.rect.size.x * 2)
				{
					MatrixOps.DownscaleFast(src, dst);
				}
				else
				{
					MatrixOps.Resize(src, dst);
				}
			}
			else
			{
				MatrixOps.ResizeNearestNeighbor(src, dst);
			}
		}

		public static void CopyIntersected(MatrixSet src, MatrixSet dst)
		{
			foreach (KeyValuePair<Prototype, Matrix> prototypesMatrix in src.prototypesMatrices)
			{
				Prototype key = prototypesMatrix.Key;
				Matrix value = prototypesMatrix.Value;
				if (!dst.prototypesMatrices.TryGetValue(key, out var value2))
				{
					value2 = new Matrix(dst.rect);
					dst.prototypesMatrices.Add(key, value2);
				}
				Matrix.CopyIntersected(value, value2);
			}
		}

		public static void CopyResized(MatrixSet src, MatrixSet dst, Vector2D srcRectPos, Vector2D srcRectSize, Coord dstRectPos, Coord dstRectSize)
		{
			foreach (KeyValuePair<Prototype, Matrix> prototypesMatrix in src.prototypesMatrices)
			{
				Prototype key = prototypesMatrix.Key;
				Matrix value = prototypesMatrix.Value;
				if (!dst.prototypesMatrices.TryGetValue(key, out var value2))
				{
					value2 = new Matrix(dst.rect);
					dst.prototypesMatrices.Add(key, value2);
				}
				Matrix.CopyResized(value, value2, srcRectPos, srcRectSize, dstRectPos, dstRectSize);
			}
		}
	}
}
