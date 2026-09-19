using UnityEngine;

namespace Features.AudioServiceModule.Scripts
{
	public class SoundSourceBehaviour : MonoBehaviour, ITransformBasedSoundSource, ISoundSource
	{
		[SerializeField]
		private Transform _soundSourceTransform;

		[SerializeField]
		private SoundSourceKind _kind;

		private int _attributedPlayerId = -1;

		public Transform SoundSourceTransform => _soundSourceTransform;

		public SoundSourceKind Kind => _kind;

		public int AttributedPlayerId => _attributedPlayerId;

		public Vector3 SourcePosition
		{
			get
			{
				if (!(SoundSourceTransform != null))
				{
					return Vector3.zero;
				}
				return SoundSourceTransform.position;
			}
		}

		int ISoundSource.ID => ID;

		public int ID => SoundSourceTransform.GetInstanceID();

		public void SetAttributedPlayerId(int playerId)
		{
			_attributedPlayerId = playerId;
		}

		private void OnValidate()
		{
			if (_soundSourceTransform == null)
			{
				_soundSourceTransform = base.transform;
			}
		}
	}
}
