using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletFire : MonoBehaviour
{
    public float speed = -1.5f;
    public float range = 5f;
    
    private float startX;

    // Start is called before the first frame update
    void Start()
    {
        startX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(speed * Time.deltaTime, 0, 0);

        if((Mathf.Abs(startX-transform.position.x))>=range)
        {
            Destroy(gameObject);
        }
    }
}
