using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldAnchor : MonoBehaviour
{
    public AnimationCurve rotationGraphX, rotationGraphY, rotationGraphZ;
    public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.eulerAngles = new Vector3(rotationGraphX.Evaluate(Time.deltaTime), rotationGraphY.Evaluate(Time.deltaTime), rotationGraphZ.Evaluate(Time.deltaTime));
        transform.Rotate(rotationGraphX.Evaluate(Time.deltaTime), rotationGraphY.Evaluate(Time.deltaTime), rotationGraphZ.Evaluate(Time.deltaTime));

        //transform.rotation = Quaternion.Euler(rotationGraphX.Evaluate(Time.deltaTime), 0,0);
        //transform.position += transform.up * speed * Time.deltaTime;
    }
}
