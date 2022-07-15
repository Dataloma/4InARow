using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using DeveloperTools;

public class LobbyManager : NetworkBehaviour
{
    string a;
    LobbyView lobbyView;
    // Start is called before the first frame update
    void Awake()
    {
        lobbyView = DevTools.FindGameObject("MainMenu/LobbyView").GetComponent<LobbyView>();
        Screen.SetResolution(1440, 900, FullScreenMode.MaximizedWindow, 60);
        Debug.Log("Res Set");
    }

    

    public async void quickJoin()
    {
        QuickJoinLobbyOptions options = new QuickJoinLobbyOptions();
        options.Filter = new List<QueryFilter>();
        
        try
        {
            await Lobbies.Instance.QuickJoinLobbyAsync(options);
            Debug.Log("Joined Lobby: " + lobbyView.selectedLobby.Id);
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
        await Lobbies.Instance.CreateLobbyAsync("TestLobby", 2, options );
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
        await Lobbies.Instance.JoinLobbyByIdAsync(lobbyView.selectedLobby.Id);
        Debug.Log("Joined Lobby: " + lobbyView.selectedLobby.Id);
    }

}
