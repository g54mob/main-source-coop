using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using VContainer;

namespace EvilCore.Inputs
{
	public class InputRemappingService : MonoBehaviour, IInputRemappingService
	{
		private struct MapSnapshot
		{
			public ControllerType controllerType;

			public int controllerId;

			public int categoryId;

			public int layoutId;

			public string xml;
		}

		[Header("Dependencies")]
		[SerializeField]
		private InputGlyphService inputGlyphService;

		[Header("Settings")]
		[SerializeField]
		private float listenTimeout = 5f;

		[SerializeField]
		[Tooltip("Seconds to wait after a rebind starts before listening, so the activating click is not captured.")]
		private float activationDebounce = 0.12f;

		[SerializeField]
		[Tooltip("Actions that cannot be rebound or overwritten (core-loop / mouse-button actions).")]
		private string[] protectedActionNames = new string[4] { "PrimaryInteraction", "HeldItemPrimaryUse", "FinishObjectPlacing", "RotateObject" };

		[Inject]
		private IInputCoActivationProvider _coActivation;

		private InputMapper _keyboardMapper;

		private InputMapper _mouseMapper;

		private InputMapper _joystickMapper;

		private bool _isListening;

		private readonly List<MapSnapshot> _revertSnapshot = new List<MapSnapshot>();

		private bool _canRevert;

		private int _pendingActionId;

		private AxisRange _pendingAxisRange;

		private int _pendingAemToReplaceId = -1;

		private Coroutine _armRoutine;

		private InputMapper.Context _pendingKbContext;

		private InputMapper.Context _pendingMouseContext;

		private InputMapper.Context _pendingJoystickContext;

		private bool _hasKbContext;

		private bool _hasMouseContext;

		private bool _hasJoystickContext;

		private readonly HashSet<string> _protectedActions = new HashSet<string>();

		private const float MinActivationDebounce = 0.2f;

		private static readonly string[] DefaultProtectedActions = new string[4] { "PrimaryInteraction", "HeldItemPrimaryUse", "FinishObjectPlacing", "RotateObject" };

		private static readonly Dictionary<int, string> CategoryNames = new Dictionary<int, string>
		{
			{ 1, "Base" },
			{ 2, "On Foot" },
			{ 3, "Driving" },
			{ 4, "Equipping" },
			{ 5, "Placement" },
			{ 6, "Sitting" }
		};

		private static readonly HashSet<string> ExcludedActions = new HashSet<string> { "LookHorizontal", "LookVertical" };

		public bool IsListening => _isListening;

		public bool CanRevert => _canRevert;

		public event Action OnListeningStarted;

		public event Action<InputRemappingResult> OnInputRemapped;

		public event Action<InputRemappingResult> OnRemapOutcome;

		public event Action OnListeningStopped;

		private void Awake()
		{
			_keyboardMapper = CreateMapper();
			_mouseMapper = CreateMapper();
			_joystickMapper = CreateMapper();
			BuildProtectedActions();
		}

		private void Start()
		{
			_ = _coActivation;
		}

		private void Update()
		{
			if (_isListening && ReInput.isReady)
			{
				Keyboard keyboard = ReInput.controllers.Keyboard;
				if (keyboard != null && keyboard.GetKeyDown(KeyCode.Escape))
				{
					CancelListening();
				}
			}
		}

		private InputMapper CreateMapper()
		{
			InputMapper inputMapper = new InputMapper();
			inputMapper.options.timeout = listenTimeout;
			inputMapper.options.ignoreMouseXAxis = true;
			inputMapper.options.ignoreMouseYAxis = true;
			inputMapper.options.allowButtonsOnFullAxisAssignment = false;
			inputMapper.options.checkForConflicts = false;
			inputMapper.InputMappedEvent += OnInputMappedHandler;
			inputMapper.StoppedEvent += OnStoppedHandler;
			return inputMapper;
		}

		private void OnDestroy()
		{
			StopAllMappers();
			if (_keyboardMapper != null)
			{
				_keyboardMapper.InputMappedEvent -= OnInputMappedHandler;
				_keyboardMapper.StoppedEvent -= OnStoppedHandler;
				_keyboardMapper.RemoveAllEventListeners();
			}
			if (_mouseMapper != null)
			{
				_mouseMapper.InputMappedEvent -= OnInputMappedHandler;
				_mouseMapper.StoppedEvent -= OnStoppedHandler;
				_mouseMapper.RemoveAllEventListeners();
			}
			if (_joystickMapper != null)
			{
				_joystickMapper.InputMappedEvent -= OnInputMappedHandler;
				_joystickMapper.StoppedEvent -= OnStoppedHandler;
				_joystickMapper.RemoveAllEventListeners();
			}
		}

