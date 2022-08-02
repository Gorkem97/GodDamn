using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float ballSpeed = 3;
    public float time = 10;
    GameObject taret;
    // Start is called before the first frame update
    void Start()
    {
        taret = GameObject.Find("TargetBall");
        StartCoroutine(BallTime(time));
        transform.LookAt(taret.transform);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position += transform.forward * Time.deltaTime * ballSpeed;
    }
    IEnumerator BallTime(float titi)
    {
        yield return new WaitForSeconds(titi);
        Destroy(this.gameObject);
    }
}
