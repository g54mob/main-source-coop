using System.Collections.Generic;
using NWH.Common.Input;
using NWH.Common.Vehicles;
using UnityEngine;
using UnityEngine.Events;

namespace NWH.Common.SceneManagement
{
	[DefaultExecutionOrder(500)]
	public class VehicleChanger : MonoBehaviour
	{
		public enum CharacterLocation
		{
			OutOfRange = 0,
			Near = 1,
			Inside = 2
		}

		[Tooltip("    Index of the current vehicle in vehicles list.")]
		public int activeVehicleIndex;

		[Tooltip("    Should the vehicles that the player is currently not using be put to sleep to improve performance?")]
		public bool putOtherVehiclesToSleep = true;

		[Tooltip("List of all of the vehicles that can be selected and driven in the scene.")]
		public List<Vehicle> vehicles = new List<Vehicle>();

		[Tooltip("Is vehicle changing character based? When true changing vehicles will require getting close to them\r\nto be able to enter, opposed to pressing a button to switch between vehicles.")]
		public bool characterBased;

		[Tooltip("    Game object representing a character. Can also be another vehicle.")]
		public GameObject characterObject;

		[Range(0.2f, 3f)]
		[Tooltip("    Maximum distance at which the character will be able to enter the vehicle.")]
		public float enterDistance = 2f;

		[Tooltip("Tag of the object representing the point from which the enter distance will be measured. Useful if you want to enable you character to enter only when near the door.")]
		public string enterExitTag = "EnterExitPoint";

		[Tooltip("    Maximum speed at which the character will be able to enter / exit the vehicle.")]
		public float maxEnterExitVehicleSpeed = 2f;

		[Tooltip("When the location is Near, the player can enter the vehicle.")]
		public CharacterLocation location;

		[Tooltip("Should the player start inside the vehicle?")]
		public bool startInVehicle;

		public UnityEvent onVehicleChanged = new UnityEvent();

		public UnityEvent onDeactivateAll = new UnityEvent();

		private Vehicle _nearestVehicle;

		private Vector3 _relativeEnterPosition;

		private GameObject[] _enterExitPoints;

		private GameObject _nearestEnterExitPoint;

		public static VehicleChanger Instance { get; private set; }

		private static Vehicle ActiveVehicle
		{
			get
			{
				if (Instance == null)
				{
					return null;
				}
				if (Instance.activeVehicleIndex >= 0 && Instance.activeVehicleIndex < Instance.vehicles.Count)
				{
					return Instance.vehicles[Instance.activeVehicleIndex];
				}
				return null;
			}
		}

		private void Awake()
		{
			Instance = this;
			for (int num = vehicles.Count - 1; num >= 0; num--)
			{
				if (vehicles[num] == null)
				{
					Debug.LogWarning("There is a null reference in the vehicles list. Removing. Make sure that vehicles list does not contain any null references.");
					vehicles.RemoveAt(num);
				}
			}
		}

		private void Start()
		{
			if (characterBased && !startInVehicle)
			{
				DeactivateAllIncludingActive();
			}
			else
			{
				DeactivateAllExceptActive();
			}
			if (startInVehicle && ActiveVehicle != null)
			{
				EnterVehicle(ActiveVehicle);
				_relativeEnterPosition = new Vector3(-2.5f, 1f, 0.5f);
			}
		}

