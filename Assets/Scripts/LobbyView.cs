using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;
using DeveloperTools;

public class LobbyView : MonoBehaviour
{
    private Transform lobbyGrid;
    private Transform lobbyInfo;
    private LobbySelected lobbyUI;
    private Text lobbyID;
    private Text lobbyName;
    private Text slots;
    private Text player;

    [SerializeField] private GameObject lobbyPrefab;


    public Lobby selectedLobby;

    void Awake()
    {
        lobbyInfo = DevTools.FindTransform("LobbyInfo", transform);
        lobbyGrid = DevTools.FindTransform("Grid", transform);
        
        lobbyID =   DevTools.FindTransform("LobbyID/Value", lobbyInfo).GetComponent<Text>();
        lobbyName = DevTools.FindTransform("Name/Value", lobbyInfo).GetComponent<Text>();
        slots =     DevTools.FindTransform("Slots/Value", lobbyInfo).GetComponent<Text>();
        player =    DevTools.FindTransform("Player/Value", lobbyInfo).GetComponent<Text>();

    }
    public void displayLobbies(List<Lobby> lobbies)
    {
        DevTools.DestroyAllChildren(lobbyGrid.gameObject);
        if (lobbies != null && lobbies.Count > 0)
        {
            foreach (var lobbie in lobbies)
            {
                var lobbyUI = Instantiate(lobbyPrefab, lobbyGrid.transform);
                var txt = lobbyUI.GetComponent<Text>();
                txt.text = lobbie.Name;
                lobbyUI.GetComponent<LobbySelected>().assignLobby(lobbie);
            }
        }
    }
   

    public void displayLobbyInfo(Lobby lobbie)
    {
        lobbyID.text = lobbie.Id;
        slots.text = lobbie.AvailableSlots.ToString() + '/' + lobbie.MaxPlayers.ToString();
        lobbyName.text = lobbie.Name;
        player.text = lobbie.Players.ToArray()[0].Id;
    }

    public void select(Lobby lobbie, LobbySelected ls)
    {
        if(lobbyUI != null)
            lobbyUI.deselect();
        lobbyUI = ls;
        lobbyUI.select();
        selectedLobby = lobbie;
        displayLobbyInfo(selectedLobby);
    }
}
