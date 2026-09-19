using System;

namespace Features.StoreModule.Scripts
{
	public class ActiveStoreModel
	{
		private StoreEntity _activeStoreEntity;

		public StoreEntity ActiveStoreEntity
		{
			get
			{
				return _activeStoreEntity;
			}
			set
			{
				_activeStoreEntity = value;
				this.OnActiveStoreEntityChanged?.Invoke(_activeStoreEntity);
			}
		}

		public event Action<StoreEntity> OnActiveStoreEntityChanged;
	}
}
