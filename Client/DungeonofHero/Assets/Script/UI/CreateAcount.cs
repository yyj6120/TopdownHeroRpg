using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using SJ.GameServer.Service;
using SJ.GameServer.Service.GameService.Message;
using System;

public class CreateAcount : MonoBehaviour
{
    public InputField usernameLabel, passwordLabel, confirmLabel;

    public void OnCreateClicked()
    {
        var username = usernameLabel.text;
        var password = passwordLabel.text;
        var confirm = confirmLabel.text;

        var request = new AccountCreateRequest();

        request.username = username;
        request.password = password;
        request.confirm = confirm;

        var serviceClient = new SJServiceClient<AccountCreateRequest, AccountCreateResponse>();
        serviceClient.Post(AccountCreateRequest.uri, request,
            (response)
            =>
            {
                MainUI.s_Instance.ShowInfoPopup("계정 생성 완료.");
                MainUI.s_Instance.ShowLoginPanel();
            },
            Error);
    }

    protected bool Error(AccountCreateResponse response)
    {
        switch (response.errorCode)
        {
            case (int)CommonErrorCode.Auth_Add_User_Fail:
                MainUI.s_Instance.ShowInfoPopup("이미 등록된 유저 입니다. Auth_Add_User_Fail");
                return true;

            case (int)CommonErrorCode.Auth_Fail_Not_Registered_Username:
                MainUI.s_Instance.ShowInfoPopup("레지스터에 등록시킬수없는 유저 이름입니다. Auth_Fail_Not_Registered_Username");
                return true;

            case (int)CommonErrorCode.Auth_Fail_Mismatch_Confirm_Password:
                MainUI.s_Instance.ShowInfoPopup("재확인 비밀번호가 맞지 않습니다. Auth_Fail_Mismatch_Confirm_Password");
                return true;
        }
        return false;
    }

    public void OnBackClicked()
    {
        MainUI.s_Instance.ShowLoginPanel();
    }
}

