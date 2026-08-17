using UnityEngine;

namespace NomadDrive.Features.FloatingOrigin
{
	[CreateAssetMenu(fileName = "FloatingOriginConfig", menuName = "NomadDrive/Floating Origin/Config")]
	public class FloatingOriginConfig : ScriptableObject
	{
		[Header("Master Kill Switch")]
		[Tooltip("OFF = the ENTIRE floating-origin system is inert: no rebasing, spawns unparented, seeds + vehicle sync untouched — byte-identical to the pre-feature behaviour. A clean one-checkbox revert to the old system (toggle in the editor before Play). All other settings below are ignored when this is OFF.")]
		public bool enabled = true;

		[Header("Rebase Trigger")]
		[Tooltip("When the tracked focus' planar (XZ) distance from the render origin exceeds this many metres, the world is rebased back toward origin. MUST stay well below the observed jitter onset (~3-4 km for NWH). 1024 = one MapMagic tile, which keeps the focus within ~1 km of origin (a 3-4x safety margin). Lower = more frequent but cheaper shifts (no terrain regen under the parent-move strategy).")]
		[Min(64f)]
		public float shiftThresholdMeters = 1024f;

		[Header("Multiplayer (Phase 2a: shared shift)")]
		[Tooltip("ON = server-authoritative SHARED origin shift: the server decides shifts and replicates the cumulative shift so every client applies the SAME shift in lockstep (one shared frame); the vehicle snapshot carries the sender's shift so the vehicle stays glitch-free across the transition. Works well when players travel together. LIMITATION: a single shared origin tracks the HOST — players who split FAR from the host get pushed away from origin (their jitter returns), and remote players/loot may briefly glitch on the shift frame. Full per-client origins = Phase 2b. OFF = no rebasing in real MP (solo / host-alone still rebases regardless).")]
		public bool enabledInMultiplayer = true;
	}
}
