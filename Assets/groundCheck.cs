using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour
{
    SpecialEnemyThings adam;
    public GameObject serhat;
    // Start is called before the first frame update
    void Start()
    {
        adam = serhat.GetComponent<SpecialEnemyThings>();
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        Ray GroundLay = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(GroundLay, out hit, 0.5f))
        {
            if (hit.collider.tag == "Ground")
            {
                adam.emptyRoad = false;
            }
            else
            {
                adam.emptyRoad = true;
            }
        }
        else
        {
            adam.emptyRoad = true;
        }


        RaycastHit hitF;
        Ray GroundLayF = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(GroundLayF, out hitF, 0.3f))
        {
            if (hitF.collider.tag == "Ground")
            {
                adam.wall = true;
            }
            else
            {
                adam.wall = false;
            }
        }
        else
        {
            adam.wall = false;
        }
    }
}
