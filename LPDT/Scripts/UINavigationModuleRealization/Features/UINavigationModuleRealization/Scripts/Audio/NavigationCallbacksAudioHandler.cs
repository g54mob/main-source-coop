using FMODUnity;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UINavigationModuleRealization.Scripts.Audio
{
	public class NavigationCallbacksAudioHandler : MonoBehaviour
	{
		[HideInInspector]
		[SerializeField]
		private MonoBehaviour _selectableSource;

		[HideInInspector]
		[SerializeField]
		private MonoBehaviour _submittableSource;

		[HideInInspector]
		[SerializeField]
		private MonoBehaviour _mouseSelectableSource;

		[HideInInspector]
		[SerializeField]
		private MonoBehaviour _dropdownSource;

		[HideInInspector]
		[SerializeField]
		private bool _hasSelectable;

		[HideInInspector]
		[SerializeField]
		private bool _hasSubmittable;

		[HideInInspector]
		[SerializeField]
		private bool _hasMouseSelectable;

		[HideInInspector]
		[SerializeField]
		private bool _hasDropdown;

		[SerializeField]
		private EventReference _onSelect;

		[SerializeField]
		private EventReference _onDeSelect;

		[SerializeField]
		private EventReference _onHighlight;

		[SerializeField]
		private EventReference _onPressed;

		[SerializeField]
		private EventReference _onUnActive;

		[SerializeField]
		private EventReference _onActive;

		[SerializeField]
		private EventReference _onSubmit;

		[SerializeField]
		private EventReference _onMouseSelect;

		[SerializeField]
		private EventReference _onMouseDeSelect;

		[SerializeField]
		private EventReference _onDropdownSelect;

		[SerializeField]
		private EventReference _onDropdownOpen;

		[SerializeField]
		private EventReference _onDropdownItemChange;

		[SerializeField]
		private EventReference _onDropdownItemHover;

		private ISelectableWithNavigationCallbacks _selectable;

		private ISubmittableWithNavigationCallbacks _submittable;

		private IMouseSelectableWithNavigationCallbacks _mouseSelectable;

		private IDropdownSelectableWithNavigationCallbacks _dropdown;

		public bool HasSelectable => _hasSelectable;

		public bool HasSubmittable => _hasSubmittable;

		public bool HasMouseSelectable => _hasMouseSelectable;

		public bool HasDropdown => _hasDropdown;

		public MonoBehaviour SelectableSource => _selectableSource;

		public MonoBehaviour SubmittableSource => _submittableSource;

		public MonoBehaviour MouseSelectableSource => _mouseSelectableSource;

		public MonoBehaviour DropdownSource => _dropdownSource;

		public void RefreshSources()
		{
			_selectableSource = null;
			_submittableSource = null;
			_mouseSelectableSource = null;
			_dropdownSource = null;
			_hasSelectable = false;
			_hasSubmittable = false;
			_hasMouseSelectable = false;
			_hasDropdown = false;
			MonoBehaviour[] components = GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour monoBehaviour in components)
			{
				if (!(monoBehaviour == null) && !(monoBehaviour == this))
				{
					if (!_hasSelectable && monoBehaviour is ISelectableWithNavigationCallbacks)
					{
						_selectableSource = monoBehaviour;
						_hasSelectable = true;
					}
					if (!_hasSubmittable && monoBehaviour is ISubmittableWithNavigationCallbacks)
					{
						_submittableSource = monoBehaviour;
						_hasSubmittable = true;
					}
					if (!_hasMouseSelectable && monoBehaviour is IMouseSelectableWithNavigationCallbacks)
					{
						_mouseSelectableSource = monoBehaviour;
						_hasMouseSelectable = true;
					}
					if (!_hasDropdown && monoBehaviour is IDropdownSelectableWithNavigationCallbacks)
					{
						_dropdownSource = monoBehaviour;
						_hasDropdown = true;
					}
				}
			}
		}

		private void Awake()
		{
			RefreshSources();
			CacheInterfaces();
			BindEvents();
		}

		private void OnDestroy()
		{
			UnbindEvents();
		}

		private void CacheInterfaces()
		{
			_selectable = _selectableSource as ISelectableWithNavigationCallbacks;
			_submittable = _submittableSource as ISubmittableWithNavigationCallbacks;
			_mouseSelectable = _mouseSelectableSource as IMouseSelectableWithNavigationCallbacks;
			_dropdown = _dropdownSource as IDropdownSelectableWithNavigationCallbacks;
		}

		private void BindEvents()
		{
			if (_selectable != null)
			{
				_selectable.OnSelectEvent += OnSelectableSelect;
				_selectable.OnDeSelectEvent += OnSelectableDeSelect;
				_selectable.OnHighlightEvent += OnSelectableHighlight;
				_selectable.OnPressedEvent += OnSelectablePressed;
				_selectable.OnUnActiveEvent += OnSelectableUnActive;
				_selectable.OnActiveEvent += OnSelectableActive;
			}
			if (_submittable != null)
			{
				_submittable.OnSubmitEvent += OnSubmittableSubmit;
			}
			if (_mouseSelectable != null)
			{
				_mouseSelectable.OnMouseSelectEvent += OnMouseSelect;
				_mouseSelectable.OnMouseDeSelectEvent += OnMouseDeSelect;
			}
			if (_dropdown != null)
			{
				_dropdown.OnSelectEvent += OnDropdownSelect;
				_dropdown.OnOpenEvent += OnDropdownOpen;
				_dropdown.OnItemChangeEvent += OnDropdownItemChange;
				_dropdown.OnItemHoverEvent += OnDropdownItemHover;
			}
		}

		private void UnbindEvents()
		{
			if (_selectable != null)
			{
				_selectable.OnSelectEvent -= OnSelectableSelect;
				_selectable.OnDeSelectEvent -= OnSelectableDeSelect;
				_selectable.OnHighlightEvent -= OnSelectableHighlight;
				_selectable.OnPressedEvent -= OnSelectablePressed;
				_selectable.OnUnActiveEvent -= OnSelectableUnActive;
				_selectable.OnActiveEvent -= OnSelectableActive;
			}
			if (_submittable != null)
			{
				_submittable.OnSubmitEvent -= OnSubmittableSubmit;
			}
			if (_mouseSelectable != null)
			{
				_mouseSelectable.OnMouseSelectEvent -= OnMouseSelect;
				_mouseSelectable.OnMouseDeSelectEvent -= OnMouseDeSelect;
			}
			if (_dropdown != null)
			{
				_dropdown.OnSelectEvent -= OnDropdownSelect;
				_dropdown.OnOpenEvent -= OnDropdownOpen;
				_dropdown.OnItemChangeEvent -= OnDropdownItemChange;
				_dropdown.OnItemHoverEvent -= OnDropdownItemHover;
			}
		}

		private void OnSelectableSelect(ISelectableWithNavigationCallbacks _)
		{
			PlaySound(_onSelect, _selectable?.GetSelectable());
		}

		private void OnSelectableDeSelect(ISelectableWithNavigationCallbacks _)
		{
			PlaySound(_onDeSelect, _selectable?.GetSelectable());
		}

		private void OnSelectableHighlight(ISelectableWithNavigationCallbacks _)
		{
			PlaySound(_onHighlight, _selectable?.GetSelectable());
		}

		private void OnSelectablePressed(ISelectableWithNavigationCallbacks _)
		{
			PlaySound(_onPressed, _selectable?.GetSelectable());
		}

		private void OnSelectableUnActive(ISelectableWithNavigationCallbacks _)
		{
			PlaySound(_onUnActive, _selectable?.GetSelectable());
		}

		private void OnSelectableActive(ISelectableWithNavigationCallbacks _)
		{
			PlaySound(_onActive, _selectable?.GetSelectable());
		}

		private void OnSubmittableSubmit(ISubmittableWithNavigationCallbacks _)
		{
			PlaySound(_onSubmit, _submittable?.GetSelectable());
		}

		private void OnMouseSelect(IMouseSelectableWithNavigationCallbacks _)
		{
			PlaySound(_onMouseSelect, _mouseSelectable?.GetSelectable());
		}

		private void OnMouseDeSelect(IMouseSelectableWithNavigationCallbacks _)
		{
			PlaySound(_onMouseDeSelect, _mouseSelectable?.GetSelectable());
		}

		private void OnDropdownSelect()
		{
			PlaySound(_onDropdownSelect, _dropdownSource as Selectable);
		}

		private void OnDropdownOpen()
		{
			PlaySound(_onDropdownOpen, _dropdownSource as Selectable);
		}

		private void OnDropdownItemChange()
		{
			PlaySound(_onDropdownItemChange, _dropdownSource as Selectable);
		}

		private void OnDropdownItemHover()
		{
			PlaySound(_onDropdownItemHover, _dropdownSource as Selectable);
		}

		private void PlaySound(EventReference uiEvent, Selectable selectable)
		{
			if (Application.isPlaying && !(selectable == null) && selectable.IsActive() && !uiEvent.IsNull)
			{
				RuntimeManager.PlayOneShot(uiEvent);
			}
		}
	}
}
