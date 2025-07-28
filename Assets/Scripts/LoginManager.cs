using UnityEngine;
using TMPro;
using System.Collections;
using MiniJSON;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public GameObject loginPanel, mainMenuPanel;
    public TMP_InputField gmailInput, passwordInput, classIdInput;
    public TMP_Text error;
    public void OnLoginClicked1()
    {
        string gmail = gmailInput.text.Trim();
        string password = passwordInput.text.Trim();
        string classId = classIdInput.text.Trim();

        if (string.IsNullOrEmpty(gmail) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(classId))
        {
            StartCoroutine(OnError("Please input email!"));
            Debug.LogWarning("Please fill in all fields.");
            return;
        }

        Debug.Log($"Logged in as: {gmail} | Class: {classId}");

        // Transition to main menu
        // loginPanel.SetActive(false);
        // mainMenuPanel.SetActive(true);
    }




    void Start()
    {
        // PlayerPrefs.DeleteAll();
        error.gameObject.SetActive(false);
        gmailInput.text = PlayerPrefs.GetString("email", "");
        passwordInput.text = PlayerPrefs.GetString("password", "");
        classIdInput.text = PlayerPrefs.GetString("classId", "");

    }

    public void OnQuit()
    {
        Debug.LogError("applicationquit");
        Application.Quit();
    
   }

    public void OnLoginClicked()
    {
        if (string.IsNullOrEmpty(gmailInput.text) || string.IsNullOrEmpty(passwordInput.text) || string.IsNullOrEmpty(classIdInput.text))
        {
            StartCoroutine(OnError("Please input email!"));
            Debug.LogWarning("Please fill in all fields.");
            return;
        }
        StartCoroutine(WebServices.LogIn(gmailInput.text, passwordInput.text,classIdInput.text, (dicData) =>
        {
        if (dicData == null)
        {
            Debug.LogError("Login failed or invalid response.");
            return;
        }

        if (dicData.ContainsKey("state") && dicData["state"].ToString() == "success")
        {
            PlayerPrefs.SetString("email", gmailInput.text);
            PlayerPrefs.SetString("password", passwordInput.text);
            PlayerPrefs.SetString("classId", classIdInput.text);
            Debug.Log("Login Success!");
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogError("Login failed: " + Json.Serialize(dicData));
        }
    }));

    }


    IEnumerator OnError(string msg)
    {
        //error.text = "You input wrong email or password.Please try again"+"!";//msg;
        error.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        error.gameObject.SetActive(false);
    }
}



