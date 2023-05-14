using DeveloperTools;
using Unity.Netcode;
using UnityEngine;


public class Hit : NetworkBehaviour
{
    [SerializeField]
    private int column;
    private GameManager gm;

    private void Start()
    {
        gm = DevTools.FindGameObject("GameManager").GetComponent<GameManager>();
    }
    public void OnMouseDown()
    {
        if (gm.myTurn)
        {
            if (Time.time - gm.lastPlayed >= gm.cooldown)
            {
                gm.registerPieceServerRpc(column);
            }
        }
    }
}
    //-7 2.5
