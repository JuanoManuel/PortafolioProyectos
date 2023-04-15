using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Hides the gameobject Im attach with only when there is a Firebase user
/// </summary>
public class HideWhenGuessMode : MonoBehaviour
{
    [SerializeField]FirebaseUser firebaseUser;
    private void Start()
    {
        if (firebaseUser.user != null)
            gameObject.SetActive(false);
    }
}
