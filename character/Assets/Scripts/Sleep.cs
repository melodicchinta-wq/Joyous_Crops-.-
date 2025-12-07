using System.Collections;
using UnityEngine;

public class Sleep : MonoBehaviour
{
    internal void DoSleep()
    {
        StartCoroutine(SleepRoutine());
    }

    IEnumerator SleepRoutine()
    {
        OverlayFade overlayFade = GameManager.instance.overlayFade;

        // Matikan pengaruh day/night fade
        overlayFade.sleepMode = true;

        // Fade hitam 2 detik
        yield return StartCoroutine(overlayFade.FadeTo(1f, 2f));

        // --- UBAH WAKTU & DAY ---
        TimeController.instance.day++;   // tambah hari
        TimeController.instance.hour = 6;
        TimeController.instance.minute = 0;
        WeatherManager.instance.OnNewDay(); // Update cuaca

        // Tunggu sebentar (opsional)
        yield return new WaitForSeconds(0.5f);

        // Fade kembali ke day/night 2 detik
        yield return StartCoroutine(overlayFade.FadeTo(overlayFade.maxDarkness, 2f));

        // Nyalakan kembali day/night fade
        overlayFade.sleepMode = false;
    }
}
