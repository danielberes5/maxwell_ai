using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MyAgent : Agent
{
    Rigidbody m_rigidbody;

    public GameObject RedS;
    public GameObject BlueS;

    bool isOpen = false;


    private enum ACTIONS
    {
        OPEN = 0,
        NOTHING = 1,
        CLOSE = 2
    }

    void Start()
    {
        // Eredeti anyag mentése
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // We reset the agent's position
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // We don't need this function now because we use the RayPerceptionSensor
        // Note however that we could add additional observations here, if we wanted to, like the speed & velocity of the agent etc.
    }

    /*
    void Update()
    {
        if (Input.GetKey("w"))
        {
            OpenDoor();
        }
        else if (Input.GetKey("s"))
        {
            CloseDoor();
        }
    }*/


    public override void Heuristic(in ActionBuffers actionsOut)
    {

        ActionSegment<int> actions = actionsOut.DiscreteActions;

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        if (horizontal == -1)
        {
            actions[0] = (int)ACTIONS.OPEN;
        }
        else if (horizontal == +1)
        {
            actions[0] = (int)ACTIONS.CLOSE;
        }
        else
        {
            actions[0] = (int)ACTIONS.NOTHING;
        }


    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        var actionTaken = actions.DiscreteActions[0];

        switch (actionTaken)
        {
            case (int)ACTIONS.NOTHING:
                break;
            case (int)ACTIONS.OPEN:
                OpenDoor();
                break;
            case (int)ACTIONS.CLOSE:
               CloseDoor();
                break;
        }

        if (!isOpen) {
            AddReward(0.1f);
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // Ellenõrzi, hogy ütközött-e a labda
        if (collision.gameObject.CompareTag("BlueBall"))
        {

            // Meghatározza, hogy a labda melyik oldalán ütközik az ajtónak
            bool ballOnLeftSide = collision.contacts[0].point.z < transform.position.z;
            //EndThisEpisode();
        }

        if (collision.gameObject.CompareTag("RedBall"))
        {

            // Meghatározza, hogy a labda melyik oldalán ütközik az ajtónak
            bool ballOnLeftSide = collision.contacts[0].point.z > transform.position.z;
            //EndThisEpisode();
        }
    }

    private void EndThisEpisode() {

        var parentB = BlueS.transform;
        int numberOfChildrenB = parentB.childCount;

        for (int i = 0; i < numberOfChildrenB; i++)
        {
            if (parentB.GetChild(i).tag == "BlueBall")
            {
                Destroy(parentB.GetChild(i).gameObject);
            }
        }

        var parentR = RedS.transform;
        int numberOfChildrenR = parentR.childCount;

        for (int i = 0; i < numberOfChildrenR; i++)
        {
            if (parentR.GetChild(i).tag == "RedBall")
            {
                Destroy(parentR.GetChild(i).gameObject);
            }
        }

        EndEpisode();

    }

    // Az ajtó nyitására szolgáló metódus
    private void OpenDoor()
    {
        GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 0.2f); // Átlátszó anyag beállítása
        GetComponent<BoxCollider>().enabled = false; // Collider kikapcsolása, hogy áthaladhassunk rajta
        isOpen = true;
    }

    // Az ajtó bezárására szolgáló metódus
    private void CloseDoor()
    {
        GetComponent<Renderer>().material.color = new Color(1f, 0f, 1f, 1f); // Eredeti anyag visszaállítása
        GetComponent<BoxCollider>().enabled = true; // Collider újra engedélyezése
        isOpen = false;
    }

}