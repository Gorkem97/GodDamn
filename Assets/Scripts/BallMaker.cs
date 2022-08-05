using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMaker : MonoBehaviour
{
    public GameObject Ballz;
    public float timmy = 3;
    bool Atesle = true;

    void Update()
    {
        if (Atesle)
        {
            Instantiate(Ballz, transform.position, transform.rotation);
            StartCoroutine(time(timmy));
            Atesle = false;
        }
    }
    IEnumerator time(float timi)
    {
        yield return new WaitForSeconds(timi);
        Atesle = true;
    }
}
