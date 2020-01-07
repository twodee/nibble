using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
  public string color;
  public float speed;

  private new Rigidbody2D rigidbody;

  void Start() {
    rigidbody = GetComponent<Rigidbody2D>();
  }

  void Update() {
    float horizontal = Input.GetAxis($"{color} Horizontal");
    rigidbody.velocity = new Vector2(speed * horizontal, rigidbody.velocity.y);
  }

  void FixedUpdate() {
    if (Input.GetButtonDown($"{color} Vertical")) {
      rigidbody.AddForce(Vector3.up * 100);
    }
  }
}
