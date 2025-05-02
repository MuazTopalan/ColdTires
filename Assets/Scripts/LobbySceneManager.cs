using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class LobbySceneManager : MonoBehaviour
{
    public Text lobbyCodeText;
    public GameObject playerListContent;
    public GameObject playerPrefab;
    public Button startButton; // Start button for host
    public Button leaveButton; // Leave button for players
    private Lobby currentLobby;

    // Dictionary to store player names and readiness states
    Dictionary<string, string> playerNames = new Dictionary<string, string>();  // Player ID -> Name
    Dictionary<string, bool> playerReadiness = new Dictionary<string, bool>(); // Player ID -> IsReady

    private async void Start()
    {
        // Fetch current lobby (make sure to properly get it)
        currentLobby = await LobbyService.Instance.GetLobbyAsync(currentLobby.Id);

        // Display the lobby code in the text field
        if (lobbyCodeText != null && currentLobby != null)
        {
            lobbyCodeText.text = "Lobby Code: " + currentLobby.LobbyCode;
        }

        // Set up start button for the host
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
            startButton.gameObject.SetActive(IsHost()); // Show the button only for the host
        }

        // Set up leave button for all players
        if (leaveButton != null)
        {
            leaveButton.onClick.AddListener(LeaveLobby);
        }

        // Populate the player list
        UpdatePlayerList();
    }

    private void UpdatePlayerList()
    {
        // Clear current player list UI
        foreach (Transform child in playerListContent.transform)
        {
            Destroy(child.gameObject);
        }

        // Create a UI entry for each player in the lobby
        foreach (var player in currentLobby.Players)
        {
            var playerUI = Instantiate(playerPrefab);
            playerUI.transform.SetParent(playerListContent.transform, false);

            var playerNameText = playerUI.GetComponentInChildren<Text>();
            string playerId = player.Id; // Get player ID
            playerNameText.text = playerNames.ContainsKey(playerId) ? playerNames[playerId] : "Player " + playerId; // Display player name

            // Check if the player is ready
            var readyButton = playerUI.GetComponentInChildren<Button>();
            bool isReady = playerReadiness.ContainsKey(playerId) && playerReadiness[playerId];
            readyButton.interactable = !isReady; // Allow interaction only if the player is not ready
            readyButton.GetComponentInChildren<Text>().text = isReady ? "Ready" : "Not Ready"; // Display the readiness state
        }
    }

    private bool IsHost()
    {
        // Check if the current player is the host by comparing PlayerId with the HostId
        return currentLobby.HostId == AuthenticationService.Instance.PlayerId;  // Compare the current player's ID with the lobby's host ID
    }

    public void StartGame()
    {
        // Check if all players are ready before starting the game
        if (AllPlayersReady())
        {
            // Start the game for all players
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.Log("Not all players are ready.");
        }
    }

    private bool AllPlayersReady()
    {
        // Iterate through all players in the lobby and check if they are ready
        foreach (var player in currentLobby.Players)
        {
            // Custom check for player readiness (implement your own logic)
            if (!playerReadiness.ContainsKey(player.Id) || !playerReadiness[player.Id]) // Replace with the actual check
            {
                return false;
            }
        }
        return true;
    }

    public async void LeaveLobby()
    {
    try
    {
        // Use LobbyService.Instance to call LeaveLobbyAsync instead of ILobbyService
        await LobbyService.Instance.LeaveLobbyAsync(currentLobby.Id);  // Correct method call here
        SceneManager.LoadScene("MainMenu"); // Load the main menu or another scene
    }
    catch (System.Exception ex)
    {
        Debug.LogError($"Failed to leave lobby: {ex.Message}");
    }
    }

}
