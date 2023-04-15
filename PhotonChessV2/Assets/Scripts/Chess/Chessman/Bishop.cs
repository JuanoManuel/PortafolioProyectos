using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Chessman
{
    public override void ShowMovements()
    {
        //Destroy all previous plates
        gameMaster.DestroyAllPlates();
        //Spawn Lineal plates to all diagonal directions
        LinealSpawnPlate(-1, 1);
        LinealSpawnPlate(1, 1);
        LinealSpawnPlate(-1, -1);
        LinealSpawnPlate(1, -1);
    }
}
