using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using DeveloperTools;
using static UnityEditor.Progress;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using System.Collections;
using System;
using System.Runtime.CompilerServices;

public class LobbyManager : NetworkBehaviour
{
    
    LobbyView lobbyView;
    private Lobby mylobbie = null;
    private Coroutine mylobbieUpdate = null;
    private Coroutine mylobbieHeartbeat = null;
    // Start is called before the first frame update
    void Awake()
    {
        lobbyView = DevTools.FindGameObject("MainMenu/LobbyView").GetComponent<LobbyView>();
        Debug.Log(lobbyView);
        Screen.SetResolution(1440, 900, FullScreenMode.MaximizedWindow, 60);
        Debug.Log("Res Set");
        StartCoroutine(updateMylobbie());
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
        await leaveLobby();
        var options = new CreateLobbyOptions();
        options.IsPrivate = false;
        try
        {
            mylobbie = await Lobbies.Instance.CreateLobbyAsync("TestLobby", 2, options);
            startHeartbeat();
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


    //Lobby needs heartbeat to stay active
    private IEnumerator heartbeat()
    {
        while(true)
        {
            if(mylobbie.HostId == AuthenticationService.Instance.PlayerId)
            {
                LobbyService.Instance.SendHeartbeatPingAsync(mylobbie.Id);
                Debug.Log("HeartBeat");
            }
            yield return new WaitForSeconds(3);
        }
        
    }
    private void startHeartbeat()
    {
        mylobbieHeartbeat=StartCoroutine(heartbeat());
    }
    private void stopHeartbeat()
    {
        try { 
        StopCoroutine(mylobbieHeartbeat);
        mylobbieHeartbeat=null;
        }
        catch
        {
            return;
        }
    }

    private IEnumerator updateMylobbie()
    {
        while (true)
        {
            _ = Task.Run(async () =>
                mylobbie = await LobbyService.Instance.GetLobbyAsync(mylobbie.Id));
            yield return new WaitForSeconds(2);
        }
    }
    private async Task test()
    {
        await Task.Delay(2000);
        Debug.Log("Result");
    }
    private async Task leaveLobby()
    {
        if (mylobbie != null){
            await LobbyService.Instance.RemovePlayerAsync(mylobbie.Id, AuthenticationService.Instance.PlayerId);
            mylobbie= null;
        }
        stopHeartbeat();



    }

}
