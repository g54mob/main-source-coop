using UnityEngine;

public class PainterDrawingShowe : MonoBehaviour
{
	[SerializeField]
	private GameObject _drawing;

	private Dialoger _dialoger;

	private Animator _animator;

	private bool _waitingForDialogToEnd;

	private void Start()
	{
		_animator = GetComponent<Animator>();
		_dialoger = GetComponent<Dialoger>();
		_waitingForDialogToEnd = false;
	}

	private void Update()
	{
		if (_dialoger.CurrentState.HasFlag(Dialoger.DialogState.InDialog))
		{
			_waitingForDialogToEnd = true;
		}
		if (_waitingForDialogToEnd && !_dialoger.CurrentState.HasFlag(Dialoger.DialogState.InDialog))
		{
			_waitingForDialogToEnd = false;
			StartPaintAnimation();
		}
	}

	public void ShowDrawing()
	{
		Vector3 cameraPosition = StaticInstance<CameraController>.Instance.GetCameraPosition();
		cameraPosition.z = 0f;
		Object.Instantiate(_drawing, cameraPosition, Quaternion.identity);
	}

	public void StartPaintAnimation()
	{
		_animator.Play("Painter_Paint");
	}
}
