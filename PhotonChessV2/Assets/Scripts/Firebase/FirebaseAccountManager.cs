using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Authenticate a user or creates a new user in the firebase authentication service
/// </summary>
public class FirebaseAccountManager : FirebaseManager
{
    //Login fields
    [Header("Login form")]
    [SerializeField] InputField inputEmail;
    [SerializeField] InputField inputPassword;
    //Signup fields
    [Header("Signup form")]
    [SerializeField] InputField inputNewEmail;
    [SerializeField] InputField inputNewUser;
    [SerializeField] InputField inputNewPassword;
    [Header("Paneles")]
    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject signupPanel;
    [SerializeField] GameObject matchMakingPanel;

    
    public void Login()
    {
        StartCoroutine(firebase.Login(inputEmail.text,inputPassword.text, OnLoginFinished));
        Debug.Log("termino");
    }

    public void Register()
    {
        StartCoroutine(firebase.SignUp(inputNewUser.text,inputNewEmail.text, inputNewPassword.text, OnRegisterFinished));
    }

    private void OnLoginFinished(string title, string description, AlertPanel.AlertType alertType)
    {
        InstantiateAlertMessage(title, description, alertType,matchMakingPanel,loginPanel);
    }

    private void OnRegisterFinished(string title,string description,AlertPanel.AlertType alertType)
    {
        InstantiateAlertMessage(title, description, alertType, matchMakingPanel, signupPanel);
    }

    private void InstantiateAlertMessage(string title, string description, AlertPanel.AlertType alertType,GameObject canvasToShow,GameObject canvasToHide)
    {
        if (alertType != AlertPanel.AlertType.Info)
        {
            SpawnAlertMessage(title, description, alertType);
        }
        else
        {
            canvasToHide.SetActive(false);
            canvasToShow.SetActive(true);
        }
    }
}
