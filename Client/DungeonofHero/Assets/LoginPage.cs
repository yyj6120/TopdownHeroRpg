//using UnityEngine;
//using SJ.GameServer.Service;
//using SJ.GameServer.Service.GameService.Message;

//public class LoginPage : MonoBehaviour
//{
//    public UILabel usernameLabel;
//    public UILabel passwordLabel;
//    public UILabel gemCountLabel;

//    public void Login()
//    {
//        var username = usernameLabel.GetComponent<UIInput>().value;
//        var password = passwordLabel.GetComponent<UIInput>().value;

//        var request = new LoginRequest();

//        request.username = username;
//        request.password = password;
//        request.clientVersion = 0;
//        request.dataVersion = 0;
//        request.marketType = 0;

//        var serviceClient = new SJServiceClient<LoginRequest, LoginResponse>();
//        serviceClient.Post(LoginRequest.uri, request,
//            (response) =>
//            {
//                Debug.Log("정상적으로 로그인 되었습니다");

//                gemCountLabel.text = response.accountVo.gem.ToString();
//                ServiceAuthentication.SetAuthInfo(new RequestBase.AuthInfo()
//                {
//                    username = request.username,
//                    authToken = response.authToken
//                });
//            },
//            Error);
//    }

//    protected bool Error(LoginResponse response)
//    {
//        switch (response.errorCode)
//        {
//            case (int)CommonErrorCode.Auth_Fail_Wrong_Username_PW:
//                Debug.Log("유저이름과 비밀번호가 맞지 않습니다");
//                return true;

//            case (int)CommonErrorCode.Auth_Fail_Not_Registered_Username:
//                Debug.Log("등록 되지 않은 유저이름 입니다");
//                return true;

//            case (int)CommonErrorCode.Auth_Fail_Expiration_AuthToken:
//                Debug.Log("토큰이 만료 되었습니다");
//                return true;

//            case (int)CommonErrorCode.Auth_Fail_Mismatch_Client_Ver:
//                Debug.Log("맞지 않는 버전입니다");
//                return true;
//        }

//        return false;
//    }
//}
