using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class ShotScript : MonoBehaviour
{
    [SerializeField] private GameObject bullet;             // Prefab da bala
    [SerializeField] private Transform firePoint;           // Ponto de disparo
    [SerializeField] private float bulletSpeed = 10f;       // Velocidade da bala

    public InputHelpers.Button shootButton;                 // Botão para atirar
    public XRNode inputSource;                              // Controlador VR (esquerdo ou direito)
    public float activationThreshold = 0.1f;                // Sensibilidade do botão

    private bool wasPressedLastFrame = false;               // Para evitar disparos contínuos

    void Update()
    {
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), shootButton, out bool isPressed, activationThreshold);

        // Só dispara no momento em que o botão é pressionado (evita múltiplos tiros por frame)
        if (isPressed && !wasPressedLastFrame)
        {
            FireShot();
        }

        wasPressedLastFrame = isPressed;
    }

    public void FireShot()
    {
        GameObject spawnBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        Rigidbody rb = spawnBullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * bulletSpeed;
        }
    }

    // Gizmo opcional para ver a direção do firePoint no editor
    private void OnDrawGizmos()
    {
        if (firePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(firePoint.position, firePoint.forward * 2);
        }
    }
}
