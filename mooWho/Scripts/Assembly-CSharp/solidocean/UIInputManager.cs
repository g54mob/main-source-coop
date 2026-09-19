using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace solidocean
{
	public class UIInputManager : MonoBehaviour
	{
		private LastUIInputActions lastuiInputActions;

		private CanvasManager canvasManager;

		[Tooltip("Assign whatever button you want it to start with selected in here.")]
		public Button FirstSelectedButton;

		[HideInInspector]
		public GameObject SelectedButton;

		private void OnEnable()
		{
			lastuiInputActions.Enable();
		}

		private void OnDisable()
		{
			lastuiInputActions.Disable();
		}

		public void Awake()
		{
			lastuiInputActions = new LastUIInputActions();
			canvasManager = Singleton<CanvasManager>.GetInstance();
			SelectedButton = FirstSelectedButton.gameObject;
		}

		private void Start()
		{
			FirstSelectedButton.Select();
			lastuiInputActions.LastUI.Cancel.performed += delegate
			{
				ifCancelPressed();
			};
			lastuiInputActions.LastUI.Navigate.performed += delegate(InputAction.CallbackContext ctx)
			{
				changeSliderValue(ctx.ReadValue<Vector2>());
			};
			lastuiInputActions.LastUI.Submit.performed += delegate
			{
				SubmitPerformed();
			};
		}

		private void Update()
		{
			SelectedButton = EventSystem.current.currentSelectedGameObject;
		}

		private void ifCancelPressed()
		{
			if (canvasManager.ActiveCanvas.canvasName.canGoPreviousCanvas)
			{
				StartCoroutine(canvasManager.PlayPreviousCanvasAnimation());
			}
		}

		private void changeSliderValue(Vector2 direction)
		{
			if (SelectedButton.TryGetComponent<ItemController>(out var _) && SelectedButton.GetComponent<ItemController>().itemType == ItemController.itemTypes.HorizontalSelector)
			{
				if (direction.x == -1f)
				{
					SelectedButton.transform.GetChild(0).GetChild(0).GetComponent<Button>()
						.onClick.Invoke();
				}
				if (direction.x == 1f)
				{
					SelectedButton.transform.GetChild(0).GetChild(1).GetComponent<Button>()
						.onClick.Invoke();
				}
			}
		}

		private void SubmitPerformed()
		{
			if (SelectedButton.TryGetComponent<ItemController>(out var _) && SelectedButton.GetComponent<ItemController>().itemType == ItemController.itemTypes.Toggle)
			{
				if (SelectedButton.GetComponentInChildren<Toggle>().isOn)
				{
					SelectedButton.GetComponentInChildren<Toggle>().isOn = false;
				}
				else
				{
					SelectedButton.GetComponentInChildren<Toggle>().isOn = true;
				}
			}
		}

		public void SelectObject(Selectable select)
		{
			select.Select();
		}
	}
}
