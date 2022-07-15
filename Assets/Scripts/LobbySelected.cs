using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.Services.Lobbies.Models;
using DeveloperTools;
public class LobbySelected : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Lobby lobby;
    private LobbyView lobbyView;
    private bool isSelected = false;
    private Text txt;

    [SerializeField] private Color NormalColor;
    [SerializeField] private Color SelectedColor;
    [SerializeField] private Color HoverColor;

    public void Awake()
    {
        
        lobbyView = DevTools.FindGameObject("MainMenu/LobbyView").GetComponent<LobbyView>();
        txt = GetComponent<Text>();
        txt.color = NormalColor;
    }
    public void assignLobby(Lobby lobbie)
    {
        lobby = lobbie;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        lobbyView.select(lobby, this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        if (!isSelected)
            txt.color = HoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       
        if(!isSelected)
            txt.color = NormalColor;
    }

    public void select()
    {
        isSelected = true;
        txt.color = SelectedColor;
    }
    public void deselect()
    {
        isSelected=false;
        txt.color = NormalColor;
    }
}
