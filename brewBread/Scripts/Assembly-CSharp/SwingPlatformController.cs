using System.Collections;
using PenguinPackage.Utilities;
using UnityEngine;

[ExecuteAlways]
public class SwingPlatformController : MonoBehaviour
{
	[SerializeField]
	private float _displacement;

	[SerializeField]
	private float _radius;

	[SerializeField]
	private float _swingTime;

	[Space(10f)]
	[SerializeField]
	private Transform[] _ropeStartPoints;

	[SerializeField]
	private Transform[] _ropeEndPoints;

	private Vector2 _originalPos;

	private Vector2 _centerPos;

	private Vector2 _targetPos;

	private void Awake()
	{
		_originalPos = base.transform.position;
		_centerPos = _originalPos + new Vector2(0f, _radius);
		float num = Mathf.Sqrt(_radius * _radius - _displacement * _displacement);
		_targetPos = new Vector2(0f - _displacement + _centerPos.x, 0f - num + _centerPos.y);
	}

	private void WindBlow(bool state)
	{
		if (state)
		{
			StartCoroutine(MovePlatform());
		}
		else
		{
			StartCoroutine(ReturnPlatform());
		}
	}

	private IEnumerator MovePlatform()
	{
		float t = 0f;
		do
		{
			t += Time.deltaTime;
			base.transform.position = Vector3.Slerp(_originalPos, _targetPos, Mathp.Remap(t, 0f, _swingTime));
			yield return new WaitForEndOfFrame();
		}
		while (!(t > 1f));
		base.transform.position = Vector3.Slerp(_originalPos, _targetPos, 1f);
	}

	private IEnumerator ReturnPlatform()
	{
		float t = 0f;
		do
		{
			t += Time.deltaTime;
			base.transform.position = Vector3.Slerp(_targetPos, _originalPos, Mathp.Remap(t, 0f, _swingTime));
			yield return new WaitForEndOfFrame();
		}
		while (!(t > 1f));
		base.transform.position = Vector3.Slerp(_targetPos, _originalPos, 1f);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(_centerPos, _radius);
		Gizmos.color = Color.red;
		_centerPos = (Vector2)base.transform.position + new Vector2(0f, _radius);
		float num = Mathf.Sqrt(_radius * _radius - _displacement * _displacement);
		Gizmos.DrawLine(new Vector2(0f - _displacement + _centerPos.x, 0f - num + _centerPos.y), _centerPos);
		RaycastHit2D[] array = Physics2D.RaycastAll(base.transform.position, Vector2.up, _radius);
		Gizmos.color = Color.red;
		Vector2 vector = (Vector2)base.transform.position + Vector2.up * _radius;
		RaycastHit2D[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			RaycastHit2D raycastHit2D = array2[i];
			if (raycastHit2D.transform.gameObject.layer == 10)
			{
				Gizmos.color = Color.green;
				vector = raycastHit2D.point;
				for (int j = 0; j < _ropeEndPoints.Length; j++)
				{
					_ropeEndPoints[j].position = new Vector2(_ropeEndPoints[j].position.x, vector.y);
				}
			}
		}
		Gizmos.DrawLine(base.transform.position, vector);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == 16)
		{
			collision.GetComponent<WindController>().WindBlow.AddListener(WindBlow);
		}
	}
}
