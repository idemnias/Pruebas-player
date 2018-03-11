using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

public class Knife : MonoBehaviour {

    [SerializeField]
    private float speed;

    private Rigidbody2D myrigibody2D;

    private Vector2 direction;


	// Use this for initialization
	void Start () {
        myrigibody2D = GetComponent<Rigidbody2D>();
	}

    void FixedUpdate()
    {
        myrigibody2D.velocity = direction * speed;    
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
