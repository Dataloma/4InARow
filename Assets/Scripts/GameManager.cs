using Unity.Netcode;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public enum playerColor
    {
        Red = 1,
        Yellow = 2
    }

    public playerColor myColor;
    public bool myTurn = false;

    public bool redturn = true;
    public float cooldown;
    public float lastPlayed;
    public GameObject redWon;
    public GameObject yellowWon;
    const int heightOfBoard = 6;
    const int lenghtOfBoard = 7;
    public int[,] BoardArray = new int[lenghtOfBoard, heightOfBoard];
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
    /*void Start()
    {
        redWon.gameObject.SetActive(false);
        yellowWon.gameObject.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
        {
            myColor = (playerColor)Random.Range(1, 3);
            Debug.Log("Your Color Is " + myColor.ToString() + "!");
            photonView.RPC("setPlayerColor", RpcTarget.Others, (int)myColor == 1 ? playerColor.Yellow : playerColor.Red);

            if (myColor == playerColor.Red)
            {
                myTurn = true;
                Debug.Log("It is your turn!");
                photonView.RPC("setPlayerTurn", RpcTarget.Others, false);
            }
            else
            {
                myTurn = false;
                Debug.Log("It is your Opponent's turn!");
                photonView.RPC("setPlayerTurn", RpcTarget.Others, true);
            }
        }

    }

    

    [PunRPC]
    public void setPlayerColor(playerColor color)
    {
        myColor = color;
        Debug.Log("Your Color Is " + myColor.ToString() + "!");

    }
    [PunRPC]
    public void setPlayerTurn(bool turn)
    {
        myTurn = turn;
        if (myTurn)
        {
            Debug.Log("It is your turn!");
        }
        else
        {
            Debug.Log("It is your Opponent's turn!");
        };
    }
    
    
    [PunRPC]
    public void resetCooldown()
    {
        lastPlayed = Time.time;
    }
    [PunRPC]
    public void redWins(){redWon.SetActive(true); }
    [PunRPC]
    public void yellowWins() {yellowWon.SetActive(true); }
    [PunRPC]
    public void log(string str) {Debug.Log(str); }


    public void switchTurns()
    {
        if (!myTurn)
        {
            myTurn = true;
            Debug.Log("It is your turn!");
            photonView.RPC("setPlayerTurn", RpcTarget.Others, false);
        }
        else
        {
            myTurn = false;
            Debug.Log("It is your Opponent's turn!");
            photonView.RPC("setPlayerTurn", RpcTarget.Others, true);
        }
    }


    public void WinCheck(playerColor player_color)
    {
        int playerNum = (int)player_color;
        //horizontal
        for(int x = 0; x < lenghtOfBoard - 3; x++)
        {
            for(int y = 0; y < heightOfBoard; y++)
            {
                if(BoardArray[x, y] == playerNum && BoardArray[x+1, y] == playerNum && BoardArray[x+2, y] == playerNum && BoardArray[x+3, y] == playerNum)
                {
                    if (playerNum == 1)
                    {
                        photonView.RPC("redWins", RpcTarget.All);
                    }

                    if (playerNum == 2)
                    {
                        photonView.RPC("yellowWins", RpcTarget.All);
                    }
                }
            }
        }

        //vertical
        for (int x = 0; x < lenghtOfBoard; x++)
        {
            for (int y = 0; y < heightOfBoard - 3; y++)
            {
                if (BoardArray[x, y] == playerNum && BoardArray[x, y+1] == playerNum && BoardArray[x, y+2] == playerNum && BoardArray[x, y+3] == playerNum)
                {
                    if (playerNum == 1)
                    {
                        photonView.RPC("redWins", RpcTarget.All);
                    }

                    if (playerNum == 2)
                    {
                        photonView.RPC("yellowWins", RpcTarget.All);
                    }
                }
            }
        }

        //y=x
        for (int x = 0; x < lenghtOfBoard - 3; x++)
        {
            for (int y = 0; y < heightOfBoard - 3; y++)
            {
                if (BoardArray[x, y] == playerNum && BoardArray[x + 1, y + 1] == playerNum && BoardArray[x + 2, y + 2] == playerNum && BoardArray[x + 3, y + 3] == playerNum)
                {
                    if (playerNum == 1)
                    {
                        photonView.RPC("redWins", RpcTarget.All);
                    }

                    if (playerNum == 2)
                    {
                        photonView.RPC("yellowWins", RpcTarget.All);
                    }
                }
            }
        }

        //y=-x
        for (int x = 0; x < lenghtOfBoard - 3; x++)
        {
            for (int y = 0; y < heightOfBoard - 3; y++)
            {
                if (BoardArray[x, y + 3] == playerNum && BoardArray[x + 1, y + 2] == playerNum && BoardArray[x + 2, y + 1] == playerNum && BoardArray[x + 3, y] == playerNum)
                {
                    if (playerNum == 1)
                    {
                        photonView.RPC("redWins", RpcTarget.All);
                    }

                    if (playerNum == 2)
                    {
                        photonView.RPC("yellowWins", RpcTarget.All);
                    }
                }
            }
        }
    }

    */
}