		public List<InputRemappingActionData> GetActionsForCategory(int mapCategoryId)
		{
			List<InputRemappingActionData> result = new List<InputRemappingActionData>();
			if (!ReInput.isReady)
			{
				return result;
			}
			Player player = ReInput.players.GetPlayer(0);
			if (player == null)
			{
				return result;
			}
			if (inputGlyphService != null && inputGlyphService.ActiveDeviceFamily == InputDeviceFamily.KeyboardMouse)
			{
				CollectActionsFromControllerType(player, ControllerType.Keyboard, mapCategoryId, result);
				CollectActionsFromControllerType(player, ControllerType.Mouse, mapCategoryId, result);
			}
			else
			{
				CollectActionsFromControllerType(player, ControllerType.Joystick, mapCategoryId, result);
			}
			return result;
		}

		private void CollectActionsFromControllerType(Player player, ControllerType controllerType, int mapCategoryId, List<InputRemappingActionData> result)
		{
			int controllerId = GetControllerId(player, controllerType);
			if (controllerId < 0)
			{
				return;
			}
			IList<ControllerMap> maps = player.controllers.maps.GetMaps(controllerType, controllerId);
			if (maps == null)
			{
				return;
			}
			HashSet<string> hashSet = new HashSet<string>();
			foreach (ControllerMap item2 in maps)
			{
				if (item2.categoryId != mapCategoryId)
				{
					continue;
				}
				foreach (ActionElementMap allMap in item2.AllMaps)
				{
					InputAction action = ReInput.mapping.GetAction(allMap.actionId);
					if (action != null && !ExcludedActions.Contains(action.name))
					{
						AxisRange axisRange = ((action.type != InputActionType.Axis) ? AxisRange.Positive : allMap.axisRange);
						string item = $"{allMap.actionId}_{(int)axisRange}";
						if (hashSet.Add(item))
						{
							result.Add(new InputRemappingActionData
							{
								ActionId = allMap.actionId,
								ActionName = action.name,
								ActionDescriptiveName = action.descriptiveName,
								ActionType = action.type,
								AxisRange = axisRange,
								CurrentBindingName = allMap.elementIdentifierName,
								ActionElementMapId = allMap.id,
								MapCategoryId = mapCategoryId
							});
						}
					}
				}
			}
		}

		public Dictionary<int, List<InputRemappingActionData>> GetAllActions()
		{
			Dictionary<int, List<InputRemappingActionData>> dictionary = new Dictionary<int, List<InputRemappingActionData>>();
			foreach (KeyValuePair<int, string> categoryName in CategoryNames)
			{
				List<InputRemappingActionData> actionsForCategory = GetActionsForCategory(categoryName.Key);
				if (actionsForCategory.Count > 0)
				{
					dictionary[categoryName.Key] = actionsForCategory;
				}
			}
			return dictionary;
		}

