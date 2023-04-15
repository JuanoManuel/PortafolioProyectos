using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Fills the display name of the user if its already logged in
/// Called only in the matchmaking canvas of the menu scene
/// </summary>
public class FillUserData : MonoBehaviour
{
    [SerializeField] Text userText;
    [SerializeField] FirebaseUser firebaseUser;
    
    private void OnEnable()
    {
        if (firebaseUser.user != null)
            userText.text = firebaseUser.user.DisplayName;
        else
            userText.text = "Guess mode";
    }
}
