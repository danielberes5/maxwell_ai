using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSpawner : MonoBehaviour
{
    public GameObject BallPrefab;

    public void Begin() {
        for (int i = 0; i < 5; i++)
        {
            // We instantiate a new Ball at the generated axis while keeping the Y and Z axes constant
            var ball = Instantiate(BallPrefab, transform.position + new Vector3(0, 25, -12), Quaternion.identity, gameObject.transform);

            // We make sure that the Ball is properly scaled
            ball.transform.localScale = new Vector3(5, 5, 5);

            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = new Vector3(Random.Range(50, 100) * (Random.Range(0, 1) == 0 ? 1 : -1), Random.Range(50, 100) * (Random.Range(0, 1) == 0 ? 1 : -1), Random.Range(50, 100) * (Random.Range(0, 1) == 0 ? 1 : -1));
                rb.useGravity = false; // Gravitáció kikapcsolása
            }
        }
    }
    void Start()
    {
        
    }

    private void Update()
    {
        
    }


}