		public void StartKeyboardMouseRemapping(int actionId, AxisRange axisRange, int actionElementMapToReplaceId)
		{
			if (!ReInput.isReady)
			{
				return;
			}
			Player player = ReInput.players.GetPlayer(0);
			if (player == null)
			{
				return;
			}
			StopAllMappers();
			InputAction action = ReInput.mapping.GetAction(actionId);
			if (action == null)
			{
				return;
			}
			if (_protectedActions.Contains(action.name))
			{
				FireOutcome(RemapOutcome.Protected, actionId, null, ControllerType.Keyboard, 0);
				return;
			}
			_pendingActionId = actionId;
			_pendingAxisRange = axisRange;
			_pendingAemToReplaceId = actionElementMapToReplaceId;
			CaptureRevertSnapshot(player, new ControllerType[2]
			{
				ControllerType.Keyboard,
				ControllerType.Mouse
			});
			ControllerMap controllerMap = null;
			ActionElementMap actionElementMap = null;
			ControllerType controllerType = ControllerType.Keyboard;
			if (actionElementMapToReplaceId >= 0)
			{
				foreach (ControllerMap map in player.controllers.maps.GetMaps(ControllerType.Keyboard, 0))
				{
					actionElementMap = FindActionElementMapInMap(map, actionElementMapToReplaceId);
					if (actionElementMap != null)
					{
						controllerMap = map;
						controllerType = ControllerType.Keyboard;
						break;
					}
				}
				if (actionElementMap == null)
				{
					foreach (ControllerMap map2 in player.controllers.maps.GetMaps(ControllerType.Mouse, 0))
					{
						actionElementMap = FindActionElementMapInMap(map2, actionElementMapToReplaceId);
						if (actionElementMap != null)
						{
							controllerMap = map2;
							controllerType = ControllerType.Mouse;
							break;
						}
					}
				}
			}
			ControllerMap firstMap = GetFirstMap(player, ControllerType.Keyboard);
			ControllerMap firstMap2 = GetFirstMap(player, ControllerType.Mouse);
			_hasKbContext = false;
			_hasMouseContext = false;
			_hasJoystickContext = false;
			if (firstMap != null)
			{
				InputMapper.Context context = new InputMapper.Context
				{
					actionId = actionId,
					actionRange = axisRange,
					controllerMap = firstMap
				};
				if (controllerType == ControllerType.Keyboard && actionElementMap != null && controllerMap == firstMap)
				{
					context.actionElementMapToReplace = actionElementMap;
				}
				_pendingKbContext = context;
				_hasKbContext = true;
			}
			if (firstMap2 != null)
			{
				InputMapper.Context context2 = new InputMapper.Context
				{
					actionId = actionId,
					actionRange = axisRange,
					controllerMap = firstMap2
				};
				if (controllerType == ControllerType.Mouse && actionElementMap != null && controllerMap == firstMap2)
				{
					context2.actionElementMapToReplace = actionElementMap;
				}
				_pendingMouseContext = context2;
				_hasMouseContext = true;
			}
			_isListening = true;
			this.OnListeningStarted?.Invoke();
			_armRoutine = StartCoroutine(ArmKeyboardMouseMappers());
		}

		public void StartJoystickRemapping(int actionId, AxisRange axisRange, int actionElementMapToReplaceId)
		{
			if (!ReInput.isReady)
			{
				return;
			}
			Player player = ReInput.players.GetPlayer(0);
			if (player == null)
			{
				return;
			}
			StopAllMappers();
			InputAction action = ReInput.mapping.GetAction(actionId);
			if (action == null)
			{
				return;
			}
			if (_protectedActions.Contains(action.name))
			{
				FireOutcome(RemapOutcome.Protected, actionId, null, ControllerType.Joystick, 0);
			}
			else
			{
				if (player.controllers.GetLastActiveController(ControllerType.Joystick) == null)
				{
					return;
				}
				ControllerMap controllerMapForCategory = GetControllerMapForCategory(player, ControllerType.Joystick, actionElementMapToReplaceId);
				if (controllerMapForCategory != null)
				{
					_pendingActionId = actionId;
					_pendingAxisRange = axisRange;
					_pendingAemToReplaceId = actionElementMapToReplaceId;
					CaptureRevertSnapshot(player, new ControllerType[1] { ControllerType.Joystick });
					InputMapper.Context context = new InputMapper.Context
					{
						actionId = actionId,
						actionRange = axisRange,
						controllerMap = controllerMapForCategory
					};
					if (actionElementMapToReplaceId >= 0)
					{
						context.actionElementMapToReplace = FindActionElementMapInMap(controllerMapForCategory, actionElementMapToReplaceId);
					}
					_hasKbContext = false;
					_hasMouseContext = false;
					_hasJoystickContext = true;
					_pendingJoystickContext = context;
					_isListening = true;
					this.OnListeningStarted?.Invoke();
					_armRoutine = StartCoroutine(ArmJoystickMapper());
				}
			}
		}

		public void CancelListening()
		{
			bool isListening = _isListening;
			int pendingActionId = _pendingActionId;
			StopAllMappers();
			if (isListening)
			{
				FireOutcome(RemapOutcome.Cancelled, pendingActionId, null, ControllerType.Keyboard, 0);
				this.OnListeningStopped?.Invoke();
			}
		}

		public void SaveBindings()
		{
			if (ReInput.isReady && ReInput.userDataStore != null)
			{
				ReInput.userDataStore.Save();
			}
		}

		public void LoadBindings()
		{
			if (ReInput.isReady && ReInput.userDataStore != null)
			{
				ReInput.userDataStore.Load();
				inputGlyphService?.InvalidateCache();
			}
		}

