using System.Collections.Generic;
using PenguinPackage.Utilities;
using UnityEngine;

[CreateAssetMenu(fileName = "PenguinAnchor", menuName = "Actions/AnchorDirectional", order = 1)]
public class AnchorDirectionalAction : AnchorAction
{
	[SerializeField]
	private List<Sprite> _anchorDirections;

	private bool _spriteFlip;

	public override void Enter()
	{
		base.Enter();
		_spriteFlip = Controller.Sprite.flipX;
		_controllerTransform.localScale = Vector2.one;
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (Controller.Companion.CurrentState.Type == PenguinActions.Swing || Controller.Companion.CurrentState.Type == PenguinActions.Grabbed)
		{
			SetSprite();
		}
	}

	private void SetSprite()
	{
		Controller.AnimatorEnabled(value: false);
		float num = Mathp.AngleBetween2Points(_controllerTransform.position, Controller.CompanionTransform.position, 90f);
		if (num < 0f)
		{
			Controller.Sprite.flipX = true;
		}
		else
		{
			Controller.Sprite.flipX = false;
		}
		int num2 = (int)Mathf.Abs(num) / (180 / _anchorDirections.Count);
		if (num2 < _anchorDirections.Count - 1)
		{
			Controller.Sprite.sprite = _anchorDirections[num2];
		}
	}

	public override void Exit()
	{
		Controller.AnimatorEnabled(value: true);
		base.Exit();
		bool spriteFlip = (Controller.Sprite.flipX = _spriteFlip);
		_spriteFlip = spriteFlip;
	}
}
