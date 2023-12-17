using UnityEngine;

public class Character : MonoBehaviour {
    [SerializeField] private float movementSpeed = 10.0f;
    private Rigidbody2D _rigidBody;
    private Vector2 _movement;

    void Awake() {
        _rigidBody = GetComponent<Rigidbody2D>();
        Physics2D.queriesStartInColliders = false;
    }

    void Update() {
        var keyD = Input.GetKey(KeyCode.D);
        var keyA = Input.GetKey(KeyCode.A);
        var keyW = Input.GetKey(KeyCode.W);
        var keyS = Input.GetKey(KeyCode.S);

        _movement.x = (keyD || keyA) ? (keyD ? 1 : -1) : 0;
        _movement.y = (keyW || keyS) ? (keyW ? 1 : -1) : 0;
    }

    void FixedUpdate() {
        _rigidBody.MovePosition(_rigidBody.position + _movement * movementSpeed * Time.fixedDeltaTime);
    }
}
