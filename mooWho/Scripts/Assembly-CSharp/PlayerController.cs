using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerModelController))]
public class PlayerController : NetworkBehaviour
{
	private enum SurfaceType
	{
		Grass = 0,
		Dirt = 1
	}

	[Header("Movement — Animal")]
	public float animalWalkSpeed = 2f;

	public float animalRunSpeed = 4f;

	[Header("Movement — Hunter")]
	public float hunterWalkSpeed = 1f;

	public float hunterRunSpeed = 1.5f;

	public float gravity = -20f;

	public float rotationSpeed = 10f;

	[Tooltip("Bir lag/hitch/alt-tab karesinde Time.deltaTime çok büyüyünce tek karede dev bir düşüş hızı birikip _cc.Move ince zeminlerden geçebiliyordu (ayaklar zemine gömülü kalıp bir daha GroundCheck zemini yakalayamıyordu — sadece yeniden katılınca düzeliyordu). Yer çekimi entegrasyonu bu üst sınırla sabitlenir.")]
	public float maxGravityDeltaTime = 0.05f;

	[Header("Ground Check")]
	public Transform groundCheck;

	public float groundRadius = 0.3f;

	public LayerMask groundMask;

	[Header("Animator")]
	[Tooltip("Smoothing for Speed_f damp")]
	public float animDampTime = 0.1f;

	[Header("Ayak Sesi")]
	[Tooltip("Bu kadar mesafe kat edilince bir ayak sesi çalar — hız arttıkça (koşarken) sıklık otomatik artar, ayrı bir walk/run süresi gerekmez.")]
	public float footstepStrideLength = 2f;

	[Tooltip("Atanmazsa otomatik eklenir (3D — pozisyonel)")]
	public AudioSource footstepAudioSource;

	public AudioClip[] grassFootstepClips;

	public AudioClip[] dirtFootstepClips;

	[Range(0f, 1f)]
	public float footstepVolume = 0.6f;

	private static TerrainData _cachedTerrainData;

	private static SurfaceType[] _layerSurfaceCache;

	private float _footstepDistance;

	private Vector3 _lastFootstepPos;

	private bool _hasLastFootstepPos;

	private int _lastGrassClipIndex = -1;

	private int _lastDirtClipIndex = -1;

	private CharacterController _cc;

	private PlayerModelController _modelController;

	private NetworkedCameraController _cam;

	private AnimalEatController _eatController;

	private AnimalEnergyController _energyController;

	private Vector3 _velocity;

	private bool _isGrounded;

	private float _currentSpeed;

	private static readonly int _hashSpeedF;

	private static readonly int _hashSpeedH;

	private float _currentSpeedF;

	private float _currentSpeedH;

	private Animator _anim
	{
		get
		{
			if (!(_modelController != null))
			{
				return null;
			}
			return _modelController.ActiveAnimator;
		}
	}

	public bool IsMoving { get; private set; }

	public bool IsRunning { get; private set; }

	public bool WantsToRunWithoutEnergy { get; private set; }

