using System;

namespace SJ.GameServer.Service
{
    public static class UriBase
    {
        public const string game = "GameService.svc/";
    }

    public abstract class RequestBase
    {
        [Serializable]
        public class AuthInfo
        {
            public string username;
            public string authToken;
        }

        public AuthInfo authInfo;
    }

    public abstract class ResponseBase
    {
        public int serverCheckTime = 0;                                // 서버 점검을 할 시간(초단위)
        public int errorCode = (int)CommonErrorCode.None;
    }
}
