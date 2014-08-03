﻿using UnityEngine;

namespace Assets.Script
{
    public class CameraFollow : MonoBehaviour 
    {
        public float XMargin = 1f;		// Distance in the x axis the trackingObject can move before the camera follows.
        public float YMargin = 1f;		// Distance in the y axis the trackingObject can move before the camera follows.
        public float XSmooth = 8f;		// How smoothly the camera catches up with it's target movement in the x axis.
        public float YSmooth = 8f;		// How smoothly the camera catches up with it's target movement in the y axis.
        public Vector2 MaxXAndY;		// The maximum x and y coordinates the camera can have.
        public Vector2 MinXAndY;		// The minimum x and y coordinates the camera can have.

        public Transform TrackingObject;		// Reference to the trackingObject's transform.

        void Awake ()
        {
            ImmortalBook.Add(this);
        }

        bool CheckXMargin()
        {
            // Returns true if the distance between the camera and the trackingObject in the x axis is greater than the x margin.
            return Mathf.Abs(transform.position.x - TrackingObject.position.x) > XMargin;
        }

        bool CheckYMargin()
        {
            // Returns true if the distance between the camera and the trackingObject in the y axis is greater than the y margin.
            return Mathf.Abs(transform.position.y - TrackingObject.position.y) > YMargin;
        }

        void FixedUpdate ()
        {
            TrackPlayer();
        }
	
        void TrackPlayer ()
        {
            if(TrackingObject != null)
            {
                // By default the target x and y coordinates of the camera are it's current x and y coordinates.
                var targetX = transform.position.x;
                var targetY = transform.position.y;

                // If the trackingObject has moved beyond the x margin...
                if(CheckXMargin())
                    // ... the target x coordinate should be a Lerp between the camera's current x position and the trackingObject's current x position.
                    targetX = Mathf.Lerp(transform.position.x, TrackingObject.position.x, XSmooth * Time.deltaTime);

                // If the trackingObject has moved beyond the y margin...
                if(CheckYMargin())
                    // ... the target y coordinate should be a Lerp between the camera's current y position and the trackingObject's current y position.
                    targetY = Mathf.Lerp(transform.position.y, TrackingObject.position.y, YSmooth * Time.deltaTime);

                // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
                //targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
                //targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

                // Set the camera's position to the target position with the same z component.
                transform.position = new Vector3(targetX, targetY, transform.position.z);
            }
        }
    }
}