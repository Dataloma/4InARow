using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
using DeveloperTools;
using System.Collections;
using Unity.Services.Lobbies;

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

    public void startAsHost()
    {
        netManager.StartHost();
        netManager.SceneManager.LoadScene(Game_SceneName, LoadSceneMode.Single);

    }
    
    public void startClient()
    {
        Debug.Log("Starting Client");
        netManager.StartClient();
    }
    
    //This is temporary for testing
    public void startGame()
    {
        netManager.StartHost();
        
    }
}
