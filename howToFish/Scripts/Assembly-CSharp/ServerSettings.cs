using FishNet.Object;
using FishNet.Object.Synchronizing;

public class ServerSettings : NetworkBehaviour
{
	public static ServerSettings Instance;

	public readonly SyncVar<bool> _useFriendlyFire = new SyncVar<bool>(initialValue: true);

	public readonly SyncVar<bool> _useOneShot = new SyncVar<bool>();

	private bool NetworkInitialize___EarlyServerSettingsAssembly_002DCSharp_002Edll_Excuted;

	private bool NetworkInitialize___LateServerSettingsAssembly_002DCSharp_002Edll_Excuted;

	public static bool UseFriendlyFire => Instance._useFriendlyFire.Value;

	public static bool OneShotEnabled => Instance._useOneShot.Value;

	public virtual void Awake()
	{
		NetworkInitialize___Early();
		Awake_UserLogic_ServerSettings_Assembly_002DCSharp_002Edll();
		NetworkInitialize___Late();
	}

	public void ToggleFriendlyFire(bool to)
	{
		if (base.IsServerInitialized)
		{
			_useFriendlyFire.Value = to;
		}
	}

	public void ToggleOneShot()
	{
		if (base.IsServerInitialized)
		{
			_useOneShot.Value = !_useOneShot.Value;
		}
	}

	public override void NetworkInitialize___Early()
	{
		if (!NetworkInitialize___EarlyServerSettingsAssembly_002DCSharp_002Edll_Excuted)
		{
			NetworkInitialize___EarlyServerSettingsAssembly_002DCSharp_002Edll_Excuted = true;
			base.NetworkInitialize___Early();
			_useOneShot.InitializeEarly(this, 1u, isSyncObject: false);
			_useFriendlyFire.InitializeEarly(this, 0u, isSyncObject: false);
		}
	}

	public override void NetworkInitialize___Late()
	{
		if (!NetworkInitialize___LateServerSettingsAssembly_002DCSharp_002Edll_Excuted)
		{
			NetworkInitialize___LateServerSettingsAssembly_002DCSharp_002Edll_Excuted = true;
			base.NetworkInitialize___Late();
			_useOneShot.InitializeLate();
			_useFriendlyFire.InitializeLate();
		}
	}

	public override void NetworkInitializeIfDisabled()
	{
		NetworkInitialize___Early();
		NetworkInitialize___Late();
	}

	private void Awake_UserLogic_ServerSettings_Assembly_002DCSharp_002Edll()
	{
		Instance = this;
	}
}
