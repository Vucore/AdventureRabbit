using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        rb.transform.right = rb.velocity; //Huong Ox trung huong van toc
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Target")
        {
    
            Destroy(gameObject);
            Destroy(other.gameObject);
            UIDisplay.instance.UpdateScore();
        }
        else if(other.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
