using System.Web.Security;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;

namespace Gwent.NET.Webservice.Auth
{
    public class MachineKeyProtector : IDataProtector
    {
        private readonly string[] _purpose =
        {
            typeof (OAuthAuthorizationServerMiddleware).Namespace,
            "Access_Token",
            "v1"
        };

        public byte[] Protect(byte[] userData)
        {
            return MachineKey.Protect(userData, _purpose);
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return MachineKey.Unprotect(protectedData, _purpose);
        }
    }
}