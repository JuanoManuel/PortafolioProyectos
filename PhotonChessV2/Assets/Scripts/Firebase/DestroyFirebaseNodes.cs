using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Destroy all objects with photonview components and related to multiplayer settings
/// </summary>
public class DestroyFirebaseNodes : MonoBehaviour
{
    //Objects to destroy
    private GameObject[] gameObjects;
    private void Start()
    {
        gameObjects = GameObject.FindGameObjectsWithTag("MultiplayerSettings");
    }
    /// <summary>
    /// function called from the play again button when a match ends
    /// destroys all the multiplayer settings objects in scene
    /// </summary>
    public void DestroyMultiplayerSettings()
    {
        foreach (GameObject obj in gameObjects)
            Destroy(obj);
    }
}
