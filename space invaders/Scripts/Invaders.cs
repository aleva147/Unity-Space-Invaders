using UnityEngine;
using UnityEngine.SceneManagement;

public class Invaders : MonoBehaviour {
    // Za inicijalizaciju matrice:
    public Invader[] prefabs;   // Imace elemenata koliko ima vrsti matrice neprijatelja. U i-toj vrsti svi neprijatelji su identicni i uzimaju se kao i-ti elem ovog niza.
    public int rows = 5;
    public int cols = 11;

    // Za kretanje matrice:
    public AnimationCurve speed;                   // Zelimo da brzina zavisi od procenta ubijenih neprijatelja, pa ce biti kao grafik gde je po x-osi kolicina ubijenih nepr. a po y-osi brzina. 
    private Vector3 direction = Vector2.right;

    // Za pracenje kolicine ubijenih neprijatelja:
    public int amountKilled { get; private set; }  // Javan atribut za dohvatanje, privatan za setovanje.
    public int amountTotal => rows * cols;     // Samo '=' ne bi dozvolio jer su nestaticki atributi zdesna, morao bi u Awake metodu. 
    public float percentKilled => (float)amountKilled / (float)amountTotal;

    // Za ispaljivanje raketi:
    public int amountAlive => amountTotal - amountKilled;
    public float missileAttackRate = 1.0f;     // Koliko cesto pozivamo metodu za prolazak kroz matricu i pokusaj ispaljivanja rakete.
    public Projectile missilePrefab;

    private void Awake() {

        // Ne zelimo da gornji levi ugao matrice neprijatelja bude pozicija Invaders game objekta, nego da tu bude sredina matrice:
        float width = 2.0f * (cols - 1);    // Ukupna sirina matrice (racunajuci padding koji smo resili da stavljamo izmedju neprijatelja, 2.0f).
        float height = 2.0f * (rows - 1);
        Vector2 centering = new Vector2(-width / 2, -height / 2);   // Ovo je sad pozicija koju zelimo da ima gornji levi ugao matrice.

        for (int row = 0; row < rows; row++) {
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * 2.0f), 0.0f);   // Pozicija prvog neprijatelja u vrsti (tj pozicija pocetka vrste)
           
            for (int col = 0; col < cols; col++) {
                Invader invader = Instantiate(prefabs[row], this.transform);    // Stvara objekat zadatog neprijatelja iz niza prefabs kao dete objekta Invaders (this objekta).
                invader.killed += InvaderKilled;    // Dakle svaki objekat neprijatelja ima svog delegata, i svaki taj delegat ima jedno pozivanje ovog metoda zasebno.

                Vector3 position = rowPosition;                 // Pozicioniranje tekuceg objekta neprijatelja na odgovarajuce mesto u vrsti.
                position.x += 2.0f * col;
                invader.transform.localPosition = position;     // localPosition da bi se u odnosu na roditeljski (Invaders) objekat pozicioniralo, a ne centar sveta.
            }
        }
    }

    private void Start() {
        InvokeRepeating(nameof(MissileAttack), missileAttackRate, missileAttackRate);
    }

    private void Update() {
        //this.transform.position += direction * speed * Time.deltaTime;   // deltaTime kako ne bi variralo od frejmrejta koliko cesto (time i koliko brzo) se vrsi pomeranje.
        this.transform.position += direction * speed.Evaluate(percentKilled) * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);     // 0 0 0 
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);  // 1 0 0 

        // Prolazimo kroz svu decu Invaders objekta (sve neprijatelje u matrici) i gledamo da li je bilo ko od njih dosao blizu ivice ekrana - efikasnije bi mozda moglo
        //      da imas atribut koji pamti najdesnijeg i najlevljeg aktivnog neprijatelja, pa samo njih da proveravas. Ako vise nisu aktivni onda prolazis kroz matricu
        //      trazeci novog najdesnijeg/najlevljeg pa za njih proveru radis.
        foreach (Transform invader in this.transform) {
            if (!invader.gameObject.activeInHierarchy) continue;       // Kada ubijemo neprijatelja, samo cemo ga deaktivirati u hijerarhiji a necemo ga obrisati, pa zato ova provera.

            if (direction == Vector3.right && invader.position.x >= (rightEdge.x - 1.0f))
                AdvanceRow();
            else if (direction == Vector3.left && invader.position.x <= (leftEdge.x + 1.0f))
                AdvanceRow();
        }
    }

    private void AdvanceRow() {
        direction.x *= -1.0f;    // Obrcemo stranu kretanja.
        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;         // Nzm zasto nije moglo direktno samo 'this.transform.position.y -= 1.0f'
    }

    private void InvaderKilled() {
        amountKilled++;

        if (amountKilled >= amountTotal) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void MissileAttack() {
        foreach (Transform invader in this.transform) {
            if (!invader.gameObject.activeInHierarchy) continue;

            if (Random.value < (1.0f / (float)amountAlive)) {
                Instantiate(missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }
}
