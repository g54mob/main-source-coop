using System;
using System.Collections.Generic;
using UnityEngine;

namespace PaintCore
{
	[Serializable]
	public abstract class CwCommand
	{
		public int Index;

		public bool Preview;

		public int Priority;

		public CwHashedMaterial Material;

		public int Pass;

		public CwHashedModel Model;

		public int Submesh;

		public CwHashedTexture LocalMaskTexture;

		public Vector4 LocalMaskChannel;

		private static int _LocalMaskTexture = Shader.PropertyToID("_LocalMaskTexture");

		private static int _LocalMaskChannel = Shader.PropertyToID("_LocalMaskChannel");

		public abstract bool RequireMesh { get; }

		public static void BuildMaterial(ref Material material, ref int materialHash, string path, string keyword = null)
		{
			material = CwCommon.BuildMaterial(path, keyword);
			materialHash = CwSerialization.TryRegister(material);
		}

		public static int Compare(CwCommand a, CwCommand b)
		{
			int num = a.Priority.CompareTo(b.Priority);
			if (num > 0)
			{
				return 1;
			}
			if (num < 0)
			{
				return -1;
			}
			return a.Index.CompareTo(b.Index);
		}

		public void SetState(bool preview, int priority)
		{
			Preview = preview;
			Priority = priority;
			Index = 0;
		}

		public virtual void Apply(Material material)
		{
			material.SetTexture(_LocalMaskTexture, LocalMaskTexture);
			material.SetVector(_LocalMaskChannel, LocalMaskChannel);
		}

		public abstract void Pool();

		public abstract void Transform(Matrix4x4 posMatrix, Matrix4x4 rotMatrix, Matrix4x4 rotMatrix2);

		public abstract CwCommand SpawnCopy();

		public CwCommand SpawnCopyLocal(Transform transform)
		{
			CwCommand cwCommand = SpawnCopy();
			Matrix4x4 worldToLocalMatrix = transform.worldToLocalMatrix;
			cwCommand.Transform(worldToLocalMatrix, Matrix4x4.Rotate(worldToLocalMatrix.rotation), Matrix4x4.identity);
			return cwCommand;
		}

		public CwCommand SpawnCopyWorld(Transform transform)
		{
			CwCommand cwCommand = SpawnCopy();
			Matrix4x4 localToWorldMatrix = transform.localToWorldMatrix;
			cwCommand.Transform(localToWorldMatrix, Matrix4x4.Rotate(localToWorldMatrix.rotation), Matrix4x4.identity);
			return cwCommand;
		}

		protected T SpawnCopy<T>(Stack<T> pool) where T : CwCommand, new()
		{
			T obj = ((pool.Count > 0) ? pool.Pop() : new T());
			obj.Index = Index;
			obj.Preview = Preview;
			obj.Priority = Priority;
			obj.Material = Material;
			obj.Pass = Pass;
			obj.Model = Model;
			obj.Submesh = Submesh;
			obj.LocalMaskTexture = LocalMaskTexture;
			obj.LocalMaskChannel = LocalMaskChannel;
			return obj;
		}

		public virtual void Apply(CwPaintableTexture paintableTexture)
		{
			LocalMaskTexture = paintableTexture.LocalMaskTexture;
			LocalMaskChannel = CwCommon.IndexToVector((int)paintableTexture.LocalMaskChannel);
		}
	}
}
