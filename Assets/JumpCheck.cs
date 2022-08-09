using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCheck : MonoBehaviour
{
    public SpecialEnemyThings adam;

    void Start()
    {
        adam = GetComponentInParent<SpecialEnemyThings>();
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        Ray GroundLay = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(GroundLay, out hit, 5f))
        {
            if (hit.collider.tag == "Ground")
            {
                adam.jumpable = true;
            }
            else
            {
                adam.jumpable = false;
            }
        }
        else
        {
            adam.jumpable = false;
        }
    }
}
