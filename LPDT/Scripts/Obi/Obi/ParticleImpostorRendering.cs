using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

namespace Obi
{
	public class ParticleImpostorRendering
	{
		private static ProfilerMarker m_ParticlesToMeshPerfMarker = new ProfilerMarker("ParticlesToMesh");

		private List<Mesh> meshes = new List<Mesh>();

		private List<Vector3> vertices = new List<Vector3>(4000);

		private List<Vector3> normals = new List<Vector3>(4000);

		private List<Color> colors = new List<Color>(4000);

		private List<int> triangles = new List<int>(6000);

		private List<Vector4> anisotropy1 = new List<Vector4>(4000);

		private List<Vector4> anisotropy2 = new List<Vector4>(4000);

		private List<Vector4> anisotropy3 = new List<Vector4>(4000);

		private int particlesPerDrawcall;

		private int drawcallCount;

		private Vector3 particleOffset0 = new Vector3(1f, 1f, 0f);

		private Vector3 particleOffset1 = new Vector3(-1f, 1f, 0f);

		private Vector3 particleOffset2 = new Vector3(-1f, -1f, 0f);

		private Vector3 particleOffset3 = new Vector3(1f, -1f, 0f);

		public IEnumerable<Mesh> Meshes => meshes.AsReadOnly();

		private void Apply(Mesh mesh)
		{
			mesh.Clear();
			mesh.SetVertices(vertices);
			mesh.SetNormals(normals);
			mesh.SetColors(colors);
			mesh.SetUVs(0, anisotropy1);
			mesh.SetUVs(1, anisotropy2);
			mesh.SetUVs(2, anisotropy3);
			mesh.SetTriangles(triangles, 0, calculateBounds: true);
		}

		public void ClearMeshes()
		{
			foreach (Mesh mesh in meshes)
			{
				Object.DestroyImmediate(mesh);
			}
			meshes.Clear();
		}

		public void UpdateMeshes(IObiParticleCollection collection, bool[] visible = null, Color[] tint = null)
		{
			using (m_ParticlesToMeshPerfMarker.Auto())
			{
				particlesPerDrawcall = 16250;
				drawcallCount = collection.activeParticleCount / particlesPerDrawcall + 1;
				particlesPerDrawcall = Mathf.Min(particlesPerDrawcall, collection.activeParticleCount);
				if (drawcallCount != meshes.Count)
				{
					ClearMeshes();
					for (int i = 0; i < drawcallCount; i++)
					{
						Mesh mesh = new Mesh();
						mesh.name = "Particle impostors";
						mesh.hideFlags = HideFlags.HideAndDontSave;
						meshes.Add(mesh);
					}
				}
				Vector4 b = new Vector4(1f, 0f, 0f, 0f);
				Vector4 b2 = new Vector4(0f, 1f, 0f, 0f);
				Vector4 b3 = new Vector4(0f, 0f, 1f, 0f);
				int num = ((visible != null) ? visible.Length : 0);
				int num2 = ((tint != null) ? tint.Length : 0);
				for (int j = 0; j < drawcallCount; j++)
				{
					vertices.Clear();
					normals.Clear();
					colors.Clear();
					triangles.Clear();
					anisotropy1.Clear();
					anisotropy2.Clear();
					anisotropy3.Clear();
					int num3 = 0;
					int num4 = Mathf.Min((j + 1) * particlesPerDrawcall, collection.activeParticleCount);
					for (int k = j * particlesPerDrawcall; k < num4; k++)
					{
						if (k >= num || visible[k])
						{
							int particleRuntimeIndex = collection.GetParticleRuntimeIndex(k);
							Vector3 particlePosition = collection.GetParticlePosition(particleRuntimeIndex);
							collection.GetParticleAnisotropy(particleRuntimeIndex, ref b, ref b2, ref b3);
							Color particleColor = collection.GetParticleColor(particleRuntimeIndex);
							if (k < num2)
							{
								particleColor *= tint[k];
							}
							vertices.Add(particlePosition);
							vertices.Add(particlePosition);
							vertices.Add(particlePosition);
							vertices.Add(particlePosition);
							normals.Add(particleOffset0);
							normals.Add(particleOffset1);
							normals.Add(particleOffset2);
							normals.Add(particleOffset3);
							colors.Add(particleColor);
							colors.Add(particleColor);
							colors.Add(particleColor);
							colors.Add(particleColor);
							anisotropy1.Add(b);
							anisotropy1.Add(b);
							anisotropy1.Add(b);
							anisotropy1.Add(b);
							anisotropy2.Add(b2);
							anisotropy2.Add(b2);
							anisotropy2.Add(b2);
							anisotropy2.Add(b2);
							anisotropy3.Add(b3);
							anisotropy3.Add(b3);
							anisotropy3.Add(b3);
							anisotropy3.Add(b3);
							triangles.Add(num3 + 2);
							triangles.Add(num3 + 1);
							triangles.Add(num3);
							triangles.Add(num3 + 3);
							triangles.Add(num3 + 2);
							triangles.Add(num3);
							num3 += 4;
						}
					}
					Apply(meshes[j]);
				}
			}
		}
	}
}
