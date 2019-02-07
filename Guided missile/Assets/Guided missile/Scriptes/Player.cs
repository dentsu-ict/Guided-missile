using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector2 velocity;
    Rigidbody2D _rigidbody;
    public float speed=7.0f;
    public GameObject Beam;
    public float beamspeed = 20;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();                    
    }

    // Update is called once per frame
    void Update()
    {
        velocity = Vector2.zero;
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        velocity = new Vector2(x, y);
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchScreenPosition = Input.mousePosition;
            Vector3 screenPos = Camera.main.ScreenToWorldPoint(touchScreenPosition);
            screenPos.z = -1.0f;
            RaycastHit2D[] hits = Physics2D.RaycastAll(screenPos, new Vector3(0, 0, 2));
            foreach(var hit in hits)
            {

                if (hit.collider)
                {
                    if (hit.collider.gameObject.tag == "enemy")
                    {
                        GameObject obj = Instantiate(Beam, transform.position, Quaternion.identity);
                        BeamMove bm = obj.GetComponent<BeamMove>();
                        Vector2 direction=(hit.collider.gameObject.transform.position - transform.position).normalized;
                        bm.SetMissile(hit.collider.gameObject, direction, beamspeed,10);                            
                        break;
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = velocity*speed;
    }

    private void LateUpdate()
    {
        transform.localPosition = Clamp(transform.localPosition);
    }

    public Vector2 Clamp(Vector2 pos)
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        pos.x = Mathf.Clamp(pos.x, min.x + 0.5f, max.x - 0.5f);
        pos.y = Mathf.Clamp(pos.y, min.y + 0.5f, max.y - 0.5f);
        return pos;
    }

}