		public void ResetToDefaults()
		{
			if (ReInput.isReady)
			{
				Player player = ReInput.players.GetPlayer(0);
				if (player != null)
				{
					player.controllers.maps.LoadDefaultMaps(ControllerType.Keyboard);
					player.controllers.maps.LoadDefaultMaps(ControllerType.Mouse);
					player.controllers.maps.LoadDefaultMaps(ControllerType.Joystick);
					ReInput.userDataStore?.Save();
					_canRevert = false;
					_revertSnapshot.Clear();
					inputGlyphService?.InvalidateCache();
				}
			}
		}

		public void RevertLastRemap()
		{
			if (!_canRevert || !ReInput.isReady)
			{
				return;
			}
			Player player = ReInput.players.GetPlayer(0);
			if (player == null)
			{
				return;
			}
			StopAllMappers();
			foreach (MapSnapshot item in _revertSnapshot)
			{
				player.controllers.maps.RemoveMap(item.controllerType, item.controllerId, item.categoryId, item.layoutId);
				player.controllers.maps.AddMapFromXml(item.controllerType, item.controllerId, item.xml);
			}
			inputGlyphService?.InvalidateCache();
			SaveBindings();
			_canRevert = false;
			_revertSnapshot.Clear();
		}

		private void CaptureRevertSnapshot(Player player, ControllerType[] types)
		{
			_revertSnapshot.Clear();
			_canRevert = false;
			foreach (ControllerType controllerType in types)
			{
				int controllerId = GetControllerId(player, controllerType);
				if (controllerId < 0)
				{
					continue;
				}
				IList<ControllerMap> maps = player.controllers.maps.GetMaps(controllerType, controllerId);
				if (maps == null)
				{
					continue;
				}
				foreach (ControllerMap item in maps)
				{
					_revertSnapshot.Add(new MapSnapshot
					{
						controllerType = controllerType,
						controllerId = controllerId,
						categoryId = item.categoryId,
						layoutId = item.layoutId,
						xml = item.ToXmlString()
					});
				}
			}
			_canRevert = _revertSnapshot.Count > 0;
		}

		public string GetCategoryDisplayName(int mapCategoryId)
		{
			if (!CategoryNames.TryGetValue(mapCategoryId, out var value))
			{
				return $"Category {mapCategoryId}";
			}
			return value;
		}

		private void OnInputMappedHandler(InputMapper.InputMappedEventData data)
		{
			StopAllMappers();
			ActionElementMap actionElementMap = data.actionElementMap;
			int actionId = actionElementMap.actionId;
			int id = actionElementMap.id;
			if (IsReservedElement(actionElementMap))
			{
				RevertLastRemap();
				FireOutcome(RemapOutcome.Cancelled, actionId, null, actionElementMap.controllerMap.controllerType, actionElementMap.controllerMap.categoryId);
				this.OnListeningStopped?.Invoke();
				return;
			}
			Player player = ReInput.players.GetPlayer(0);
			if (player != null && _coActivation != null)
			{
				(int, int)? tuple = FindBlockingConflict(player, actionElementMap, actionId);
				if (tuple.HasValue)
				{
					int item = tuple.Value.Item1;
					int item2 = tuple.Value.Item2;
					InputAction action = ReInput.mapping.GetAction(item);
					RemapOutcome outcome = ((!IsActionProtected(item)) ? RemapOutcome.Conflict : RemapOutcome.Protected);
					RevertLastRemap();
					FireOutcome(outcome, actionId, actionElementMap.elementIdentifierName, actionElementMap.controllerMap.controllerType, actionElementMap.controllerMap.categoryId, action?.name, GetCategoryDisplayName(item2));
					this.OnListeningStopped?.Invoke();
					return;
				}
			}
			if (player != null)
			{
				ControllerType[] array = ((actionElementMap.controllerMap.controllerType != ControllerType.Joystick) ? new ControllerType[2]
				{
					ControllerType.Keyboard,
					ControllerType.Mouse
				} : new ControllerType[1] { ControllerType.Joystick });
				ControllerType[] array2 = array;
				foreach (ControllerType controllerType in array2)
				{
					int controllerId = GetControllerId(player, controllerType);
					if (controllerId < 0)
					{
						continue;
					}
					foreach (ControllerMap map in player.controllers.maps.GetMaps(controllerType, controllerId))
					{
						List<int> list = new List<int>();
						foreach (ActionElementMap allMap in map.AllMaps)
						{
							if (allMap.actionId == actionId && allMap.id != id && (allMap.axisRange == _pendingAxisRange || _pendingAxisRange == AxisRange.Full))
							{
								list.Add(allMap.id);
							}
						}
						foreach (int item3 in list)
						{
							map.DeleteElementMap(item3);
						}
					}
				}
				if (_pendingAemToReplaceId >= 0)
				{
					foreach (ControllerMap allMap2 in player.controllers.maps.GetAllMaps())
					{
						if (allMap2 != actionElementMap.controllerMap && FindActionElementMapInMap(allMap2, _pendingAemToReplaceId) != null)
						{
							allMap2.DeleteElementMap(_pendingAemToReplaceId);
						}
					}
				}
			}
			inputGlyphService?.InvalidateCache();
			InputAction action2 = ReInput.mapping.GetAction(actionId);
			InputRemappingResult obj = new InputRemappingResult
			{
				ActionId = actionId,
				ActionName = (action2?.name ?? "Unknown"),
				NewElementName = actionElementMap.elementIdentifierName,
				ControllerType = actionElementMap.controllerMap.controllerType,
				MapCategoryId = actionElementMap.controllerMap.categoryId,
				Outcome = RemapOutcome.Applied
			};
			this.OnInputRemapped?.Invoke(obj);
			this.OnRemapOutcome?.Invoke(obj);
			SaveBindings();
		}

