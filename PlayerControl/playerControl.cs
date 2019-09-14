using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    public float moveSpeed;
    public float dist;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            gameController.sharedOverseer.shoot(transform.localPosition+(Vector3.right*dist), 45f,20f);
            gameController.sharedOverseer.shoot(transform.localPosition+(Vector3.right*dist*-1), 135f,20f);
        }
        transform.localPosition+=new Vector3(Input.GetAxisRaw("Horizontal")*moveSpeed*Time.deltaTime, Input.GetAxisRaw("Vertical")*moveSpeed*Time.deltaTime,0f);
    }
}
