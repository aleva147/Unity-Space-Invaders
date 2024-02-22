using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour {
    // Za kretanje:
    public float speed = 5.0f;

    // Za pucanje:
    public Projectile laserPrefab;
    private bool shotFired;

    private void Update() {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            this.transform.position += Vector3.left * speed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            this.transform.position += Vector3.right * speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
            Shoot();
    }

    private void Shoot() {
        if (!shotFired) {
            Projectile projectile = Instantiate(laserPrefab, this.transform.position, Quaternion.identity);   // On je i kao treci parametar 'Quaternion.identity' - sto znaci bez rotacije jer je nebitna ovde
            shotFired = true;
            projectile.destroyed += LaserDestroyed;     // Dodajemo metod LaserDestroyed delegatu u klasi Projectile ciji se svi metodi izvrsavaju kada taj objekat dodje
                                                        //  u koliziju.
        }
    }

    private void LaserDestroyed() {
        shotFired = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Invader") || collision.gameObject.layer == LayerMask.NameToLayer("Missile"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
