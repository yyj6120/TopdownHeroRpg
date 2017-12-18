
namespace SJ.GameServer.Service
{
    public enum CommonErrorCode
    {
        None = 0,        // 에러가 아니다

        Server_Status_Pathcing = 1,    // 서버 상태 패치 중
        Server_Status_Stop = 2,

        Server_Error = 11,

        User_Permission = 22,    // 유저 권한이 낮음        

        Client_TrumpedUp_Data = 31,

        Reload_GameData_Permission_IP = 41,    // (관리자용)허용 IP가 아님
        Reload_GameData_Set_Time = 42,

        Prev_Request_Not_Complete = 61,   // 이전 요청이 끝나지 않았다

        Auth_Fail_Wrong_Username_PW = 101,
        Auth_Fail_No_User,
        Auth_Fail_Not_Registered_Username,
        Auth_Fail_Expiration_AuthToken,
        Auth_Fail_Mismatch_Confirm_Password,
        Auth_Fail_Mismatch_Client_Ver,

        Auth_Add_User_Fail = 150,
        Auth_Add_Company_Fail,
        Auth_Add_Mission_Quest_Fail,
        Auth_Add_Daily_Login_Event_Fail,
        Auth_Add_Accrue_Login_Event_Fail,
        Auth_Daily_Quest_Fail,

        Redis_Unknown = 10100,
        Redis_Expired_Token = 10101,

        LastIndex
    }
}
