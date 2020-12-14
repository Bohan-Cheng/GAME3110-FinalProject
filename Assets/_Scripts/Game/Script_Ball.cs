using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_Ball : MonoBehaviour
{
    public ParticleSystem ImpactPS;

    [SerializeField] float BallSpeed = 15.0f;
    [SerializeField] float MaxSpeed = 15.0f;
    [SerializeField] bool CanMove = true;
    [SerializeField] Text Player1Goal;
    [SerializeField] Text Player2Goal;
    [SerializeField] AudioSource CollisionSound;
    [SerializeField] AudioSource CollisionPlayerSound;
    [SerializeField] AudioSource HappySound;
    Vector3 Direction = Vector3.zero;
    Rigidbody rigi;

    void RandomDirection()
    {
        Direction = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
    }

    void Respawn()
    {
        gameObject.SetActive(true);
        CanMove = false;
        Invoke("Movement", 1.0f);
        transform.localPosition = new Vector3(0, -0.25f, 0);
        RandomDirection();
    }

    void Movement()
    {
        CanMove = true;
    }

    void ShowPlayer1Goal()
    {
        Player1Goal.gameObject.SetActive(true);
    }

    void HindPlayer1Goal()
    {
        Player1Goal.gameObject.SetActive(false);
    }

    void ShowPlayer2Goal()
    {
        Player2Goal.gameObject.SetActive(true);
    }

    void HindPlayer2Goal()
    {
        Player2Goal.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        Player1Goal.gameObject.SetActive(false);
        Player2Goal.gameObject.SetActive(false);
        rigi = gameObject.GetComponent<Rigidbody>();
        RandomDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if(CanMove)
        {
            rigi.AddForce(Direction * BallSpeed);

            // Limit max speed of the player
            rigi.velocity = Vector3.ClampMagnitude(rigi.velocity, MaxSpeed);
        }

        else
        {
            rigi.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider trigger)
    {
        //ImpactPS.gameObject.SetActive(true);
        //ImpactPS.Play();

        if (trigger.gameObject.tag == "Hole")
        {
            Script_Score.Player1Score += 1;
            ShowPlayer1Goal();
            Invoke("HindPlayer1Goal", 2.0f);
            Invoke("Respawn", 2.0f);
            gameObject.SetActive(false);
        }
        if (trigger.gameObject.tag == "PlayerHole")
        {
            Script_Score.Player2Score += 1;
            ShowPlayer2Goal();
            Invoke("HindPlayer2Goal", 2.0f);
            Invoke("Respawn", 2.0f);
            gameObject.SetActive(false);
        }

        HappySound.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ImpactPS.gameObject.SetActive(true);
        ImpactPS.Play();
        
        if (collision.gameObject.tag == "Player")
        {
            Direction = Vector3.Reflect(Direction, collision.contacts[0].normal) + collision.gameObject.GetComponent<Rigidbody>().velocity/10;
            Direction = Direction.normalized;
            CollisionPlayerSound.Play();
            
        }
        else
        {
            Direction = Vector3.Reflect(Direction, collision.contacts[0].normal);
            CollisionSound.Play();

            //if (collision.gameObject.tag == "Hole")
            //{
            //    Script_Score.Player1Score += 1;
            //    RandomDirection();
            //}
            //if(collision.gameObject.tag == "PlayerHole")
            //{
            //    Script_Score.Player2Score += 1;
            //    RandomDirection();
            //}
        }
        
        rigi.velocity = Vector3.zero;
    }
}
