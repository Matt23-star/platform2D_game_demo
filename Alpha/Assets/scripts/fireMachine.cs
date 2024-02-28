using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireMachine : MonoBehaviour
{
    public GameObject bullet;
    public float timeInterval = 1f;

    private float time=0;
    // Start is called before the first frame update
    void Start()
    {
                
    }

    // Update is called once per frame
    void Update()
    {
        time = time + Time.deltaTime;
        if(time>=timeInterval)
        {
            Instantiate(bullet,this.transform.position,this.transform.rotation);
            time = 0;
        }
    }
}
