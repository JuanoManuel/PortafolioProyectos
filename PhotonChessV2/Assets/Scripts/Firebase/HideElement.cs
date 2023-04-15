using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideElement : MonoBehaviour
{
    [SerializeField] FirebaseUser firebase;
    [SerializeField] bool hideWhenLoggedIn;   

    private void Start()
    {
        if (firebase.user != null)
            gameObject.SetActive(!hideWhenLoggedIn);
        else
            gameObject.SetActive(hideWhenLoggedIn);
    }

    private void OnEnable()
    {
        if (firebase.user != null)
            gameObject.SetActive(!hideWhenLoggedIn);
        else
            gameObject.SetActive(hideWhenLoggedIn);
    }
}
