using System;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Authentication;
using DeveloperTools;
using System.Threading.Tasks;

public class UnityServicesManager : MonoBehaviour
{
    MainMenuManager mainMenu;
    private void Awake()
    {
        mainMenu = DevTools.FindGameObject("MainMenu").GetComponent<MainMenuManager>();
        PlayerPrefs.SetString("Test", "Testicles");
    }
        
    void Start()
    {
        _ = startUnityServices();
    }

    private async Task startUnityServices()
    {
        try
        {
            await UnityServices.InitializeAsync(new InitializationOptions().SetEnvironmentName("production"));
            setupEvents();


            if (AuthenticationService.Instance.SessionTokenExists)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                Debug.Log("Logging in with cached Session Token: " + AuthenticationService.Instance.AccessToken);
                mainMenu.displayProfile(AuthenticationService.Instance.Profile);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public async void loginAnonymously()
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        

    }
    private void setupEvents()
    {
        AuthenticationService.Instance.SignedIn += () => 
        {
            Debug.Log("Signed In as:"+"\nPlayerID: " + AuthenticationService.Instance.PlayerId +"\nProfile: " + AuthenticationService.Instance.Profile);
            mainMenu.displayProfile(AuthenticationService.Instance.Profile);

        };
        AuthenticationService.Instance.SignedOut += () => 
        {
            Debug.Log("Signed Out as:" + "\nPlayerID: " + AuthenticationService.Instance.PlayerId + "\nProfile: " + AuthenticationService.Instance.Profile);
            mainMenu.clearProfile();
        };
        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
            mainMenu.clearProfile();

        };
        AuthenticationService.Instance.SignInFailed += (RequestFailedException exception) =>
        {
            DevTools.LogException(exception, "Failed To Log In");
        };
    }
}
