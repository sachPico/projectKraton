using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
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
            GameController.sharedOverseer.shoot(transform.localPosition+(Vector3.right*dist), 45f,20f);
            GameController.sharedOverseer.shoot(transform.localPosition+(Vector3.right*dist*-1), 135f,20f);
        }
        transform.localPosition+=new Vector3(Input.GetAxisRaw("Horizontal")*moveSpeed*Time.deltaTime, Input.GetAxisRaw("Vertical")*moveSpeed*Time.deltaTime,0f);
    }
}
