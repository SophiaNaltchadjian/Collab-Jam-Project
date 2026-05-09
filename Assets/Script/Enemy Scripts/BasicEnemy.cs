using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : EnemyBase
{

    private void Update()
    {
        RotateToFacePlayer();
        PlayerDistanceCheck();
    }

}
