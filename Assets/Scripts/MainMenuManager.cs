using UnityEngine;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviour
{
    [SerializeField]private Text Profile;

    



    public void displayProfile(string profile)
    {
        Profile.text = profile;
    }
    public void clearProfile()
    {
        Profile.text = "";
    }
}
