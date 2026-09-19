using UnityEngine;

namespace Obi
{
	[AddComponentMenu("Physics/Obi/Obi Bone Override", 882)]
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public class ObiBoneOverride : MonoBehaviour
	{
		[SerializeField]
		protected ObiBone.BonePropertyCurve _radius = new ObiBone.BonePropertyCurve(0.1f, 1f);

		[SerializeField]
		protected ObiBone.BonePropertyCurve _mass = new ObiBone.BonePropertyCurve(0.1f, 1f);

		[SerializeField]
		protected ObiBone.BonePropertyCurve _rotationalMass = new ObiBone.BonePropertyCurve(0.1f, 1f);

		[SerializeField]
		protected ObiBone.BonePropertyCurve _skinCompliance = new ObiBone.BonePropertyCurve(0.01f, 1f);

		[SerializeField]
		protected ObiBone.BonePropertyCurve _skinRadius = new ObiBone.BonePropertyCurve(0.1f, 1f);

		[SerializeField]
		protected ObiBone.BonePropertyCurve _stretchCompliance = new ObiBone.BonePropertyCurve(0f, 1f);

		[SerializeField]
		protected ObiBone.BonePropertyCurve _shear1Compliance = new ObiBone.BonePropertyCurve(0f, 1f);

		[SerializeField]
		protected ObiBone.BonePropertyCurve _shear2Compliance = new ObiBone.BonePropertyCurve(0f, 1f);

		[SerializeField]
		protected ObiBone.BonePropertyCurve _torsionCompliance = new ObiBone.BonePropertyCurve(0f, 1f);

		[SerializeField]
		protected ObiBone.BonePropertyCurve _bend1Compliance = new ObiBone.BonePropertyCurve(0f, 1f);

		[SerializeField]
		protected ObiBone.BonePropertyCurve _bend2Compliance = new ObiBone.BonePropertyCurve(0f, 1f);

		[SerializeField]
		protected ObiBone.BonePropertyCurve _plasticYield = new ObiBone.BonePropertyCurve(0f, 1f);

		[SerializeField]
		protected ObiBone.BonePropertyCurve _plasticCreep = new ObiBone.BonePropertyCurve(0f, 1f);

		[SerializeField]
		protected ObiBone.BonePropertyCurve _drag = new ObiBone.BonePropertyCurve(0.05f, 1f);

		[SerializeField]
		protected ObiBone.BonePropertyCurve _lift = new ObiBone.BonePropertyCurve(0.02f, 1f);

		private ObiBone bone;

		public ObiBone.BonePropertyCurve radius
		{
			get
			{
				return _radius;
			}
			set
			{
				_radius = value;
				bone.UpdateRadius();
			}
		}

		public ObiBone.BonePropertyCurve mass
		{
			get
			{
				return _mass;
			}
			set
			{
				_mass = value;
				bone.UpdateMasses();
			}
		}

		public ObiBone.BonePropertyCurve rotationalMass
		{
			get
			{
				return _rotationalMass;
			}
			set
			{
				_rotationalMass = value;
				bone.UpdateMasses();
			}
		}

		public ObiBone.BonePropertyCurve skinCompliance
		{
			get
			{
				return _skinCompliance;
			}
			set
			{
				_skinCompliance = value;
				bone.SetConstraintsDirty(Oni.ConstraintType.Skin);
			}
		}

		public ObiBone.BonePropertyCurve skinRadius
		{
			get
			{
				return _skinRadius;
			}
			set
			{
				_skinRadius = value;
				bone.SetConstraintsDirty(Oni.ConstraintType.Skin);
			}
		}

		public ObiBone.BonePropertyCurve stretchCompliance
		{
			get
			{
				return _stretchCompliance;
			}
			set
			{
				_stretchCompliance = value;
				bone.SetConstraintsDirty(Oni.ConstraintType.StretchShear);
			}
		}

		public ObiBone.BonePropertyCurve shear1Compliance
		{
			get
			{
				return _shear1Compliance;
			}
			set
			{
				_shear1Compliance = value;
				bone.SetConstraintsDirty(Oni.ConstraintType.StretchShear);
			}
		}

		public ObiBone.BonePropertyCurve shear2Compliance
		{
			get
			{
				return _shear2Compliance;
			}
			set
			{
				_shear2Compliance = value;
				bone.SetConstraintsDirty(Oni.ConstraintType.StretchShear);
			}
		}

		public ObiBone.BonePropertyCurve torsionCompliance
		{
			get
			{
				return _torsionCompliance;
			}
			set
			{
				_torsionCompliance = value;
				bone.SetConstraintsDirty(Oni.ConstraintType.BendTwist);
			}
		}

		public ObiBone.BonePropertyCurve bend1Compliance
		{
			get
			{
				return _bend1Compliance;
			}
			set
			{
				_bend1Compliance = value;
				bone.SetConstraintsDirty(Oni.ConstraintType.BendTwist);
			}
		}

		public ObiBone.BonePropertyCurve bend2Compliance
		{
			get
			{
				return _bend2Compliance;
			}
			set
			{
				_bend2Compliance = value;
				bone.SetConstraintsDirty(Oni.ConstraintType.BendTwist);
			}
		}

		public ObiBone.BonePropertyCurve plasticYield
		{
			get
			{
				return _plasticYield;
			}
			set
			{
				_plasticYield = value;
				bone.SetConstraintsDirty(Oni.ConstraintType.BendTwist);
			}
		}

		public ObiBone.BonePropertyCurve plasticCreep
		{
			get
			{
				return _plasticCreep;
			}
			set
			{
				_plasticCreep = value;
				bone.SetConstraintsDirty(Oni.ConstraintType.BendTwist);
			}
		}

		public ObiBone.BonePropertyCurve drag
		{
			get
			{
				return _drag;
			}
			set
			{
				_drag = value;
				bone.SetConstraintsDirty(Oni.ConstraintType.Aerodynamics);
			}
		}

		public ObiBone.BonePropertyCurve lift
		{
			get
			{
				return _lift;
			}
			set
			{
				_lift = value;
				bone.SetConstraintsDirty(Oni.ConstraintType.Aerodynamics);
			}
		}

		public void Awake()
		{
			bone = GetComponentInParent<ObiBone>();
		}

		protected void OnValidate()
		{
			if (bone != null)
			{
				bone.UpdateRadius();
				bone.UpdateMasses();
				bone.SetConstraintsDirty(Oni.ConstraintType.Skin);
				bone.SetConstraintsDirty(Oni.ConstraintType.StretchShear);
				bone.SetConstraintsDirty(Oni.ConstraintType.BendTwist);
				bone.SetConstraintsDirty(Oni.ConstraintType.Aerodynamics);
			}
		}
	}
}