		private void OnStoppedHandler(InputMapper.StoppedEventData data)
		{
			if (_isListening)
			{
				int pendingActionId = _pendingActionId;
				_isListening = false;
				FireOutcome(RemapOutcome.TimedOut, pendingActionId, null, ControllerType.Keyboard, 0);
				this.OnListeningStopped?.Invoke();
			}
		}

		private void StopAllMappers()
		{
			_isListening = false;
			if (_armRoutine != null)
			{
				StopCoroutine(_armRoutine);
				_armRoutine = null;
			}
			_keyboardMapper?.Stop();
			_mouseMapper?.Stop();
			_joystickMapper?.Stop();
		}

		private static ControllerMap GetFirstMap(Player player, ControllerType controllerType)
		{
			int controllerId = GetControllerId(player, controllerType);
			if (controllerId < 0)
			{
				return null;
			}
			using (IEnumerator<ControllerMap> enumerator = player.controllers.maps.GetMaps(controllerType, controllerId).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return null;
		}

		private ControllerMap GetControllerMapForCategory(Player player, ControllerType controllerType, int actionElementMapToReplaceId)
		{
			int controllerId = GetControllerId(player, controllerType);
			if (controllerId < 0)
			{
				return null;
			}
			IList<ControllerMap> maps = player.controllers.maps.GetMaps(controllerType, controllerId);
			if (maps == null)
			{
				return null;
			}
			if (actionElementMapToReplaceId >= 0)
			{
				foreach (ControllerMap item in maps)
				{
					if (FindActionElementMapInMap(item, actionElementMapToReplaceId) != null)
					{
						return item;
					}
				}
			}
			using (IEnumerator<ControllerMap> enumerator = maps.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return null;
		}

		private ActionElementMap FindActionElementMap(Player player, int aemId)
		{
			foreach (ControllerMap allMap in player.controllers.maps.GetAllMaps())
			{
				ActionElementMap actionElementMap = FindActionElementMapInMap(allMap, aemId);
				if (actionElementMap != null)
				{
					return actionElementMap;
				}
			}
			return null;
		}

		private static int GetControllerId(Player player, ControllerType controllerType)
		{
			if (controllerType == ControllerType.Keyboard || controllerType == ControllerType.Mouse)
			{
				return 0;
			}
			Controller lastActiveController = player.controllers.GetLastActiveController(ControllerType.Joystick);
			if (lastActiveController != null)
			{
				return lastActiveController.id;
			}
			using (IEnumerator<Joystick> enumerator = player.controllers.Joysticks.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current.id;
				}
			}
			return -1;
		}

		private ActionElementMap FindActionElementMapInMap(ControllerMap map, int aemId)
		{
			foreach (ActionElementMap allMap in map.AllMaps)
			{
				if (allMap.id == aemId)
				{
					return allMap;
				}
			}
			return null;
		}

		private void BuildProtectedActions()
		{
			_protectedActions.Clear();
			string[] array = ((protectedActionNames != null && protectedActionNames.Length != 0) ? protectedActionNames : DefaultProtectedActions);
			foreach (string text in array)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					_protectedActions.Add(text);
				}
			}
		}

