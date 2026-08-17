namespace NomadDrive.Features.Restoration
{
	public enum SurfaceOperation : byte
	{
		ClearDirt = 0,
		ApplyFullDirt = 1,
		ClearRust = 2,
		ApplyFullRust = 3,
		ClearPaint = 4,
		ApplyFullPaint = 5,
		ResetPaintColor = 6,
		ClearPolish = 7,
		ApplyFullPolish = 8,
		ClearMetal = 9,
		ApplyFullMetal = 10,
		ResetToInitialState = 11,
		FullRestoration = 12,
		PristineFinish = 13
	}
}
