using System;
using System.Collections.Generic;
using Mimicraft.Networking;
using Mimicraft.VoxelEditor;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class PlayerDebris : MonoBehaviour
	{
		[Header("Saçılma")]
		[Tooltip("Patlama kuvveti. Parçalar gövde merkezinden dışarı savrulur.")]
		[SerializeField]
		[Min(0f)]
		private float explosionForce = 350f;

		[Tooltip("Patlamanın etki yarıçapı (metre).")]
		[SerializeField]
		[Min(0.1f)]
		private float explosionRadius = 4f;

		[Tooltip("Patlama merkezinin gövde tabanından ne kadar aşağıda olduğu. Büyük değer parçaları yanlara değil yukarı fırlatır.")]
		[SerializeField]
		private float explosionUpwards = 0.6f;

		[Tooltip("Parçalara verilen rastgele dönme miktarı.")]
		[SerializeField]
		[Min(0f)]
		private float spinTorque = 12f;

		[Header("Ömür")]
		[Tooltip("Parçaların kaç saniye yerde kalacağı.")]
		[SerializeField]
		[Min(0f)]
		private float lifetime = 4f;

		[Tooltip("Küçülerek kaybolma süresi (saniye).")]
		[SerializeField]
		[Min(0.01f)]
		private float shrinkSeconds = 0.5f;

		[Header("Fizik")]
		[Tooltip("Parçaların ÇARPMAYACAĞI katmanlar. Oyuncu katmanını buraya koy - yoksa patlayan bir modelin parçaları etraftaki oyuncuları itip kakar.")]
		[SerializeField]
		private LayerMask excludeLayers;

		[Tooltip("Parçalara verilecek katman. Boş bırakırsan kaynağın katmanını devralırlar.")]
		[SerializeField]
		private int debrisLayer = -1;

		private PlayerHealth health;

		private PlayerVoxelBody voxelBody;

		private HiderLimbs limbs;

		private bool exploded;

		private readonly List<Renderer> hiddenPieces = new List<Renderer>();

		private void Awake()
		{
			health = GetComponent<PlayerHealth>();
			voxelBody = GetComponent<PlayerVoxelBody>();
			limbs = GetComponent<HiderLimbs>();
		}

		private void OnEnable()
		{
			if (!(health == null))
			{
				NetworkVariable<int> networkVariable = health.Health;
				networkVariable.OnValueChanged = (NetworkVariable<int>.OnValueChangedDelegate)Delegate.Combine(networkVariable.OnValueChanged, new NetworkVariable<int>.OnValueChangedDelegate(OnHealthChanged));
				if (exploded && health.Health.Value > 0)
				{
					Restore();
				}
			}
		}

		private void OnDisable()
		{
			if (health != null)
			{
				NetworkVariable<int> networkVariable = health.Health;
				networkVariable.OnValueChanged = (NetworkVariable<int>.OnValueChangedDelegate)Delegate.Remove(networkVariable.OnValueChanged, new NetworkVariable<int>.OnValueChangedDelegate(OnHealthChanged));
			}
		}

		private void OnHealthChanged(int previous, int current)
		{
			if (previous <= 0 && current > 0)
			{
				Restore();
			}
			else if (current <= 0 && previous > 0)
			{
				Explode();
			}
		}

		private void Restore()
		{
			foreach (Renderer hiddenPiece in hiddenPieces)
			{
				if (hiddenPiece != null)
				{
					hiddenPiece.enabled = true;
				}
			}
			hiddenPieces.Clear();
			exploded = false;
		}

		public void Explode()
		{
			if (exploded || voxelBody == null || !voxelBody.HasVoxelBody)
			{
				return;
			}
			exploded = true;
			Vector3 center;
			Vector3 origin = (voxelBody.TryGetBodyCenterWorld(out center) ? center : base.transform.position);
			GameObject gameObject = ((limbs != null) ? limbs.LimbRig : null);
			if (gameObject != null)
			{
				gameObject.transform.localScale = limbs.VisibleRigScale;
			}
			List<Renderer> list = new List<Renderer>();
			VoxelModel[] componentsInChildren = GetComponentsInChildren<VoxelModel>(includeInactive: false);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Renderer component = componentsInChildren[i].GetComponent<Renderer>();
				if (component != null && component.enabled)
				{
					list.Add(component);
				}
			}
			if (gameObject != null)
			{
				Renderer[] componentsInChildren2 = gameObject.GetComponentsInChildren<Renderer>(includeInactive: false);
				foreach (Renderer renderer in componentsInChildren2)
				{
					if (renderer.enabled)
					{
						list.Add(renderer);
					}
				}
			}
			foreach (Renderer item in list)
			{
				SpawnChunk(item, origin);
			}
			hiddenPieces.Clear();
			foreach (Renderer item2 in list)
			{
				item2.enabled = false;
				hiddenPieces.Add(item2);
			}
			if (gameObject != null)
			{
				gameObject.transform.localScale = Vector3.zero;
			}
		}

		private void SpawnChunk(Renderer source, Vector3 origin)
		{
			Mesh mesh = ExtractMesh(source);
			if (!(mesh == null))
			{
				GameObject obj = new GameObject("Debris_" + source.name);
				obj.transform.SetPositionAndRotation(source.transform.position, source.transform.rotation);
				obj.transform.localScale = source.transform.lossyScale;
				obj.layer = ((debrisLayer >= 0) ? debrisLayer : source.gameObject.layer);
				obj.AddComponent<MeshFilter>().sharedMesh = mesh;
				obj.AddComponent<MeshRenderer>().sharedMaterials = source.sharedMaterials;
				Bounds bounds = mesh.bounds;
				BoxCollider boxCollider = obj.AddComponent<BoxCollider>();
				boxCollider.center = bounds.center;
				boxCollider.size = bounds.size;
				boxCollider.excludeLayers = excludeLayers;
				Rigidbody rigidbody = obj.AddComponent<Rigidbody>();
				rigidbody.mass = Mathf.Clamp(bounds.size.x * bounds.size.y * bounds.size.z, 0.1f, 20f);
				rigidbody.excludeLayers = excludeLayers;
				rigidbody.AddExplosionForce(explosionForce, origin - Vector3.up * explosionUpwards, explosionRadius, 0f, ForceMode.Impulse);
				rigidbody.AddTorque(UnityEngine.Random.insideUnitSphere * spinTorque, ForceMode.Impulse);
				obj.AddComponent<DebrisPiece>().Begin(lifetime, shrinkSeconds);
			}
		}

		private static Mesh ExtractMesh(Renderer source)
		{
			if (source is SkinnedMeshRenderer skinnedMeshRenderer)
			{
				if (skinnedMeshRenderer.sharedMesh == null)
				{
					return null;
				}
				Mesh mesh = new Mesh();
				skinnedMeshRenderer.BakeMesh(mesh);
				return mesh;
			}
			MeshFilter component = source.GetComponent<MeshFilter>();
			if (!(component != null))
			{
				return null;
			}
			return component.sharedMesh;
		}
	}
}
