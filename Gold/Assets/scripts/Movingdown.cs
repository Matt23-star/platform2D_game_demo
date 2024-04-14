using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movingdown : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(0, speed * Time.deltaTime, 0);
    }
}
