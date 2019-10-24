using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsBehaviour : MonoBehaviour
{
    PlayerControl pc;
    Vector3 translate;
    public GameObject player;
    float vspeed = 10f;
    public bool isGrazed = false;
    // Start is called before the first frame update
    void Start()
    {
        pc = GameController.sharedOverseer.player.GetComponent<PlayerControl>();
        player = GameController.sharedOverseer.player;
    }

    // Update is called once per frame
    void Update()
    {
        if(isGrazed==false)
        {
            translate.y = vspeed*Time.deltaTime;
            CheckDistance();
        }
        else
        {
            translate.x = transform.localPosition.x-player.transform.localPosition.x;
            translate.y = transform.localPosition.y-player.transform.localPosition.y;
            translate.z = 0;
            translate = Vector3.Normalize(translate)*-1/2;
            //Debug.Break();
        }
        transform.localPosition += translate;
        vspeed -= 10f * Time.deltaTime;
    }

    void CheckDistance()
    {
        if(Vector3.Distance(transform.localPosition, player.transform.localPosition)<10)
        {
            isGrazed=true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            pc.powerCounter++;
            isGrazed=false;
            this.gameObject.SetActive(false);
        }
    }
}
