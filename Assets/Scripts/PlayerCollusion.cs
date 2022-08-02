using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollusion : MonoBehaviour
{
    GameObject Player;
    walk Script;
    // Start is called before the first frame update
    void Start()
    {
        Player = transform.parent.gameObject;
        Script = Player.GetComponent<walk>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Light")
        {
            Script.Health += 30 * Time.deltaTime;
            Player.GetComponent<walk>().BeforeTransform = Player.transform.position;
        }
        if (other.gameObject.name == "PlayerDetection")
        {
            other.gameObject.GetComponentInParent<EnemyBehaviour>().StateFollow = true;
        }
    }
}
