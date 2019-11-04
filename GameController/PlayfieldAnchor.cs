using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldAnchor : MonoBehaviour
{
    public AnimationCurve rotationGraphX, rotationGraphY, rotationGraphZ;
    public float speed, recRotX, recRotY, recRotZ;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationGraphX.Evaluate(Time.time)-recRotX,rotationGraphY.Evaluate(Time.time)-recRotY,rotationGraphZ.Evaluate(Time.time)-recRotZ,Space.Self);
        recRotX=rotationGraphX.Evaluate(Time.time);
        recRotY=rotationGraphY.Evaluate(Time.time);
        recRotZ=rotationGraphZ.Evaluate(Time.time);
        //Quaternion.Euler(rotationGraphX.Evaluate(Time.time),rotationGraphY.Evaluate(Time.time),rotationGraphZ.Evaluate(Time.time));
        //transform.rotation = Quaternion.Euler(rotationGraphX.Evaluate(Time.time),rotationGraphY.Evaluate(Time.time),rotationGraphZ.Evaluate(Time.time));
        transform.position += transform.up * speed * Time.deltaTime;
    }
}
