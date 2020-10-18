using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerControl : MonoBehaviour
{
    bool IsHost = true;

    float MoveSpeed = 50000.0f;
    float MaxSpeed = 6;
    Rigidbody rigi;

    public void SetHost(bool host)
    {
        IsHost = host;
    }

    void Start()
    {
        if (!IsHost)
            MoveSpeed *= -1;
        rigi = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        // Move the player
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        rigi.AddForce(movement * MoveSpeed * Time.deltaTime);

        // Limit max speed of the player
        rigi.velocity = Vector3.ClampMagnitude(rigi.velocity, MaxSpeed);

        // Slow stop the player
        if (moveHorizontal == 0)
            rigi.velocity = Vector3.Lerp(rigi.velocity, Vector3.zero, 20.0f * Time.deltaTime);
    }
}
