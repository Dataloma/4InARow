using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using DeveloperTools;
using static UnityEditor.Progress;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class LobbyManager : NetworkBehaviour
{
    
    LobbyView lobbyView;
    private Lobby mylobbie = null;
    // Start is called before the first frame update
    void Awake()
    {
        lobbyView = DevTools.FindGameObject("MainMenu/LobbyView").GetComponent<LobbyView>();
        Debug.Log(lobbyView);
        Screen.SetResolution(1440, 900, FullScreenMode.MaximizedWindow, 60);
        Debug.Log("Res Set");
    }

    

    public async void quickJoin()
    {
        await leaveLobby();
        QuickJoinLobbyOptions options = new QuickJoinLobbyOptions();
        options.Filter = new List<QueryFilter>();
        
        try
        {
            mylobbie = await Lobbies.Instance.QuickJoinLobbyAsync(options);
            
            Debug.Log("Joined Lobby: " + mylobbie.Id);
            lobbyView.displayLobbyInfo(mylobbie);
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log("Error:"+ ex.Message);
            Debug.LogException(ex);
        }
    }


    public async void createLobby() 
    {
        var options = new CreateLobbyOptions();
        options.IsPrivate = false;
        try
        {
            mylobbie = await Lobbies.Instance.CreateLobbyAsync("TestLobby", 2, options);
            InvokeRepeating("heartbeat", 0f, 27f);
            Debug.Log("Joined Lobby: " + mylobbie.Id);
            lobbyView.displayLobbyInfo(mylobbie);
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex.Message);
            Debug.LogError(ex);
        }
    }

    public async void getLobbies()
    {
        var options = new QueryLobbiesOptions();
        options.Count = 10;
        

        try
        {
            var lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);
            lobbyView.displayLobbies(lobbies.Results);
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public async void joinSelectedLobby()
    {
        await leaveLobby();
        await Lobbies.Instance.JoinLobbyByIdAsync(lobbyView.selectedLobby.Id);
        Debug.Log("Joined Lobby: " + lobbyView.selectedLobby.Id);
    }

    private void heartbeat()
    {
        LobbyService.Instance.SendHeartbeatPingAsync(mylobbie.Id);
        Debug.Log("IT WORKS!!!");
    }
    private async Task leaveLobby()
    {
        if (mylobbie != null){
            await LobbyService.Instance.RemovePlayerAsync(mylobbie.Id, AuthenticationService.Instance.PlayerId);
            mylobbie= null;
        }


    }

}
