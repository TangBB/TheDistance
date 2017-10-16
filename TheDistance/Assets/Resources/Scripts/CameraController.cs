﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public int rayCount;
    float xRaySpacing;
    float yRaySpacing;

    Vector3 bottomLeft;
    Vector3 bottomRight;
    Vector3 topLeft;
    Vector3 topRight;

    Camera cam;

    public LayerMask collisionMask;

	void Start () {
        cam = GetComponent<Camera>();
	}
	
	void UpdateCollisionBox() {
        Vector2 curSize = new Vector2(200, 200);
        //Vector2 curSize = new Vector2(cam.pixelWidth, cam.pixelHeight);
        xRaySpacing = curSize.x / (rayCount - 1);
        yRaySpacing = curSize.y / (rayCount - 1);
        bottomLeft = 
            cam.transform.position + new Vector3(-curSize.x, -curSize.y);
        bottomRight = 
            cam.transform.position + new Vector3(curSize.x, -curSize.y);
        topLeft = 
            cam.transform.position + new Vector3(-curSize.x, curSize.y);
        topRight = 
            cam.transform.position + new Vector3(curSize.x, curSize.y);
	}

    public void Move(Vector3 velocity)
    {
        UpdateCollisionBox();
        if (velocity.x != 0)
            HorizontalCheck(ref velocity);
        if (velocity.y != 0)
            VerticalCheck(ref velocity);
        transform.Translate(velocity);
    }

    void HorizontalCheck(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x);
        for(int i = 0; i < rayCount; i++)
        {
            Vector3 rayOrigin = (directionX == -1) ?
                bottomLeft : bottomRight;
            rayOrigin += Vector3.up * (xRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector3.right * directionX, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
            if (hit)
            {
                print("Hit something!" + hit.transform.name);
                velocity.x = (hit.distance) * directionX;
                rayLength = hit.distance;
            }
        }
    }

    void VerticalCheck(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y);
        for (int i = 0; i < rayCount; i++)
        {
            Vector3 rayOrigin = (directionY == -1) ?
                bottomLeft : topLeft;
            rayOrigin += Vector3.up * (xRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector3.up* directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);
            if (hit)
            {
                velocity.y = (hit.distance) * directionY;
                rayLength = hit.distance;
            }
        }
    }
}