		public bool IsActionProtected(int actionId)
		{
			if (!ReInput.isReady)
			{
				return false;
			}
			InputAction action = ReInput.mapping.GetAction(actionId);
			if (action != null)
			{
				return _protectedActions.Contains(action.name);
			}
			return false;
		}

		private static bool IsReservedElement(ActionElementMap aem)
		{
			if (aem != null && aem.controllerMap != null && aem.controllerMap.controllerType == ControllerType.Keyboard)
			{
				return aem.keyboardKeyCode == KeyboardKeyCode.Escape;
			}
			return false;
		}

		private (int actionId, int categoryId)? FindBlockingConflict(Player player, ActionElementMap newAem, int targetActionId)
		{
			int categoryId = newAem.controllerMap.categoryId;
			InputAction action = ReInput.mapping.GetAction(targetActionId);
			foreach (ControllerMap allMap in player.controllers.maps.GetAllMaps())
			{
				foreach (ActionElementMap allMap2 in allMap.AllMaps)
				{
					if (allMap2.actionId == targetActionId || !SameElement(allMap2, newAem))
					{
						continue;
					}
					int categoryId2 = allMap.categoryId;
					if (_coActivation.CanCoActivate(categoryId, categoryId2))
					{
						InputAction action2 = ReInput.mapping.GetAction(allMap2.actionId);
						if (action == null || action2 == null || !_coActivation.IsSanctionedSharedDefault(action.name, action2.name))
						{
							return (allMap2.actionId, categoryId2);
						}
					}
				}
			}
			return null;
		}

		private static bool SameElement(ActionElementMap a, ActionElementMap b)
		{
			if (a.controllerMap.controllerType == b.controllerMap.controllerType && a.elementType == b.elementType)
			{
				return a.elementIdentifierId == b.elementIdentifierId;
			}
			return false;
		}

		private void FireOutcome(RemapOutcome outcome, int actionId, string newElementName, ControllerType controllerType, int mapCategoryId, string conflictingActionName = null, string conflictingCategoryName = null)
		{
			InputAction inputAction = (ReInput.isReady ? ReInput.mapping.GetAction(actionId) : null);
			this.OnRemapOutcome?.Invoke(new InputRemappingResult
			{
				ActionId = actionId,
				ActionName = (inputAction?.name ?? "Unknown"),
				NewElementName = newElementName,
				ControllerType = controllerType,
				MapCategoryId = mapCategoryId,
				Outcome = outcome,
				ConflictingActionName = conflictingActionName,
				ConflictingCategoryName = conflictingCategoryName
			});
		}

		private IEnumerator ArmKeyboardMouseMappers()
		{
			yield return ActivationDelay();
			if (!_isListening)
			{
				_armRoutine = null;
				yield break;
			}
			if (_hasKbContext)
			{
				_keyboardMapper.Start(_pendingKbContext);
			}
			if (_hasMouseContext)
			{
				_mouseMapper.Start(_pendingMouseContext);
			}
			_armRoutine = null;
		}

		private IEnumerator ArmJoystickMapper()
		{
			yield return ActivationDelay();
			if (!_isListening)
			{
				_armRoutine = null;
				yield break;
			}
			if (_hasJoystickContext)
			{
				_joystickMapper.Start(_pendingJoystickContext);
			}
			_armRoutine = null;
		}

		private IEnumerator ActivationDelay()
		{
			yield return null;
			float minDelay = Mathf.Max(activationDebounce, 0.2f);
			float elapsed = 0f;
			while (elapsed < minDelay || IsActivationInputHeld())
			{
				elapsed += Time.unscaledDeltaTime;
				if (!(elapsed > 1.5f))
				{
					yield return null;
					continue;
				}
				break;
			}
		}

		private static bool IsActivationInputHeld()
		{
			if (!ReInput.isReady)
			{
				return false;
			}
			Mouse mouse = ReInput.controllers.Mouse;
			if (mouse != null && (mouse.GetButton(0) || mouse.GetButton(1) || mouse.GetButton(2)))
			{
				return true;
			}
			Keyboard keyboard = ReInput.controllers.Keyboard;
			if (keyboard != null && (keyboard.GetKey(KeyCode.Escape) || keyboard.GetKey(KeyCode.Return) || keyboard.GetKey(KeyCode.KeypadEnter)))
			{
				return true;
			}
			return false;
		}
	}
}
