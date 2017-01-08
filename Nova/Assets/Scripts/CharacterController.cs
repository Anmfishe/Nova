using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {
    private Rigidbody2D rb2d;
    private float jumpForce = 150f;
    private float moveForce = 2f;
    private bool jumping = false;
	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            rb2d.AddForce(Vector3.up * jumpForce);
        }
        /*if (Input.GetKeyDown(KeyCode.D))
        {
            Vector3 position = this.transform.position;
            position.x++;
            this.transform.position = position;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 position = this.transform.position;
            position.x--;
            this.transform.position = position;
        }*/
        var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += move * moveForce * Time.deltaTime;

    }
}
