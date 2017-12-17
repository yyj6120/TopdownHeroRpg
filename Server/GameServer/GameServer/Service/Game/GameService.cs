using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using SJ.GameServer.DataAccess.Entity;
using SJ.GameServer.DataAccess.RDB;
using SJ.GameServer.DataAccess.Redis.Repository;
using SJ.GameServer.Service.ValueObjects;
using Dapper;

namespace SJ.GameServer.Service.GameService
{
    public class GameService : IGameService
    {
        public GameService()
        {
            GameServer.Instance.Initialize();
        }

        // 클라 -> 서버 -> DB
        // 클라 <- 서버 <- DB
        public Message.LoginResponse Login(Stream jsonRequestMessage)
        {
            Message.LoginRequest requestMessage = 
                (Message.LoginRequest)new DataContractJsonSerializer(typeof(Message.LoginRequest)).ReadObject(jsonRequestMessage);

            Message.LoginResponse responseMessage = new Message.LoginResponse();

            using (var connection = ConnectionFactory.GetConnection())
            {
                connection.Open();

                var account = connection.Query<Account>(
                    "select * from Account where username = @username", 
                    new{ username = requestMessage.username }).Single();

                if (account.password == requestMessage.password)
                {
                    responseMessage.accountVo = account.Map<AccountVo>();
                    responseMessage.authToken = CreateSession(account.accountId, account.username).authToken;
                }
                else
                {
                    responseMessage.errorCode = (int)CommonErrorCode.Auth_Fail_Wrong_Username_PW;
                }
            }

            return responseMessage;
        }

        public Message.GemIncreaseResponse IncreaseGem(Stream jsonRequestMessage)
        {
            Message.GemIncreaseRequest requestMessage = (Message.GemIncreaseRequest)new DataContractJsonSerializer(typeof(Message.GemIncreaseRequest)).ReadObject(jsonRequestMessage);
            Message.GemIncreaseResponse responseMessage = new Message.GemIncreaseResponse();
            Session session = null;

            if (!Authentication.IsValidToken(ref session, requestMessage.authInfo.username, requestMessage.authInfo.authToken))
            {
                responseMessage.errorCode = (int)CommonErrorCode.Auth_Fail_Expiration_AuthToken;
                return responseMessage;
            }

            using (var connection = ConnectionFactory.GetConnection())
            {
                connection.Open();

                var account = connection.Query<Account>(
                    "select * from Account where username = @username",
                    new
                    {
                        username = requestMessage.authInfo.username
                    }).Single();

                int gem = account.gem;
                gem++;

                connection.Execute("update Account set gem = @gem where username = @username",
                    new
                    {
                        gem = gem,
                        username = requestMessage.authInfo.username
                    });

                responseMessage.gem = gem;
            }

            return responseMessage;
        }

        protected Session CreateSession(long accountId, string username)
        {
            var newSession = new Session()
            {
                accountId = accountId,
                username = username,
                // sagolboss : 일회적으로 사용되는 값이므로 System.Guid를 사용하지 않고 간단한 Guid를 사용한다.
                authToken = Authentication.GenerateSecureNumber2(1, 9, 8),
            };

            SessionRepository sessionRepository = new SessionRepository();
            sessionRepository.Add(newSession.username, newSession);

            return newSession;
        }
    }
}
