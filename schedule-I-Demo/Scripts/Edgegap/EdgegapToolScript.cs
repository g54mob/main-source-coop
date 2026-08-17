using Edgegap;
using IO.Swagger.Model;
using UnityEngine;

public class EdgegapToolScript : MonoBehaviour
{
	public Status ServerStatus => EdgegapServerDataManager.GetServerStatus();
}
