using UnityEngine;

public class Candle : MonoBehaviour
{
    [Header("Efectos Individuales")]
    [SerializeField] private GameObject flameAndLight; // Objeto hijo con la llama y luz puntual de esta vela

    private bool isLit = false;

    private void Awake()
    {
        // Iniciar apagada
        if (flameAndLight != null)
        {
            flameAndLight.SetActive(false);
        }
    }

    public void LightCandle()
    {
        if (isLit) return;

        isLit = true;

        if (flameAndLight != null)
        {
            flameAndLight.SetActive(true);
        }
    }
}