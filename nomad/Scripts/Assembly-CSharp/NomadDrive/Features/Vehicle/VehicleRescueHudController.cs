using EvilCore;
using EvilCore.Extensions;
using NomadDrive.Features.Inputs;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace NomadDrive.Features.Vehicle
{
	public class VehicleRescueHudController : MonoBehaviour
	{
		[SerializeField]
		private Image fillImage;

		[SerializeField]
		private float holdDuration = 1f;

		[Inject]
		private IVehicleRescueService _vehicleRescueService;

		private float _heldTime;

		private void Start()
		{
			base.gameObject.InjectGameObject();
			ResetFill();
		}

		private void Update()
		{
			if (_vehicleRescueService == null || !OnFootInputs.GetRescueVehicleButton())
			{
				ResetFill();
				return;
			}
			if (!_vehicleRescueService.CanRescueVehicle)
			{
				ResetFill();
				return;
			}
			_heldTime += Time.deltaTime;
			float num = ((holdDuration > 0f) ? Mathf.Clamp01(_heldTime / holdDuration) : 1f);
			if (fillImage != null)
			{
				fillImage.fillAmount = num;
			}
			if (num >= 1f)
			{
				ResetFill();
				_vehicleRescueService.RequestRescueVehicle();
			}
		}

		private void ResetFill()
		{
			_heldTime = 0f;
			if (fillImage != null)
			{
				fillImage.fillAmount = 0f;
			}
		}
	}
}
