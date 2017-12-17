
namespace SJ.GameServer.Service.GameService.Message
{
    public class GemIncreaseRequest : RequestBase
    {
        public const string uri = UriBase.game + "IncreaseGem";
    }

    public class GemIncreaseResponse : ResponseBase
    {
        public int gem;
    }
}
