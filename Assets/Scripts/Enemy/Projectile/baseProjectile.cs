using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseProjectile : MonoBehaviour
{
    SPProjectile _sStatsAttb;

    // Start is called before the first frame update
    void Start()
    {
        _sStatsAttb = GetComponent<SPProjectile>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static void _SpeedCalculation()
    {
        
    }
}
