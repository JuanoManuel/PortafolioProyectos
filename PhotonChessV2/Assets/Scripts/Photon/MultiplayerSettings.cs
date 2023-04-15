using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSettings : MonoBehaviour
{
    public static MultiplayerSettings multiplayerSettings;

    public int maxPlayers;
    public int multiplayerScene;

    //Setting the singleton
    private void Awake()
    {
        if (MultiplayerSettings.multiplayerSettings == null)
            MultiplayerSettings.multiplayerSettings = this;
        else
        {
            if (MultiplayerSettings.multiplayerSettings != this)
                Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
