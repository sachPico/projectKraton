using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsBehaviour : MonoBehaviour
{
    PlayerControl pc;
    Vector3 translate;
    float vspeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        pc = GameController.sharedOverseer.player.GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        translate.y = vspeed*Time.deltaTime;;
        transform.localPosition += translate;
        vspeed -= 10f * Time.deltaTime;
        return;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            pc.powerCounter++;
            //Debug.Log(pc.power);
            vspeed=10f;
            gameObject.SetActive(false);    
        }
    }
}
