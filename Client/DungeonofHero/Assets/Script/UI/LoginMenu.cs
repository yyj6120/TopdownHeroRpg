using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using SJ.GameServer.Service;
using SJ.GameServer.Service.GameService.Message;

public class LoginMenu : MonoBehaviour
{
    public GameObject serverCanvas, clientCanvas;
    public ClientHUD clientHudScript;
    public ServerHUD serverHUDScript;

    public InputField usernameLabel , passwordLabel;

    public void Login()
    {
        var username = usernameLabel.text;
        var password = passwordLabel.text;

        var request = new LoginRequest();

        request.username = username;
        request.password = password;
        request.clientVersion = 0;
        request.dataVersion = 0;
        request.marketType = 0;

        var serviceClient = new SJServiceClient<LoginRequest, LoginResponse>();
        serviceClient.Post(LoginRequest.uri, request,
            (response) =>
            {
                Debug.Log("정상적으로 로그인 되었습니다");

                //gemCountLabel.text = response.accountVo.gem.ToString();
                ServiceAuthentication.SetAuthInfo(new RequestBase.AuthInfo()
                {
                    username = request.username,
                    authToken = response.authToken
                });
                StartClient();
            },
            Error);
    }

    protected bool Error(LoginResponse response)
    {
        switch (response.errorCode)
        {
            case (int)CommonErrorCode.Auth_Fail_Wrong_Username_PW:
                Debug.Log("유저이름과 비밀번호가 맞지 않습니다");
                return true;

            case (int)CommonErrorCode.Auth_Fail_Not_Registered_Username:
                Debug.Log("등록 되지 않은 유저이름 입니다");
                return true;

            case (int)CommonErrorCode.Auth_Fail_Expiration_AuthToken:
                Debug.Log("토큰이 만료 되었습니다");
                return true;

            case (int)CommonErrorCode.Auth_Fail_Mismatch_Client_Ver:
                Debug.Log("맞지 않는 버전입니다");
                return true;
        }

        return false;
    }

    public void StartServer()
    {
        Debug.Log("StartServer");
        serverHUDScript.enabled = true;
        serverCanvas.SetActive(true);
        SceneManager.LoadScene("LobbyScene");
        
    }

    public void StartClient()
    {
        Debug.Log("StartClient");
        clientHudScript.enabled = true;
        clientCanvas.SetActive(true);
        SceneManager.LoadScene("LobbyScene");
    }

    public void OnBackClicked()
    {
        MainUI.s_Instance.ShowSplashPanel();
    }

    public void OnCreateAcountPressed()
    {
        MainUI.s_Instance.ShowCreateAccount();
    }
}
