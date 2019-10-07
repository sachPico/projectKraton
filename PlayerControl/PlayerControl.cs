using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed;
    public float dist;
    private Vector3 limit;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            GameController.sharedOverseer.shoot("AlifNormal",transform.localPosition+(Vector3.right*dist), 45f,20f, Color.white);
            GameController.sharedOverseer.shoot("AlifNormal",transform.localPosition+(Vector3.right*dist*-1), 135f,20f, Color.white);
        }
        transform.localPosition+= (Input.GetAxisRaw("Horizontal")*moveSpeed*Time.deltaTime*transform.right)+ (Input.GetAxisRaw("Vertical")*moveSpeed*Time.deltaTime*transform.up);
    }

    void LateUpdate()
    {
        transform.localPosition.Set(
            Mathf.Clamp(transform.localPosition.x,GameController.sharedOverseer.border[0].x,GameController.sharedOverseer.border[1].x),
            Mathf.Clamp(transform.localPosition.y,GameController.sharedOverseer.border[2].y,GameController.sharedOverseer.border[3].y),
            0f);
    }
}
