using UnityEngine;

public class PlayerDataSounds : MonoBehaviour
{
	private static readonly int Damage = Animator.StringToHash("Damage");

	public Player player;

	private float prevHealth;

	private float prevStamina;

	public float dmg;

	public Animator dmgLvl1;

	public Animator dmgLvl2;

	public Animator dmgLvl3;

	public Animator heal;

	public Animator dead;

	public Animator stamina;

	public Animator staminaReturn;

	public Animator oxygen;

	public Animator oxygenReturn;

	public AudioSource staminaLoop;

	public AudioSource oxygenLoop;

	public AudioSource oxygenTimer;

	private bool t;

	private bool t2;

	private bool t3;

	public AudioSource throwCharge;

	public SFX_Instance throwStart;

	public SFX_Instance throwEndLvl1;

	public SFX_Instance throwEndLvl2;

	private float prevCharge;

	public SFX_Instance bedOn;

	public SFX_Instance bedOff;

	private bool sleepToggle;

	public AudioSource fallLoop;

	public AudioSource swimLoop;

	private PlayerController m_playerController;

	private void Start()
	{
		prevHealth = player.data.health;
		prevStamina = player.data.currentStamina;
		m_playerController = GetComponent<PlayerController>();
	}

	private void Update()
	{
		dmgLvl1.SetBool(Damage, value: false);
		dmgLvl2.SetBool(Damage, value: false);
		dmgLvl3.SetBool(Damage, value: false);
		heal.SetBool(Damage, value: false);
		staminaReturn.SetBool(Damage, value: false);
		dead.SetBool(Damage, value: false);
		oxygen.SetBool(Damage, value: false);
		oxygenReturn.SetBool(Damage, value: false);
		stamina.SetBool(Damage, value: false);
		staminaLoop.volume = Mathf.Lerp(staminaLoop.volume, 0f, 25f * Time.deltaTime);
		staminaLoop.pitch = Mathf.Lerp(staminaLoop.pitch, player.data.currentStamina * 0.002f + 2f, 2.5f * Time.deltaTime);
		if (prevHealth != player.data.health)
		{
			dmg = prevHealth - player.data.health;
		}
		if (prevHealth - player.data.health < 0f)
		{
			heal.SetBool(Damage, value: true);
		}
		if (prevHealth - player.data.health > 0f)
		{
			dmgLvl1.SetBool(Damage, value: true);
		}
		if (prevHealth - player.data.health >= 20f)
		{
			dmgLvl2.SetBool(Damage, value: true);
		}
		if (prevHealth - player.data.health >= 50f)
		{
			dmgLvl3.SetBool(Damage, value: true);
		}
		if (player.data.currentStamina < 0.1f)
		{
			stamina.SetBool(Damage, value: true);
		}
		if (!player.data.dead)
		{
			t2 = false;
		}
		if (player.data.dead && !t2)
		{
			dead.SetBool(Damage, value: true);
			t2 = true;
		}
		if (player.data.remainingOxygen > 100f)
		{
			if (t)
			{
				oxygenReturn.SetBool(Damage, value: true);
			}
			oxygenLoop.gameObject.SetActive(value: false);
			oxygenTimer.gameObject.SetActive(value: false);
			t = false;
		}
		oxygenLoop.pitch = player.data.remainingOxygen * 0.0025f;
		oxygenTimer.pitch = 25f / (player.data.remainingOxygen + 0.01f) + 0.75f;
		if (player.data.remainingOxygen < 100f && !t)
		{
			oxygen.SetBool(Damage, value: true);
			oxygenLoop.gameObject.SetActive(value: true);
			oxygenTimer.gameObject.SetActive(value: true);
			t = true;
		}
		if (player.data.dead)
		{
			oxygenLoop.gameObject.SetActive(value: false);
			oxygenTimer.gameObject.SetActive(value: false);
		}
		if (prevStamina < player.data.currentStamina)
		{
			staminaLoop.volume = Mathf.Lerp(staminaLoop.volume, 0.025f, 50f * Time.deltaTime);
		}
		if (prevStamina != player.data.currentStamina && prevStamina < player.data.currentStamina && prevStamina > m_playerController.maxStamina - 0.1f)
		{
			staminaReturn.SetBool(Damage, value: true);
		}
		if (player.data.throwCharge > 0f && prevCharge <= player.data.throwCharge)
		{
			throwCharge.enabled = true;
			throwCharge.volume = Mathf.Lerp(throwCharge.volume, 0.1f, Time.deltaTime * 10f);
			throwCharge.pitch = Mathf.Lerp(throwCharge.pitch, 0.25f + player.data.throwCharge * 4f, Time.deltaTime * 10f);
			if (!t3)
			{
				throwStart.Play(base.transform.position);
				t3 = true;
			}
		}
		if (prevCharge > player.data.throwCharge)
		{
			throwCharge.enabled = false;
			throwCharge.volume = Mathf.Lerp(throwCharge.volume, 0f, Time.deltaTime * 10f);
			throwCharge.pitch = Mathf.Lerp(throwCharge.pitch, 0.25f, Time.deltaTime * 10f);
			if (t3 && prevCharge <= 0.5f)
			{
				throwEndLvl1.Play(throwCharge.transform.position);
			}
			if (t3 && prevCharge > 0.5f)
			{
				throwEndLvl2.Play(throwCharge.transform.position);
			}
			t3 = false;
		}
		if ((bool)player.data.currentBed && !sleepToggle)
		{
			sleepToggle = true;
			bedOn.Play(throwCharge.transform.position);
		}
		if (!player.data.currentBed && sleepToggle)
		{
			sleepToggle = false;
			bedOff.Play(throwCharge.transform.position);
		}
		if (player.data.isGrounded)
		{
			fallLoop.enabled = false;
			fallLoop.volume = 0f;
			fallLoop.pitch = 0.5f;
		}
		if (!player.data.isGrounded)
		{
			fallLoop.enabled = true;
			fallLoop.volume = Mathf.Lerp(fallLoop.volume, (Mathf.Abs(player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.linearVelocity.x) + Mathf.Abs(player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.linearVelocity.y) + Mathf.Abs(player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.linearVelocity.z)) * 0.0167f, Time.deltaTime * 10f);
			fallLoop.pitch = Mathf.Lerp(fallLoop.pitch, (Mathf.Abs(player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.linearVelocity.x) + Mathf.Abs(player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.linearVelocity.y) + Mathf.Abs(player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.linearVelocity.z)) * 0.004167f + 0.75f, Time.deltaTime * 10f);
		}
		if ((bool)swimLoop)
		{
			swimLoop.volume = Mathf.Lerp(swimLoop.volume, 0f, Time.deltaTime);
			if (player.data.inWaterAmount > 0.1f)
			{
				swimLoop.volume = Mathf.Lerp(swimLoop.volume, Mathf.Abs(player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.linearVelocity.x) + Mathf.Abs(player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.linearVelocity.y) + Mathf.Abs(player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.linearVelocity.z) * player.data.inWaterAmount * 0.125f, Time.deltaTime * 10f);
			}
		}
		prevHealth = player.data.health;
		prevStamina = player.data.currentStamina;
		prevCharge = player.data.throwCharge;
	}
}
