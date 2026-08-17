using System;
using System.Collections.Generic;
using UnityEngine;

namespace RootMotion.FinalIK
{
	[Serializable]
	public class InteractionEffector
	{
		private Poser poser;

		private IKEffector effector;

		private float timer;

		private float length;

		private float weight;

		private float fadeInSpeed;

		private float defaultPositionWeight;

		private float defaultRotationWeight;

		private float defaultPull;

		private float defaultReach;

		private float defaultPush;

		private float defaultPushParent;

		private float defaultBendGoalWeight;

		private float defaultPoserWeight;

		private float resetTimer;

		private float prevPositionWeight;

		private float prevRotationWeight;

		private float prevRotateBoneWeight;

		private float prevPull;

		private float prevReach;

		private float prevPush;

		private float prevPushParent;

		private float prevBendGoalWeight;

		private float prevPoserWeight;

		private float switchTimer;

		private bool positionWeightUsed;

		private bool rotationWeightUsed;

		private bool pullUsed;

		private bool reachUsed;

		private bool pushUsed;

		private bool pushParentUsed;

		private bool bendGoalWeightUsed;

		private bool poserUsed;

		private bool prevPositionWeightUsed;

		private bool prevRotationWeightUsed;

		private bool prevRotateBoneWeightUsed;

		private bool prevPullUsed;

		private bool prevReachUsed;

		private bool prevPushUsed;

		private bool prevPushParentUsed;

		private bool prevBendGoalWeightUsed;

		private bool prevPoserUsed;

		private bool pickedUp;

		private bool defaults;

		private bool pickUpOnPostFBBIK;

		private Vector3 pickUpPosition;

		private Vector3 pausePositionRelative;

		private Vector3 prevTargetPosition;

		private Quaternion pickUpRotation;

		private Quaternion pauseRotationRelative;

		private Quaternion prevTargetRotation = Quaternion.identity;

		private Quaternion prevRotateBoneValue = Quaternion.identity;

		private InteractionTarget interactionTarget;

		private Transform target;

		private List<bool> triggered = new List<bool>();

		private InteractionSystem interactionSystem;

		private bool started;

		private bool isSwitching;

		public FullBodyBipedEffector effectorType { get; private set; }

		public bool isPaused { get; private set; }

		public InteractionObject interactionObject { get; private set; }

		public bool inInteraction => interactionObject != null;

		public float progress
		{
			get
			{
				if (!inInteraction)
				{
					return 0f;
				}
				if (length == 0f)
				{
					return 0f;
				}
				return timer / length;
			}
		}

		public InteractionEffector(FullBodyBipedEffector effectorType)
		{
			this.effectorType = effectorType;
		}

		public void Initiate(InteractionSystem interactionSystem)
		{
			this.interactionSystem = interactionSystem;
			effector = interactionSystem.ik.solver.GetEffector(effectorType);
			poser = effector.bone.GetComponent<Poser>();
			StoreDefaults();
		}

		public void StoreDefaults()
		{
			if (!(interactionSystem == null))
			{
				defaultPositionWeight = interactionSystem.ik.solver.GetEffector(effectorType).positionWeight;
				defaultRotationWeight = interactionSystem.ik.solver.GetEffector(effectorType).rotationWeight;
				defaultPoserWeight = ((poser != null) ? poser.weight : 0f);
				defaultPull = interactionSystem.ik.solver.GetChain(effectorType).pull;
				defaultReach = interactionSystem.ik.solver.GetChain(effectorType).reach;
				defaultPush = interactionSystem.ik.solver.GetChain(effectorType).push;
				defaultPushParent = interactionSystem.ik.solver.GetChain(effectorType).pushParent;
				defaultBendGoalWeight = interactionSystem.ik.solver.GetChain(effectorType).bendConstraint.weight;
			}
		}

