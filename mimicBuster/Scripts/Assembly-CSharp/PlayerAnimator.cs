using System.Collections.Generic;
using System.Text;
using Mimicraft.Gameplay;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimator : MonoBehaviour
{
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private GameObject pistol;

	[Tooltip("Left empty, taken from this GameObject. Only needed for trigger parameters - floats and bools replicate through its own polling without anyone asking.")]
	[SerializeField]
	private NetworkAnimator networkAnimator;

	[Header("Rigging")]
	[SerializeField]
	private RigBuilder rigBuilder;

	[SerializeField]
	private List<Rig> armedRigs = new List<Rig>();

	[Tooltip("Silahsızken de AÇIK kalmasını istediğin rigler - BodyAim gibi. Varsayılan olarak silahsız bir oyuncunun bütün rigleri kapanır, çünkü hepsi bir silah doğrultmak için var; gövdeyi kameraya döndüren bir rig ise silahtan bağımsız olarak her zaman doğrudur. Buradaki rigler hem silahlıyken hem silahsızken açık kalır - liste 'sadece silahsızken' değil, 'silahsızken DE' demek. Ragdoll sırasında yine hepsi kapanır (bkz. SetRigsSuspended): yerde yatan bir bedenin nişan alacağı bir şey yok.")]
	[SerializeField]
	private List<Rig> unarmedRigs = new List<Rig>();

	[Header("Nişan alma (ADS) rigi")]
	[Tooltip("Oyuncu nişan alırken (ADS ya da dürbün) ağırlığı 1'e, almıyorken 0'a giden rig - üçüncü şahısta da nişan alındığı görünsün diye. RigBuilder'ın katmanlarından biri olmalı; Armed Rigs / Unarmed Rigs listelerine EKLEME, ağırlığını yalnızca bu alan yönetir. Silahsızken ve ragdoll'dayken her zaman 0. Boş bırakılırsa hiçbir şey yapılmaz.")]
	[SerializeField]
	private Rig adsRig;

	[Tooltip("ADS riginin ağırlığının 0 ile 1 arasında gidip gelme hızı. Yüksek = daha ani.")]
	[SerializeField]
	[Min(0.1f)]
	private float adsRigBlendSpeed = 12f;

	[Header("Hareket karışımı")]
	[Tooltip("Horizontal/Vertical'in yeni değere ulaşması için geçen süre (saniye). 0 = anında, yani eski davranış. Yükseltmek geçişi yumuşatır ama karakteri girdinin gerisine düşürür.")]
	[SerializeField]
	[Min(0f)]
	private float inputDamping = 0.12f;

	private readonly int verticalHash = Animator.StringToHash("Vertical");

	private readonly int horizontalHash = Animator.StringToHash("Horizontal");

	private readonly int armedHash = Animator.StringToHash("Armed");

	private readonly int crouchHash = Animator.StringToHash("Crouch");

	private readonly int jumpHash = Animator.StringToHash("Jump");

	private bool armedPoseHeld;

	private bool requestedArmed;

	private bool customWeaponHeld;

	private readonly Dictionary<Rig, float> authoredRigWeights = new Dictionary<Rig, float>();

	private bool authoredWeightsCaptured;

	private bool rigsSuspended;

	private bool lastArmed;

	private bool aimingTarget;

	[Header("Dövüş")]
	[Tooltip("Animator'deki dövüş katmanının adı. Yumruklar ve duruş bu katmanda yaşar; taban katman yürümeye devam eder, o yüzden dövüşürken de hareket edilebilir.")]
	[SerializeField]
	private string fightingLayerName = "Fighting";

	[Tooltip("Katman ağırlığının 0 ile 1 arasında geçiş hızı. Sertçe açıp kapatmak yerine harmanlanır, yoksa duruşa giriş tek karede bir poz sıçraması olur.")]
	[SerializeField]
	[Min(0.1f)]
	private float fightingBlendSpeed = 9f;

	[Tooltip("Yumruk yiyince oynayan tepki katmanının adı. ADDITIVE olmalı ve ağırlığı 1'de durmalı: katman tepkiyi altta ne oynuyorsa onun ÜSTÜNE ekler, o yüzden yürürken de koşarken de aynı animasyon kullanılabilir. Varsayılan state'i nötr (boş) bir poz olmalı.")]
	[SerializeField]
	private string hitLayerName = "Hit";

	private readonly int punchHash = Animator.StringToHash("Punch");

	private readonly int punchIndexHash = Animator.StringToHash("PunchIndex");

	private readonly int hitHash = Animator.StringToHash("Hit");

	private readonly int blockHash = Animator.StringToHash("Block");

	private int hitLayer = -1;

	private bool hitLayerResolved;

	private bool warnedMissingHitLayer;

	private bool warnedHitLayerSilent;

	private int fightingLayer = -1;

	private bool fightingLayerResolved;

	private float fightingWeightTarget;

	private bool warnedMissingFightingLayer;

	private Animator viewModelAnimator;

	private GameObject viewModelSource;

	private bool viewModelSettled;

	private PlayerWeapons weapons;

	private readonly HashSet<string> viewModelParameters = new HashSet<string>();

	private int mirrorAttempts;

	private string lastMirror = "henuz hic denenmedi";

	private const string PunchParameter = "Punch";

	private const string PunchIndexParameter = "PunchIndex";

	private const string HitParameter = "Hit";

	private const string BlockParameter = "Block";

	private const string FightingParameter = "Fighting";

	private bool warnedMissingAnimator;

	public Animator Animator => animator;

	public string LastMirrorReport => lastMirror;

	private void Awake()
	{
		if (animator == null)
		{
			animator = GetComponentInChildren<Animator>(includeInactive: true);
		}
		if (networkAnimator == null)
		{
			networkAnimator = GetComponent<NetworkAnimator>();
		}
		ReportAnimationSetupProblems();
	}

	private void ReportAnimationSetupProblems()
	{
		if (animator == null)
		{
			Debug.LogWarning("[PlayerAnimator] Hicbir Animator bulunamadi - karakter T-pose'da kalir.", this);
			return;
		}
		if (animator.runtimeAnimatorController == null)
		{
			Debug.LogWarning("[PlayerAnimator] '" + animator.name + "' uzerindeki Animator'un Controller'i yok - oynatacak hicbir sey olmadigi icin T-pose'da kalir.", animator);
		}
		if (animator.avatar == null)
		{
			Debug.LogWarning("[PlayerAnimator] '" + animator.name + "' uzerindeki Animator'un Avatar'i yok - humanoid klipler bu iskelete eslenemez.", animator);
		}
		else if (!animator.avatar.isValid)
		{
			Debug.LogWarning("[PlayerAnimator] '" + animator.name + "' uzerindeki Avatar gecersiz - iskelet Avatar'in bekledigi hiyerarsiyle ayni degil (model yeniden import edildiyse veya prefab bolunduyse burasi kirilir).", animator);
		}
		if (rigBuilder != null && rigBuilder.gameObject != animator.gameObject)
		{
			Debug.LogWarning("[PlayerAnimator] RigBuilder '" + rigBuilder.gameObject.name + "' uzerinde ama Animator '" + animator.gameObject.name + "' uzerinde. Animation Rigging ikisinin AYNI GameObject'te olmasini istiyor; ayri olduklarinda builder hicbir seye baglanmaz.", rigBuilder);
		}
	}

	public void SetArmed(bool armed)
	{
		requestedArmed = armed;
		if (!armedPoseHeld)
		{
			ApplyArmed(armed);
		}
	}

	public void HoldArmedPose(bool hold)
	{
		if (armedPoseHeld != hold)
		{
			armedPoseHeld = hold;
			if (!hold && requestedArmed != lastArmed)
			{
				ApplyArmed(requestedArmed);
			}
		}
	}

	private void ApplyArmed(bool armed)
	{
		lastArmed = armed;
		if (!rigsSuspended)
		{
			ApplyRigState(armed);
		}
		ApplyDefaultWeaponVisibility();
		if (animator != null)
		{
			animator.SetBool(armedHash, armed);
		}
	}

	public void SetCustomWeaponHeld(bool held)
	{
		if (customWeaponHeld != held)
		{
			customWeaponHeld = held;
			ApplyDefaultWeaponVisibility();
		}
	}

	private void ApplyDefaultWeaponVisibility()
	{
		if (pistol != null)
		{
			pistol.SetActive(lastArmed && !customWeaponHeld);
		}
	}

	private void ApplyRigState(bool armed)
	{
		CaptureAuthoredRigWeights();
		foreach (Rig armedRig in armedRigs)
		{
			if (armedRig != null && armedRig != adsRig)
			{
				armedRig.weight = ((armed || unarmedRigs.Contains(armedRig)) ? 1f : 0f);
			}
		}
		if (rigBuilder == null)
		{
			return;
		}
		foreach (RigLayer layer in rigBuilder.layers)
		{
			if (layer != null && !(layer.rig == null) && !armedRigs.Contains(layer.rig) && !(layer.rig == adsRig))
			{
				bool flag = armed || unarmedRigs.Contains(layer.rig);
				layer.rig.weight = ((flag && authoredRigWeights.TryGetValue(layer.rig, out var value)) ? value : 0f);
			}
		}
	}

	public void RebuildRig()
	{
		if (rigBuilder == null || !rigBuilder.enabled)
		{
			return;
		}
		rigBuilder.Build();
		if (rigsSuspended)
		{
			foreach (RigLayer layer in rigBuilder.layers)
			{
				if (layer != null && layer.rig != null)
				{
					layer.rig.weight = 0f;
				}
			}
			return;
		}
		ApplyRigState(lastArmed);
	}

	private void CaptureAuthoredRigWeights()
	{
		if (authoredWeightsCaptured || rigBuilder == null)
		{
			return;
		}
		authoredWeightsCaptured = true;
		foreach (RigLayer layer in rigBuilder.layers)
		{
			if (layer != null && layer.rig != null)
			{
				authoredRigWeights[layer.rig] = layer.rig.weight;
			}
		}
	}

	public string RigReport()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append($"rigler: suspended={rigsSuspended} armed={lastArmed} aiming={aimingTarget} held={armedPoseHeld}");
		if (rigBuilder != null)
		{
			foreach (RigLayer layer in rigBuilder.layers)
			{
				if (layer != null && layer.rig != null)
				{
					stringBuilder.Append(string.Format(" | {0}={1:0.00}{2}", layer.rig.name, layer.rig.weight, layer.active ? "" : " (kapali)"));
				}
			}
		}
		if (adsRig != null)
		{
			stringBuilder.Append($" | ads={adsRig.weight:0.00}");
		}
		return stringBuilder.ToString();
	}

	public void SetRigsSuspended(bool suspended)
	{
		if (rigsSuspended == suspended)
		{
			return;
		}
		rigsSuspended = suspended;
		if (suspended)
		{
			CaptureAuthoredRigWeights();
			if (!(rigBuilder != null))
			{
				return;
			}
			{
				foreach (RigLayer layer in rigBuilder.layers)
				{
					if (layer != null && layer.rig != null)
					{
						layer.rig.weight = 0f;
					}
				}
				return;
			}
		}
		ApplyRigState(lastArmed);
	}

	public void SetAiming(bool aiming)
	{
		aimingTarget = aiming;
	}

	private void UpdateAdsRig()
	{
		if (adsRig == null)
		{
			return;
		}
		float num = ((aimingTarget && lastArmed && !armedPoseHeld && !rigsSuspended) ? 1f : 0f);
		float weight = adsRig.weight;
		if (!Mathf.Approximately(weight, num))
		{
			weight = Mathf.Lerp(weight, num, 1f - Mathf.Exp((0f - adsRigBlendSpeed) * Time.deltaTime));
			if (Mathf.Abs(weight - num) < 0.01f)
			{
				weight = num;
			}
			adsRig.weight = weight;
		}
	}

	public bool TryTriggerCharacter(string parameter)
	{
		return TriggerIfPresent(animator, parameter);
	}

	public static bool SetFloatIfPresent(Animator target, string parameter, float value)
	{
		if (target == null || string.IsNullOrWhiteSpace(parameter) || !target.isActiveAndEnabled || !target.isInitialized || target.runtimeAnimatorController == null)
		{
			return false;
		}
		AnimatorControllerParameter[] parameters = target.parameters;
		foreach (AnimatorControllerParameter animatorControllerParameter in parameters)
		{
			if (animatorControllerParameter.type == AnimatorControllerParameterType.Float && !(animatorControllerParameter.name != parameter))
			{
				target.SetFloat(parameter, value);
				return true;
			}
		}
		return false;
	}

	public static bool TriggerIfPresent(Animator target, string parameter)
	{
		if (target == null || string.IsNullOrWhiteSpace(parameter) || !target.isActiveAndEnabled || !target.isInitialized || target.runtimeAnimatorController == null)
		{
			return false;
		}
		AnimatorControllerParameter[] parameters = target.parameters;
		foreach (AnimatorControllerParameter animatorControllerParameter in parameters)
		{
			if (animatorControllerParameter.type == AnimatorControllerParameterType.Trigger && !(animatorControllerParameter.name != parameter))
			{
				target.SetTrigger(parameter);
				return true;
			}
		}
		return false;
	}

	public void SetInput(Vector2 input)
	{
		if (animator == null)
		{
			if (!warnedMissingAnimator)
			{
				warnedMissingAnimator = true;
				Debug.LogWarning("PlayerAnimator on '" + base.name + "' has no Animator - no character animation will play. Assign one, or parent the model under this object.", this);
			}
		}
		else
		{
			animator.SetFloat(verticalHash, input.y, inputDamping, Time.deltaTime);
			animator.SetFloat(horizontalHash, input.x, inputDamping, Time.deltaTime);
		}
	}

	public void SetCrouch(bool crouching)
	{
		if (animator != null)
		{
			animator.SetBool(crouchHash, crouching);
		}
	}

	public void TriggerJump()
	{
		if (networkAnimator != null && networkAnimator.IsSpawned)
		{
			networkAnimator.SetTrigger(jumpHash);
		}
		else if (animator != null)
		{
			animator.SetTrigger(jumpHash);
		}
	}

	public void SetFighting(bool fighting)
	{
		fightingWeightTarget = (fighting ? 1f : 0f);
		MirrorBool("Fighting", fighting);
	}

	private void ResolveViewModelAnimator()
	{
		if (weapons == null)
		{
			weapons = GetComponentInParent<PlayerWeapons>();
		}
		GameObject gameObject = ((weapons != null) ? weapons.FirstPersonInstance : null);
		if (gameObject == viewModelSource && viewModelSettled)
		{
			return;
		}
		viewModelSource = gameObject;
		viewModelAnimator = ((gameObject != null) ? gameObject.GetComponentInChildren<Animator>(includeInactive: true) : null);
		viewModelSettled = viewModelAnimator == null || viewModelAnimator.isInitialized;
		viewModelParameters.Clear();
		if (viewModelSettled && !(viewModelAnimator == null) && !(viewModelAnimator.runtimeAnimatorController == null))
		{
			AnimatorControllerParameter[] parameters = viewModelAnimator.parameters;
			foreach (AnimatorControllerParameter animatorControllerParameter in parameters)
			{
				viewModelParameters.Add(animatorControllerParameter.name);
			}
		}
	}

	private bool ViewModelHas(string parameter)
	{
		ResolveViewModelAnimator();
		if (viewModelAnimator != null)
		{
			return viewModelParameters.Contains(parameter);
		}
		return false;
	}

	private void MirrorTrigger(string parameter)
	{
		bool flag = ViewModelHas(parameter);
		mirrorAttempts++;
		lastMirror = $"{parameter} #{mirrorAttempts} gonderildi={flag}" + " animator=" + ((viewModelAnimator != null) ? viewModelAnimator.name : "YOK") + $" parametre={viewModelParameters.Count}";
		if (flag)
		{
			viewModelAnimator.SetTrigger(parameter);
		}
	}

	private void MirrorBool(string parameter, bool value)
	{
		if (ViewModelHas(parameter))
		{
			viewModelAnimator.SetBool(parameter, value);
		}
	}

	private void MirrorInteger(string parameter, int value)
	{
		if (ViewModelHas(parameter))
		{
			viewModelAnimator.SetInteger(parameter, value);
		}
	}

	public void SetBlocking(bool blocking)
	{
		MirrorBool("Block", blocking);
		if (!(animator == null) && ResolveFightingLayer())
		{
			animator.SetBool(blockHash, blocking);
		}
	}

	public void TriggerPunch(int variantIndex)
	{
		MirrorInteger("PunchIndex", variantIndex);
		MirrorTrigger("Punch");
		if (!(animator == null) && ResolveFightingLayer())
		{
			animator.SetInteger(punchIndexHash, variantIndex);
			if (networkAnimator != null && networkAnimator.IsSpawned)
			{
				networkAnimator.SetTrigger(punchHash);
			}
			else
			{
				animator.SetTrigger(punchHash);
			}
		}
	}

	public void TriggerHit()
	{
		MirrorTrigger("Hit");
		if (!(animator == null) && ResolveHitLayer())
		{
			if (!warnedHitLayerSilent && Mathf.Approximately(animator.GetLayerWeight(hitLayer), 0f))
			{
				warnedHitLayerSilent = true;
				Debug.LogWarning("[PlayerAnimator] '" + hitLayerName + "' katmaninin agirligi 0 - vurulma animasyonu tetikleniyor ama gorunmuyor. Additive katmanlar agirlik 1'de durur.", this);
			}
			animator.SetTrigger(hitHash);
		}
	}

	private bool ResolveHitLayer()
	{
		if (hitLayerResolved)
		{
			return hitLayer >= 0;
		}
		hitLayerResolved = true;
		hitLayer = ((animator != null && !string.IsNullOrWhiteSpace(hitLayerName)) ? animator.GetLayerIndex(hitLayerName) : (-1));
		if (hitLayer < 0 && !warnedMissingHitLayer)
		{
			warnedMissingHitLayer = true;
			Debug.LogWarning("[PlayerAnimator] Animator'de '" + hitLayerName + "' adinda bir katman yok - vurulma animasyonu oynatilmayacak.", this);
		}
		return hitLayer >= 0;
	}

	private bool ResolveFightingLayer()
	{
		if (fightingLayerResolved)
		{
			return fightingLayer >= 0;
		}
		fightingLayerResolved = true;
		fightingLayer = ((animator != null && !string.IsNullOrWhiteSpace(fightingLayerName)) ? animator.GetLayerIndex(fightingLayerName) : (-1));
		if (fightingLayer < 0 && !warnedMissingFightingLayer)
		{
			warnedMissingFightingLayer = true;
			Debug.LogWarning("[PlayerAnimator] Animator'de '" + fightingLayerName + "' adinda bir katman yok - dovus duruşu ve yumruklar oynatilmayacak.", this);
		}
		return fightingLayer >= 0;
	}

	private void Update()
	{
		ResolveViewModelAnimator();
		UpdateAdsRig();
		if (animator == null || !ResolveFightingLayer())
		{
			return;
		}
		float layerWeight = animator.GetLayerWeight(fightingLayer);
		if (!Mathf.Approximately(layerWeight, fightingWeightTarget))
		{
			float num = Mathf.Lerp(layerWeight, fightingWeightTarget, 1f - Mathf.Exp((0f - fightingBlendSpeed) * Time.deltaTime));
			if (Mathf.Abs(num - fightingWeightTarget) < 0.01f)
			{
				num = fightingWeightTarget;
			}
			animator.SetLayerWeight(fightingLayer, num);
		}
	}
}
