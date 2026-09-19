using UnityEngine;

namespace Obi
{
	[CreateAssetMenu(fileName = "collision material", menuName = "Obi/Collision Material", order = 180)]
	public class ObiCollisionMaterial : ScriptableObject
	{
		protected ObiCollisionMaterialHandle materialHandle;

		public float dynamicFriction;

		public float staticFriction;

		public float stickiness;

		public float stickDistance;

		public Oni.MaterialCombineMode frictionCombine;

		public Oni.MaterialCombineMode stickinessCombine;

		[Space]
		public bool rollingContacts;

		[Indent]
		[VisibleIf("rollingContacts", false)]
		public float rollingFriction;

		public ObiCollisionMaterialHandle handle
		{
			get
			{
				CreateMaterialIfNeeded();
				return materialHandle;
			}
		}

		private void OnEnable()
		{
			UpdateMaterial();
		}

		private void OnDisable()
		{
			ObiColliderWorld.GetInstance().DestroyCollisionMaterial(materialHandle);
		}

		private void OnValidate()
		{
			UpdateMaterial();
		}

		public void UpdateMaterial()
		{
			ObiColliderWorld instance = ObiColliderWorld.GetInstance();
			CollisionMaterial value = instance.collisionMaterials[handle.index];
			value.FromObiCollisionMaterial(this);
			instance.collisionMaterials[handle.index] = value;
		}

		protected void CreateMaterialIfNeeded()
		{
			if (materialHandle == null || !materialHandle.isValid)
			{
				ObiColliderWorld instance = ObiColliderWorld.GetInstance();
				materialHandle = instance.CreateCollisionMaterial();
				materialHandle.owner = this;
				CollisionMaterial value = instance.collisionMaterials[materialHandle.index];
				value.FromObiCollisionMaterial(this);
				instance.collisionMaterials[materialHandle.index] = value;
			}
		}
	}
}