		public void StorePrevious(InteractionObject interactionObject)
		{
			if (!(interactionSystem == null))
			{
				prevTargetPosition = interactionSystem.transform.InverseTransformPoint(effector.position);
				prevTargetRotation = Quaternion.Inverse(interactionSystem.transform.rotation) * effector.rotation;
				prevPositionWeightUsed = interactionObject.CurveUsed(InteractionObject.WeightCurve.Type.PositionWeight);
				prevRotationWeightUsed = interactionObject.CurveUsed(InteractionObject.WeightCurve.Type.RotationWeight);
				prevRotateBoneWeightUsed = interactionObject.CurveUsed(InteractionObject.WeightCurve.Type.RotateBoneWeight);
				prevPullUsed = interactionObject.CurveUsed(InteractionObject.WeightCurve.Type.Pull);
				prevReachUsed = interactionObject.CurveUsed(InteractionObject.WeightCurve.Type.Reach);
				prevPushUsed = interactionObject.CurveUsed(InteractionObject.WeightCurve.Type.Push);
				prevPushParentUsed = interactionObject.CurveUsed(InteractionObject.WeightCurve.Type.PushParent);
				prevBendGoalWeightUsed = interactionObject.CurveUsed(InteractionObject.WeightCurve.Type.BendGoalWeight);
				prevPoserUsed = poser != null && interactionObject.CurveUsed(InteractionObject.WeightCurve.Type.PoserWeight);
				prevPositionWeight = interactionSystem.ik.solver.GetEffector(effectorType).positionWeight;
				prevRotationWeight = interactionSystem.ik.solver.GetEffector(effectorType).rotationWeight;
				prevPoserWeight = ((poser != null) ? poser.weight : 0f);
				prevPull = interactionSystem.ik.solver.GetChain(effectorType).pull;
				prevReach = interactionSystem.ik.solver.GetChain(effectorType).reach;
				prevPush = interactionSystem.ik.solver.GetChain(effectorType).push;
				prevPushParent = interactionSystem.ik.solver.GetChain(effectorType).pushParent;
				prevBendGoalWeight = interactionSystem.ik.solver.GetChain(effectorType).bendConstraint.weight;
				switchTimer = 0f;
				isSwitching = true;
			}
		}

		public bool Switch(float speed, float deltaTime)
		{
			if (!isSwitching)
			{
				return false;
			}
			switchTimer = Mathf.MoveTowards(switchTimer, 1f, deltaTime * speed);
			if (effector.isEndEffector)
			{
				if (prevPullUsed)
				{
					interactionSystem.ik.solver.GetChain(effectorType).pull = Mathf.Lerp(prevPull, interactionSystem.ik.solver.GetChain(effectorType).pull, switchTimer);
				}
				if (prevReachUsed)
				{
					interactionSystem.ik.solver.GetChain(effectorType).reach = Mathf.Lerp(prevReach, interactionSystem.ik.solver.GetChain(effectorType).reach, switchTimer);
				}
				if (prevPushUsed)
				{
					interactionSystem.ik.solver.GetChain(effectorType).push = Mathf.Lerp(prevPush, interactionSystem.ik.solver.GetChain(effectorType).push, switchTimer);
				}
				if (prevPushParentUsed)
				{
					interactionSystem.ik.solver.GetChain(effectorType).pushParent = Mathf.Lerp(prevPushParent, interactionSystem.ik.solver.GetChain(effectorType).pushParent, switchTimer);
				}
				if (prevBendGoalWeightUsed)
				{
					interactionSystem.ik.solver.GetChain(effectorType).bendConstraint.weight = Mathf.Lerp(prevBendGoalWeight, interactionSystem.ik.solver.GetChain(effectorType).bendConstraint.weight, switchTimer);
				}
			}
			if (prevPositionWeightUsed)
			{
				effector.positionWeight = Mathf.Lerp(prevPositionWeight, effector.positionWeight, switchTimer);
				effector.position = Vector3.Lerp(interactionSystem.transform.TransformPoint(prevTargetPosition), effector.position, switchTimer);
			}
			if (prevRotationWeightUsed)
			{
				effector.rotationWeight = Mathf.Lerp(prevRotationWeight, effector.rotationWeight, switchTimer);
				effector.rotation = Quaternion.Lerp(prevTargetRotation, interactionSystem.transform.rotation * effector.rotation, switchTimer);
			}
			if (switchTimer >= 1f)
			{
				prevPullUsed = false;
				prevReachUsed = false;
				prevPushUsed = false;
				prevPushParentUsed = false;
				prevPositionWeightUsed = false;
				prevRotationWeightUsed = false;
				prevBendGoalWeightUsed = false;
				prevPoserUsed = false;
				isSwitching = false;
			}
			return true;
		}

