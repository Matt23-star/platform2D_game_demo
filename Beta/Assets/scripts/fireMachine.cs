using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireMachine : MonoBehaviour
{
    public GameObject block;
    public float timeInterval;
    private int colorIndex;
    //public float block_x;
    //public float block_y;

    private float time=0;
    public bool onlyGray = false;
    //public bool isRandomInterval = false;
    public bool delay;
    public float startDelay;
    private bool delayCompleted = false;

 

    private Color[] color = { Color.black, Color.white, Color.gray };

    
    // Start is called before the first frame update
    void Start()
    {
        if (delay)
        {
            StartCoroutine(StartDelayCoroutine());
        }
        else
        {
            delayCompleted = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!delayCompleted) return;
        
        time = time + Time.deltaTime;
        if(time>= timeInterval)
        {
            if (onlyGray)
            {
                colorIndex = 2;
            }

            else
            {
                colorIndex = Random.Range(0, 3);
            }



            block.GetComponent<SpriteRenderer>().color = color[colorIndex];
            //bullet.GetComponent<SpriteRenderer>().size = new Vector2(block_x, block_y);

           
            Instantiate(block,this.transform.position,this.transform.rotation);

            time = 0;
        }
    }
    IEnumerator StartDelayCoroutine()
    {
        yield return new WaitForSeconds(startDelay);
        delayCompleted = true; // Delay is over, start firing
    }
}


