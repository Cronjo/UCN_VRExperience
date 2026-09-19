using System.Collections;
using UnityEngine;

public class ChapelManager : MonoBehaviour
{
    [Header("Velas del Altar")]
    [Tooltip("Asigna las 3 velas (la de la entrada y las 2 fijas del candelabro)")]
    [SerializeField] private Candle[] allCandles;

    [Header("Efecto General del Altar")]
    [Tooltip("Sistema de particulas de lucecitas flotantes en el ambiente")]
    [SerializeField] private ParticleSystem floatingLightsVFX;

    [Header("Efectos de Audio")]
    [SerializeField] private AudioSource audioSource;
    [Tooltip("Sonido de encendido de fuego/chispa")]
    [SerializeField] private AudioClip igniteSound;
    [Tooltip("Sonido del canto angelical corto")]
    [SerializeField] private AudioClip angelicChantSound;

    [Header("Herramientas de Testeo (Sin VR)")]
    [SerializeField] private bool testActivateAltar = false;

    private bool isActivated = false;

    private void Awake()
    {
        // Asegurar que el VFX de particulas comience detenido
        if (floatingLightsVFX != null)
        {
            floatingLightsVFX.Stop();
        }
    }

    private void Update()
    {
        // Testeo rapido en el Inspector de Unity
        if (testActivateAltar)
        {
            testActivateAltar = false;
            ActivateAltarExperience();
        }
    }

    // Este metodo se ejecuta al colocar la vela en el Socket
    [ContextMenu("Test - Activar Capilla")]
    public void ActivateAltarExperience()
    {
        if (isActivated) return;

        isActivated = true;

        // 1. Encender todas las velas (las 2 estaticas + la del jugador)
        foreach (Candle candle in allCandles)
        {
            if (candle != null)
            {
                candle.LightCandle();
            }
        }

        // 2. Reproducir el sistema de particulas por 5 segundos
        if (floatingLightsVFX != null)
        {
            floatingLightsVFX.gameObject.SetActive(true);
            floatingLightsVFX.Play();
        }

        // 3. Reproducir los dos sonidos al mismo tiempo
        if (audioSource != null)
        {
            if (igniteSound != null)
            {
                audioSource.PlayOneShot(igniteSound);
            }

            if (angelicChantSound != null)
            {
                audioSource.PlayOneShot(angelicChantSound);
            }
        }

        // 4. Desactivar el GameObject de particulas tras 5 segundos
        StartCoroutine(DisableVFXAfterTime(5.0f));
    }

    private IEnumerator DisableVFXAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (floatingLightsVFX != null)
        {
            floatingLightsVFX.gameObject.SetActive(false);
        }
    }
}