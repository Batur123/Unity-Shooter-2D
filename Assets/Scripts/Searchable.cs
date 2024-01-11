using UnityEngine;

public class Searchable : MonoBehaviour {
    private bool _isKeyPressed;
    KeyCode keyToCheck = KeyCode.E;

    private bool isColliderPlayer(string colliderTag) {
        if (colliderTag == "Character") {
            return true;
        }

        return false;
    }
    private void OnTriggerEnter2D(Collider2D col) {
        if (isColliderPlayer(col.gameObject.tag)) {
            UIController.Instance.SetTextValue(UIController.TextType.INFO_TEXT, "Press E to Search");
            Debug.Log(col.gameObject.tag + " ENTER");
        }
    }
    
    private void OnTriggerExit2D(Collider2D col) {
        if (isColliderPlayer(col.gameObject.tag)) {
            UIController.Instance.SetTextValue(UIController.TextType.INFO_TEXT, "");
            Debug.Log(col.gameObject.tag + " EXIT");
        }
    }
    
    private void OnTriggerStay2D(Collider2D col) {
        if (isColliderPlayer(col.gameObject.tag) && Input.GetKey(keyToCheck) && !_isKeyPressed) {
            _isKeyPressed = true;
            Debug.Log(keyToCheck.ToString() + " pressed.");
            Debug.Log("Player gets random x item");
            Destroy(gameObject);
        }
    }
}
