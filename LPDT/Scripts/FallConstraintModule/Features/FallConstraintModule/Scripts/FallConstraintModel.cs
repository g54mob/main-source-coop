using System.Collections.Generic;
using UnityEngine;

namespace Features.FallConstraintModule.Scripts
{
	public class FallConstraintModel
	{
		private readonly Dictionary<FallConstraintType, Transform> _fallConstraints = new Dictionary<FallConstraintType, Transform>();

		public IReadOnlyDictionary<FallConstraintType, Transform> FallConstraints => _fallConstraints;

		public void RegisterBound(FallConstraintType fallConstraintType, Transform fallConstraint)
		{
			_fallConstraints[fallConstraintType] = fallConstraint;
		}

		public void UnregisterBound(FallConstraintType fallConstraintType)
		{
			_fallConstraints.Remove(fallConstraintType);
		}
	}
}
