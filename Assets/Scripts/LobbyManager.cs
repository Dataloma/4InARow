using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using DeveloperTools;
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
    private NetworkManagerScript netManagerScript;
    // Start is called before the first frame update
    void Awake()
    {
        lobbyView = DevTools.FindGameObject("MainMenu/LobbyView").GetComponent<LobbyView>();
        netManagerScript = DevTools.FindGameObject("NetworkManager/Scripts").GetComponent<NetworkManagerScript>();
        Screen.SetResolution(1440, 900, FullScreenMode.MaximizedWindow, 60);
        
        mylobbieUpdate=StartCoroutine(startUpdateMylobbie());
        
    }

    

    public async void quickJoin()
    {
        await leaveLobby();
        QuickJoinLobbyOptions options = new QuickJoinLobbyOptions();
        options.Filter = new List<QueryFilter>();
        
        try
        {
            mylobbie = await Lobbies.Instance.QuickJoinLobbyAsync(options);
            await updateMylobbie();
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
        if(lobbyView.selectedLobby.Id != null)
        {
            await leaveLobby();
            mylobbie =  await Lobbies.Instance.JoinLobbyByIdAsync(lobbyView.selectedLobby.Id);
            await updateMylobbie();
        }
        Debug.Log("Joined Lobby: " + lobbyView.selectedLobby.Id);
    }


    //Lobby needs heartbeat to stay active
    private IEnumerator heartbeat()
    {
        while(true)
        {
            LobbyService.Instance.SendHeartbeatPingAsync(mylobbie.Id);
            Debug.Log("HeartBeat");
            yield return new WaitForSeconds(10);
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

    private async Task updateMylobbie()
    {
        if (mylobbie == null) { return; }
        mylobbie = await LobbyService.Instance.GetLobbyAsync(mylobbie.Id);
        if (mylobbie.HostId == AuthenticationService.Instance.PlayerId)
        {
            if (mylobbieHeartbeat == null)
                startHeartbeat();
        }

        if (mylobbie.AvailableSlots == 0)
        {
            if (mylobbie.HostId == AuthenticationService.Instance.PlayerId)
            {
                netManagerScript.startAsHost();
                Debug.Log("started host");
                StopCoroutine(mylobbieHeartbeat);
                StopCoroutine(mylobbieUpdate);
            }
            else
            {

                netManagerScript.startClient();
                StopCoroutine(mylobbieUpdate);

            }
        }
    }
    private IEnumerator startUpdateMylobbie()
    {
        while (true)
        {
            _ = updateMylobbie();
            yield return new WaitForSeconds(4);
        }
    }
    
    private void startmatch()
    {
        Debug.Log("startmatch!!!!");
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
