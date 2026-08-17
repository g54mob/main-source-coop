using System;
using UnityEngine;

public class ReSkining : MonoBehaviour
{
	[SerializeField]
	private string _skinName;

	private SpriteRenderer _sprite;

	private void Start()
	{
		_sprite = GetComponent<SpriteRenderer>();
	}

	private void LateUpdate()
	{
		Sprite[] array = Resources.LoadAll<Sprite>("Skins/" + _skinName);
		string BreadName = _sprite.sprite.name.Replace("Bread", "Fred");
		_sprite.sprite = Array.Find(array, (Sprite item) => item.name == BreadName);
	}
}
