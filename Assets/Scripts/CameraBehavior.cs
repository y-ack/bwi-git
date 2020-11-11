using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraBehavior : MonoBehaviour
{
    public Transform follow;
    public Grid theLevel;
    public Vector3 offset;
	private float smoothSpeed = 3f;
	public Vector3 p;
    public Vector3 minValues, maxValue;
    public float tileSize = 0.32f;
	void Update()
    {
        minValues.x = 15f ;
        minValues.y = 9f;
        minValues.z = -10;

        maxValue.x = 45f;
        maxValue.y = 30f;
        maxValue.z = -10;
        Vector3 targetPosition = follow.position + offset;
        //Verify if the targetPosition is out of bound or not
        //Limit it to the min and max values
        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minValues.x, maxValue.x),
            Mathf.Clamp(targetPosition.y, minValues.y, maxValue.y),
            Mathf.Clamp(targetPosition.z, minValues.z, maxValue.z));

        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothSpeed * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }

}
