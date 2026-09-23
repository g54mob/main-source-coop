using Mimicraft;
using Mimicraft.Localization;
using UnityEngine;

public static class ContentGate
{
	public static bool IsListed(this ContentAvailability state)
	{
		if (state == ContentAvailability.Invisible)
		{
			return Application.isEditor;
		}
		return true;
	}

	public static bool IsPlayable(this ContentAvailability state)
	{
		return state switch
		{
			ContentAvailability.Available => true, 
			ContentAvailability.NotAvailableInDemo => Build.Kind != BuildKind.Demo, 
			_ => false, 
		};
	}

	public static string Flare(this ContentAvailability state)
	{
		switch (state)
		{
		case ContentAvailability.ComingSoon:
			return Loc.Get("Content.ComingSoon");
		case ContentAvailability.Invisible:
			return Loc.Get("Content.Invisible");
		case ContentAvailability.NotAvailableInDemo:
			if (Build.Kind == BuildKind.Demo || Application.isEditor)
			{
				return Loc.Get("Content.NotAvailableInDemo");
			}
			break;
		}
		return "";
	}

	public static int SortOffset(this ContentAvailability state)
	{
		switch (state)
		{
		case ContentAvailability.ComingSoon:
			return 100;
		case ContentAvailability.NotAvailableInDemo:
			if (Build.Kind == BuildKind.Demo)
			{
				return 200;
			}
			break;
		}
		return 0;
	}

	public static string FlareSuffix(this ContentAvailability state)
	{
		string text = state.Flare();
		if (!string.IsNullOrEmpty(text))
		{
			return "  <size=70%><color=#FFB020>[" + text + "]</color></size>";
		}
		return "";
	}
}
