using System.Collections.Generic;
using CW.Common;
using UnityEngine;
using UnityEngine.XR;

namespace PaintIn3D
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwVrManager")]
	[AddComponentMenu("CW/Paint in 3D/CW VR Manager")]
	public class CwVrManager : MonoBehaviour
	{
		private class SimulatedState
		{
			public XRNode Node;

			public bool Set;

			public Vector3 Position;

			public Quaternion Rotation = Quaternion.identity;

			public SimulatedState(XRNode newNode)
			{
				Node = newNode;
			}
		}

		[SerializeField]
		private float grabDistance = 0.3f;

		[SerializeField]
		private KeyCode simulatedLeftTrigger = KeyCode.Mouse0;

		[SerializeField]
		private KeyCode simulatedLeftGrip = KeyCode.LeftControl;

		[SerializeField]
		private KeyCode simulatedRightTrigger = KeyCode.Mouse1;

		[SerializeField]
		private KeyCode simulatedRightGrip = KeyCode.RightControl;

		[SerializeField]
		private Vector3 simulatedTilt = new Vector3(0f, -15f, 0f);

		[SerializeField]
		private Vector3 simulatedOffset = new Vector3(0f, 0f, -0.2f);

		[SerializeField]
		private float simulatedReach = 1f;

		[SerializeField]
		private Vector3 simulatedEyeOffset = new Vector3(-0.0325f, 0f, 0f);

		[Range(0f, 1f)]
		[SerializeField]
		private float simulatedNormalInfluence = 0.25f;

		private SimulatedState[] simulatedStates = new SimulatedState[6]
		{
			new SimulatedState(XRNode.LeftEye),
			new SimulatedState(XRNode.RightEye),
			new SimulatedState(XRNode.CenterEye),
			new SimulatedState(XRNode.Head),
			new SimulatedState(XRNode.LeftHand),
			new SimulatedState(XRNode.RightHand)
		};

		private float hitDistance;

		private Quaternion hitRotation = Quaternion.identity;

		public bool LeftTrigger;

		public bool RightTrigger;

		public bool LeftGrip;

		public bool RightGrip;

		public bool PrevLeftTrigger;

		public bool PrevRightTrigger;

		public bool PrevLeftGrip;

		public bool PrevRightGrip;

		private static List<XRNodeState> states = new List<XRNodeState>();

		private static List<CwVrTool> tempTools = new List<CwVrTool>();

		private static LinkedList<CwVrManager> instances = new LinkedList<CwVrManager>();

		private LinkedListNode<CwVrManager> instancesNode;

		public float GrabDistance
		{
			get
			{
				return grabDistance;
			}
			set
			{
				grabDistance = value;
			}
		}

		public KeyCode SimulatedLeftTrigger
		{
			get
			{
				return simulatedLeftTrigger;
			}
			set
			{
				simulatedLeftTrigger = value;
			}
		}

		public KeyCode SimulatedLeftGrip
		{
			get
			{
				return simulatedLeftGrip;
			}
			set
			{
				simulatedLeftGrip = value;
			}
		}

		public KeyCode SimulatedRightTrigger
		{
			get
			{
				return simulatedRightTrigger;
			}
			set
			{
				simulatedRightTrigger = value;
			}
		}

		public KeyCode SimulatedRightGrip
		{
			get
			{
				return simulatedRightGrip;
			}
			set
			{
				simulatedRightGrip = value;
			}
		}

		public Vector3 SimulatedTilt
		{
			get
			{
				return simulatedTilt;
			}
			set
			{
				simulatedTilt = value;
			}
		}

		public Vector3 SimulatedOffset
		{
			get
			{
				return simulatedOffset;
			}
			set
			{
				simulatedOffset = value;
			}
		}

		public float SimulatedDistanceMax
		{
			get
			{
				return simulatedReach;
			}
			set
			{
				simulatedReach = value;
			}
		}

		public Vector3 SimulatedEyeOffset
		{
			get
			{
				return simulatedEyeOffset;
			}
			set
			{
				simulatedEyeOffset = value;
			}
		}

		public float SimulatedNormalInfluence
		{
			get
			{
				return simulatedNormalInfluence;
			}
			set
			{
				simulatedNormalInfluence = value;
			}
		}

		public static LinkedList<CwVrManager> Instances => instances;

		public bool IsSimulation => !XRSettings.enabled;

		public void SetRightTrigger(bool value)
		{
			RightTrigger = value;
		}

		public void SetLeftTrigger(bool value)
		{
			LeftTrigger = value;
		}

		public void SetLeftGrip(bool value)
		{
			LeftGrip = value;
		}

		public void SetRightGrip(bool value)
		{
			RightGrip = value;
		}

		public bool GetTrigger(XRNode node)
		{
			return node switch
			{
				XRNode.LeftHand => LeftTrigger, 
				XRNode.RightHand => RightTrigger, 
				_ => false, 
			};
		}

		public bool GetTriggerPressed(XRNode node)
		{
			switch (node)
			{
			case XRNode.LeftHand:
				if (LeftTrigger)
				{
					return !PrevLeftTrigger;
				}
				return false;
			case XRNode.RightHand:
				if (RightTrigger)
				{
					return !PrevRightTrigger;
				}
				return false;
			default:
				return false;
			}
		}

		public bool GetTriggerReleased(XRNode node)
		{
			switch (node)
			{
			case XRNode.LeftHand:
				if (!LeftTrigger)
				{
					return PrevLeftTrigger;
				}
				return false;
			case XRNode.RightHand:
				if (!RightTrigger)
				{
					return PrevRightTrigger;
				}
				return false;
			default:
				return false;
			}
		}

		public bool GetGrip(XRNode node)
		{
			return node switch
			{
				XRNode.LeftHand => LeftGrip, 
				XRNode.RightHand => RightGrip, 
				_ => false, 
			};
		}

		public bool GetGripPressed(XRNode node)
		{
			switch (node)
			{
			case XRNode.LeftHand:
				if (LeftGrip)
				{
					return !PrevLeftGrip;
				}
				return false;
			case XRNode.RightHand:
				if (RightGrip)
				{
					return !PrevRightGrip;
				}
				return false;
			default:
				return false;
			}
		}

		public bool GetGripReleased(XRNode node)
		{
			switch (node)
			{
			case XRNode.LeftHand:
				if (!LeftGrip)
				{
					return PrevLeftGrip;
				}
				return false;
			case XRNode.RightHand:
				if (!RightGrip)
				{
					return PrevRightGrip;
				}
				return false;
			default:
				return false;
			}
		}

		public XRNode GetClosestNode(Vector3 point, float maximumDistance)
		{
			XRNode result = (XRNode)(-1);
			float num = maximumDistance;
			Vector3 position = default(Vector3);
			if (TryGetPosition(XRNode.LeftHand, ref position))
			{
				float num2 = Vector3.Distance(point, position);
				if (num2 < num)
				{
					num = num2;
					result = XRNode.LeftHand;
				}
			}
			if (TryGetPosition(XRNode.RightHand, ref position))
			{
				float num3 = Vector3.Distance(point, position);
				if (num3 < num)
				{
					num = num3;
					result = XRNode.RightHand;
				}
			}
			return result;
		}

		private void SetSimulatedState(XRNode node, Vector3 position, Quaternion rotation)
		{
			SimulatedState[] array = simulatedStates;
			foreach (SimulatedState simulatedState in array)
			{
				if (simulatedState.Node == node)
				{
					simulatedState.Set = true;
					simulatedState.Position = position;
					simulatedState.Rotation = rotation;
					break;
				}
			}
		}

		public bool TryGetPosition(XRNode node, ref Vector3 position)
		{
			if (IsSimulation)
			{
				SimulatedState[] array = simulatedStates;
				foreach (SimulatedState simulatedState in array)
				{
					if (simulatedState.Node == node)
					{
						if (!simulatedState.Set)
						{
							return false;
						}
						position = simulatedState.Position;
						return true;
					}
				}
			}
			else
			{
				InputTracking.GetNodeStates(states);
				foreach (XRNodeState state in states)
				{
					if (state.nodeType == node)
					{
						return state.TryGetPosition(out position);
					}
				}
			}
			return false;
		}

		public bool TryGetRotation(XRNode node, ref Quaternion rotation)
		{
			if (IsSimulation)
			{
				SimulatedState[] array = simulatedStates;
				foreach (SimulatedState simulatedState in array)
				{
					if (simulatedState.Node == node)
					{
						if (!simulatedState.Set)
						{
							return false;
						}
						rotation = simulatedState.Rotation;
						return true;
					}
				}
			}
			else
			{
				InputTracking.GetNodeStates(states);
				foreach (XRNodeState state in states)
				{
					if (state.nodeType == node)
					{
						return state.TryGetRotation(out rotation);
					}
				}
			}
			return false;
		}

		protected virtual void OnEnable()
		{
			instancesNode = instances.AddLast(this);
		}

		protected virtual void OnDisable()
		{
			instances.Remove(instancesNode);
			instancesNode = null;
		}

		protected virtual void Start()
		{
			hitDistance = simulatedReach * 0.25f;
		}

		protected virtual void Update()
		{
			if (IsSimulation)
			{
				LeftTrigger = CwInput.GetKeyIsHeld(simulatedLeftTrigger);
				LeftGrip = CwInput.GetKeyIsHeld(simulatedLeftGrip);
				RightTrigger = CwInput.GetKeyIsHeld(simulatedRightTrigger);
				RightGrip = CwInput.GetKeyIsHeld(simulatedRightGrip);
				if (CwInputManager.PointOverGui(CwInput.GetMousePosition()))
				{
					LeftTrigger = false;
					LeftGrip = false;
					RightTrigger = false;
					RightGrip = false;
				}
			}
		}

		protected virtual void LateUpdate()
		{
			Camera camera = CwHelper.GetCamera(null);
			if (camera != null)
			{
				Ray ray = camera.ScreenPointToRay(CwInput.GetMousePosition());
				RaycastHit hitInfo = default(RaycastHit);
				Quaternion rotation = camera.transform.rotation;
				if (Physics.Raycast(ray, out hitInfo, simulatedReach))
				{
					hitDistance = hitInfo.distance;
					hitRotation = Quaternion.Inverse(rotation) * Quaternion.LookRotation(-hitInfo.normal);
				}
				Quaternion quaternion = Quaternion.Slerp(rotation, rotation * hitRotation, simulatedNormalInfluence) * Quaternion.Euler(simulatedTilt.x, 0f - simulatedTilt.y, simulatedTilt.z);
				Vector3 position = ray.GetPoint(hitDistance) + quaternion * new Vector3(simulatedOffset.x, simulatedOffset.y, simulatedOffset.z);
				SetSimulatedState(XRNode.LeftHand, position, quaternion);
				Quaternion quaternion2 = Quaternion.Slerp(rotation, rotation * hitRotation, simulatedNormalInfluence) * Quaternion.Euler(simulatedTilt.x, simulatedTilt.y, simulatedTilt.z);
				Vector3 position2 = ray.GetPoint(hitDistance) + quaternion2 * new Vector3(simulatedOffset.x, 0f - simulatedOffset.y, simulatedOffset.z);
				SetSimulatedState(XRNode.RightHand, position2, quaternion2);
				SetSimulatedState(XRNode.Head, camera.transform.position, camera.transform.rotation);
				SetSimulatedState(XRNode.CenterEye, camera.transform.position, camera.transform.rotation);
				SetSimulatedState(XRNode.LeftEye, camera.transform.TransformPoint(simulatedEyeOffset.x, simulatedEyeOffset.y, simulatedEyeOffset.z), camera.transform.rotation);
				SetSimulatedState(XRNode.RightEye, camera.transform.TransformPoint(0f - simulatedEyeOffset.x, simulatedEyeOffset.y, simulatedEyeOffset.z), camera.transform.rotation);
			}
			for (int i = 0; i <= 8; i++)
			{
				UpdateTools((XRNode)i);
			}
			PrevLeftTrigger = LeftTrigger;
			PrevLeftGrip = LeftGrip;
			PrevRightTrigger = RightTrigger;
			PrevRightGrip = RightGrip;
		}

		private void UpdateTools(XRNode node)
		{
			CwVrTool.GetTools(node, ref tempTools);
			foreach (CwVrTool tempTool in tempTools)
			{
				if (tempTool != null && tempTool.Node == node)
				{
					tempTool.UpdateGripped(this);
				}
			}
		}
	}
}
