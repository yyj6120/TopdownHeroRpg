using System;

namespace SJ.GameServer.Service.GameService.Message
{
    public class AccountCreateRequest : RequestBase
    {
        public const string uri = UriBase.game + "CreateAccount";
        public string username;
        public string password;
        public string confirm;
    }

    public class AccountCreateResponse : ResponseBase
    {
        
    }
}
