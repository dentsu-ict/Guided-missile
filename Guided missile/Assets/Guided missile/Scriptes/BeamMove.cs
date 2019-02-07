using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamMove : MonoBehaviour
{
    float Degree;
    Vector2 SP, nsp;
    Rigidbody2D _rigidbody;
    private Vector2 Camera_max, Camera_min;
    bool OutFlag;
    int num, BeamNum;
    GameObject Enemy;


    public void SetMissile(GameObject _Enemy,Vector2 direction, float speed, float degree)
    {
        Enemy = _Enemy;
        //向きとスピードの設定
        SP = speed * direction;
        Degree = degree;
        //ビームの長さ
        BeamNum = GetComponent<Beam>().OldPosNum;
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        Camera_min = Camera.main.ViewportToWorldPoint(Vector2.zero);
        Camera_max = Camera.main.ViewportToWorldPoint(Vector2.one);
        OutFlag = false;
    }

    void FixedUpdate()
    {
        if (Enemy != null)
        {
            Vector2 b = Enemy.transform.position - transform.position;
            float cross = SP.x * b.y - SP.y * b.x;
            cross /= (b.magnitude * SP.magnitude);
            if (cross > 0.2f)
            {
                SP = Quaternion.Euler(0.0f, 0.0f, Degree) * SP;
            }
            else if (cross < -0.2f)
            {
                SP = Quaternion.Euler(0.0f, 0.0f, -Degree) * SP;
            }
        }
        _rigidbody.velocity = SP;
    }
    private void LateUpdate()
    {
        Vector2 pos = transform.position;
        if (OutFlag == true)
        {
            num++;
            if (num >= BeamNum) Destroy(gameObject);
        }
        else
        {
            if (pos.x > Camera_max.x || pos.x < Camera_min.x || pos.y > Camera_max.y || pos.y < Camera_min.y)
            {
                SP = new Vector2(0, 0);
                OutFlag = true;
                num = 0;
            }
        }

    }
}
