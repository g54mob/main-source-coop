using System.Collections.Generic;
using Features.GrabModule.Scripts.Tips;
using Features.TipsModule.Scripts.Data;
using UnityEngine;

namespace Features.CartCannonModule.Scripts
{
	public class CartCannonTipsReactor : GrabbableTipsReactorBase
	{
		[SerializeField]
		private CartCannonWeapon _weapon;

		[SerializeField]
		private List<TipType> _baseTips = new List<TipType>();

		[SerializeField]
		private List<TipType> _raycastTips = new List<TipType> { TipType.PickUpCartTip };

		private readonly List<TipType> _tipsBuffer = new List<TipType>();

		private bool _canShoot;

		public override IReadOnlyList<TipType> GetTips()
		{
			_canShoot = CanShoot();
			_tipsBuffer.Clear();
			_tipsBuffer.AddRange(_baseTips);
			if (_canShoot)
			{
				_tipsBuffer.Add(TipType.ShootTip);
			}
			return _tipsBuffer;
		}

		public override IReadOnlyList<TipType> GetRaycastTips()
		{
			return _raycastTips;
		}

		private void Update()
		{
			bool flag = CanShoot();
			if (flag != _canShoot)
			{
				_canShoot = flag;
				RaiseTipsChanged();
			}
		}

		private bool CanShoot()
		{
			if (_weapon != null)
			{
				return _weapon.CanShootLocally();
			}
			return false;
		}
	}
}
