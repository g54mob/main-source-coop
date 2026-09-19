using UnityEngine;
using UnityEngine.UI;

namespace Fusion.Statistics
{
	public class FusionStatisticsForecastObjectStats : MonoBehaviour
	{
		[SerializeField]
		private MultilineGraph _velocityCorrection;

		[SerializeField]
		private MultilineGraph _stallHeuristic;

		[SerializeField]
		private MultilineGraph _collisionEnterHeuristic;

		[SerializeField]
		private Text _title;

		[SerializeField]
		private Button _closeButton;

		public NetworkId ID;

		private NetworkTransformTrace.BufferedDataReader<NetworkTransformTrace.CollisionEnterData> _collisionEnterReader;

		private NetworkTransformTrace.BufferedDataReader<NetworkTransformTrace.ForecastData> _forecastReader;

		private NetworkTransformTrace.BufferedDataReader<NetworkTransformTrace.StallHeuristicData> _stallHeuristicReader;

		private NetworkTransformTrace.BufferedDataReader<NetworkTransformTrace.StallData> _stallReader;

		private NetworkRunner _runner;

		private int _impactSpeedLine;

		private int _totalSpeedLine;

		private int _minImpactThreshold;

		private int _currentVelocityLine;

		private int _desiredVelocityLine;

		private int _lerpedVelocityLine;

		private int _lerpAlphaLine;

		private int _errorSimilarityLine;

		private int _correctionProgressLine;

		private int _accruedScoreLine;

		private int _errorSimilarityThreshold;

		private int _correctionProgressThreshold;

		private int _accruedScoreThreshold;

		public void Setup(FusionStatisticsForecastObjectPage objectPage, string title, NetworkId id)
		{
			_title.text = title;
			_runner = objectPage.Runner;
			ID = id;
			_closeButton.onClick.RemoveAllListeners();
			_closeButton.onClick.AddListener(delegate
			{
				objectPage.RemoveMonitoredNetworkObject(this);
			});
			NetworkTransform networkTransform = _runner.TryGetNetworkedBehaviourFromNetworkedObjectRef<NetworkTransform>(ID);
			PhysicsSettings physicsSettings = networkTransform.PhysicsSettings;
			_impactSpeedLine = _collisionEnterHeuristic.AddLine(Color.cyan, "Impact Speed");
			_totalSpeedLine = _collisionEnterHeuristic.AddLine(Color.magenta, "Total Speed");
			_minImpactThreshold = _collisionEnterHeuristic.AddThreshold(physicsSettings.MinImpactfulCollisionAlignment, Color.cyan, "Min Impact Speed");
			_collisionEnterReader = new NetworkTransformTrace.BufferedDataReader<NetworkTransformTrace.CollisionEnterData>(networkTransform.CurrentTrace?.CollisionEnterBuffer ?? null);
			_currentVelocityLine = _velocityCorrection.AddLine(Color.cyan, "Current Velocity");
			_desiredVelocityLine = _velocityCorrection.AddLine(Color.magenta, "Desired Velocity");
			_lerpedVelocityLine = _velocityCorrection.AddLine(Color.yellow, "Lerped Velocity");
			_lerpAlphaLine = _velocityCorrection.AddLine(Color.green, "Lerp Alpha");
			_forecastReader = new NetworkTransformTrace.BufferedDataReader<NetworkTransformTrace.ForecastData>(networkTransform.CurrentTrace?.ForecastBuffer ?? null);
			_errorSimilarityLine = _stallHeuristic.AddLine(Color.cyan, "Error Similarity");
			_correctionProgressLine = _stallHeuristic.AddLine(Color.magenta, "Correction Progress");
			_accruedScoreLine = _stallHeuristic.AddLine(Color.yellow, "Accrued Score");
			_errorSimilarityThreshold = _stallHeuristic.AddThreshold(physicsSettings.HighErrorSimilarityThreshold, Color.cyan, "Error Similarity<");
			_correctionProgressThreshold = _stallHeuristic.AddThreshold(physicsSettings.LowCorrectionProgressThreshold, Color.magenta, "Corr Progress<");
			_accruedScoreThreshold = _stallHeuristic.AddThreshold(physicsSettings.MaxErrorTotalTime, Color.yellow, "Accrued Score>");
			_stallHeuristicReader = new NetworkTransformTrace.BufferedDataReader<NetworkTransformTrace.StallHeuristicData>(networkTransform.CurrentTrace?.StallHeuristicBuffer ?? null);
			_stallReader = new NetworkTransformTrace.BufferedDataReader<NetworkTransformTrace.StallData>(networkTransform.CurrentTrace?.StallBuffer ?? null);
		}

		public void SetData(FusionStatisticsManager statisticsManager)
		{
			PhysicsSettings physicsSettings = _runner.TryGetNetworkedBehaviourFromNetworkedObjectRef<NetworkTransform>(ID).PhysicsSettings;
			_stallHeuristic.SetThresholdValue(_errorSimilarityThreshold, physicsSettings.HighErrorSimilarityThreshold);
			_stallHeuristic.SetThresholdValue(_correctionProgressThreshold, physicsSettings.LowCorrectionProgressThreshold);
			_stallHeuristic.SetThresholdValue(_accruedScoreThreshold, physicsSettings.MaxErrorTotalTime);
			bool flag = false;
			NetworkTransformTrace.StallHeuristicData value;
			while (_stallHeuristicReader.Read(out value))
			{
				flag = true;
				_stallHeuristic.AddValue(_errorSimilarityLine, value.ErrorSimilarity);
				_stallHeuristic.AddValue(_correctionProgressLine, value.CorrectionProgress);
			}
			if (!flag)
			{
				_stallHeuristic.AddValue(_errorSimilarityLine, _stallHeuristicReader.Recent.ErrorSimilarity);
				_stallHeuristic.AddValue(_correctionProgressLine, _stallHeuristicReader.Recent.CorrectionProgress);
			}
			flag = false;
			NetworkTransformTrace.StallData value2;
			while (_stallReader.Read(out value2))
			{
				_stallHeuristic.AddValue(_accruedScoreLine, value2.StallProgress);
				flag = true;
			}
			if (!flag)
			{
				_stallHeuristic.AddValue(_accruedScoreLine, _stallReader.Recent.StallProgress);
			}
			NetworkTransformTrace.ForecastData value3;
			while (_forecastReader.Read(out value3))
			{
				_velocityCorrection.AddValue(_currentVelocityLine, value3.PreviousVelocity.magnitude);
				_velocityCorrection.AddValue(_desiredVelocityLine, value3.DesiredVelocity.magnitude);
				_velocityCorrection.AddValue(_lerpedVelocityLine, value3.NewVelocity.magnitude);
				_velocityCorrection.AddValue(_lerpAlphaLine, value3.LerpAlpha);
			}
			NetworkTransformTrace.CollisionEnterData value4;
			while (_collisionEnterReader.Read(out value4))
			{
				_collisionEnterHeuristic.AddValue(_impactSpeedLine, value4.ImpactAlignment);
				_collisionEnterHeuristic.AddValue(_totalSpeedLine, value4.RelativeVelocity);
			}
			_collisionEnterHeuristic.SetThresholdValue(_minImpactThreshold, physicsSettings.MinImpactfulCollisionAlignment);
		}

		public void RefreshView()
		{
			_velocityCorrection.RefreshDisplay();
			_stallHeuristic.RefreshDisplay();
			_collisionEnterHeuristic.RefreshDisplay();
		}
	}
}