		public bool ResetToDefaults(float speed, float deltaTime)
		{
			if (inInteraction)
			{
				return false;
			}
			if (isPaused)
			{
				return false;
			}
			if (defaults)
			{
				return false;
			}
			resetTimer = Mathf.MoveTowards(resetTimer, 0f, deltaTime * speed);
			if (effector.isEndEffector)
			{
				if (pullUsed)
				{
					interactionSystem.ik.solver.GetChain(effectorType).pull = Mathf.Lerp(defaultPull, interactionSystem.ik.solver.GetChain(effectorType).pull, resetTimer);
				}
				if (reachUsed)
				{
					interactionSystem.ik.solver.GetChain(effectorType).reach = Mathf.Lerp(defaultReach, interactionSystem.ik.solver.GetChain(effectorType).reach, resetTimer);
				}
				if (pushUsed)
				{
					interactionSystem.ik.solver.GetChain(effectorType).push = Mathf.Lerp(defaultPush, interactionSystem.ik.solver.GetChain(effectorType).push, resetTimer);
				}
				if (pushParentUsed)
				{
					interactionSystem.ik.solver.GetChain(effectorType).pushParent = Mathf.Lerp(defaultPushParent, interactionSystem.ik.solver.GetChain(effectorType).pushParent, resetTimer);
				}
				if (bendGoalWeightUsed)
				{
					interactionSystem.ik.solver.GetChain(effectorType).bendConstraint.weight = Mathf.Lerp(defaultBendGoalWeight, interactionSystem.ik.solver.GetChain(effectorType).bendConstraint.weight, resetTimer);
				}
			}
			if (positionWeightUsed)
			{
				effector.positionWeight = Mathf.Lerp(defaultPositionWeight, effector.positionWeight, resetTimer);
			}
			if (rotationWeightUsed)
			{
				effector.rotationWeight = Mathf.Lerp(defaultRotationWeight, effector.rotationWeight, resetTimer);
			}
			if (resetTimer <= 0f)
			{
				pullUsed = false;
				reachUsed = false;
				pushUsed = false;
				pushParentUsed = false;
				positionWeightUsed = false;
				rotationWeightUsed = false;
				bendGoalWeightUsed = false;
				poserUsed = false;
				defaults = true;
			}
			return true;
		}

		public bool Pause()
		{
			if (!inInteraction)
			{
				return false;
			}
			isPaused = true;
			pausePositionRelative = target.InverseTransformPoint(effector.position);
			pauseRotationRelative = Quaternion.Inverse(target.rotation) * effector.rotation;
			if (interactionSystem.OnInteractionPause != null)
			{
				interactionSystem.OnInteractionPause(effectorType, interactionObject);
			}
			return true;
		}

		public bool Resume()
		{
			if (!inInteraction)
			{
				return false;
			}
			isPaused = false;
			if (interactionSystem.OnInteractionResume != null)
			{
				interactionSystem.OnInteractionResume(effectorType, interactionObject);
			}
			return true;
		}

		public bool Start(InteractionObject interactionObject, string tag, float fadeInTime, bool interrupt)
		{
			if (inInteraction && !interrupt)
			{
				return false;
			}
			InteractionTarget interactionTarget = null;
			target = interactionObject.GetTarget(effectorType, tag);
			if (target != null)
			{
				interactionTarget = target.GetComponent<InteractionTarget>();
			}
			return Start(interactionObject, interactionTarget, fadeInTime, interrupt);
		}

