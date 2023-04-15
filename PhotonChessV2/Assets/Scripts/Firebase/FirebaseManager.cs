using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Father class of all the firebaseManager subclases
/// Share behaviour to its children
/// </summary>
public class FirebaseManager : MonoBehaviour
{
    //Error handler prefab
    [Header("Popup Prefab")]
    [SerializeField]protected GameObject prefab;//error handler prefab
    [SerializeField]protected Canvas canvas;//canvas where the prefab will be instantiated
    [Header("Firebase connector")]
    [SerializeField] protected FirebaseUser firebase;//firebase middleware

    public void Awake()
    {
        //if the firebase services hadn't been initialized
        if(firebase.auth == null)
            firebase.StartFirebaseService();
    }
    /// <summary>
    /// Instantientes a new Alert Popup window
    /// </summary>
    /// <param name="title">Title of the error</param>
    /// <param name="description">Description of the error</param>
    /// <param name="alertType">Type of alert</param>
    protected void SpawnAlertMessage(string title, string description, AlertPanel.AlertType alertType)
    {
        GameObject alert = Instantiate(prefab, Vector3.zero, Quaternion.identity, canvas.transform);
        alert.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        alert.GetComponent<AlertPanel>().Activate(title, description, alertType);
    }

    public string GetDisplayName()
    {
        return firebase.user.DisplayName;
    }
}
