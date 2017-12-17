using SJ.GameServer.Service.ValueObjects;

namespace SJ.GameServer.Service.GameService.Message
{
    public class LoginRequest : RequestBase
    {
        public const string uri = UriBase.game + "Login";

        public string username;
        public string password;
        public short clientVersion;
        public short dataVersion;
        public short marketType;
    }

    public class LoginResponse : ResponseBase
    {
        public string authToken;
        public AccountVo accountVo;
    }
}
