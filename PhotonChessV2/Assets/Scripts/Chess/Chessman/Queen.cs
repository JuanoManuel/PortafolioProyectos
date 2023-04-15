using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Chessman
{
    public override void ShowMovements()
    {
        //Destroy all previous plates
        gameMaster.DestroyAllPlates();
        //The Queen can move all directions//
        LinealSpawnPlate(-1, -1);
        LinealSpawnPlate(-1, 0);
        LinealSpawnPlate(-1, 1);
        LinealSpawnPlate(0, -1);
        LinealSpawnPlate(0, 1);
        LinealSpawnPlate(1, -1);
        LinealSpawnPlate(1, 0);
        LinealSpawnPlate(1, 1);
    }
}