		private void Update()
		{
			if (!characterBased)
			{
				if (InputProvider.CombinedInput((SceneInputProviderBase i) => i.ChangeVehicle()))
				{
					NextVehicle();
				}
			}
			else
			{
				if (!(characterObject != null))
				{
					return;
				}
				if (location != CharacterLocation.Inside)
				{
					location = CharacterLocation.OutOfRange;
					if (!characterObject.activeSelf)
					{
						characterObject.SetActive(value: true);
					}
					_enterExitPoints = GameObject.FindGameObjectsWithTag(enterExitTag);
					_nearestEnterExitPoint = null;
					float num = 1f / 0f;
					GameObject[] enterExitPoints = _enterExitPoints;
					foreach (GameObject gameObject in enterExitPoints)
					{
						float num3 = Vector3.SqrMagnitude(characterObject.transform.position - gameObject.transform.position);
						if (num3 < num)
						{
							num = num3;
							_nearestEnterExitPoint = gameObject;
						}
					}
					if (_nearestEnterExitPoint == null)
					{
						return;
					}
					if (Vector3.Magnitude(Vector3.ProjectOnPlane(_nearestEnterExitPoint.transform.position - characterObject.transform.position, Vector3.up)) < enterDistance)
					{
						location = CharacterLocation.Near;
						_nearestVehicle = _nearestEnterExitPoint.GetComponentInParent<Vehicle>();
					}
				}
				bool flag = InputProvider.CombinedInput((SceneInputProviderBase i) => i.ChangeVehicle());
				if (InputProvider.Instances.Count > 0 && flag)
				{
					if (location == CharacterLocation.Near && _nearestVehicle.Speed < maxEnterExitVehicleSpeed)
					{
						EnterVehicle(_nearestVehicle);
					}
					else if (location == CharacterLocation.Inside && _nearestVehicle.Speed < maxEnterExitVehicleSpeed)
					{
						ExitVehicle(_nearestVehicle);
					}
				}
			}
		}

		public void EnterVehicle(Vehicle v)
		{
			_nearestVehicle = v;
			if (characterBased)
			{
				characterObject.SetActive(value: false);
				_relativeEnterPosition = v.transform.InverseTransformPoint(characterObject.transform.position);
				location = CharacterLocation.Inside;
			}
			Instance.ChangeVehicle(v);
		}

		public void ExitVehicle(Vehicle v)
		{
			Instance.DeactivateAllIncludingActive();
			location = CharacterLocation.OutOfRange;
			if (characterBased)
			{
				characterObject.transform.position = v.transform.TransformPoint(_relativeEnterPosition);
				characterObject.transform.forward = v.transform.right;
				characterObject.transform.up = Vector3.up;
				characterObject.SetActive(value: true);
			}
		}

		public void RegisterVehicle(Vehicle v)
		{
			if (!vehicles.Contains(v))
			{
				vehicles.Add(v);
				if (activeVehicleIndex != vehicles.Count - 1)
				{
					v.enabled = false;
				}
			}
		}

		public void DeregisterVehicle(Vehicle v)
		{
			if (ActiveVehicle == v)
			{
				NextVehicle();
			}
			vehicles.Remove(v);
		}

		public void ChangeVehicle(int index)
		{
			if (vehicles.Count != 0)
			{
				activeVehicleIndex = index;
				if (activeVehicleIndex >= vehicles.Count)
				{
					activeVehicleIndex = 0;
				}
				DeactivateAllExceptActive();
				onVehicleChanged.Invoke();
			}
		}

		public void ChangeVehicle(Vehicle ac)
		{
			int num = vehicles.IndexOf(ac);
			if (num >= 0)
			{
				ChangeVehicle(num);
			}
		}

		public void NextVehicle()
		{
			if (vehicles.Count != 1)
			{
				ChangeVehicle(activeVehicleIndex + 1);
			}
		}

		public void PreviousVehicle()
		{
			if (vehicles.Count != 1)
			{
				int index = ((activeVehicleIndex == 0) ? (vehicles.Count - 1) : (activeVehicleIndex - 1));
				ChangeVehicle(index);
			}
		}

		public void DeactivateAllExceptActive()
		{
			for (int i = 0; i < vehicles.Count; i++)
			{
				if (i == activeVehicleIndex)
				{
					vehicles[i].enabled = true;
				}
				else if (putOtherVehiclesToSleep)
				{
					vehicles[i].enabled = false;
				}
			}
		}

		public void DeactivateAllIncludingActive()
		{
			for (int i = 0; i < vehicles.Count; i++)
			{
				vehicles[i].enabled = false;
			}
			onDeactivateAll.Invoke();
		}
	}
}
