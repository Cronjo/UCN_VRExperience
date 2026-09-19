using UnityEngine;
using TMPro;

public class PingPongManager : MonoBehaviour
{
    [Header("UI en Pared")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text statusText;

    [Header("Referencias de Juego")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballSpawnPoint;

    private GameObject currentBall;
    private int score = 0;
    private bool isGameActive = false;

    private void Start()
    {
        // Estado inicial antes de pulsar el botón
        scoreText.text = "";
        statusText.text = "Pulsa el botón para iniciar";
    }

    // Método que se conecta al evento del botón Poke
    public void StartGame()
    {
        // Si ya hay una pelota en escena, la destruimos
        if (currentBall != null)
        {
            Destroy(currentBall);
        }

        // Reiniciar variables
        score = 0;
        isGameActive = true;
        
        // Actualizar UI
        scoreText.text = "Impactos: 0";
        statusText.text = "";

        // Aparecer pelota
        currentBall = Instantiate(ballPrefab, ballSpawnPoint.position, ballSpawnPoint.rotation);
        
        // Vincular el evento de la pelota con este Manager
        PingPongBall ballScript = currentBall.GetComponent<PingPongBall>();
        if (ballScript != null)
        {
            ballScript.Setup(this);
        }
    }

    public void RegisterWallHit()
    {
        if (!isGameActive) return;

        score++;
        scoreText.text = $"Impactos: {score}";
    }

    public void GameOver()
    {
        if (!isGameActive) return;

        isGameActive = false;
        statusText.text = "¡Has Perdido!";
        
        // La pelota desaparece
        if (currentBall != null)
        {
            Destroy(currentBall, 0.1f);
        }
    }
}