using Mirror.RemoteCalls;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.AutoLANClientController
{
	public class NetworkSceneScript : NetworkBehaviour
	{
		public Button clientButton;

		public Text textResult;

		public GameObject panelClient;

		public GameObject panelServer;

		private void Start()
		{
			clientButton.onClick.AddListener(ClientButton);
			panelServer.SetActive(value: false);
			panelClient.SetActive(value: false);
			if (base.isServer)
			{
				panelServer.SetActive(value: true);
			}
			if (base.isClient)
			{
				panelClient.SetActive(value: true);
			}
		}

		private void ClientButton()
		{
			if (base.isClient)
			{
				CmdSendText("Text: " + Random.Range(0, 999));
			}
		}

		[Command(requiresAuthority = false)]
		public void CmdSendText(string _value)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteString(_value);
			SendCommandInternal("System.Void Mirror.Examples.AutoLANClientController.NetworkSceneScript::CmdSendText(System.String)", 1114031274, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSendText__String(string _value)
		{
			textResult.text = _value;
		}

		protected static void InvokeUserCode_CmdSendText__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSendText called on client.");
			}
			else
			{
				((NetworkSceneScript)obj).UserCode_CmdSendText__String(reader.ReadString());
			}
		}

		static NetworkSceneScript()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkSceneScript), "System.Void Mirror.Examples.AutoLANClientController.NetworkSceneScript::CmdSendText(System.String)", InvokeUserCode_CmdSendText__String, requiresAuthority: false);
		}
	}
}
