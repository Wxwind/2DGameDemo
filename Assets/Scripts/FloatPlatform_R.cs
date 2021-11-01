using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatPlatform_R : MonoBehaviour
{
    [SerializeField] private List<Transform> pointsList = new List<Transform>();
    public float moveSpeed;
    public float restTime;
    private Rigidbody2D rg;
    [SerializeField]private int index;
    [SerializeField]private bool arrived;
    private Timer timer;
    // Start is called before the first frame update
    private void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        index = 1;
        arrived = false;
        timer = new Timer(restTime, () => arrived = false);
        transform.position = pointsList[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        timer.Tick(Time.deltaTime);
        if (Vector2.Distance(transform.position, pointsList[index].position) < 0.01f)
        {
            arrived = true;
            timer.ResetAndRun();
            index++;
            if (index >= pointsList.Count)
            {
                index = 0;             
            }
        }
        if (arrived)
        {
            return;
        }
        else transform.position = Vector2.MoveTowards(transform.position, pointsList[index].position, moveSpeed * Time.deltaTime);
    }

}
