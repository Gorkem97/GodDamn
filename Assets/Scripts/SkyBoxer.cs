using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxer : MonoBehaviour
{

    public Material SkyBoxOne;
    public Material SkyBoxTwo;
    public Material SkyBoxThree;
    public int crisp = 1;
    void Start()
    {
        if (crisp == 1)
        {
            RenderSettings.skybox = SkyBoxOne;
        }
        if (crisp == 2)
        {
            RenderSettings.skybox = SkyBoxTwo;
        }
        if (crisp == 3)
        {
            RenderSettings.skybox = SkyBoxThree;
        }
    }
    public void SkyChange(float which)
    {
        GameObject.Find("Spotlight").GetComponent<AudioSource>().Play();
        if (which == 1)
        {
            RenderSettings.skybox = SkyBoxOne;
        }
        if (which == 2)
        {
            RenderSettings.skybox = SkyBoxTwo;
        }
        if (which == 3)
        {
            RenderSettings.skybox = SkyBoxThree;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerCollider")
        {
            SkyChange(1);
        }
    }
}
