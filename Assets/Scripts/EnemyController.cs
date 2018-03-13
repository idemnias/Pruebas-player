using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    bool seenPlayer = false;
    RaycastHit2D seePlayer;

    Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Just a debug visual representation of the Linecast, can only see this in scene view! Doesn't actually do anything!
        Debug.DrawLine(transform.position, (transform.position - transform.forward * 10), Color.magenta);

        //Using linecast which takes (start point, end point, layermask) so we can make it only detect objects with specified layers
        //its wrapped in an if statement, so that while the tip of the Linecast is touching an object with layer, the code inside executes
        if (Physics2D.Linecast(transform.position, (transform.position - transform.forward * 10), 1 << LayerMask.NameToLayer("GameObject")))
        {
            //store the collider object the Linecast hit so that we can do something with that specific object
            //each time the linecast touches a new object with layer
            seePlayer = Physics2D.Linecast(transform.position, (transform.position - transform.forward*10), 1 << LayerMask.NameToLayer("Guard"));
            seenPlayer = true; //since the linecase is touching the guard and we are in range, we can now interact!
        }
        else
        {
            seenPlayer = false; //if the linecast is not touching
        }

    }






    //     if(LeftRightZ)
    //             {
    //                 if(EyeScanZ< 30)
    //                 {
    //                     EyeScanZ += 100 * Time.deltaTime;
    //                 }
    //                 else
    //                 {
    //                     LeftRightZ = false;
    //                 }
    //             }
    //             else
    //             {
    //                 if (EyeScanZ > -30)
    //                 {
    //                     EyeScanZ -= 100 * Time.deltaTime;
    //                 }
    //                 else
    //                 {
    //                     LeftRightZ = true;
    //                 }
    //             }
    //             transform.Find("MEyes").transform.localEulerAngles = new Vector3(0, EyeScanZ);


    //RaycastHit hit;
    //Debug.DrawRay(transform.Find("MEyes").position, transform.Find("MEyes").transform.forward* ViewDistance);

    //             if (Physics.Raycast(transform.Find("MEyes").position, transform.Find("MEyes").transform.forward* ViewDistance, out hit, ViewDistance))
    //             {
    //                 if(hit.transform.gameObject.tag == "Player")
    //                 {
    //                     Debug.Log(gameObject.name + " CAN see Player");
    //                 }

    //             }   https://answers.unity.com/questions/438175/enemy-ai-detect-player-when-seen.html





}
