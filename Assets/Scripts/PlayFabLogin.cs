using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private GameObject signInDisplay = default;
    [SerializeField] public TMP_InputField usernameInputField = default;
    [SerializeField] private TMP_InputField emailInputField = default;
    [SerializeField] private TMP_InputField passwordInputField = default;

    public static string SessionTicket;
    public static string EntityId;

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
            Username = usernameInputField.text,
            Email = emailInputField.text,
            Password = passwordInputField.text
        }, result =>
        {
            SessionTicket = result.SessionTicket;
            EntityId = result.EntityToken.Entity.Id;
            signInDisplay.SetActive(false);
        }, error =>
        {
            Debug.LogError(error.GenerateErrorReport());
        });
    }

    public void SignIn()
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = usernameInputField.text,
            Password = passwordInputField.text
        }, result =>
        {
            SessionTicket = result.SessionTicket;
            EntityId = result.EntityToken.Entity.Id;
            signInDisplay.SetActive(false);
            SceneManager.LoadScene("RoomMenu");
        }, error =>
        {
            Debug.LogError(error.GenerateErrorReport());
        });
    }
}