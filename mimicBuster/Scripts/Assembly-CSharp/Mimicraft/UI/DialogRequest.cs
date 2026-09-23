namespace Mimicraft.UI
{
	public readonly struct DialogRequest
	{
		public readonly string Message;

		public readonly string ConfirmLabel;

		public readonly string AlternateLabel;

		public readonly string CancelLabel;

		public readonly bool WantsInput;

		public readonly string InputPrefill;

		public readonly string InputPlaceholder;

		public readonly bool InputIsPassword;

		public readonly bool RequiresInput;

		public readonly bool AlternateIsDestructive;

		public DialogRequest(string message, string confirmLabel = null, string cancelLabel = null, string alternateLabel = null, bool wantsInput = false, string inputPrefill = null, string inputPlaceholder = null, bool inputIsPassword = false, bool requiresInput = false, bool alternateIsDestructive = false)
		{
			Message = message;
			ConfirmLabel = confirmLabel;
			AlternateLabel = alternateLabel;
			CancelLabel = cancelLabel;
			WantsInput = wantsInput;
			InputPrefill = inputPrefill;
			InputPlaceholder = inputPlaceholder;
			InputIsPassword = inputIsPassword;
			RequiresInput = requiresInput;
			AlternateIsDestructive = alternateIsDestructive;
		}
	}
}
