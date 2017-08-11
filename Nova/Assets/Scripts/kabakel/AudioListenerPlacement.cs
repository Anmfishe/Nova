using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListenerPlacement : MonoBehaviour {

    public GameObject CameraObj;
    public GameObject PlayerObj;

    [Range(0,1)]
    public float PlayerToCameraPercentX;

    [Range(0,1)]
    public float PlayerToCameraPercentY;

    [Range(0,1)]
    public float PlayerToCameraPercentZ;

	// Update is called once per frame
    void LateUpdate () {
        Vector3 playerPos = PlayerObj.transform.position;
        Vector3 cameraPos = CameraObj.transform.position;
        Vector3 desiredPos;
        desiredPos.x = Mathf.Lerp(playerPos.x, cameraPos.x, PlayerToCameraPercentX);
        desiredPos.y = Mathf.Lerp(playerPos.y, cameraPos.y, PlayerToCameraPercentY);
        desiredPos.z = Mathf.Lerp(playerPos.z, cameraPos.z, PlayerToCameraPercentZ);
        transform.position = desiredPos;
	}
}
