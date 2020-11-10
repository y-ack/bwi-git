using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraBehavior : MonoBehaviour
{
    public Transform follow;
    public Vector3 offset;
	private float smoothSpeed = 3f;
	public Vector3 p;
    public Vector3 minValues, maxValue;
	void Update()
    {
        minValues.x = 5.76f;
        minValues.y = 3.52f;
        minValues.z = -10;

        maxValue.x = 13.28f;
        maxValue.y = 8.8f;
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
