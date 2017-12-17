using System.IO;
using System.ServiceModel;

namespace SJ.GameServer.Service.GameService
{
    [ServiceContract]
    public interface IGameService
    {
        [OperationContract]
        Message.LoginResponse Login(Stream jsonRequestMessage);

        [OperationContract]
        Message.GemIncreaseResponse IncreaseGem(Stream jsonRequestMessage);
    }
}