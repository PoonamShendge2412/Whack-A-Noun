using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using MiniJSON; // Ensure you have MiniJSON or equivalent JSON parser

public static class WebServices
{
public static IEnumerator LogIn(string email, string password, string schoolId, System.Action<Dictionary<string, object>> onComplete)
{
    //string schoolId = "HKR-01"; // You can make this dynamic if needed
    string purl = "https://metamersiveedu-backend.onrender.com/auth/signin";

    Dictionary<string, string> param = new()
    {
        { "email", email },
        { "password", password },
        { "schoolId", schoolId }
    };

    string jsonString = Json.Serialize(param);
    UnityWebRequest request = new UnityWebRequest(purl, "POST");
    byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");

    yield return request.SendWebRequest();

    string responseText = request.downloadHandler.text;

    if (request.result != UnityWebRequest.Result.Success)
    {
        Debug.LogError("Request failed: " + request.error);
        Debug.LogError("Response: " + responseText);
        onComplete?.Invoke(null);
        yield break;
    }

    Debug.Log("Login response: " + responseText);

    try
    {
        object result = Json.Deserialize(responseText);
        if (result is Dictionary<string, object> dict)
        {
            onComplete?.Invoke(dict);
        }
        else
        {
            Debug.LogError("JSON is not a dictionary.");
            onComplete?.Invoke(null);
        }
    }
    catch (System.Exception ex)
    {
        Debug.LogError("JSON parse error: " + ex.Message);
        onComplete?.Invoke(null);
    }
}

}