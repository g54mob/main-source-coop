using EvilCore.Audio;

namespace NomadDrive.Features.LiquidTransferSystem
{
	public class LiquidTransferProcessorFactory : ILiquidTransferProcessorFactory
	{
		private readonly IAudioManager _audioManager;

		public LiquidTransferProcessorFactory(IAudioManager audioManager)
		{
			_audioManager = audioManager;
		}

		public LiquidTransferProcessor Create(ILiquidContainer source, ILiquidContainer target)
		{
			return new LiquidTransferProcessor(source, target, _audioManager);
		}

		public LiquidTransferProcessor Create(ILiquidContainer target, float fillingSpeed)
		{
			return new LiquidTransferProcessor(target, fillingSpeed, _audioManager);
		}
	}
}
