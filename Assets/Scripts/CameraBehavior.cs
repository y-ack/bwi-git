using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraBehavior : MonoBehaviour
{
    public Transform follow;
    private float smoothSpeed = 3f;
	public Vector3 p;
    public float scale = 0.0f;
    public Vector3 minValues, maxValue;
    public Vector3 targetPosition;


    private void Start() 
    {
        minValues.x = 15f - scale;
        minValues.y = 10f - scale;
        minValues.z = -10;
        
        maxValue.x = 44.2f + scale;
        maxValue.y = 29f + scale;
        maxValue.z = -10;  

    }
	void Update()
    {        
        FollowPlayer();
    }
    
    public void FollowPlayer()
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
