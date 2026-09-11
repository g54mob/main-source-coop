using System;
using DefaultNamespace.Petter.TitleCard;
using Photon.Pun;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Zorro.ControllerSupport;
using Zorro.Core.Serizalization;

public class TitleCardItemInstance : ItemInstanceBehaviour
{
	private OnOffEntry onOffEntry;

	private TitleCardCanvas titleCardCanvas;

	private TitleCardSyncer syncer;

	private MouseBrush mouseBrush;

	private TitleCardDataEntry titleCardDataEntry;

	private Player playerHoldingItem;

	public bool InTitleCardTerminal
	{
		get
		{
			return Player.localPlayer.data.isInTitleCardTerminal;
		}
		set
		{
			Debug.Log($"Setting InTitleCardTerminal to {value}");
			mouseBrush.Show = value;
			Player.localPlayer.data.forceAimPressed = value;
			Player.localPlayer.data.isInTitleCardTerminal = value;
			onOffEntry.on = value;
			onOffEntry.SetDirty();
		}
	}

	private void Awake()
	{
		titleCardCanvas = GetComponentInChildren<TitleCardCanvas>();
		mouseBrush = GetComponentInChildren<MouseBrush>();
		syncer = GetComponentInChildren<TitleCardSyncer>();
		playerHoldingItem = base.transform.root.GetComponentInChildren<Player>();
	}

	public void Update()
	{
		bool flag = onOffEntry.on;
		if (playerHoldingItem != null)
		{
			mouseBrush.Show = flag;
			playerHoldingItem.data.forceAimPressed = flag;
			playerHoldingItem.data.isInCostomizeTerminal = flag;
		}
		bool flag2 = (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame) || (Gamepad.current != null && Gamepad.current.leftTrigger.wasPressedThisFrame);
		if (isHeldByMe && InTitleCardTerminal && flag2)
		{
			Debug.Log("Escape pressed");
			InTitleCardTerminal = false;
		}
		if (isHeldByMe && !Player.localPlayer.HasLockedInput() && !InTitleCardTerminal && flag2)
		{
			InTitleCardTerminal = true;
		}
		if (isHeldByMe && InTitleCardTerminal)
		{
			if (InputHandler.GetCurrentUsedInputScheme() == InputScheme.KeyboardMouse)
			{
				if (Mouse.current.leftButton.wasPressedThisFrame)
				{
					RaycastHit[] array = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 10000f);
					foreach (RaycastHit raycastHit in array)
					{
						if (raycastHit.collider.TryGetComponent<TitleCardButton>(out var component))
						{
							switch (component.buttonType)
							{
							case TitleCardButton.ButtonType.white:
								titleCardCanvas.SetBrushColorToWhite();
								break;
							case TitleCardButton.ButtonType.black:
								titleCardCanvas.SetBrushColorToBlack();
								break;
							case TitleCardButton.ButtonType.clear:
								titleCardCanvas.Clear();
								break;
							default:
								throw new ArgumentOutOfRangeException();
							}
						}
					}
				}
				if (Mouse.current.scroll.value.y < 0f)
				{
					titleCardCanvas.SetBrushColorToWhite();
				}
				if (Mouse.current.scroll.value.y > 0f)
				{
					titleCardCanvas.SetBrushColorToBlack();
				}
				if (Keyboard.current.rKey.wasPressedThisFrame)
				{
					titleCardCanvas.Clear();
				}
			}
			if (Gamepad.current != null)
			{
				if (Gamepad.current.leftShoulder.wasPressedThisFrame)
				{
					titleCardCanvas.SetBrushColorToWhite();
				}
				if (Gamepad.current.rightShoulder.wasPressedThisFrame)
				{
					titleCardCanvas.SetBrushColorToBlack();
				}
				if (Gamepad.current.buttonNorth.wasPressedThisFrame)
				{
					titleCardCanvas.Clear();
				}
			}
		}
		if (!titleCardCanvas.IsDirty)
		{
			return;
		}
		titleCardCanvas.ClearDirty();
		if (IsBlocked())
		{
			Debug.Log("Blocking drawing");
			return;
		}
		syncer.ToByte(delegate(NativeArray<uint> data)
		{
			titleCardDataEntry.data = new NativeArray<uint>(data, Allocator.Persistent);
			titleCardDataEntry.SetForceDirty();
		});
	}

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		if (data.TryGetEntry<TitleCardDataEntry>(out titleCardDataEntry))
		{
			Debug.Log("TitleCardDataEntry found");
			if (!IsBlocked())
			{
				syncer.ToRt(titleCardDataEntry.data);
				titleCardCanvas.CopyActiveToSnapShot();
			}
		}
		else
		{
			titleCardDataEntry = new TitleCardDataEntry
			{
				data = new NativeArray<uint>(titleCardCanvas.titleCardRt.width * titleCardCanvas.titleCardRt.height / 32, Allocator.Persistent)
			};
			data.AddDataEntry(titleCardDataEntry);
		}
		Debug.Log("Subscribing to ondatachanged with sync canvas");
		TitleCardDataEntry obj = titleCardDataEntry;
		obj.OnDataChanged = (Action)Delegate.Combine(obj.OnDataChanged, new Action(SyncCanvas));
		if (data.TryGetEntry<OnOffEntry>(out onOffEntry))
		{
			onOffEntry.on = false;
		}
		else
		{
			onOffEntry = new OnOffEntry
			{
				on = false
			};
			data.AddDataEntry(onOffEntry);
		}
		itemInstance.RegisterRPC(ItemRPC.RPC0, RPCA_SyncCanvas);
	}

	private void RPCA_SyncCanvas(BinaryDeserializer deserializer)
	{
		SyncCanvas();
	}

	private void OnDestroy()
	{
		if (titleCardDataEntry != null)
		{
			TitleCardDataEntry obj = titleCardDataEntry;
			obj.OnDataChanged = (Action)Delegate.Remove(obj.OnDataChanged, new Action(SyncCanvas));
		}
	}

	private void SyncCanvas()
	{
		if (!IsBlocked())
		{
			Debug.Log("Syncing Canvas");
			syncer.ToRt(titleCardDataEntry.data);
			titleCardCanvas.CopyActiveToSnapShot();
		}
	}

	private bool IsBlocked()
	{
		foreach (Player player in PlayerHandler.instance.players)
		{
			if (!player.refs.view.Controller.IsLocal && (!player.TryGetGlobalPlayerData(out var d) || d.isBlocked))
			{
				return true;
			}
		}
		return false;
	}
}
