using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Ball : MonoBehaviour
{
    public ParticleSystem ImpactPS;

    [SerializeField] float BallSpeed = 15.0f;
    [SerializeField] float MaxSpeed = 15.0f;
    Vector3 Direction = Vector3.zero;
    Rigidbody rigi;

    void RandomDirection()
    {
        Direction = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
    }

    // Start is called before the first frame update
    void Start()
    {
        rigi = gameObject.GetComponent<Rigidbody>();
        RandomDirection();
    }

    // Update is called once per frame
    void Update()
    {
        rigi.AddForce(Direction * BallSpeed * Time.deltaTime);

        // Limit max speed of the player
        rigi.velocity = Vector3.ClampMagnitude(rigi.velocity, MaxSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ImpactPS.gameObject.SetActive(true);
        ImpactPS.Play();
        
        if (collision.gameObject.tag == "Player")
        {
            Direction = Vector3.Reflect(Direction, collision.contacts[0].normal) + collision.gameObject.GetComponent<Rigidbody>().velocity/10;
            Direction = Direction.normalized;
        }
        else
        {
            Direction = Vector3.Reflect(Direction, collision.contacts[0].normal);
        }
            rigi.velocity = Vector3.zero;
    }
}
