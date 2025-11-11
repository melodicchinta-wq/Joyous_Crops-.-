using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    [SerializeField] private Transform[] eyeOptions;
    [SerializeField] private Transform[] hairOptions;
    [SerializeField] private Transform[] coatOptions;
    [SerializeField] private Transform[] pantsOptions;
    [SerializeField] private Transform[] shoeOptions;

    void Start()
    {
        // Matikan semua dulu
        foreach (Transform t in eyeOptions) t.gameObject.SetActive(false);
        foreach (Transform t in hairOptions) t.gameObject.SetActive(false);
        foreach (Transform t in coatOptions) t.gameObject.SetActive(false);
        foreach (Transform t in pantsOptions) t.gameObject.SetActive(false);
        foreach (Transform t in shoeOptions) t.gameObject.SetActive(false);

        // Ambil data dari PlayerPrefs
        int eyeSelected = PlayerPrefs.GetInt("eyeSelected", 0);
        int hairSelected = PlayerPrefs.GetInt("hairSelected", 0);
        int coatSelected = PlayerPrefs.GetInt("coatSelected", 0);
        int pantsSelected = PlayerPrefs.GetInt("pantsSelected", 0);
        int shoesSelected = PlayerPrefs.GetInt("shoesSelected", 0);

        // Aktifkan bagian sesuai pilihan
        eyeOptions[eyeSelected].gameObject.SetActive(true);
        hairOptions[hairSelected].gameObject.SetActive(true);
        coatOptions[coatSelected].gameObject.SetActive(true);
        pantsOptions[pantsSelected].gameObject.SetActive(true);
        shoeOptions[shoesSelected].gameObject.SetActive(true);
    }
}
