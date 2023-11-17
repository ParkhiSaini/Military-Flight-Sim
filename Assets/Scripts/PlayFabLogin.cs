using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private GameObject CreateAccountDisplay = default;
    [SerializeField] public TMP_InputField reg_usernameInputField = default;
    [SerializeField] private TMP_InputField reg_emailInputField = default;
    [SerializeField] private TMP_InputField reg_passwordInputField = default;
    [SerializeField] private TextMeshProUGUI errorMessage;
    public TMP_InputField log_usernameInputField = default;
    [SerializeField] private TMP_InputField log_passwordInputField = default;
    [SerializeField] private GameObject signinDisplay;
    public static string SessionTicket;
    public static string EntityId;
    public static string LoginUsername;

    public static PlayFabLogin Instance { get; private set; }

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

    public void CreateAccount()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = reg_usernameInputField.text,
            Email = reg_emailInputField.text,
            Password = reg_passwordInputField.text
        }, result =>
        {
            SessionTicket = result.SessionTicket;
            EntityId = result.EntityToken.Entity.Id;
            CreateAccountDisplay.SetActive(false);
            signinDisplay.SetActive(true);
        }, error =>
        {
            if (error.Error == PlayFabErrorCode.InvalidParams && error.ErrorDetails.ContainsKey("Password"))
            {
                errorMessage.text = "Error: Password should be between 6-10 characters.";
            }

            else
            {
                errorMessage.text = "Error: " + error.ErrorMessage;
            }
            Debug.LogError(error.GenerateErrorReport());
        });
    }

    public void SignIn()
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = log_usernameInputField.text,
            Password = log_passwordInputField.text
        }, result =>
        {
            SessionTicket = result.SessionTicket;
            EntityId = result.EntityToken.Entity.Id;
            LoginUsername = log_usernameInputField.text;
            signinDisplay.SetActive(false);
            SceneManager.LoadScene("MainMenu");
        }, error =>
        {
            if (error.Error == PlayFabErrorCode.InvalidParams && error.ErrorDetails.ContainsKey("Username"))
            {
                errorMessage.text = "Error: Incorrect username.";
            }
            else if (error.Error == PlayFabErrorCode.InvalidParams && error.ErrorDetails.ContainsKey("Password"))
            {
                errorMessage.text = "Error: Incorrect password.";
            }
            else
            {
                errorMessage.text = "Error: " + error.ErrorMessage;
            }
            Debug.LogError(error.GenerateErrorReport());
        });
    }

    public void HaveAnAccount()
    {
        CreateAccountDisplay.SetActive(false);
        signinDisplay.SetActive(true);
    }
}