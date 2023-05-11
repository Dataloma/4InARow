using DeveloperTools;
using System;
using Unity.Netcode;
using UnityEngine;
public class GameManager : NetworkBehaviour
{
    public enum playerColor
    {
        Red = 1,
        Yellow = 2
    }

    public bool myTurn { get; private set; } = false;
    private playerColor myColor;

    private NetworkManager netManager;
    private GameObject PiecesContainer;

    private GameObject redWon;
    private GameObject yellowWon;

    [SerializeField]
    private GameObject redPiece;
    [SerializeField]
    private GameObject yellowPiece;

    public float cooldown { get; private set; }
    public float lastPlayed { get; private set; }

    public const int lenghtOfBoard = 7;
    public const int heightOfBoard = 6;
    private int[,] BoardArray = new int[lenghtOfBoard, heightOfBoard];
    /*
    {
        {0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0 },  Turn 90 Degrees counter clockwise
        {0, 0, 0, 0, 0, 0 },  This is 7X6 Board
        {0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0 }
    };
    */
    
    private void Awake()
    {
        redWon = DevTools.FindGameObject("Canvas/RedWon");
        yellowWon = DevTools.FindGameObject("Canvas/YellowWon");
        PiecesContainer = DevTools.FindGameObject("Board/Pieces");
        redWon.SetActive(false);
        yellowWon.SetActive(false);

        netManager = DevTools.FindGameObject("NetworkManager").GetComponent<NetworkManager>();
        if (netManager.IsHost)
        {
            myColor = playerColor.Red;
            myTurn = true;
            Debug.Log("Hosting");
        }
        else
        {
            myColor = playerColor.Yellow;
            myTurn = false;
        }

    }
    public void resetCooldown()
    {
        lastPlayed = Time.time;
    }

    void switchTurn() {
        if (myTurn) { myTurn = false; }
        else { myTurn = true; }
        resetCooldown();
    }
        

    [ServerRpc]
    public void testServerRpc()
    {
        testServerClientRpc();
    }
    [ClientRpc]
    public void testServerClientRpc() 
    {
        Debug.Log("private works");
    }

    
    private void redWins() { redWon.SetActive(true); }
    private void yellowWins() { yellowWon.SetActive(true); }


    [ServerRpc(RequireOwnership = false)]
    public void registerPieceServerRpc(int column)
    {
        for (int row = 0; row < heightOfBoard - 1; row++)
        {

            if (BoardArray[column, row] == 0)
            {
                if (myTurn)
                {
                    
                    BoardArray[column, row] = (int)myColor;
                    spawnPiece(myColor, (float)(column - 3) * 2.5f, 8.5f);
                    registerPieceClientRpc();
                    WinCheck(myColor);

                }
                else
                {
                    BoardArray[column, row] = (int)myColor+1;
                    spawnPiece(myColor+1, (float)(column - 3) * 2.5f, 8.5f);
                    registerPieceClientRpc();
                    WinCheck(myColor+1);
                }
                break;
            }
        }
        
    }
    [ClientRpc]
    public void registerPieceClientRpc()
    {
        
        resetCooldown();
        switchTurn();
    }

    [ServerRpc]
    private void winServerRpc(playerColor color)
    {
        winClientRpc(color);
    }
    [ClientRpc]
    private void winClientRpc(playerColor color)
    {
        if (color == playerColor.Red)
        {
            redWins();
        }
        else
        {
            yellowWins();
        }
    }
    private void WinCheck(playerColor player_color)
    {
        

        //horizontal
        for (int y = 0; y < heightOfBoard; y++)
        {
            for (int x = 0; x < lenghtOfBoard - 3; x++)
            {
                
                if (BoardArray[x, y] == (int)player_color && BoardArray[x + 1, y] == (int)player_color && BoardArray[x + 2, y] == (int)player_color && BoardArray[x + 3, y] == (int)player_color)
                {
                    winServerRpc(player_color); 
                }
            }
        }

        //vertical
        for (int x = 0; x < lenghtOfBoard; x++)
        {
            for (int y = 0; y < heightOfBoard - 3; y++)
            {
                if (BoardArray[x, y] == (int)player_color && BoardArray[x, y + 1] == (int)player_color && BoardArray[x, y + 2] == (int)player_color && BoardArray[x, y + 3] == (int)player_color)
                {
                    winServerRpc(player_color);
                }
            }
        }

        //y=x
        for (int y = 0; y < heightOfBoard - 3; y++)
        {
            for (int x = 0; x < lenghtOfBoard - 3; x++)
            {
                if (BoardArray[x, y] == (int)player_color && BoardArray[x + 1, y + 1] == (int)player_color && BoardArray[x + 2, y + 2] == (int)player_color && BoardArray[x + 3, y + 3] == (int)player_color)
                {
                    winServerRpc(player_color);
                }
            }
        }

        //y=-x
        for (int y = 0; y < heightOfBoard - 3; y++)
        {
            for (int x = 0; x < lenghtOfBoard - 3; x++)
            {
                if (BoardArray[x, y + 3] == (int)player_color && BoardArray[x + 1, y + 2] == (int)player_color && BoardArray[x + 2, y + 1] == (int)player_color && BoardArray[x + 3, y] == (int)player_color)
                {
                    winServerRpc(player_color);
                }
            }
        }
    }

    
    private void spawnPiece(playerColor color, float x, float y)
    {
        GameObject piece;
        if(color == playerColor.Yellow)
        {
            piece = Instantiate(yellowPiece, new Vector3(x,y,0), Quaternion.identity, PiecesContainer.transform);
        }
        else
        {
            piece = Instantiate(redPiece, new Vector3(x, y, 0), Quaternion.identity, PiecesContainer.transform);

        }
        piece.GetComponent<NetworkObject>().Spawn();
    }
   
}
