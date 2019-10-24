using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletPattern
{
    public List<BulletParameter> parameter=new List<BulletParameter>();
    
    public List<Actions.Action> act=new List<Actions.Action>();

}