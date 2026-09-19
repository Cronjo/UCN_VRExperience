using UnityEngine;

public class PingPongBall : MonoBehaviour
{
    [Header("Configuracion de Fisica")]
    [SerializeField] private float forceMultiplier = 1.2f;

    [Header("Efectos de Audio")]
    [SerializeField] private AudioClip hitSound;
    private AudioSource audioSource;

    private Rigidbody rb;
    private PingPongManager manager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Obtener o agregar el componente AudioSource dinamicamente
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configuracion recomendada de audio para efectos breves
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1.0f; // Audio 3D en VR
    }

    public void Setup(PingPongManager gameManager)
    {
        manager = gameManager;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (manager == null) return;

        // Golpe con la raqueta
        if (collision.gameObject.CompareTag("Paddle"))
        {
            Vector3 hitDirection = collision.contacts[0].normal;
            rb.AddForce(-hitDirection * forceMultiplier, ForceMode.Impulse);
            PlayHitSound();
        }
        // Impacto exitoso en la pared
        else if (collision.gameObject.CompareTag("Wall"))
        {
            manager.RegisterWallHit();
            PlayHitSound();
        }
        // Caida al suelo
        else if (collision.gameObject.CompareTag("Floor"))
        {
            manager.GameOver();
        }
    }

    private void PlayHitSound()
    {
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }
}