using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Utils;

public class PowerUpCinematicControl : WeakSingleton<PowerUpCinematicControl>
{
    [SerializeField]
    private CinemachineVirtualCamera powerUpCamera;

    private void Awake()
    {
        ControlSingleton();
    }

    public void ActivatePowerUpCinematic()
    {
        StartCoroutine(PowerUpCoroutine());
    }

    private IEnumerator PowerUpCoroutine()
    {
        powerUpCamera.gameObject.SetActive(true);
        DOTween.To(() => Time.timeScale, value => Time.timeScale = value, 0.2f, 0.2f);
        yield return new WaitForSeconds(0.5f);
        DOTween.To(() => Time.timeScale, value => Time.timeScale = value, 1.0f, 0.8f);
        powerUpCamera.gameObject.SetActive(false);
    }
}
