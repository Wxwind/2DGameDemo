using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public GameObject beam;
    public GameObject beamHint;

    private void Start()
    {
        beam=transform.Find("beam").gameObject;
        beamHint=transform.Find("beamHint").gameObject;
        Hint();
    }

    private void Hint()
    {
        beamHint.SetActive(true);
        beam.SetActive(false);
    }
    

    public void Damage()
    {
        beamHint.SetActive(false);
        beam.SetActive(true);
    }
}
