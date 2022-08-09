using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fİNDaNDtERMİNATE : MonoBehaviour
{
    Vector3 kiki;
    public GameObject target;
    public GameObject TheOne;
    bool Rotatyeah = true;
    GameObject player;
    Quaternion rotationknow;
    Animator adam;
    void Start()
    {

        player = GameObject.Find("Player");
        adam = player.GetComponent<Animator>();
        kiki = new Vector3(1000, 0, 0);
    }

    void Update()
    {
        target.transform.position = new Vector3(TheOne.transform.position.x, TheOne.transform.position.y + 1.3f, TheOne.transform.position.z);
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Enemy")
        {
            Vector3 thisfar = other.transform.position - transform.position;
            if (other.gameObject == TheOne || Mathf.Abs(thisfar.x) < Mathf.Abs(kiki.x))
            {
                kiki = thisfar;
                TheOne = other.gameObject;
                target.transform.position = TheOne.transform.position;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == TheOne)
        {
            kiki = new Vector3(1000, 0, 0);
            TheOne = this.gameObject;
        }
    }
}
