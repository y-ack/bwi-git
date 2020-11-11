using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraBehavior : MonoBehaviour
{
    public Transform follow;
	private float smoothSpeed = 3f;
	public Vector3 p;
    public Vector3 minValues, maxValue;

    private void Start() 
    {
    minValues.x = 14f;
    minValues.y = 8.5f;
    minValues.z = -10;

    maxValue.x = 45.95f;
    maxValue.y = 30.5f;
    maxValue.z = -10;  
    }
	void Update()
    {        
        Vector3 targetPosition = follow.position;
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
