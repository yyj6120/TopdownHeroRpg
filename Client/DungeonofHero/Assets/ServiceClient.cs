using UnityEngine;
using System;
using System.Text;
using BestHTTP;
using Newtonsoft.Json;
using SJ.GameServer.Service;

public static class ServiceAuthentication
{
    public static RequestBase.AuthInfo authInfo = new RequestBase.AuthInfo();

    public static void SetAuthInfo(RequestBase.AuthInfo authInfo)
    {
        ServiceAuthentication.authInfo.username = authInfo.username;
        ServiceAuthentication.authInfo.authToken = authInfo.authToken;
    }
}

public class SJServiceClient<TRequest, TResponse> where TRequest : RequestBase 
                                                  where TResponse : ResponseBase
{
    private bool usingActivityIndicator;
    private OnSuccessResponseDelegate onSuccessResponse;
    private OnErrorResponseDelegate onErrorResponse;

    public delegate void OnSuccessResponseDelegate(TResponse response);
    public delegate bool OnErrorResponseDelegate(TResponse response);

    private const string url = "http://localhost:10302/"; // + GameService.svc/Login

    public void Post(string uri, TRequest data, OnSuccessResponseDelegate onSuccessResponse, OnErrorResponseDelegate onErrorResponse, bool usingActivityIndicator = true)
    {
        var request = new HTTPRequest(new Uri(url + uri), HTTPMethods.Post, OnRequestFinished);
        request.RawData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        request.Send();

        this.onSuccessResponse = onSuccessResponse;
        this.onErrorResponse = onErrorResponse;
        this.usingActivityIndicator = usingActivityIndicator;

        if (usingActivityIndicator)
        {
            // 처리 중 인디케이트 처리
        }
    }

	public void AuthPost(string uri, TRequest data, OnSuccessResponseDelegate onSuccessResponse, OnErrorResponseDelegate onErrorResponse, bool usingActivityIndicator = true)
	{
		data.authInfo = ServiceAuthentication.authInfo;
		Post(uri, data, onSuccessResponse, onErrorResponse, usingActivityIndicator);
	}

    private void OnRequestFinished(HTTPRequest originalRequest, HTTPResponse httpResponse)
    {
        if (usingActivityIndicator)
        {
            // 처리 중 인디케이트 처리
        }

        if (httpResponse.IsSuccess)
        {
            var response = JsonConvert.DeserializeObject<TResponse>(httpResponse.DataAsText);
            if (response.errorCode == (int)CommonErrorCode.None)
            {
                if (onSuccessResponse != null)
                    onSuccessResponse(response);
            }
            else
                Error(response);
        }
        else
        {
            Debug.LogError("Response Error : requestUri = " + originalRequest.Uri);
            // 에러 메시지 박스 처리 
        }
    }

    private void Error(TResponse response)
	{
        if (onErrorResponse != null && onErrorResponse(response))
            return;

        switch (response.errorCode)
		{
            case (int)CommonErrorCode.Redis_Expired_Token:
                // 에러 메시지 박스 처리 
                break;

            default:
                // 에러 메시지 박스 처리 
                break;
        }
    }
}
