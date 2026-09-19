using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy
{
	public class AnchorRopeLineView : MonoBehaviour
	{
		[SerializeField]
		private LineRenderer _lineRenderer;

		[SerializeField]
		private Transform _from;

		[SerializeField]
		private Transform _to;

		private AnchorEnemyContext _context;

		private bool _active;

		[Inject]
		private void InjectDependencies(AnchorEnemyContext context)
		{
			_context = context;
		}

		private void Awake()
		{
			if (_lineRenderer != null)
			{
				_lineRenderer.useWorldSpace = true;
				_lineRenderer.positionCount = 2;
			}
			SetActive(active: false);
		}

		private void LateUpdate()
		{
			if (!(_context == null) && !(_lineRenderer == null) && !(_from == null) && !(_to == null) && !(_context.Object == null) && _context.Object.IsValid)
			{
				bool isAnchorThrown = _context.IsAnchorThrown;
				if (isAnchorThrown != _active)
				{
					SetActive(isAnchorThrown);
				}
				if (isAnchorThrown)
				{
					_lineRenderer.SetPosition(0, _from.position);
					_lineRenderer.SetPosition(1, _to.position);
				}
			}
		}

		private void SetActive(bool active)
		{
			_active = active;
			_lineRenderer.enabled = active;
		}
	}
}
