using System;

namespace Ami.BroAudio.Runtime
{
	public class MusicPlayer : AudioPlayerDecorator, IMusicPlayer, IEffectDecoratable, IVolumeSettable, IAudioStoppable
	{
		private static AudioPlayer _currentBGMPlayer;

		private Transition _transition;

		private StopMode _stopMode;

		private float _overrideFade = -1f;

		public static AudioPlayer CurrentBGMPlayer
		{
			get
			{
				return _currentBGMPlayer;
			}
			set
			{
				if (_currentBGMPlayer != value)
				{
					_currentBGMPlayer = value;
					IAudioPlayer obj = ((value != null) ? value.GetInstanceWrapper() : null);
					MusicPlayer.OnBGMChanged?.Invoke(obj);
				}
			}
		}

		public bool IsWaitingForTransition { get; private set; }

		internal static event Action<IAudioPlayer> OnBGMChanged;

		public MusicPlayer(AudioPlayer audioPlayer)
			: base(audioPlayer)
		{
		}

		public override void Recycle()
		{
			if (CurrentBGMPlayer == base.Instance)
			{
				CurrentBGMPlayer = null;
			}
			base.Recycle();
			_transition = Transition.Default;
			_stopMode = StopMode.Stop;
			_overrideFade = -1f;
		}

		IAudioPlayer IMusicPlayer.SetTransition(Transition transition, StopMode stopMode, float overrideFade)
		{
			_transition = transition;
			_stopMode = stopMode;
			_overrideFade = overrideFade;
			return this;
		}

		public void DoTransition(ref PlaybackPreference pref)
		{
			if (CurrentBGMPlayer == null)
			{
				CurrentBGMPlayer = base.Instance;
				return;
			}
			HandleCurrentBGM();
			HandleNewBGM(ref pref);
			void HandleCurrentBGM()
			{
				IsWaitingForTransition = (_transition == Transition.Default || _transition == Transition.OnlyFadeOut) && CurrentBGMPlayer.IsPlaying;
				if (IsWaitingForTransition)
				{
					StopCurrentPlayer(FinishTransition);
				}
				else
				{
					StopCurrentPlayer();
					CurrentBGMPlayer = base.Instance;
				}
			}
			void HandleNewBGM(ref PlaybackPreference reference)
			{
				reference.SetNextFadeIn(_transition switch
				{
					Transition.Immediate => 0f, 
					Transition.OnlyFadeOut => 0f, 
					Transition.OnlyFadeIn => _overrideFade, 
					Transition.Default => _overrideFade, 
					Transition.CrossFade => _overrideFade, 
					_ => throw new ArgumentOutOfRangeException(), 
				});
			}
		}

		private void FinishTransition()
		{
			IsWaitingForTransition = false;
			CurrentBGMPlayer = base.Instance;
		}

		private void StopCurrentPlayer(Action onFinished = null)
		{
			float overrideFade = ((_transition == Transition.Immediate || _transition == Transition.OnlyFadeIn) ? 0f : _overrideFade);
			CurrentBGMPlayer.Stop(overrideFade, _stopMode, onFinished);
		}

		public static void CleanUp()
		{
			MusicPlayer.OnBGMChanged = null;
			_currentBGMPlayer = null;
		}
	}
}
