using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;

public class TestAuthentication : MonoBehaviour
{
    async void Start()
    {
        await UnityServices.InitializeAsync();
        await SignIn();
    }

    async Task SignIn()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Signed in! Player ID: " + AuthenticationService.Instance.PlayerId);
        }
        else
        {
            Debug.Log("Already signed in. Player ID: " + AuthenticationService.Instance.PlayerId);
        }
    }
}
