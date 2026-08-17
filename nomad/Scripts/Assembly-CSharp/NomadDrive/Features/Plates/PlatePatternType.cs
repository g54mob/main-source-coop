namespace NomadDrive.Features.Plates
{
	public enum PlatePatternType : byte
	{
		None = 0,
		LowNumber = 1,
		RepeatingPair = 2,
		SequentialDigits = 3,
		AllDigitsSame = 4,
		AllLettersSame = 5,
		Solid = 6
	}
}
