using UnityEngine;
using UnityEngine.U2D;

[ExecuteInEditMode]
public class RailsPlacer : MonoBehaviour
{
	[SerializeField]
	private SpriteShapeController _spriteController;

	[SerializeField]
	private MovablePlatform _movablePlatform;

	[SerializeField]
	private Vector3 _offset;

	private void Update()
	{
		SetSpline();
	}

	private void SetSpline()
	{
		Spline spline = _spriteController.spline;
		spline.Clear();
		for (int i = 0; i < _movablePlatform.points.Length; i++)
		{
			Vector3 point = _movablePlatform.points[i].Point.position - base.transform.position + _offset;
			spline.InsertPointAt(i, point);
			if (i > 0)
			{
				spline.SetSpriteIndex(i - 1, GetRailDirection(_movablePlatform.points[i].Point.position, _movablePlatform.points[i - 1].Point.position));
			}
		}
		if (_movablePlatform.closeCircuit)
		{
			_ = _movablePlatform.points[0].Point.position - base.transform.position + _offset + new Vector3(0.1f, 0f, 0f);
			spline.isOpenEnded = false;
			spline.SetSpriteIndex(_movablePlatform.points.Length - 1, GetRailDirection(_movablePlatform.points[0].Point.position, _movablePlatform.points[_movablePlatform.points.Length - 1].Point.position));
		}
		else
		{
			spline.isOpenEnded = true;
		}
		_spriteController.UpdateSpriteShapeParameters();
	}

	private int GetRailDirection(Vector3 currentPoint, Vector3 previousPoint)
	{
		float num = Mathf.Atan2(currentPoint.y - previousPoint.y, currentPoint.x - previousPoint.x) * 57.29578f;
		if ((num > 60f && num < 120f) || (num < -60f && num > -120f))
		{
			return 1;
		}
		if ((num >= 120f && num <= 180f) || (num <= -120f && num >= -180f))
		{
			return 2;
		}
		return 0;
	}
}
