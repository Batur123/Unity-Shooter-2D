using UnityEngine;

public class Searchable : MonoBehaviour {
    private bool _isKeyPressed;
    private const KeyCode SearchKey = KeyCode.E;

    private bool IsPlayer(string colliderTag) {
        return colliderTag == "Character";
    }
    
    private void OnTriggerEnter2D(Collider2D col) {
        if (IsPlayer(col.gameObject.tag)) {
            UIController.Instance.SetTextValue(UIController.TextType.INFO_TEXT, "Press E to Search");
        }
    }
    
    private void OnTriggerExit2D(Collider2D col) {
        if (IsPlayer(col.gameObject.tag)) {
            UIController.Instance.SetTextValue(UIController.TextType.INFO_TEXT, "");
        }
    }
    
    private void OnTriggerStay2D(Collider2D col) {
        if (IsPlayer(col.gameObject.tag) && Input.GetKey(SearchKey) && !_isKeyPressed) {
            _isKeyPressed = true;
            Debug.Log("Player gets random x item");
            Destroy(gameObject);
        }
    }
}
