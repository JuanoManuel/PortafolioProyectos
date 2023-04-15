using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelsManager : MonoBehaviour
{
    public GameObject matchMakingPanel;
    public GameObject loginPanel;
    public FirebaseUser firebase;

    private void Start()
    {
        matchMakingPanel.SetActive(firebase.user != null);
        loginPanel.SetActive(firebase.user == null);
    }
}
