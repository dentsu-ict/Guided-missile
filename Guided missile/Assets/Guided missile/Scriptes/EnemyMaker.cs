using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaker : MonoBehaviour
{
    public GameObject Enemy;
    float time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            Vector2 pos = new Vector2(Random.Range(-11, 12), Random.Range(-6, 7));
            Instantiate(Enemy,pos,Quaternion.identity);
            time = 3.0f;
        }
    }
}
