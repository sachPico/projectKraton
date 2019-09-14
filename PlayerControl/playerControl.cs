using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            gameController.sharedOverseer.shoot(transform.localPosition, 0f);
        }
        transform.localPosition+=new Vector3(Input.GetAxisRaw("Horizontal")*2*Time.deltaTime, Input.GetAxisRaw("Vertical")*2*Time.deltaTime,0f);
    }
}
