using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 100f; 
    public Rigidbody rb; 
    public Vector3 movement;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        moveCharacter(movement);
    }

    void moveCharacter(Vector3 direction)
    {
        rb.linearVelocity = direction * speed * Time.fixedDeltaTime;
    }
}
