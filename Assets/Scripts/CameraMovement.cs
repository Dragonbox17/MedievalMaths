using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    GameObject player;

    float xDist = 12.5f;
    float yDist = 18;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x >= -xDist && player.transform.position.x <= xDist)
        {
            if(player.transform.position.y >= -yDist && player.transform.position.y <= yDist)
            {
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
            }
            else if(player.transform.position.y < -yDist)
            {
                transform.position = new Vector3(player.transform.position.x, -yDist, -10);
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x, yDist, -10);
            }
        }
        else if (player.transform.position.x < -xDist)
        {
            if (player.transform.position.y >= -yDist && player.transform.position.y <= yDist)
            {
                transform.position = new Vector3(-xDist, player.transform.position.y, -10);
            }
            else if (player.transform.position.y < -yDist)
            {
                transform.position = new Vector3(-xDist, -yDist, -10);
            }
            else
            {
                transform.position = new Vector3(-xDist, yDist, -10);
            }
        }
        else
        {
            if (player.transform.position.y >= -yDist && player.transform.position.y <= yDist)
            {
                transform.position = new Vector3(xDist, player.transform.position.y, -10);
            }
            else if (player.transform.position.y < -yDist)
            {
                transform.position = new Vector3(xDist, -yDist, -10);
            }
            else
            {
                transform.position = new Vector3(xDist, yDist, -10);
            }
        }

    }
}
