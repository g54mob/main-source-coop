using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public static class SurfaceColorSampler
	{
		private const int MaxCachedSize = 256;

		private static readonly string[] ColorPropertyNames = new string[6] { "_BaseColor", "_Color", "_MainColor", "_TintColor", "_Tint", "_1st_ShadeColor" };

		private static readonly string[] TexturePropertyNames = new string[4] { "_BaseMap", "_MainTex", "_BaseColorMap", "_1st_ShadeMap" };

		private static readonly Dictionary<Texture2D, Texture2D> readableCopies = new Dictionary<Texture2D, Texture2D>();

		public static bool LogSamples;

		private static readonly HashSet<int> logged = new HashSet<int>();

		private static readonly HashSet<int> warned = new HashSet<int>();

		private static void WarnOnce(UnityEngine.Object source, string message)
		{
			if (source != null && warned.Add(source.GetInstanceID()))
			{
				Debug.LogWarning(message, source);
			}
		}

		public static bool TrySample(RaycastHit hit, out Color32 color)
		{
			color = default(Color32);
			Renderer renderer = FindRenderer(hit.collider);
			if (renderer == null)
			{
				WarnOnce(hit.collider, "[SurfaceColorSampler] '" + hit.collider.name + "' uzerinde (ve ust/alt hiyerarsisinde) Renderer yok - rengi okunamaz.");
				return false;
			}
			Material material = MaterialForHit(hit, renderer);
			if (material == null)
			{
				WarnOnce(renderer, "[SurfaceColorSampler] '" + renderer.name + "' hicbir materyal tasimiyor - rengi okunamaz.");
				return false;
			}
			Color color2;
			bool flag = TryGetTint(material, out color2);
			Color color3;
			bool flag2 = TryGetTexel(hit, material, out color3);
			Describe(hit, material, flag, color2, flag2, color3);
			Color color4 = color3;
			if (flag2 && TryAccept(flag ? (color4 * color2) : color4, hit, material, out color))
			{
				return true;
			}
			if (flag && TryAccept(color2, hit, material, out color))
			{
				return true;
			}
			string text = ((material.shader != null) ? material.shader.name : "(shader yok)");
			WarnOnce(material, "[SurfaceColorSampler] '" + material.name + "' (" + text + ") icinde okunacak renk yok - ne bilinen bir renk ozelligi (" + string.Join(", ", ColorPropertyNames) + ") ne de bir doku (" + string.Join(", ", TexturePropertyNames) + ") bulundu. '" + hit.collider.name + "' orneklenemedi.");
			return false;
		}

		private static void Describe(RaycastHit hit, Material material, bool hasTint, Color tint, bool hasTexel, Color texel)
		{
			if (LogSamples && !(hit.collider == null) && logged.Add(hit.collider.GetInstanceID()))
			{
				string arg = ((hit.collider is MeshCollider meshCollider) ? (string.Format("MeshCollider (convex={0}, mesh='{1}'", meshCollider.convex, (meshCollider.sharedMesh != null) ? meshCollider.sharedMesh.name : "yok") + $", readable={meshCollider.sharedMesh != null && meshCollider.sharedMesh.isReadable})") : hit.collider.GetType().Name);
				string text = "yok";
				if (TryGetTexture(material, out var texture, out var property))
				{
					Texture2D readable = GetReadable(texture);
					text = $"'{texture.name}' {texture.width}x{texture.height} (onbellek " + ((readable != null) ? $"{readable.width}x{readable.height}" : "YOK") + ", ozellik " + property + ")";
				}
				Debug.Log($"[SurfaceColorSampler] '{hit.collider.name}' | {arg} | tri={hit.triangleIndex} " + $"| uv={hit.textureCoord} | materyal '{material.name}' " + "(" + ((material.shader != null) ? material.shader.name : "shader yok") + ") | doku " + text + " | teksel " + (hasTexel ? texel.ToString() : "okunamadi") + " | tint " + (hasTint ? tint.ToString() : "yok") + " | KULLANILAN: " + ((!hasTexel) ? (hasTint ? "tint" : "hicbiri") : (hasTint ? "teksel x tint" : "teksel")), hit.collider);
			}
		}

		private static bool TryAccept(Color candidate, RaycastHit hit, Material material, out Color32 color)
		{
			color = default(Color32);
			if (candidate.a <= 0.001f)
			{
				string text = ((material.shader != null) ? material.shader.name : "(shader yok)");
				WarnOnce(material, "[SurfaceColorSampler] '" + material.name + "' (" + text + ") saydam renk " + $"verdi ({candidate}) - '{hit.collider.name}' örneklenemedi, atlandı.");
				return false;
			}
			color = new Color(candidate.r, candidate.g, candidate.b, 1f);
			return true;
		}

		private static Renderer FindRenderer(Collider collider)
		{
			if (collider == null)
			{
				return null;
			}
			Renderer component = collider.GetComponent<Renderer>();
			if (component != null)
			{
				return component;
			}
			Renderer componentInParent = collider.GetComponentInParent<Renderer>();
			if (componentInParent != null)
			{
				return componentInParent;
			}
			return collider.GetComponentInChildren<Renderer>();
		}

		private static Material MaterialForHit(RaycastHit hit, Renderer renderer)
		{
			Material[] sharedMaterials = renderer.sharedMaterials;
			if (sharedMaterials == null || sharedMaterials.Length == 0)
			{
				return null;
			}
			if (sharedMaterials.Length == 1)
			{
				return sharedMaterials[0];
			}
			if (hit.triangleIndex >= 0 && hit.collider is MeshCollider { convex: false } meshCollider && meshCollider.sharedMesh != null)
			{
				int num = SubmeshForTriangle(meshCollider.sharedMesh, hit.triangleIndex);
				if (num >= 0 && num < sharedMaterials.Length)
				{
					return sharedMaterials[num];
				}
			}
			return sharedMaterials[0];
		}

		private static int SubmeshForTriangle(Mesh mesh, int triangleIndex)
		{
			int num = 0;
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				if (mesh.GetTopology(i) != MeshTopology.Triangles)
				{
					return -1;
				}
				int num2 = (int)(mesh.GetIndexCount(i) / 3);
				if (triangleIndex < num + num2)
				{
					return i;
				}
				num += num2;
			}
			return -1;
		}

		private static bool TryGetTint(Material material, out Color color)
		{
			string[] colorPropertyNames = ColorPropertyNames;
			foreach (string name in colorPropertyNames)
			{
				if (material.HasColor(name))
				{
					color = material.GetColor(name);
					return true;
				}
			}
			color = default(Color);
			return false;
		}

		private static bool TryGetTexel(RaycastHit hit, Material material, out Color color)
		{
			color = default(Color);
			if (!(hit.collider is MeshCollider { convex: false } meshCollider))
			{
				return false;
			}
			if (hit.triangleIndex < 0)
			{
				WarnOnce(meshCollider, "[SurfaceColorSampler] '" + meshCollider.name + "' MeshCollider'ında UV okunamıyor - modelin import ayarlarında Read/Write Enabled kapalı. Açmadan piksel rengi yerine materyalin düz rengi alınır.");
				return false;
			}
			if (meshCollider.sharedMesh != null && !meshCollider.sharedMesh.isReadable)
			{
				WarnOnce(meshCollider.sharedMesh, "[SurfaceColorSampler] '" + meshCollider.sharedMesh.name + "' Read/Write Enabled kapalı. Editor'de çalışır ama BUILD'de UV okunamaz ve eyedropper texture rengi yerine materyalin düz rengini alır. Modelin import ayarlarından aç.");
			}
			if (!TryGetTexture(material, out var texture, out var property))
			{
				return false;
			}
			Texture2D readable = GetReadable(texture);
			if (readable == null)
			{
				return false;
			}
			Vector2 textureScale = material.GetTextureScale(property);
			Vector2 textureOffset = material.GetTextureOffset(property);
			Vector2 uv = hit.textureCoord * textureScale + textureOffset;
			color = PointSample(readable, uv);
			return true;
		}

		private static Color PointSample(Texture2D texture, Vector2 uv)
		{
			int x = Mathf.Clamp((int)(Mathf.Repeat(uv.x, 1f) * (float)texture.width), 0, texture.width - 1);
			int y = Mathf.Clamp((int)(Mathf.Repeat(uv.y, 1f) * (float)texture.height), 0, texture.height - 1);
			return texture.GetPixel(x, y);
		}

		private static bool TryGetTexture(Material material, out Texture2D texture, out string property)
		{
			if (material.mainTexture is Texture2D texture2D)
			{
				texture = texture2D;
				property = (material.HasTexture("_BaseMap") ? "_BaseMap" : (material.HasTexture("_MainTex") ? "_MainTex" : null));
				if (property != null)
				{
					return true;
				}
			}
			string[] texturePropertyNames = TexturePropertyNames;
			foreach (string text in texturePropertyNames)
			{
				if (material.HasTexture(text) && material.GetTexture(text) is Texture2D texture2D2)
				{
					texture = texture2D2;
					property = text;
					return true;
				}
			}
			texture = null;
			property = null;
			return false;
		}

		private static Texture2D GetReadable(Texture2D source)
		{
			if (source == null)
			{
				return null;
			}
			if (source.isReadable)
			{
				return source;
			}
			if (readableCopies.TryGetValue(source, out var value))
			{
				return value;
			}
			Texture2D texture2D = null;
			RenderTexture active = RenderTexture.active;
			RenderTexture renderTexture = null;
			try
			{
				int num = Mathf.Max(1, Mathf.Min(source.width, 256));
				int num2 = Mathf.Max(1, Mathf.Min(source.height, 256));
				renderTexture = RenderTexture.GetTemporary(num, num2, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
				Graphics.Blit(source, renderTexture);
				RenderTexture.active = renderTexture;
				texture2D = new Texture2D(num, num2, TextureFormat.RGBA32, mipChain: false, linear: false);
				texture2D.ReadPixels(new Rect(0f, 0f, num, num2), 0, 0);
				texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: false);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[SurfaceColorSampler] '" + source.name + "' okunamadı: " + ex.Message);
				if (texture2D != null)
				{
					UnityEngine.Object.Destroy(texture2D);
					texture2D = null;
				}
			}
			finally
			{
				RenderTexture.active = active;
				if (renderTexture != null)
				{
					RenderTexture.ReleaseTemporary(renderTexture);
				}
			}
			readableCopies[source] = texture2D;
			return texture2D;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetOnPlay()
		{
			foreach (Texture2D value in readableCopies.Values)
			{
				if (value != null)
				{
					UnityEngine.Object.Destroy(value);
				}
			}
			readableCopies.Clear();
			warned.Clear();
			logged.Clear();
		}
	}
}
