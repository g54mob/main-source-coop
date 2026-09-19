using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace solidocean
{
	public class CanvasManager : Singleton<CanvasManager>
	{
		[Tooltip("You need to add all of your states in here.")]
		[Header("List of States")]
		[SerializeField]
		private List<GameObject> States = new List<GameObject>();

		[Tooltip("Assign starting state in here.")]
		public CanvasName FirstCanvas;

		[HideInInspector]
		public Animator CanvasAnimator;

		private List<CanvasController> canvasControllerList;

		[HideInInspector]
		public CanvasController ActiveCanvas;

		[HideInInspector]
		public CanvasController PreviousCanvas;

		private InspectManager inspectManager;

		protected override void Awake()
		{
			foreach (GameObject state in States)
			{
				state.SetActive(value: true);
			}
			base.Awake();
			inspectManager = Object.FindFirstObjectByType<InspectManager>();
			canvasControllerList = GetComponentsInChildren<CanvasController>().ToList();
			canvasControllerList.ForEach(delegate(CanvasController x)
			{
				x.gameObject.SetActive(value: false);
			});
			StartCoroutine(PlayNextCanvasAnimation(FirstCanvas));
		}

		private void OnEnable()
		{
			StartCoroutine(PlayNextCanvasAnimation(FirstCanvas));
		}

		public void GoToNextCanvas(CanvasName _name)
		{
			if (ActiveCanvas != null)
			{
				ActiveCanvas.gameObject.SetActive(value: false);
			}
			inspectManager.DeactiveInspector();
			CanvasController canvasController = canvasControllerList.Find((CanvasController x) => x.canvasName == _name);
			if (canvasController != null)
			{
				PreviousCanvas = ActiveCanvas;
				canvasController.gameObject.SetActive(value: true);
				ActiveCanvas = canvasController;
				canvasController.GetComponent<CanvasController>().StartSelectable.Select();
			}
			else
			{
				Debug.LogWarning("The next canvas was not found!");
			}
		}

		public void GoToPreviousCanvas()
		{
			if (ActiveCanvas != null)
			{
				ActiveCanvas.gameObject.SetActive(value: false);
			}
			inspectManager.DeactiveInspector();
			CanvasController canvasController = canvasControllerList.Find((CanvasController x) => x.canvasName == ActiveCanvas.previousCanvas);
			if (ActiveCanvas.canvasName.canGoPreviousCanvas)
			{
				PreviousCanvas = ActiveCanvas;
				canvasController.gameObject.SetActive(value: true);
				ActiveCanvas = canvasController;
				canvasController.GetComponent<CanvasController>().StartSelectable.Select();
			}
		}

		public IEnumerator PlayNextCanvasAnimation(CanvasName _type)
		{
			CanvasAnimator.Play("out_canvas");
			yield return new WaitForSeconds(0.1f);
			GoToNextCanvas(_type);
			CanvasAnimator.Play("in_canvas");
		}

		public IEnumerator PlayPreviousCanvasAnimation()
		{
			CanvasAnimator.Play("out_canvas");
			yield return new WaitForSeconds(0.1f);
			GoToPreviousCanvas();
			CanvasAnimator.Play("in_canvas");
		}

		public void LeaveGame()
		{
			Application.Quit();
			Debug.Log("When you build, your game will close when submit this button.");
		}
	}
}
