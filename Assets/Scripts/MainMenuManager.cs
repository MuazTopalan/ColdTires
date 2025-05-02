using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;

public class MainMenuManager : MonoBehaviour
{
    public Button createLobbyButton;
    public Button joinLobbyButton;
    public InputField joinCodeInputField;

    private Lobby currentLobby;
    private const string LOBBY_NAME = "RacingLobby";
    private const int MAX_PLAYERS = 4;

    private async void Start()
    {
        Debug.Log("MainMenuManager Started");

        await UnityServices.InitializeAsync();
        await SignInAnonymously();

        // Assign button functions
        createLobbyButton.onClick.AddListener(CreateLobby);
        joinLobbyButton.onClick.AddListener(JoinLobby);
    }

    private async Task SignInAnonymously()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
        }
    }

    public async void CreateLobby()
    {
        Debug.Log("Create Lobby button clicked");
        try
        {
            await LobbyManager.Instance.CreateLobby(LOBBY_NAME, MAX_PLAYERS);
            Debug.Log($"Lobby Created: {LobbyManager.Instance.CurrentLobby.Id}");

            // Load the Lobby Scene
            SceneManager.LoadScene("LobbyScene");
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError("Lobby Error: " + ex.Message);
        }
    }

    public async void JoinLobby()
    {
        Debug.Log("Join Lobby button clicked");
        try
        {
            string joinCode = joinCodeInputField.text;
            if (string.IsNullOrEmpty(joinCode))
            {
                Debug.LogError("Join code is empty!");
                return;
            }
            await LobbyManager.Instance.JoinLobby(joinCode);
            Debug.Log($"Joined lobby with code: {joinCode}");

            // Load the Lobby Scene after joining
            SceneManager.LoadScene("LobbyScene");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to join lobby: {e.Message}");
        }
    }
}
