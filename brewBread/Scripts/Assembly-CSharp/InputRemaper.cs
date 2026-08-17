using Rewired;
using UnityEngine;
using UnityEngine.UI;

public class InputRemaper : MonoBehaviour
{
	[SerializeField]
	private ControllerType _controllerType;

	[SerializeField]
	private int _actionID;

	[SerializeField]
	private Sprite _sprite;

	[SerializeField]
	private Image _image;
}
