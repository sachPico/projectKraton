using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Vector3 prePosition;
    public GameObject playerPos;
    float originDistance = 20f;
    // Start is called before the first frame update
    void Start()
    {
        prePosition.z = transform.localPosition.z;
        playerPos = GameController.sharedOverseer.player;
    }

    // Update is called once per frame
    void Update()
    {
        prePosition.x = playerPos.transform.localPosition.x/(originDistance-transform.localPosition.z)*(originDistance);

        transform.localPosition = prePosition;
    }
}
