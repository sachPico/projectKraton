using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Vector3 bakePosition, targetPosition;
    public GameObject playerPos;
    public float resetOrigin;
    float originDistance = 40f;
    // Start is called before the first frame update
    void Start()
    {
        targetPosition.z = transform.localPosition.z;
        bakePosition.z=0;
        playerPos = GameController.sharedOverseer.player;
    }

    void UpdatePosition()
    {
        targetPosition.x = playerPos.transform.localPosition.x/(originDistance-transform.localPosition.z)*(originDistance);
        transform.localPosition = targetPosition;
    }

    void ResetPosition()
    {
        if(transform.localPosition.x!=0)
        {
            bakePosition.x = -resetOrigin/200;
            transform.localPosition += bakePosition;
            bakePosition.x = -(transform.localPosition.x%bakePosition.x);
            transform.localPosition += bakePosition;
        }
        else
        {
            GameController.sharedOverseer.player.SetActive(true);
            GameController.sharedOverseer.player.transform.localPosition=Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playerPos.activeInHierarchy)
        {
            UpdatePosition();
        }
        else
        {
            ResetPosition();
        }
    }
}
