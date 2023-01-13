using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class NetworkManagerScript : MonoBehaviour
{
    [SerializeField] private string Game_SceneName;
    private NetworkManager netManager;
    // Start is called before the first frame update
    void Start()
    {
        netManager = GetComponentInParent<NetworkManager>();
        
    }

    public void startAsHost()
    {
        netManager.StartHost();
        netManager.SceneManager.LoadScene(Game_SceneName, LoadSceneMode.Single);
    }

    public void startClient()
    {
        netManager.StartClient();
    }

    
    /*

    public void Play()
    {

        PhotonNetwork.JoinOrCreateRoom(RoomNameInput.text,
                                       new RoomOptions { MaxPlayers = 2},
                                       new TypedLobby(RoomNameInput.text, LobbyType.Default)
                                       );

    }

    public override void OnJoinRoomFailed(short returnCode,string message)
    {
        Debug.Log("Join Room failed");
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Created a Room!");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a Room!");
        if (PhotonNetwork.IsMasterClient == false)
        {
            Debug.Log("You're not the master client");
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("LoadArena", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    void LoadArena()
    {
        Debug.Log("Load Arena");
        PhotonNetwork.LoadLevel("Game");    
    }
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    */
}
