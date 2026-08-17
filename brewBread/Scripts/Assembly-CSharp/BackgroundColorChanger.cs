using PenguinPackage.Utilities;
using UnityEngine;

public class BackgroundColorChanger : MonoBehaviour
{
	[SerializeField]
	private Color _initialColor;

	[SerializeField]
	private Color _finalColor;

	[SerializeField]
	private float _initialHeight;

	[SerializeField]
	private float _finalHeight;

	[SerializeField]
	private Transform _actualHeight;

	private SpriteRenderer _spriteRenderer;

	private void Start()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		_spriteRenderer.color = Color.Lerp(_initialColor, _finalColor, Mathp.Remap(_actualHeight.position.y, _initialHeight, _finalHeight));
	}
}
