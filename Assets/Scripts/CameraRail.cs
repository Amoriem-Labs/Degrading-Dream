using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRail : MonoBehaviour
{
    // declaring objects to be used
    public Transform player;
    // minimum is the left end of the rail, max is right end
    public float minX = -5f; 
    public float maxX = 5f;   
    public float smoothTime = 0.25f;

    private float velocityX;


    private Vector3 velocity;

    // we run the camera tracking code in late update (tail end of the frame)
    // because we want the camera to move after the character has 
    private void LateUpdate()
    {
        // if player object doesn't exist, defer to the next frame
        if (player == null) return;

        // follow the player's x, but never leave the rail 
        float targetX = Mathf.Clamp(player.position.x, minX, maxX);

        // ease toward it instead of snapping
        float newX = Mathf.SmoothDamp(transform.position.x, targetX, ref velocityX, smoothTime);

        // y and z stay fixed - we only want the camera to slide on x
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

}
