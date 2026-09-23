using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Mimicraft.Customization;
using Mimicraft.Gameplay;
using Mimicraft.Networking;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Dev
{
	public static class DevTrailerCommands
	{
		private static readonly List<Canvas> hiddenCanvases = new List<Canvas>();

		public static void RegisterAll(DevConsole console)
		{
			DevCommandRegistry.Register("ui", "[on|off]", "Hides the game's entire UI - HUD, crosshair, menus. The console stays.", delegate(string[] args)
			{
				Ui(console, args);
			});
			DevCommandRegistry.Register("freecam", "[on|off]", "Free camera. WASD + Q/E, mouse look, Shift fast, Ctrl slow, scroll wheel for speed.", delegate(string[] args)
			{
				FreeCam(console, args);
			}, cheat: true);
			DevCommandRegistry.Register("camspeed", "[value]", "Base speed of the free camera, metres per second.", delegate(string[] args)
			{
				CamSpeed(console, args);
			}, cheat: true);
			DevCommandRegistry.Register("cam", "[x y z pitch yaw]", "Places the free camera at a point. Without arguments, prints the current one - to note an angle you like and come back to it exactly.", delegate(string[] args)
			{
				Cam(console, args);
			}, cheat: true);
			DevCommandRegistry.Register("clip", "[near <m>] [far <m>] | <near> <far>", "The camera's near/far clip planes. `clip near 0.05`, `clip far 2000`, or both at once with `clip 0.05 2000`. Without arguments, prints the current values.", delegate(string[] args)
			{
				Clip(console, args);
			});
			DevCommandRegistry.Register("fov", "[degrees]", "Field of view. Applied to the free camera when it is on, otherwise to the main camera.", delegate(string[] args)
			{
				Fov(console, args);
			}, cheat: true);
			DevCommandRegistry.Register("phase", "[prep|hunt|end|wait]", "Forces the round phase - to reach the one you want to film without waiting. Server only.", delegate(string[] args)
			{
				Phase(console, args);
			}, cheat: true);
			DevCommandRegistry.Register("god", "[on|off]", "Makes the local player invulnerable. Server only.", delegate(string[] args)
			{
				God(console, args);
			}, cheat: true);
			DevCommandRegistry.Register("tp", "<x> <y> <z> | <clientId>", "Teleports the local player to a point, or next to another player.", delegate(string[] args)
			{
				Teleport(console, args);
			}, cheat: true);
			DevCommandRegistry.Register("pos", "", "Prints the local player's position and facing.", delegate
			{
				Pos(console);
			});
			DevCommandRegistry.Register("give", "<weaponId>", "Gives the local player a weapon. Without arguments, lists the weapons. Server only.", delegate(string[] args)
			{
				Give(console, args);
			}, cheat: true);
			DevCommandRegistry.Register("boxdebug", "[on|off]", "Shows the customization boxes: the default box, the bounds and the required cells (X-ray, visible through the model). Works on both the character and the weapon screen.", delegate(string[] args)
			{
				BoxDebug(console, args);
			});
			DevCommandRegistry.Register("role", "<hunter|hider|none|clear> [clientId]", "Pins a role, overriding the draw. Without a client id it applies to EVERYONE. `role clear` lets go, and the next Prep phase assigns normally. Server only.", delegate(string[] args)
			{
				Role(console, args);
			}, cheat: true);
			DevCommandRegistry.Register("portraits", "[weapons] [all]", "Shoots portraits for the saved characters. `portraits weapons` does the same for saved weapon skins. By default only the ones without a picture; add `all` to reshoot every one - for when the studio lighting changes.", delegate(string[] args)
			{
				Portraits(console, args);
			});
			DevCommandRegistry.Register("sun", "<pitch> [yaw]", "Rotates the directional light - a daylight angle for filming.", delegate(string[] args)
			{
				Sun(console, args);
			}, cheat: true);
		}

		private static void Ui(DevConsole console, string[] args)
		{
			int num;
			if (args.Length != 0)
			{
				string text = args[0].ToLowerInvariant();
				num = ((text == "off" || text == "0" || text == "false") ? 1 : 0);
			}
			else
			{
				num = ((hiddenCanvases.Count == 0) ? 1 : 0);
			}
			if (num != 0)
			{
				DevCameraGrid.Hide();
			}
			if (num == 0)
			{
				foreach (Canvas hiddenCanvase in hiddenCanvases)
				{
					if (hiddenCanvase != null)
					{
						hiddenCanvase.enabled = true;
					}
				}
				int count = hiddenCanvases.Count;
				hiddenCanvases.Clear();
				console.Print($"UI restored ({count} canvases).");
				return;
			}
			hiddenCanvases.Clear();
			Canvas[] array = Object.FindObjectsByType<Canvas>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			foreach (Canvas canvas in array)
			{
				if (canvas.enabled && canvas.isRootCanvas)
				{
					hiddenCanvases.Add(canvas);
					canvas.enabled = false;
				}
			}
			console.Print($"UI hidden ({hiddenCanvases.Count} canvases). `ui on` brings it back.");
		}

		private static void FreeCam(DevConsole console, string[] args)
		{
			int num;
			if (args.Length != 0)
			{
				string text = args[0].ToLowerInvariant();
				num = ((text == "on" || text == "1" || text == "true") ? 1 : 0);
			}
			else
			{
				num = ((!DevFreeCamera.Active) ? 1 : 0);
			}
			bool flag = (byte)num != 0;
			DevFreeCamera.SetActive(flag);
			console.Print(flag ? "Free camera on. WASD + Q/E to fly, Shift fast, Ctrl slow, scroll wheel adjusts speed. The player is parked." : "Free camera off - control returned to the player.");
		}

		private static void CamSpeed(DevConsole console, string[] args)
		{
			if (!DevFreeCamera.Active)
			{
				console.Print("Free camera is off - run `freecam` first.");
				return;
			}
			if (args.Length != 0 && float.TryParse(args[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
			{
				DevFreeCamera.Speed = result;
			}
			console.Print($"camspeed = {DevFreeCamera.Speed:0.##} m/s");
		}

		private static void Cam(DevConsole console, string[] args)
		{
			if (!DevFreeCamera.Active)
			{
				console.Print("Free camera is off - run `freecam` first.");
				return;
			}
			if (args.Length < 5)
			{
				console.Print(DevFreeCamera.Describe());
				return;
			}
			if (!TryVector(args, 0, out var value) || !float.TryParse(args[3], NumberStyles.Float, CultureInfo.InvariantCulture, out var result) || !float.TryParse(args[4], NumberStyles.Float, CultureInfo.InvariantCulture, out var result2))
			{
				console.Print("Usage: cam <x> <y> <z> <pitch> <yaw>");
				return;
			}
			DevFreeCamera.Place(value, result, result2);
			console.Print(DevFreeCamera.Describe());
		}

		private static void Clip(DevConsole console, string[] args)
		{
			Camera camera = ((DevFreeCamera.Camera != null) ? DevFreeCamera.Camera : Camera.main);
			if (camera == null)
			{
				console.Print("No camera.");
				return;
			}
			if (args.Length == 0)
			{
				console.Print($"Clip: near {camera.nearClipPlane:0.###}  far {camera.farClipPlane:0.#}");
				return;
			}
			string text = args[0].ToLowerInvariant();
			if (text == "near" || text == "far")
			{
				if (args.Length < 2 || !TryMetres(args[1], out var value))
				{
					console.Print("Expected 'clip " + text + " <metres>'.");
					return;
				}
				if (text == "near")
				{
					camera.nearClipPlane = Mathf.Max(0.0001f, value);
				}
				else
				{
					camera.farClipPlane = value;
				}
			}
			else
			{
				if (!TryMetres(args[0], out var value2))
				{
					console.Print("'" + args[0] + "' is not a number. `clip near 0.05` or `clip 0.05 2000`.");
					return;
				}
				camera.nearClipPlane = Mathf.Max(0.0001f, value2);
				if (args.Length > 1)
				{
					if (!TryMetres(args[1], out var value3))
					{
						console.Print("'" + args[1] + "' is not a number - far plane unchanged.");
					}
					else
					{
						camera.farClipPlane = value3;
					}
				}
			}
			camera.farClipPlane = Mathf.Max(camera.nearClipPlane + 0.01f, camera.farClipPlane);
			console.Print($"Clip: near {camera.nearClipPlane:0.###}  far {camera.farClipPlane:0.#}");
		}

		private static bool TryMetres(string text, out float value)
		{
			return float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
		}

		private static void Fov(DevConsole console, string[] args)
		{
			Camera camera = DevFreeCamera.Camera ?? Camera.main;
			if (camera == null)
			{
				console.Print("Camera not found.");
				return;
			}
			if (args.Length != 0 && float.TryParse(args[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
			{
				camera.fieldOfView = Mathf.Clamp(result, 5f, 150f);
			}
			console.Print($"fov = {camera.fieldOfView:0.#}  ({camera.name})" + (DevFreeCamera.Active ? "" : "  - the scope writes its own value every frame, so this will not stick"));
		}

		private static void Phase(DevConsole console, string[] args)
		{
			RoundManager roundManager = Object.FindFirstObjectByType<RoundManager>();
			if (roundManager == null)
			{
				console.Print("No RoundManager in the scene.");
				return;
			}
			if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsServer)
			{
				console.Print("Server only - the phase is the server's decision.");
				return;
			}
			if (args.Length == 0)
			{
				console.Print($"phase = {roundManager.CurrentPhase.Value}  (prep|hunt|end|wait)");
				return;
			}
			RoundPhase? roundPhase = args[0].ToLowerInvariant() switch
			{
				"prep" => RoundPhase.Prep, 
				"hunt" => RoundPhase.Hunt, 
				"end" => RoundPhase.RoundEnd, 
				"wait" => RoundPhase.WaitingForPlayers, 
				_ => null, 
			};
			if (!roundPhase.HasValue)
			{
				console.Print("'" + args[0] + "' is not a phase. prep|hunt|end|wait");
				return;
			}
			roundManager.DevEnterPhase(roundPhase.Value);
			console.Print($"phase = {roundManager.CurrentPhase.Value}");
		}

		private static void God(DevConsole console, string[] args)
		{
			if (!TryLocalPlayer(console, out var player))
			{
				return;
			}
			if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsServer)
			{
				console.Print("Server only - damage is applied by the server.");
				return;
			}
			PlayerHealth component = player.GetComponent<PlayerHealth>();
			if (component == null)
			{
				console.Print("Player has no PlayerHealth.");
				return;
			}
			int devInvulnerable;
			if (args.Length != 0)
			{
				string text = args[0].ToLowerInvariant();
				devInvulnerable = ((text == "on" || text == "1" || text == "true") ? 1 : 0);
			}
			else
			{
				devInvulnerable = ((!component.DevInvulnerable) ? 1 : 0);
			}
			component.DevInvulnerable = (byte)devInvulnerable != 0;
			console.Print("god: " + (component.DevInvulnerable ? "on" : "off"));
		}

		private static void Teleport(DevConsole console, string[] args)
		{
			if (!TryLocalPlayer(console, out var player))
			{
				return;
			}
			PlayerMovement component = player.GetComponent<PlayerMovement>();
			ulong result;
			Vector3 value2;
			if (component == null)
			{
				console.Print("Player has no PlayerMovement.");
			}
			else if (args.Length == 1 && ulong.TryParse(args[0], out result))
			{
				if (NetworkManager.Singleton == null || !NetworkManager.Singleton.ConnectedClients.TryGetValue(result, out var value) || value.PlayerObject == null)
				{
					console.Print($"No connected player with id {result}. `players` lists them.");
					return;
				}
				Transform transform = value.PlayerObject.transform;
				component.TeleportLocal(transform.position - transform.forward * 2f, transform.rotation);
				console.Print($"Teleported behind player {result}.");
			}
			else if (!TryVector(args, 0, out value2))
			{
				console.Print("Usage: tp <x> <y> <z>  or  tp <clientId>");
			}
			else
			{
				component.TeleportLocal(value2, player.transform.rotation);
				console.Print($"tp {value2.x:0.##} {value2.y:0.##} {value2.z:0.##}");
			}
		}

		private static void Pos(DevConsole console)
		{
			if (TryLocalPlayer(console, out var player))
			{
				Vector3 position = player.transform.position;
				console.Print($"tp {position.x:0.##} {position.y:0.##} {position.z:0.##}" + $"   (yaw {player.transform.eulerAngles.y:0.#})");
			}
		}

		private static void Give(DevConsole console, string[] args)
		{
			if (args.Length == 0)
			{
				StringBuilder stringBuilder = new StringBuilder("Weapons:");
				foreach (WeaponDefinition item in WeaponCatalog.All)
				{
					stringBuilder.Append($"\n  {item.WeaponId,-16} {item.DisplayName}");
				}
				console.Print(stringBuilder.ToString());
			}
			else if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsServer)
			{
				console.Print("Server only - the equipped weapon is written by the server.");
			}
			else
			{
				if (!TryLocalPlayer(console, out var player))
				{
					return;
				}
				WeaponDefinition weaponDefinition = WeaponCatalog.Find(args[0]);
				if (weaponDefinition == null)
				{
					console.Print("No weapon called '" + args[0] + "'. `give` lists them.");
					return;
				}
				PlayerWeapons component = player.GetComponent<PlayerWeapons>();
				if (component == null)
				{
					console.Print("Player has no PlayerWeapons.");
					return;
				}
				component.ServerEquip(weaponDefinition);
				console.Print("Given: " + weaponDefinition.DisplayName);
			}
		}

		private static void Role(DevConsole console, string[] args)
		{
			RoundManager roundManager = Object.FindFirstObjectByType<RoundManager>();
			if (roundManager == null || !roundManager.IsServer)
			{
				console.Print("Works only on the server, in a mode that assigns roles.");
				return;
			}
			if (args.Length == 0)
			{
				PrintRoles(console, roundManager);
				return;
			}
			ulong? onlyClient = null;
			if (args.Length > 1)
			{
				if (!ulong.TryParse(args[1], out var result))
				{
					console.Print("'" + args[1] + "' is not a client id. `players` lists them.");
					return;
				}
				onlyClient = result;
			}
			switch (args[0].ToLowerInvariant())
			{
			case "clear":
				roundManager.DevClearForcedRoles(onlyClient);
				console.Print(onlyClient.HasValue ? $"Pin removed for {onlyClient.Value} - the next Prep phase assigns normally." : "All pins removed - the next Prep phase assigns normally.");
				return;
			case "hunter":
				roundManager.DevForceRole(PlayerRole.Hunter, onlyClient);
				break;
			case "hider":
				roundManager.DevForceRole(PlayerRole.Hider, onlyClient);
				break;
			case "none":
				roundManager.DevForceRole(PlayerRole.None, onlyClient);
				break;
			default:
				console.Print("'" + args[0] + "' not understood. hunter | hider | none | clear");
				return;
			}
			PrintRoles(console, roundManager);
		}

		private static void PrintRoles(DevConsole console, RoundManager round)
		{
			StringBuilder stringBuilder = new StringBuilder("Roles:");
			int num = 0;
			foreach (KeyValuePair<ulong, PlayerRole> item in round.DevRoles())
			{
				num++;
				stringBuilder.Append($"\n  {item.Key}: {Name(item.Value)}");
			}
			console.Print((num == 0) ? "No roles assigned yet - start a round with `phase prep`." : stringBuilder.ToString());
		}

		private static string Name(PlayerRole role)
		{
			return role switch
			{
				PlayerRole.Hunter => "Hunter", 
				PlayerRole.Hider => "Hider", 
				_ => "none", 
			};
		}

		private static void Portraits(DevConsole console, string[] args)
		{
			int num;
			if (args.Length != 0)
			{
				string text = args[0].ToLowerInvariant();
				num = ((text == "weapon" || text == "weapons") ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			bool flag = (byte)num != 0;
			bool flag2 = false;
			foreach (string text2 in args)
			{
				flag2 |= text2.ToLowerInvariant() == "all";
			}
			if (flag)
			{
				int num2 = WeaponSkinPortraitService.RequestMissing(null, flag2);
				console.Print((num2 != 0) ? $"{num2} weapon picture(s) queued - WeaponStudio will load and shoot them." : (flag2 ? "No saved weapon skins." : "No weapon skin is missing a picture."));
				return;
			}
			CharacterAssembler characterAssembler = Object.FindFirstObjectByType<CharacterAssembler>();
			CharacterRigDefinition characterRigDefinition = ((characterAssembler != null) ? characterAssembler.Rig : null);
			if (characterRigDefinition == null)
			{
				console.Print("Rig not found - no CharacterAssembler in this scene. Try in the main menu or on the Customization screen.");
				return;
			}
			int num3 = CharacterPortraitService.RequestMissing(characterRigDefinition, flag2);
			console.Print((num3 != 0) ? $"{num3} portrait(s) queued - the studio scene will load and shoot them." : (flag2 ? "No saved characters." : "No character is missing a portrait."));
		}

		private static void BoxDebug(DevConsole console, string[] args)
		{
			int num;
			if (args.Length != 0)
			{
				string text = args[0].ToLowerInvariant();
				num = ((text == "on" || text == "1" || text == "true") ? 1 : 0);
			}
			else
			{
				num = ((!DevBoxDebug.Active) ? 1 : 0);
			}
			DevBoxDebug.SetActive((byte)num != 0);
			if (num == 0)
			{
				console.Print("Box debug off.");
				return;
			}
			console.Print(DevBoxDebug.BuildAndDescribe());
			console.Print("  blue = default box, orange = bounds, red = required cells");
		}

		private static void Sun(DevConsole console, string[] args)
		{
			Light light = null;
			Light[] array = Object.FindObjectsByType<Light>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			foreach (Light light2 in array)
			{
				if (light2.type == LightType.Directional && (light == null || light2.intensity > light.intensity))
				{
					light = light2;
				}
			}
			if (light == null)
			{
				console.Print("No directional light in the scene.");
				return;
			}
			if (args.Length == 0)
			{
				Vector3 eulerAngles = light.transform.eulerAngles;
				console.Print($"sun {eulerAngles.x:0.#} {eulerAngles.y:0.#}   ({light.name})");
				return;
			}
			if (!float.TryParse(args[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
			{
				console.Print("Usage: sun <pitch> [yaw]");
				return;
			}
			float result2 = light.transform.eulerAngles.y;
			if (args.Length > 1)
			{
				float.TryParse(args[1], NumberStyles.Float, CultureInfo.InvariantCulture, out result2);
			}
			light.transform.rotation = Quaternion.Euler(result, result2, 0f);
			console.Print($"sun {result:0.#} {result2:0.#}" + "   - baked lighting does not change, only the realtime light turns");
		}

		private static bool TryLocalPlayer(DevConsole console, out NetworkObject player)
		{
			player = null;
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton == null || !singleton.IsListening || singleton.LocalClient.PlayerObject == null)
			{
				console.Print("No local player - you are not in a round.");
				return false;
			}
			player = singleton.LocalClient.PlayerObject;
			return true;
		}

		private static bool TryVector(string[] args, int start, out Vector3 value)
		{
			value = default(Vector3);
			if (args.Length < start + 3)
			{
				return false;
			}
			if (!float.TryParse(args[start], NumberStyles.Float, CultureInfo.InvariantCulture, out var result) || !float.TryParse(args[start + 1], NumberStyles.Float, CultureInfo.InvariantCulture, out var result2) || !float.TryParse(args[start + 2], NumberStyles.Float, CultureInfo.InvariantCulture, out var result3))
			{
				return false;
			}
			value = new Vector3(result, result2, result3);
			return true;
		}
	}
}
