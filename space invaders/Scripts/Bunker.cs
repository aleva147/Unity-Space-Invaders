using UnityEngine;

public class Bunker : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Invader"))
            gameObject.SetActive(false);
    }
}
