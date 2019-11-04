using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCustomEditor : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isClass;
    [ConditionalHide("isClass", true)]
    public Desimal angka;

    [System.Serializable]
    public class Desimal
    {
        public float x,y,z;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
