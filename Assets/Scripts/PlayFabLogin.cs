using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private TMP_InputField log_usernameInputField = default;
    [SerializeField] private TMP_InputField log_passwordInputField = default;
    [SerializeField] private TextMeshProUGUI errorMessage;
    [SerializeField] private GameObject signinDisplay;

    public static string SessionTicket;
    public static string EntityId;
    public static string LoginUsername;

    public static PlayFabLogin Instance { get; private set; }

    private bool isFirstLogin = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SignIn()
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = log_usernameInputField.text,
            Password = log_passwordInputField.text,
            CreateAccount = isFirstLogin // Set to true only for the first login
        }, result =>
        {
            SessionTicket = result.SessionTicket;
            EntityId = result.EntityToken.Entity.Id;
            LoginUsername = log_usernameInputField.text;
            signinDisplay.SetActive(false);
            SceneManager.LoadScene("MainMenu");

            // Successful login, you can print a message or perform additional actions if needed.
            Debug.Log("Successfully logged in!");

            // Set the flag to false after the first login
            isFirstLogin = false;
        }, error =>
        {
            // Handle login errors
            if (error.Error == PlayFabErrorCode.InvalidParams && error.ErrorDetails.ContainsKey("Username"))
            {
                errorMessage.text = "Error: Incorrect username.";
            }
            else if (error.Error == PlayFabErrorCode.InvalidParams && error.ErrorDetails.ContainsKey("Password"))
            {
                errorMessage.text = "Error: Incorrect password.";
            }
            else if (error.Error == PlayFabErrorCode.AccountNotFound)
            {
                errorMessage.text = "Error: You are not a registered user. Please check your credentials.";
            }
            else
            {
                errorMessage.text = "Error: " + error.ErrorMessage;
            }
            Debug.LogError(error.GenerateErrorReport());
        });
    }
}
