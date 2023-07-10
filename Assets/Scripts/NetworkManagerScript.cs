using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
using DeveloperTools;
using System.Collections;
using Unity.Services.Lobbies;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using System;
using Unity.Networking.Transport.Relay;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;

public class NetworkManagerScript : MonoBehaviour
{
    [SerializeField] private string Game_SceneName;
    private NetworkManager netManager;
    // Start is called before the first frame update
    void Start()
    {
        netManager = GetComponentInParent<NetworkManager>();
        DontDestroyOnLoad(transform.parent);
    }

    public async void startAsHost(string lobbyID)
    {
        var relayServerData = await getRelayServerData();
        netManager.GetComponent<UnityTransport>().SetRelayServerData(relayServerData.Item1);
        await updateLobbyData(relayServerData.Item2, lobbyID);
        Debug.Log("Starting Host");
        netManager.StartHost();
        netManager.SceneManager.LoadScene(Game_SceneName, LoadSceneMode.Single);

    }
    
    public async void startClient(string joinCode)
    {
        var relayServerData = await getRelayServerDataFromJoinCode(joinCode);
        netManager.GetComponent<UnityTransport>().SetRelayServerData(relayServerData.Item1);
        Debug.Log("Starting Client");
        netManager.StartClient();
    }
    
    //This is temporary for testing
    public void startGame()
    {
        netManager.StartHost();
        
    }

    private async Task<(RelayServerData,string)> getRelayServerData()
    {
        try
        {
            var allocation = await RelayService.Instance.CreateAllocationAsync(2);
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            return (new RelayServerData(allocation, "dtls"),joinCode);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.LogError(ex);
        }
        return (new RelayServerData(),"");
    }
    private async Task<(RelayServerData, string)> getRelayServerDataFromJoinCode(string joinCode)
    {
        try
        {
            var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            return (new RelayServerData(allocation, "dtls"), joinCode);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.LogError(ex);
        }
        return (new RelayServerData(), "");
    }
    private async Task updateLobbyData(string joinCode, string lobbyID)
    {
        {
            UpdateLobbyOptions options = new UpdateLobbyOptions();

            options.Data = new Dictionary<string, DataObject>()
            {
                {
                    "RelayServerData", new DataObject(
                        visibility: DataObject.VisibilityOptions.Member,
                        value: joinCode)
                }
            };

            await LobbyService.Instance.UpdateLobbyAsync(lobbyID, options);
        }
    }
}
