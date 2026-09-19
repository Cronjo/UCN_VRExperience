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

    [Header("Herramientas de Testeo (Sin VR)")]
    [Tooltip("Marca esta casilla para iniciar/reiniciar el minijuego desde el Inspector")]
    [SerializeField] private bool testStartGame = false;

    [Tooltip("Marca esta casilla para simular que la pelota impacto en la pared")]
    [SerializeField] private bool testSimulateWallHit = false;

    private GameObject currentBall;
    private int score = 0;
    private bool isGameActive = false;

    private void Start()
    {
        // Estado inicial antes de pulsar el boton
        scoreText.text = "";
        statusText.text = "Pulsa el boton para iniciar";
    }

    private void Update()
    {
        // Detectar cambios en las casillas de testeo durante la ejecucion
        if (testStartGame)
        {
            testStartGame = false;
            StartGame();
        }

        if (testSimulateWallHit)
        {
            testSimulateWallHit = false;
            RegisterWallHit();
        }
    }

    // Metodo que se conecta al evento del boton Poke o a la prueba manual
    [ContextMenu("Test - Iniciar Juego")]
    public void StartGame()
    {
        // Si ya hay una pelota en escena la destruimos
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
        if (ballPrefab != null && ballSpawnPoint != null)
        {
            currentBall = Instantiate(ballPrefab, ballSpawnPoint.position, ballSpawnPoint.rotation);

            // Vincular el evento de la pelota con este Manager
            PingPongBall ballScript = currentBall.GetComponent<PingPongBall>();
            if (ballScript != null)
            {
                ballScript.Setup(this);
            }
        }
        else
        {
            Debug.LogWarning("Falta asignar el Ball Prefab o el Ball Spawn Point en el Inspector");
        }
    }

    [ContextMenu("Test - Simular Impacto Pared")]
    public void RegisterWallHit()
    {
        if (!isGameActive)
        {
            Debug.LogWarning("El juego no esta activo. Inicia el juego antes de registrar un impacto");
            return;
        }

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