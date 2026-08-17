using UnityEngine;

[CreateAssetMenu(fileName = "PenguinAnchor", menuName = "Actions/Anchor", order = 1)]
public class AnchorAction : PenguinAction
{
	public override void Enter()
	{
		base.Enter();
		RaycastHit2D raycastHit2D = Physics2D.Raycast(_controllerTransform.position, Vector2.down, 1f, Controller.GroundLayer);
		if ((bool)raycastHit2D)
		{
			_controllerTransform.position = new Vector2(_controllerTransform.position.x, raycastHit2D.point.y + 0.576237f);
		}
		else
		{
			BoxCollider2D boxCollider2D = Controller.BoxCollider2D;
			Vector2 vector = new Vector2(0f - (boxCollider2D.size.x / 2f - boxCollider2D.offset.x), 0f - (boxCollider2D.size.y / 2f - boxCollider2D.offset.y));
			Vector2 vector2 = new Vector2(boxCollider2D.size.x / 2f + boxCollider2D.offset.x, 0f - (boxCollider2D.size.y / 2f - boxCollider2D.offset.y));
			vector = _controllerTransform.TransformPoint(vector);
			vector2 = _controllerTransform.TransformPoint(vector2);
			RaycastHit2D raycastHit2D2 = Physics2D.Raycast(vector, Vector2.down, 1f, Controller.GroundLayer);
			RaycastHit2D raycastHit2D3 = Physics2D.Raycast(vector2, Vector2.down, 1f, Controller.GroundLayer);
			if ((bool)raycastHit2D2)
			{
				_controllerTransform.position = new Vector2(_controllerTransform.position.x, raycastHit2D2.point.y + 0.576237f);
			}
			else if ((bool)raycastHit2D3)
			{
				_controllerTransform.position = new Vector2(_controllerTransform.position.x, raycastHit2D3.point.y + 0.576237f);
			}
		}
		Controller.SetPlayerMobility(value: false);
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
	}

	public override void Exit()
	{
		base.Exit();
		Controller.SetPlayerMobility(value: true);
	}
}
