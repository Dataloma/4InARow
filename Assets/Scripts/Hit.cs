using UnityEngine;


public class Hit : MonoBehaviour
{
    public float spawn;
    public int column;

    public GameManager gm;
    
    /*private void OnMouseDown()
    {
        if (gm.myTurn)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //If cooldown passed
                if (Time.time - gm.lastPlayed >= gm.cooldown)
                {
                    RegisterPiece(column);
                    gm.resetCooldown();
                }
            }
            else
            {
                if (Time.time - gm.lastPlayed >= gm.cooldown)
                {
                    photonView.RPC("RegisterPiece", RpcTarget.MasterClient, column);
                    photonView.RPC("gmresetCooldown", RpcTarget.MasterClient);
                }
            }
        }


    }
    [PunRPC]
    void gmresetCooldown()
    {
        gm.resetCooldown();
    }
    [PunRPC]
    private void RegisterPiece(int column)
    {
        for (int row = 5; row > -1; row--)
        {

            if (gm.BoardArray[column, row] == 0)
            {
                if (gm.redturn)
                {
                    PhotonNetwork.Instantiate("red", new Vector3(spawn, 8.5f, 60), Quaternion.identity);
                    gm.BoardArray[column, row] = 1;
                    gm.WinCheck(GameManager.playerColor.Red);
                    gm.redturn = false;
                    gm.switchTurns();
                }
                else
                {
                    PhotonNetwork.Instantiate("yellow", new Vector3(spawn, 8.5f, 60), Quaternion.identity);
                    gm.BoardArray[column, row] = 2;
                    gm.WinCheck(GameManager.playerColor.Yellow);
                    gm.redturn = true;
                    gm.switchTurns();
                }
                Debug.Log(column + " " + row); 
                return;
            }
        }
        
    }*/
}
    //-7 2.5