		public bool Start(InteractionObject interactionObject, InteractionTarget interactionTarget, float fadeInTime, bool interrupt)
		{
			if (!inInteraction)
			{
				effector.position = effector.bone.position;
				effector.rotation = effector.bone.rotation;
			}
			else
			{
				if (!interrupt)
				{
					return false;
				}
				defaults = false;
				StorePrevious(interactionObject);
			}
			this.interactionTarget = interactionTarget;
			target = ((interactionTarget != null) ? interactionTarget.transform : interactionObject.transform);
			this.interactionObject = interactionObject;
			if (interactionSystem.OnInteractionStart != null)
			{
				interactionSystem.OnInteractionStart(effectorType, interactionObject);
			}
			interactionObject.OnStartInteraction(interactionSystem);
			triggered.Clear();
			for (int i = 0; i < interactionObject.events.Length; i++)
			{
				triggered.Add(item: false);
			}
			positionWeightUsed = interactionObject.CurveUsed(InteractionObject.WeightCurve.Type.PositionWeight);
			rotationWeightUsed = interactionObject.CurveUsed(InteractionObject.WeightCurve.Type.RotationWeight);
			pullUsed = interactionObject.CurveUsed(InteractionObject.WeightCurve.Type.Pull);
			reachUsed = interactionObject.CurveUsed(InteractionObject.WeightCurve.Type.Reach);
			pushUsed = interactionObject.CurveUsed(InteractionObject.WeightCurve.Type.Push);
			pushParentUsed = interactionObject.CurveUsed(InteractionObject.WeightCurve.Type.PushParent);
			bendGoalWeightUsed = interactionObject.CurveUsed(InteractionObject.WeightCurve.Type.BendGoalWeight);
			poserUsed = poser != null && interactionObject.CurveUsed(InteractionObject.WeightCurve.Type.PoserWeight);
			if (poser != null && poserUsed)
			{
				if (poser.poseRoot == null)
				{
					poser.weight = 0f;
				}
				if (interactionTarget != null)
				{
					if (interactionTarget.usePoser)
					{
						poser.SetPoseRoot(target.transform, interactionTarget.bones, interactionSystem.switchInteractionSpeed);
					}
				}
				else
				{
					poser.SetPoseRoot(null, null, interactionSystem.switchInteractionSpeed);
				}
				poser.AutoMapping();
			}
			if (defaults)
			{
				StoreDefaults();
			}
			timer = 0f;
			weight = 0f;
			fadeInSpeed = ((fadeInTime > 0f) ? (1f / fadeInTime) : 1000f);
			length = interactionObject.length;
			isPaused = false;
			pickedUp = false;
			pickUpPosition = Vector3.zero;
			pickUpRotation = Quaternion.identity;
			if (interactionTarget != null)
			{
				interactionTarget.RotateTo(effector.bone);
			}
			started = true;
			return true;
		}

		public void Update(Transform root, float speed, float deltaTime)
		{
			if (!inInteraction)
			{
				if (started)
				{
					isPaused = false;
					pickedUp = false;
					defaults = false;
					resetTimer = 1f;
					started = false;
				}
				return;
			}
			if (interactionTarget != null && !interactionTarget.rotateOnce)
			{
				interactionTarget.RotateTo(effector.bone);
			}
			if (isPaused)
			{
				if (!pickedUp)
				{
					effector.position = target.TransformPoint(pausePositionRelative);
					effector.rotation = target.rotation * pauseRotationRelative;
				}
				interactionObject.Apply(interactionSystem.ik.solver, effectorType, interactionTarget, timer, weight, isPaused: true);
				return;
			}
			timer += deltaTime * speed * ((interactionTarget != null) ? interactionTarget.interactionSpeedMlp : 1f);
			weight = Mathf.Clamp(weight + deltaTime * fadeInSpeed * speed, 0f, 1f);
			bool pickUp = false;
			bool pause = false;
			TriggerUntriggeredEvents(checkTime: true, out pickUp, out pause);
			Vector3 b = (pickedUp ? interactionSystem.transform.TransformPoint(pickUpPosition) : target.position);
			Quaternion b2 = (pickedUp ? (interactionSystem.transform.rotation * pickUpRotation) : target.rotation);
			effector.position = Vector3.Lerp(effector.bone.position, b, weight);
			effector.rotation = Quaternion.Lerp(effector.bone.rotation, b2, weight);
			interactionObject.Apply(interactionSystem.ik.solver, effectorType, interactionTarget, timer, weight, isPaused: false);
			if (pickUp)
			{
				PickUp(root);
			}
			if (pause)
			{
				Pause();
			}
			float value = interactionObject.GetValue(InteractionObject.WeightCurve.Type.PoserWeight, interactionTarget, timer);
			if (poser != null && poserUsed)
			{
				poser.weight = Mathf.Lerp(poser.weight, value, weight);
			}
			else if (value > 0f)
			{
				Warning.Log("InteractionObject " + interactionObject.name + " has a curve/multipler for Poser Weight, but the bone of effector " + effectorType.ToString() + " has no HandPoser/GenericPoser attached.", effector.bone);
			}
			if (timer >= length)
			{
				Stop();
			}
		}

