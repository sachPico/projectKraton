using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    // Start is called before the first frame update
    //public Transform playfieldAnchor;
    public float speed;
    Timer fireRate;
    //Actions actions;
    Actions acts;
    //public static List<Actions.Action> actionList;
    Renderer render;

    void Start()
    {
        fireRate = new Timer(1f,true);
        //actions = new Actions();
        acts = new Actions();
        render = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position+=transform.right*speed*Time.deltaTime;
        if(fireRate.timerCount())
        {
            acts._shootCircular.Execute(this.gameObject, "Kerikil1", transform.localPosition,5f, 20, 0);
            
        }
        if(GameController.sharedOverseer.mainCam.WorldToViewportPoint(transform.position).x>1.1||GameController.sharedOverseer.mainCam.WorldToViewportPoint(transform.position).x<-0.1f||GameController.sharedOverseer.mainCam.WorldToViewportPoint(transform.position).y>1.1||GameController.sharedOverseer.mainCam.WorldToViewportPoint(transform.position).y<-0.1)
        {
            gameObject.SetActive(false);
        }
    }
}
