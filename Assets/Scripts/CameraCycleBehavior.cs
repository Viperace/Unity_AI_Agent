using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCycleBehavior : MonoBehaviour
{
    public float speed = 15;
    GameObject target;

    void Start()
    {
        target = null;
    }

    Vector3 pos;
    void Update()
    {
        // Set target
        if (Input.GetKey(KeyCode.Tab))
        {
            Debug.Log("Hit tab. Setting target");

            List<Actor> actors = new List<Actor>(Actor.actors);
            // Find new target
            if(target == null)
            {
                foreach (Actor a in actors)
                    if (a != null && a.CompareTag("Hero"))
                    {
                        target = a.gameObject;
                        break;
                    }
            }                
            else // Cycle next target
            {
                // Find current target index
                int currentTargetIndex = 0;                
                for(int i = 0; i < actors.Count; i++)
                {
                    if(actors[i].gameObject == target)
                    {
                        currentTargetIndex = i;
                        break;
                    }
                }

                // Cycle other index as long as not this one
                int validIndex = 0;
                for (int i = 0; i < actors.Count; i++)
                    if (actors[i] != null && actors[i].CompareTag("Hero") && actors[i].gameObject != target)
                    {
                        validIndex = i;
                        break;
                    }

                target = actors[validIndex].gameObject;
            }

        }
    }

    void LateUpdate()
    {
        pos = transform.position;

        // Clear target and move camera
        if (Input.GetKey("w"))
        {
            pos.z += speed * Time.deltaTime;
            target = null;
        }
        if (Input.GetKey("s"))
        {
            pos.z -= speed * Time.deltaTime;
            target = null;
        }
        if (Input.GetKey("d"))
        {
            pos.x += speed * Time.deltaTime;
            target = null;
        }
        if (Input.GetKey("a"))
        {
            pos.x -= speed * Time.deltaTime;
            target = null;
        }

        if (target) // Follow target
        {
            this.transform.position = target.transform.position;
        }
        else
        {
            // Move follow key
            this.transform.position = pos;
        }

    }
}
