using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplePatrol : MonoBehaviour {

    public GameObject plataform;

    [SerializeField]
    private Transform childTransform;

    public float moveSpeed;

    public Transform currentPoint;

    public Transform[] points;

    public int pointSelection;

	// Use this for initialization
	void Start () {
        currentPoint = points[pointSelection];
	}
	
	// Update is called once per frame
	void Update () {
        plataform.transform.position = Vector3.MoveTowards(plataform.transform.position, currentPoint.position, Time.deltaTime * moveSpeed);

        if (plataform.transform.position == currentPoint.position)
        {
            pointSelection++;

            if (pointSelection == points.Length)
            {
                pointSelection = 0;
            }

            currentPoint = points[pointSelection];
        }
	}

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (i!=points.Length-1)
            {
                Gizmos.DrawLine(points[i].position, points[i].position);
            }
            
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.SetParent(null);
        }
        
    }

}
