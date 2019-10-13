using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed;
    int powerStatus;
    public int powerCounter;
    public float dist;
    private Vector3 limit;
    public Vector3 translate;
    bool isShoot;
    void Start()
    {
        isShoot=false;
        translate = new Vector3(0,0,0);
    }

    void PowerUp()
    {
        powerStatus++;
        powerCounter=0;
    }

    void PowerDown()
    {
        powerStatus--;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1"))
        {
            if(isShoot==false)
            {
                isShoot=true;
            }
            else
            {
                GameController.sharedOverseer.shoot("AlifNormal",transform.localPosition+(Vector3.right*dist), 90f,100f, Color.white);
                GameController.sharedOverseer.shoot("AlifNormal",transform.localPosition+(Vector3.right*dist*-1), 90f,100f, Color.white);
                isShoot=false;
            }
        }
        if(powerCounter>=100)
        {
            PowerUp();
        }
        translate.x=Input.GetAxisRaw("Horizontal")*moveSpeed*Time.deltaTime;
        translate.y=Input.GetAxisRaw("Vertical")*moveSpeed*Time.deltaTime;
        transform.localPosition+= translate;
        
    }

    void LateUpdate()
    {
        limit = transform.localPosition;
        limit.x = Mathf.Clamp(limit.x,GameController.sharedOverseer.border_test[0].localPosition.x, GameController.sharedOverseer.border_test[1].localPosition.x);
        limit.y = Mathf.Clamp(limit.y,GameController.sharedOverseer.border_test[2].localPosition.y, GameController.sharedOverseer.border_test[3].localPosition.y);

        transform.localPosition = limit;
    }
}
