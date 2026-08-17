using UnityEngine;

namespace EvilCore.EvilSave
{
	public class SaveableId : MonoBehaviour
	{
		[SerializeField]
		private string _id;

		public string Id => _id;
	}
}
