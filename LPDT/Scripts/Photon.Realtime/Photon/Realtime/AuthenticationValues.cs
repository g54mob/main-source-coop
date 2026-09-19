using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Photon.Realtime
{
	public class AuthenticationValues
	{
		private CustomAuthenticationType authType = CustomAuthenticationType.None;

		public CustomAuthenticationType AuthType
		{
			get
			{
				return authType;
			}
			set
			{
				authType = value;
			}
		}

		public string AuthGetParameters { get; set; }

		public object AuthPostData { get; private set; }

		protected internal object Token { get; set; }

		public string UserId { get; set; }

		public AuthenticationValues()
		{
		}

		public AuthenticationValues(string userId)
		{
			UserId = userId;
		}

		public virtual void SetAuthPostData(string stringData)
		{
			AuthPostData = (string.IsNullOrEmpty(stringData) ? null : stringData);
		}

		public virtual void SetAuthPostData(byte[] byteData)
		{
			AuthPostData = byteData;
		}

		public virtual void SetAuthPostData(Dictionary<string, object> dictData)
		{
			AuthPostData = dictData;
		}

		public virtual void AddAuthParameter(string key, string value)
		{
			string text = (string.IsNullOrEmpty(AuthGetParameters) ? "" : "&");
			AuthGetParameters = $"{AuthGetParameters}{text}{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}";
		}

		public virtual bool AreValid()
		{
			switch (authType)
			{
			case CustomAuthenticationType.Steam:
				return AuthGetParametersContain("ticket");
			case CustomAuthenticationType.Facebook:
			case CustomAuthenticationType.NintendoSwitch:
			case CustomAuthenticationType.Epic:
			case CustomAuthenticationType.FacebookGaming:
				return AuthGetParametersContain("token");
			case CustomAuthenticationType.Oculus:
				return AuthGetParametersContain("userid", "nonce");
			case CustomAuthenticationType.PlayStation4:
			case CustomAuthenticationType.PlayStation5:
				return AuthGetParametersContain("userName", "token", "env");
			case CustomAuthenticationType.Xbox:
				return AuthPostData != null;
			case CustomAuthenticationType.Viveport:
				return AuthGetParametersContain("userToken");
			default:
				return true;
			}
		}

		public bool AuthGetParametersContain(params string[] keys)
		{
			if (string.IsNullOrEmpty(AuthGetParameters))
			{
				return false;
			}
			if (keys == null)
			{
				return true;
			}
			foreach (string text in keys)
			{
				string pattern = ".*" + text + "=\\w+";
				if (!Regex.IsMatch(AuthGetParameters, pattern))
				{
					return false;
				}
			}
			return true;
		}

		public override string ToString()
		{
			return string.Format("AuthenticationValues = AuthType: {0} UserId: {1}{2}{3}{4}", AuthType, UserId, string.IsNullOrEmpty(AuthGetParameters) ? " GetParameters: yes" : "", (AuthPostData == null) ? "" : " PostData: yes", (Token == null) ? "" : " Token: yes");
		}

		public AuthenticationValues CopyTo(AuthenticationValues copy)
		{
			copy.AuthType = AuthType;
			copy.AuthGetParameters = AuthGetParameters;
			copy.AuthPostData = AuthPostData;
			copy.UserId = UserId;
			return copy;
		}
	}
}