	[Server]
	public void ServerResetVelocity()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerController::ServerResetVelocity()' called when server was not active");
			return;
		}
		_velocity = Vector3.zero;
		if (base.connectionToClient != null)
		{
			TargetResetVelocity(base.connectionToClient);
		}
	}

	[TargetRpc]
	private void TargetResetVelocity(NetworkConnectionToClient target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendTargetRPCInternal(target, "System.Void PlayerController::TargetResetVelocity(Mirror.NetworkConnectionToClient)", 752036596, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	private void Awake()
	{
		_cc = GetComponent<CharacterController>();
		_modelController = GetComponent<PlayerModelController>();
		_cam = GetComponent<NetworkedCameraController>();
		_eatController = GetComponent<AnimalEatController>();
		_energyController = GetComponent<AnimalEnergyController>();
		if (footstepAudioSource == null)
		{
			footstepAudioSource = base.gameObject.AddComponent<AudioSource>();
		}
		footstepAudioSource.playOnAwake = false;
		footstepAudioSource.spatialBlend = 1f;
	}

	private void Update()
	{
		if (!base.isLocalPlayer)
		{
			return;
		}
		Health component = GetComponent<Health>();
		if (!(component != null) || !component.IsDead)
		{
			GroundCheck();
			if (_eatController != null && _eatController.IsEating)
			{
				HandleEatingIdle();
			}
			else
			{
				HandleMovement();
			}
			ApplyGravity();
			TickFootsteps();
		}
	}

	private void GroundCheck()
	{
		Vector3 position = ((groundCheck != null) ? groundCheck.position : (base.transform.position + Vector3.down * 0.9f));
		_isGrounded = Physics.CheckSphere(position, groundRadius, groundMask);
		if (_isGrounded && _velocity.y < 0f)
		{
			_velocity.y = -2f;
		}
	}

	private void TickFootsteps()
	{
		if (!_isGrounded || !IsMoving || !_hasLastFootstepPos)
		{
			_footstepDistance = 0f;
			_lastFootstepPos = base.transform.position;
			_hasLastFootstepPos = true;
			return;
		}
		_footstepDistance += Vector3.Distance(base.transform.position, _lastFootstepPos);
		_lastFootstepPos = base.transform.position;
		if (_footstepDistance >= footstepStrideLength)
		{
			_footstepDistance = 0f;
			PlayFootstep();
		}
	}

	private void PlayFootstep()
	{
		if (footstepAudioSource == null)
		{
			return;
		}
		bool flag = DetectSurface() == SurfaceType.Grass;
		AudioClip[] array = (flag ? grassFootstepClips : dirtFootstepClips);
		if (array != null && array.Length != 0)
		{
			int num = PickClipIndex(array.Length, flag ? _lastGrassClipIndex : _lastDirtClipIndex);
			if (flag)
			{
				_lastGrassClipIndex = num;
			}
			else
			{
				_lastDirtClipIndex = num;
			}
			footstepAudioSource.PlayOneShot(array[num], footstepVolume);
		}
	}

	private static int PickClipIndex(int count, int lastIndex)
	{
		if (count == 1)
		{
			return 0;
		}
		int num = Random.Range(0, count - 1);
		if (num >= lastIndex)
		{
			num++;
		}
		return num;
	}

	private SurfaceType DetectSurface()
	{
		Terrain activeTerrain = Terrain.activeTerrain;
		if (activeTerrain == null)
		{
			return SurfaceType.Dirt;
		}
		SurfaceType[] layerSurfaceCache = GetLayerSurfaceCache(activeTerrain.terrainData);
		if (layerSurfaceCache.Length == 0)
		{
			return SurfaceType.Dirt;
		}
		int dominantTerrainLayerIndex = GetDominantTerrainLayerIndex(activeTerrain, base.transform.position);
		if (dominantTerrainLayerIndex < 0 || dominantTerrainLayerIndex >= layerSurfaceCache.Length)
		{
			return SurfaceType.Dirt;
		}
		return layerSurfaceCache[dominantTerrainLayerIndex];
	}

	private static int GetDominantTerrainLayerIndex(Terrain terrain, Vector3 worldPos)
	{
		TerrainData terrainData = terrain.terrainData;
		Vector3 vector = worldPos - terrain.transform.position;
		float num = Mathf.Clamp01(vector.x / terrainData.size.x);
		float num2 = Mathf.Clamp01(vector.z / terrainData.size.z);
		int x = Mathf.Clamp(Mathf.RoundToInt(num * (float)terrainData.alphamapWidth), 0, terrainData.alphamapWidth - 1);
		int y = Mathf.Clamp(Mathf.RoundToInt(num2 * (float)terrainData.alphamapHeight), 0, terrainData.alphamapHeight - 1);
		float[,,] alphamaps = terrainData.GetAlphamaps(x, y, 1, 1);
		int length = alphamaps.GetLength(2);
		int result = 0;
		float num3 = -1f;
		for (int i = 0; i < length; i++)
		{
			float num4 = alphamaps[0, 0, i];
			if (num4 > num3)
			{
				num3 = num4;
				result = i;
			}
		}
		return result;
	}

	private static SurfaceType[] GetLayerSurfaceCache(TerrainData data)
	{
		if (_cachedTerrainData == data && _layerSurfaceCache != null)
		{
			return _layerSurfaceCache;
		}
		TerrainLayer[] terrainLayers = data.terrainLayers;
		SurfaceType[] array = new SurfaceType[terrainLayers.Length];
		for (int i = 0; i < terrainLayers.Length; i++)
		{
			array[i] = ClassifyLayer(terrainLayers[i]);
		}
		_cachedTerrainData = data;
		_layerSurfaceCache = array;
		return array;
	}

	private static SurfaceType ClassifyLayer(TerrainLayer layer)
	{
		if (layer == null || string.IsNullOrEmpty(layer.name))
		{
			return SurfaceType.Dirt;
		}
		string text = layer.name.ToLowerInvariant();
		if (text.Contains("grass") || text.Contains("clover") || text.Contains("flower") || text.Contains("leaves") || text.Contains("moss"))
		{
			return SurfaceType.Grass;
		}
		return SurfaceType.Dirt;
	}

	private void HandleEatingIdle()
	{
		IsMoving = false;
		IsRunning = false;
		WantsToRunWithoutEnergy = false;
		_currentSpeedF = Mathf.MoveTowards(_currentSpeedF, 0f, Time.deltaTime / Mathf.Max(animDampTime, 0.001f));
		_currentSpeedH = Mathf.MoveTowards(_currentSpeedH, 0f, Time.deltaTime / Mathf.Max(animDampTime, 0.001f));
		Animator anim = _anim;
		if (anim != null)
		{
			anim.SetFloat(_hashSpeedF, _currentSpeedF);
			anim.SetFloat(_hashSpeedH, _currentSpeedH);
		}
	}

	private void HandleMovement()
	{
		Animator anim = _anim;
		float axisRaw = Input.GetAxisRaw("Horizontal");
		float axisRaw2 = Input.GetAxisRaw("Vertical");
		bool flag = _cam != null && _cam.IsFirstPerson;
		float num = (flag ? hunterWalkSpeed : animalWalkSpeed);
		float num2 = (flag ? hunterRunSpeed : animalRunSpeed);
		Vector3 vector;
		Vector3 vector2;
		if (flag)
		{
			Transform cameraTransform = _cam.CameraTransform;
			vector = ((cameraTransform != null) ? cameraTransform.forward : base.transform.forward);
			vector2 = ((cameraTransform != null) ? cameraTransform.right : base.transform.right);
		}
		else
		{
			vector = ((_cam != null) ? _cam.MovementForward : base.transform.forward);
			vector2 = ((_cam != null) ? _cam.MovementRight : base.transform.right);
		}
		vector.y = 0f;
		vector.Normalize();
		vector2.y = 0f;
		vector2.Normalize();
		Vector3 normalized = (vector * axisRaw2 + vector2 * axisRaw).normalized;
		bool flag2 = Input.GetKey(KeyCode.LeftShift);
		bool flag3 = !flag && _energyController != null && !_energyController.CanRun;
		if (flag3)
		{
			flag2 = false;
		}
		WantsToRunWithoutEnergy = !flag && Input.GetKey(KeyCode.LeftShift) && flag3 && normalized.magnitude >= 0.1f;
		float num3 = (flag2 ? num2 : num);
		if (flag)
		{
			float yaw = _cam.Yaw;
			base.transform.rotation = Quaternion.Euler(0f, yaw, 0f);
			bool flag4 = axisRaw2 > 0.01f && Mathf.Abs(axisRaw2) >= Mathf.Abs(axisRaw);
			bool flag5 = flag2 && flag4;
			float num4 = (flag5 ? num2 : num);
			bool flag6 = normalized.magnitude >= 0.1f;
			if (flag6)
			{
				_cc.Move(normalized * num4 * Time.deltaTime);
			}
			IsMoving = flag6;
			IsRunning = flag5 && flag6;
			if (normalized.magnitude >= 0.1f)
			{
				_cc.Move(normalized * num4 * Time.deltaTime);
			}
			float num5 = (flag5 ? 1f : 0.5f);
			float target = ((Mathf.Abs(axisRaw2) > 0.01f) ? (Mathf.Sign(axisRaw2) * num5) : 0f);
			float target2 = ((Mathf.Abs(axisRaw) > 0.01f) ? (Mathf.Sign(axisRaw) * 0.5f) : 0f);
			_currentSpeedF = Mathf.MoveTowards(_currentSpeedF, target, Time.deltaTime / Mathf.Max(animDampTime, 0.001f));
			_currentSpeedH = Mathf.MoveTowards(_currentSpeedH, target2, Time.deltaTime / Mathf.Max(animDampTime, 0.001f));
			if (anim != null)
			{
				anim.SetFloat(_hashSpeedF, _currentSpeedF);
				anim.SetFloat(_hashSpeedH, _currentSpeedH);
			}
		}
		else
		{
			if (_cam != null)
			{
				_ = _cam.IsFreeLooking;
			}
			else
				_ = 0;
			bool flag7 = (IsMoving = normalized.magnitude >= 0.1f);
			IsRunning = flag2 && flag7;
			if (normalized.magnitude >= 0.1f)
			{
				float b = Mathf.Atan2(normalized.x, normalized.z) * 57.29578f;
				float y = Mathf.LerpAngle(base.transform.eulerAngles.y, b, Time.deltaTime * rotationSpeed);
				base.transform.rotation = Quaternion.Euler(0f, y, 0f);
				_cc.Move(normalized * num3 * Time.deltaTime);
				_currentSpeedF = (flag2 ? 1f : 0.5f);
			}
			else
			{
				_currentSpeedF = 0f;
			}
			if (anim != null)
			{
				float value = Mathf.MoveTowards(anim.GetFloat(_hashSpeedF), _currentSpeedF, Time.deltaTime / Mathf.Max(animDampTime, 0.001f));
				anim.SetFloat(_hashSpeedF, value);
				anim.SetFloat(_hashSpeedH, 0f);
			}
		}
	}

	private void ApplyGravity()
	{
		float num = Mathf.Min(Time.deltaTime, maxGravityDeltaTime);
		_velocity.y += gravity * num;
		_cc.Move(_velocity * num);
	}

	private void OnDrawGizmosSelected()
	{
		Vector3 center = ((groundCheck != null) ? groundCheck.position : (base.transform.position + Vector3.down * 0.9f));
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(center, groundRadius);
	}

	static PlayerController()
	{
		_hashSpeedF = Animator.StringToHash("Speed_f");
		_hashSpeedH = Animator.StringToHash("Speed_h");
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerController), "System.Void PlayerController::TargetResetVelocity(Mirror.NetworkConnectionToClient)", InvokeUserCode_TargetResetVelocity__NetworkConnectionToClient);
	}

	public override bool Weaved()
	{
		return true;
	}

	protected void UserCode_TargetResetVelocity__NetworkConnectionToClient(NetworkConnectionToClient target)
	{
		_velocity = Vector3.zero;
	}

	protected static void InvokeUserCode_TargetResetVelocity__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetResetVelocity called on server.");
		}
		else
		{
			((PlayerController)obj).UserCode_TargetResetVelocity__NetworkConnectionToClient(null);
		}
	}
}
