using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CameraRail : MonoBehaviour
{
    // declaring objects to be used
    public Transform player;

    [SerializeField] Transform RailSystem;

    private List<(Vector3, Vector3)> mSegments = new List<(Vector3, Vector3)>(); // list of segments is a tuple of (start, end)

    private float mOffsetX;
    private float mOffsetZ;

    private void Start()
    {
        // get each rail segment in the system and add it to the list
        foreach (Transform Segment in RailSystem)
        {
            Vector3 start = Segment.GetChild(0).position;
            Vector3 end = Segment.GetChild(1).position; // assuming here that child 0 will always be the start and child 1 will be the end
            mSegments.Add((start, end));
        }

        mOffsetX = transform.position.x - player.position.x;
        mOffsetZ = transform.position.z - player.position.z;
    }

    // we run the camera tracking code in late update (tail end of the frame)
    // because we want the camera to move after the character has 
    private void LateUpdate()
    {
        Vector3 truePositon = new Vector3(player.position.x + mOffsetX, transform.position.y, player.position.z + mOffsetZ);

        float minDistance = float.MaxValue;
        Vector3 closestRailPos = new Vector3(0,0,0);
        // find the closest point on a rail segment to this position
        foreach ((Vector3, Vector3) segment in mSegments)
        {
            Vector3 segmentLength = segment.Item2 - segment.Item1;

            Vector3 projection = Vector3.Project(truePositon- segment.Item1, segmentLength);

            float percentRailProgress = Vector3.Dot(projection, segmentLength) / segmentLength.sqrMagnitude;

            percentRailProgress = Mathf.Clamp(percentRailProgress, 0, 1);

            Vector3 railPosition = segment.Item1 + (percentRailProgress * segmentLength);

            float distance = Vector3.Distance(truePositon, railPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestRailPos = railPosition;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, closestRailPos, player.GetComponent<PlayerMovement>().moveSpeed * Time.deltaTime);
    }
}