		private void TriggerUntriggeredEvents(bool checkTime, out bool pickUp, out bool pause)
		{
			pickUp = false;
			pause = false;
			for (int i = 0; i < triggered.Count; i++)
			{
				if (triggered[i] || (checkTime && !(interactionObject.events[i].time < timer)))
				{
					continue;
				}
				interactionObject.events[i].Activate(effector.bone);
				if (interactionObject.events[i].pickUp)
				{
					if (timer >= interactionObject.events[i].time)
					{
						timer = interactionObject.events[i].time;
					}
					pickUp = true;
				}
				if (interactionObject.events[i].pause)
				{
					if (timer >= interactionObject.events[i].time)
					{
						timer = interactionObject.events[i].time;
					}
					pause = true;
				}
				if (interactionSystem.OnInteractionEvent != null)
				{
					interactionSystem.OnInteractionEvent(effectorType, interactionObject, interactionObject.events[i]);
				}
				triggered[i] = true;
			}
		}

		private void PickUp(Transform root)
		{
			pickUpPosition = root.InverseTransformPoint(effector.position);
			pickUpRotation = Quaternion.Inverse(interactionSystem.transform.rotation) * effector.rotation;
			pickUpOnPostFBBIK = true;
			pickedUp = true;
			Rigidbody component = interactionObject.targetsRoot.GetComponent<Rigidbody>();
			if (component != null)
			{
				if (!component.isKinematic)
				{
					component.isKinematic = true;
				}
				Collider component2 = root.GetComponent<Collider>();
				if (component2 != null)
				{
					Collider[] componentsInChildren = interactionObject.targetsRoot.GetComponentsInChildren<Collider>();
					foreach (Collider collider in componentsInChildren)
					{
						if (!collider.isTrigger && collider.enabled)
						{
							Physics.IgnoreCollision(component2, collider);
						}
					}
				}
			}
			if (interactionSystem.OnInteractionPickUp != null)
			{
				interactionSystem.OnInteractionPickUp(effectorType, interactionObject);
			}
		}

		public bool Stop()
		{
			if (!inInteraction)
			{
				return false;
			}
			bool pickUp = false;
			bool pause = false;
			TriggerUntriggeredEvents(checkTime: false, out pickUp, out pause);
			if (interactionSystem.OnInteractionStop != null)
			{
				interactionSystem.OnInteractionStop(effectorType, interactionObject);
			}
			if (interactionTarget != null)
			{
				interactionTarget.ResetRotation();
			}
			interactionObject = null;
			weight = 0f;
			timer = 0f;
			isPaused = false;
			target = null;
			defaults = false;
			resetTimer = 1f;
			pickedUp = false;
			started = false;
			return true;
		}

		public void OnPostFBBIK()
		{
			if (inInteraction)
			{
				float num = interactionObject.GetValue(InteractionObject.WeightCurve.Type.RotateBoneWeight, interactionTarget, timer) * weight;
				if (isSwitching)
				{
					num = Mathf.Lerp(prevRotateBoneWeight, num, switchTimer);
				}
				else
				{
					prevRotateBoneWeight = num;
				}
				if (num > 0f)
				{
					Quaternion b = (pickedUp ? (interactionSystem.transform.rotation * pickUpRotation) : effector.rotation);
					Quaternion quaternion = Quaternion.Slerp(effector.bone.rotation, b, num * num);
					effector.bone.localRotation = Quaternion.Inverse(effector.bone.parent.rotation) * quaternion;
				}
				if (isSwitching)
				{
					effector.bone.localRotation = Quaternion.Slerp(prevRotateBoneValue, effector.bone.localRotation, switchTimer);
				}
				else
				{
					prevRotateBoneValue = effector.bone.localRotation;
				}
				if (pickUpOnPostFBBIK)
				{
					Vector3 position = effector.bone.position;
					effector.bone.position = interactionSystem.transform.TransformPoint(pickUpPosition);
					interactionObject.targetsRoot.parent = effector.bone;
					effector.bone.position = position;
					pickUpOnPostFBBIK = false;
				}
			}
		}
	}
}
