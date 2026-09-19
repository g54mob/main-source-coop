using UnityEngine;
using Zenject;

namespace Features.FallConstraintModule.Scripts
{
	public class FallConstraintRegistrar : MonoBehaviour
	{
		[SerializeField]
		private Transform _fallConstraintTransform;

		[SerializeField]
		private FallConstraintType _fallConstraintType;

		private FallConstraintModel _fallConstraintModel;

		[Inject]
		public void InjectDependencies(FallConstraintModel fallConstraintModel)
		{
			_fallConstraintModel = fallConstraintModel;
		}

		private void OnEnable()
		{
			_fallConstraintModel.RegisterBound(_fallConstraintType, _fallConstraintTransform);
		}

		private void OnDisable()
		{
			_fallConstraintModel.UnregisterBound(_fallConstraintType);
		}
	}
}
