using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public int life = 3;
    public int damageAmount = 1;

    public Renderer overlayRenderer;  // O Renderer do objeto na frente da câmera
    public float alphaIncrease = 0.2f;  // Quanto aumenta o alpha por dano
    private Material overlayMaterial;
    private Color overlayColor;

    private void Start()
    {
        if (overlayRenderer != null)
        {
            overlayMaterial = overlayRenderer.material;  // Cópia instanciada do material
            overlayColor = overlayMaterial.color;
            overlayColor.a = 0f;  // Começa invisível
            overlayMaterial.color = overlayColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damage"))
        {
            TakeDamage(damageAmount);
        }
    }

    void TakeDamage(int amount)
    {
        life -= amount;
        Debug.Log("Player levou dano! Vida restante: " + life);

        IncreaseOverlayAlpha();

        if (life <= 0)
        {
            Die();
        }
    }

    void IncreaseOverlayAlpha()
    {
        if (overlayMaterial != null)
        {
            overlayColor.a = Mathf.Clamp01(overlayColor.a + alphaIncrease);
            overlayMaterial.color = overlayColor;
        }
    }

    void Die()
    {
        Debug.Log("Player morreu!");
        SceneTransitionManager.singleton.GoToSceneAsync(2);

    }
}
