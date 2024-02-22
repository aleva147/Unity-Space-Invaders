using UnityEngine;

public class Projectile : MonoBehaviour {
    public Vector3 direction;
    public float speed;
    public System.Action destroyed; // Ovo je delegat (zasad bez ijednog metoda koji se poziva).

    private void Update() {
        this.transform.position += direction * speed * Time.deltaTime;
    }

    // Poziva se cim ovaj objekat dodje u koliziju sa bilo cime zbog cekiranog IsTrigger na BoxCollider2D, ali potrebno je da ili ovaj objekat, ili onaj sa kojim dolazi
    //  u koliziju, sadrzi Rigidbody2D komponentu kako bi se ovaj metod pozivao. Jer ovaj metod spada u metode simulacije fizike u Unuity engine-u, i da bi se u simulaciji
    //  detektovala kolizija, barem jedan od dva objekta mora biti 'fizicki' objekat (postaje fizicki objekat dodavanjem Rigidbody komp), a ne samo obican objekat.
    private void OnTriggerEnter2D(Collider2D collision) {
        if (destroyed != null)          // Jer samo za laser koristimo destroyed, za rakete nema nijednog metoda u destroyed, pa da ne izbacuje error. 
            this.destroyed.Invoke();    // Zelimo da se izvrse svi metodi destroyed delegata pre unistavanja objekta projektila.
        Destroy(gameObject);
    }
}
