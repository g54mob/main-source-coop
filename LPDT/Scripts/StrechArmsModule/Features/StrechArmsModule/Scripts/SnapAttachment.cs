using Obi;
using UnityEngine;

namespace Features.StrechArmsModule.Scripts
{
	public class SnapAttachment : MonoBehaviour
	{
		[SerializeField]
		private ObiParticleAttachment _attachment;

		[SerializeField]
		private ObiActor _actor;

		[SerializeField]
		[Range(0f, 1f)]
		private float _snapProgress = 1f;

		private void OnEnable()
		{
			if (_actor != null)
			{
				_actor.OnBlueprintLoaded += OnBlueprintLoaded;
			}
		}

		private void OnDisable()
		{
			if (_actor != null)
			{
				_actor.OnBlueprintLoaded -= OnBlueprintLoaded;
			}
		}

		private void Start()
		{
			Snap();
		}

		private void OnBlueprintLoaded(ObiActor actor, ObiActorBlueprint blueprint)
		{
			Snap();
		}

		public void Snap()
		{
			if (!(_attachment == null) && !(_actor == null) && _actor.isLoaded && !(_attachment.target == null))
			{
				ObiParticleGroup particleGroup = _attachment.particleGroup;
				if (!(particleGroup == null) && particleGroup.particleIndices.Count != 0)
				{
					int num = particleGroup.particleIndices.Count - 1;
					int index = Mathf.Clamp(Mathf.RoundToInt((float)num * _snapProgress), 0, num);
					int index2 = _actor.solverIndices[particleGroup.particleIndices[index]];
					Transform target = _attachment.target;
					_attachment.enabled = false;
					_attachment.target = null;
					_actor.solver.positions[index2] = _actor.solver.transform.InverseTransformPoint(target.position);
					_attachment.target = target;
					_attachment.enabled = true;
				}
			}
		}

		public void Snap(Vector3 position)
		{
			if (!(_attachment == null) && !(_actor == null) && _actor.isLoaded && !(_attachment.target == null))
			{
				ObiParticleGroup particleGroup = _attachment.particleGroup;
				if (!(particleGroup == null) && particleGroup.particleIndices.Count != 0)
				{
					int num = particleGroup.particleIndices.Count - 1;
					int index = Mathf.Clamp(Mathf.RoundToInt((float)num * _snapProgress), 0, num);
					int index2 = _actor.solverIndices[particleGroup.particleIndices[index]];
					Transform target = _attachment.target;
					_attachment.enabled = false;
					_attachment.target = null;
					_actor.solver.positions[index2] = _actor.solver.transform.InverseTransformPoint(position);
					_attachment.target = target;
					_attachment.enabled = true;
				}
			}
		}
	}
}
