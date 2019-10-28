using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Vector3 bakePosition, targetPosition;
    public GameObject playerPos;
    float originDistance = 20f;
    // Start is called before the first frame update
    void Start()
    {
        targetPosition.z = transform.localPosition.z;
        bakePosition.z=0;
        playerPos = GameController.sharedOverseer.player;
    }

    // Update is called once per frame
    void Update()
    {
        targetPosition.x = playerPos.transform.localPosition.x/(originDistance-transform.localPosition.z)*(originDistance);
        transform.localPosition = targetPosition;
    }
}
