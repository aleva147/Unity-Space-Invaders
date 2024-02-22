using UnityEngine;
public class Invader : MonoBehaviour {
    // Za animaciju:
    private SpriteRenderer spriteRenderer;
    public Sprite[] animationSprites;
    public float animationTime = 1.0f;      // Vreme nakon kog ce se tekuci frejm promeniti.
    private int animationFrame;             // Index tekuceg sprajta iz niza sprajtova.

    // Za ubijanje:
    public System.Action killed;

    // Awake se poziva za objekat na pocetku zivotnog veka scripta (znaci valjda cim postoji neki objekat u sceni sa ovim scriptom, cak iako je neaktivan).
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start se poziva za objekat prvi put kad se aktivira script (znaci valjda kada u sceni postoji aktivan objekat sa ovim scriptom).
    private void Start() {
        // Pozvace zadati metod nakon sto istekne vreme zadato drugim parametrom, i onda ce ga repetativno pozivati svaki put kad istekne vreme zadato trecim parametrom.
        InvokeRepeating(nameof(AnimateFrames), this.animationTime, this.animationTime);
    }

    private void AnimateFrames() {
        animationFrame++;
        if (animationFrame >= this.animationSprites.Length) {
            animationFrame = 0;
        }
        spriteRenderer.sprite = this.animationSprites[animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser")) {     
            killed.Invoke();
            this.gameObject.SetActive(false);       // Msm da bi brisanje objekta poremetilo matricu u Invaders, pa zato samo deaktiviramo objekat. Ali mozda bi i sa brisanjem radilo.
        }
    }
}

