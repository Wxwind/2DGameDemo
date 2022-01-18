
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EmitBeam : MonoBehaviour
{
    private GameObject beamPre;
    private float hintTime;
    private Timer hintTimer;
    private float lifeTime;
    private Timer lifeTimer;

    public List<Beam> beamsList=new List<Beam>();



    private void Update()
    {
        lifeTimer.Tick(Time.deltaTime);
        hintTimer.Tick(Time.deltaTime);
    }

    private void Start()
    {
        lifeTimer = new Timer(lifeTime,Delete,true);
        hintTimer = new Timer(hintTime, Damege,true);
    }

    public void Init(float lifeTime,float hintTime,GameObject beamPre)
    {
        this.lifeTime = lifeTime;
        this.hintTime = hintTime;
        this.beamPre = beamPre;
        Emit();
    }

    private void Emit()
    {
        float angle = Random.Range(-20f, 0f);
        for (int i = 0; i < 3; i++)
        {
            float a = angle - i * 30;
            var go=Instantiate(beamPre, transform);
            go.transform.Rotate(0,0,a);
            go.transform.position -= new Vector3(0.2f,0.2f,0);
            beamsList.Add(go.GetComponent<Beam>());
        }
        
    }

    private void Damege()
    {
        foreach (var beam in beamsList)
        {
            beam.Damage();
        }
    }
    private void Delete()
    {
        foreach (var beam in beamsList)
        {
            Destroy(beam.gameObject);
        }
        Destroy(this);
    }
}
