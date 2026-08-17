using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace NWH.VehiclePhysics2.Modules.Trailer
{
	[Serializable]
	public class TrailerHitchModule : VehicleComponent
	{
		[Tooltip("True if object is trailer and is attached to a towing vehicle and also true if towing vehicle and has trailer\r\nattached.")]
		public bool attached;

		[Tooltip("If the vehicle is a trailer, this is the object placed at the point at which it will connect to the towing vehicle. If the vehicle is towing, this is the object placed at point at which trailer will be coneected.")]
		public Transform attachmentPoint;

		public float attachmentTriggerRadius = 0.4f;

		public int attachmentLayer;

		[FormerlySerializedAs("attachOnPlay")]
		[Tooltip("    If a trailer is in range when the scene is started it will be attached.")]
		public bool attachOnEnable;

		[Tooltip("    Breaking force of the generated joint.")]
		public float breakForce = 1f / 0f;

		[Tooltip("    Can the trailer be detached once it is attached?")]
		public bool detachable = true;

		[Tooltip("Power reduction that will be applied when vehicle has no trailer to avoid wheel spin when controlled with a binary controller.")]
		public float noTrailerPowerCoefficient = 1f;

		public UnityEvent onTrailerAttach = new UnityEvent();

		public UnityEvent onTrailerDetach = new UnityEvent();

		[Tooltip("    Is trailer's attachment point close enough to be attached to the towing vehicle?")]
		public bool trailerInRange;

		[Tooltip("    Use for articulated busses and equipment where rotation around vertical axis is not wanted.")]
		public bool useHingeJoint;

		[NonSerialized]
		private ConfigurableJoint _configurableJoint;

		[NonSerialized]
		public TrailerModule attachedTrailerModule;

		private Collider _triggerCollider;

		private bool _hasHadFirstFixedUpdate;

		public virtual void OnTriggerEnter(Collider other)
		{
			if (!(other == null) && other.gameObject.layer == attachmentLayer)
			{
				_triggerCollider = other;
				if (!_hasHadFirstFixedUpdate && attachOnEnable)
				{
					vehicleController.input.states.trailerAttachDetach = true;
				}
			}
		}

		public virtual void OnTriggerStay(Collider other)
		{
			if (!(other == null) && other.gameObject.layer == attachmentLayer)
			{
				trailerInRange = true;
				_triggerCollider = other;
			}
		}

		protected override void VC_Initialize()
		{
			base.VC_Initialize();
			attachedTrailerModule = null;
			attached = false;
		}

		public override void VC_Update()
		{
			base.VC_Update();
			if (attachedTrailerModule != null && attachedTrailerModule.vehicleController != null)
			{
				attachedTrailerModule.vehicleController.input.states = vehicleController.input.states;
				attachedTrailerModule.vehicleController.effectsManager.lightsManager.SetStateFromInt(vehicleController.effectsManager.lightsManager.GetIntState());
			}
		}

		public override void VC_FixedUpdate()
		{
			base.VC_FixedUpdate();
			if (vehicleController.input.TrailerAttachDetach && !attached && _triggerCollider != null)
			{
				TrailerModuleWrapper componentInParent = _triggerCollider.GetComponentInParent<TrailerModuleWrapper>();
				if (componentInParent != null)
				{
					AttachTrailer(componentInParent);
				}
			}
			else if (attached && vehicleController.input.TrailerAttachDetach)
			{
				DetachTrailer(vehicleController);
			}
			if (attached && _configurableJoint == null)
			{
				DetachTrailer(vehicleController);
			}
			if (trailerInRange)
			{
				trailerInRange = false;
			}
			else
			{
				vehicleController.input.TrailerAttachDetach = false;
			}
			trailerInRange = false;
			_triggerCollider = null;
			_hasHadFirstFixedUpdate = true;
		}

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				vehicleController.powertrain.engine.powerModifiers.Add(NoTrailerPowerModifier);
				return true;
			}
			return false;
		}

		public override bool VC_Disable(bool calledByParent)
		{
			if (base.VC_Disable(calledByParent))
			{
				vehicleController.powertrain.engine.powerModifiers.Remove(NoTrailerPowerModifier);
				return true;
			}
			return false;
		}

		public float NoTrailerPowerModifier()
		{
			if (attached)
			{
				return 1f;
			}
			return noTrailerPowerCoefficient;
		}

		public void AttachTrailer(TrailerModuleWrapper trailerWrapper)
		{
			TrailerModule module = trailerWrapper.module;
			if (module == null)
			{
				Debug.LogWarning("Trying to attach a null trailer.");
				return;
			}
			VehicleController componentInParent = trailerWrapper.GetComponentInParent<VehicleController>();
			componentInParent.enabled = true;
			componentInParent.vehicleTransform.position = componentInParent.transform.position - (module.attachmentPoint.transform.position - attachmentPoint.transform.position);
			_configurableJoint = vehicleController.GetComponent<ConfigurableJoint>();
			if (_configurableJoint != null)
			{
				UnityEngine.Object.Destroy(_configurableJoint);
			}
			vehicleController.input.TrailerAttachDetach = false;
			attached = true;
			module.OnAttach(this);
			onTrailerAttach.Invoke();
			if (_configurableJoint == null)
			{
				_configurableJoint = vehicleController.gameObject.AddComponent<ConfigurableJoint>();
			}
			_configurableJoint.anchor = vehicleController.transform.InverseTransformPoint(module.attachmentPoint.position);
			_configurableJoint.xMotion = ConfigurableJointMotion.Locked;
			_configurableJoint.yMotion = ConfigurableJointMotion.Locked;
			_configurableJoint.zMotion = ConfigurableJointMotion.Locked;
			_configurableJoint.angularZMotion = ((!useHingeJoint) ? ConfigurableJointMotion.Free : ConfigurableJointMotion.Locked);
			_configurableJoint.enableCollision = true;
			_configurableJoint.breakForce = breakForce;
			_configurableJoint.connectedBody = module.vehicleController.vehicleRigidbody;
			attachedTrailerModule = module;
		}

		public void DetachTrailer(VehicleController vc)
		{
			if (detachable && attachedTrailerModule != null && !(attachedTrailerModule.vehicleController == null))
			{
				attached = false;
				if (_configurableJoint != null)
				{
					UnityEngine.Object.Destroy(_configurableJoint);
					_configurableJoint = null;
				}
				attachedTrailerModule.OnDetach();
				attachedTrailerModule = null;
				vc.input.TrailerAttachDetach = false;
				onTrailerDetach.Invoke();
			}
		}
	}
}
