using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Problem : MonoBehaviour
{
    public TextMeshProUGUI output;
    public int bitis;
    public Vector3[] Girdi;
    List<Vector3> AllThings = new List<Vector3>();
    List<float> portalKonum = new List<float>();

    float ant = 0;
    int time = 0;
    public void Initiate()
    {
        foreach (Vector3 item in Girdi)
        {
            AllThings.Add(item);
            portalKonum.Add(item.x);
        }

        while (ant < bitis)
        {
            if (portalKonum.Contains(ant))
            {
                int which = portalKonum.IndexOf(ant);
                Vector3 temporal = AllThings[which];
                if (temporal.z == 1)
                {
                    time += 1;
                    ant = temporal.y;
                    AllThings[which] = new Vector3(temporal.x, temporal.y, 0);
                }
                if (temporal.z == 0)
                {
                    AllThings[which] = new Vector3(temporal.x, temporal.y, 1);
                }
            }
            ant += 1;
            time += 1;
        }
        if (ant == bitis)
        {
            output.text = time.ToString();
            ant = 0;
            time = 0;
        }
    }
}
