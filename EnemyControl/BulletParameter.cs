using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletParameter
{
    public float range, targetDirection, bulletDirection, bulletDensity, bulletSpeed, difficultyDensity, difficultySpeed, angleSpeedModifier;
    public bool isAimingPlayer,isModifyRotation;
    public string bulletLabel;

}