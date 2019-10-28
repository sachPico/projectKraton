using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletGenerator
{
    public Transform parent;
    public Vector3 location=new Vector3();
    public Vector3 firstLoc=new Vector3();
    public List<Timer> fireTime=new List<Timer>();
    public List<BulletPattern> pattern=new List<BulletPattern>();
    public bool isShoot = false;

    void Start()
    {
    }

    public void GeneratorShoot()
    {
        for(int i=0; i<fireTime.Count; i++)
        {
            if(isShoot)
            { 
                if(fireTime[i].timerCount())
                {
                for(int j=0; j<pattern.Count;j++)
                    {
                        for(int k=0; k<pattern[j].parameter.Count; k++)
                        {
                            pattern[j].act[k].Execute(location, pattern[j].parameter[k]);
                        }
                    }
                }
            }
        }
        //Debug.Log(firstLoc.x+" "+firstLoc.y+" "+firstLoc.z);
        location = parent.localPosition + firstLoc;
    }

}