using Mimicraft.Networking;
using UnityEngine;

namespace Mimicraft.UI
{
	public class RoleHintPanelView : MonoBehaviour
	{
		[Tooltip("Açılıp kapanan blok. Boş bırakılırsa bu objenin kendisi kullanılır - o zaman bu bileşen kapanmış bir objede kalacağı için panelin kendisi değil, ÇOCUKLARI gizlenir.")]
		[SerializeField]
		private GameObject panel;

		[Header("Kime")]
		[Tooltip("Modelcilere göster.")]
		[SerializeField]
		private bool forHiders = true;

		[Tooltip("Avcılara göster.")]
		[SerializeField]
		private bool forHunters;

		[Tooltip("Rolü olmayanlara göster - rol dağıtılmadan önce, seyirciler, ve rol dağıtmayan modlar. Rol kavramı olmayan bir sahneye koyuyorsan bunu aç.")]
		[SerializeField]
		private bool forNoRole;

		[Header("Ne zaman")]
		[Tooltip("Hangi round aşamalarında görünecek.")]
		[SerializeField]
		private RoundPhaseFilter phases = RoundPhaseFilter.Prep | RoundPhaseFilter.Hunt;

		[Tooltip("Ortada bir RoundManager yokken - Pratik gibi - panel görünsün mü? Rol ve aşama koşulları sorulacak kimse olmadığı için ikisi de atlanır.")]
		[SerializeField]
		private bool showWithoutRound;

		[Tooltip("Panelin ekranda kalacağı süre, saniye. 0 = süresiz. Sıfırdan büyükse panel, koşulların İLK sağlandığı andan itibaren bu kadar süre görünür ve sonra kapanır - her round yeniden. Öğretici bir ipucunun sonsuza kadar ekranda durmaması için.")]
		[SerializeField]
		[Min(0f)]
		private float secondsVisible;

		private RoundManager roundManager;

		private bool applied;

		private bool lastShown;

		private float hideAt;

		private void OnEnable()
		{
			applied = false;
			hideAt = 0f;
		}

		private void Update()
		{
			if (roundManager == null)
			{
				roundManager = GameModeController.Current as RoundManager;
			}
			bool flag = ShouldShow();
			if (flag && secondsVisible > 0f)
			{
				if (hideAt == 0f)
				{
					hideAt = Time.unscaledTime + secondsVisible;
				}
				if (Time.unscaledTime >= hideAt)
				{
					flag = false;
				}
			}
			else if (!flag)
			{
				hideAt = 0f;
			}
			if (!applied || flag != lastShown)
			{
				applied = true;
				lastShown = flag;
				SetShown(flag);
			}
		}

		private bool ShouldShow()
		{
			if (roundManager == null)
			{
				return showWithoutRound;
			}
			if (!Matches(roundManager.CurrentPhase.Value))
			{
				return false;
			}
			return roundManager.LocalRole switch
			{
				PlayerRole.Hunter => forHunters, 
				PlayerRole.Hider => forHiders, 
				_ => forNoRole, 
			};
		}

		private bool Matches(RoundPhase phase)
		{
			return (phases & Filter(phase)) != 0;
		}

		private static RoundPhaseFilter Filter(RoundPhase phase)
		{
			return phase switch
			{
				RoundPhase.WaitingForPlayers => RoundPhaseFilter.WaitingForPlayers, 
				RoundPhase.Prep => RoundPhaseFilter.Prep, 
				RoundPhase.Hunt => RoundPhaseFilter.Hunt, 
				RoundPhase.RoundEnd => RoundPhaseFilter.RoundEnd, 
				_ => RoundPhaseFilter.None, 
			};
		}

		private void SetShown(bool shown)
		{
			if (panel != null && panel != base.gameObject)
			{
				if (panel.activeSelf != shown)
				{
					panel.SetActive(shown);
				}
				return;
			}
			for (int i = 0; i < base.transform.childCount; i++)
			{
				GameObject gameObject = base.transform.GetChild(i).gameObject;
				if (gameObject.activeSelf != shown)
				{
					gameObject.SetActive(shown);
				}
			}
		}
	}
}
