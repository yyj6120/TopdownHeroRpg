//using UnityEngine;
//using SJ.GameServer.Service.GameService.Message;

//public class GemPage : MonoBehaviour
//{
//    public UIButton increaseButton;
//    public UIButton decreaseButton;
//    public UILabel gemCountLabel;

//    public void Increase()
//    {
//        var serviceClient = new SJServiceClient<GemIncreaseRequest, GemIncreaseResponse>();
//        var request = new GemIncreaseRequest();

//        serviceClient.AuthPost(GemIncreaseRequest.uri, request,
//            (response) =>
//            {
//                gemCountLabel.text = response.gem.ToString();
//            },
//            Error);
//    }

//    protected bool Error(GemIncreaseResponse response)
//    {
//        return false;
//    }

//    public void Decrease()
//    {
//    }
//}
