using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
  public string color;
  public float speed;

  private new Rigidbody2D rigidbody;
  private LayerMask mask;
  private Collider2D[] hits;

  void Start() {
    rigidbody = GetComponent<Rigidbody2D>();
    mask = LayerMask.GetMask(color);
    hits = new Collider2D[10];
  }

  void Update() {
    float horizontal = Input.GetAxis($"{color} Horizontal");
    rigidbody.velocity = new Vector2(speed * horizontal, rigidbody.velocity.y);
  }

  void FixedUpdate() {
    if (Input.GetButtonDown($"{color} Vertical")) {
      Vector2 footPosition = new Vector2(transform.position.x, transform.position.y - 0.25f);
      int hitCount = Physics2D.OverlapCircleNonAlloc(footPosition, 0.01f, hits, mask);
      bool isHit = false;
      int i = 0;
      while (i < hitCount && !isHit) {
        isHit = hits[i].CompareTag("Block");
        i += 1;
      }
      if (isHit) {
        rigidbody.AddForce(Vector3.up * 200);
      }
    }
  }
}
