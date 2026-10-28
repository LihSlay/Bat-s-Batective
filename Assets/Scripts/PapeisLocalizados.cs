using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class PapeisLocalizados : MonoBehaviour
{
    [Header("Os 4 objetos papel1img ... papel4img (por ordem)")]
    public Renderer[] papeis;

    [Header("Materiais em português: 1, 2, 3, 4")]
    public Material[] materiaisPT;

    [Header("Materiais em inglês: 1dog, 2parrot, 3frog, 4cat")]
    public Material[] materiaisEN;

    [Header("Objeto MENU")]
    public Renderer menu;
    public Material menuPT;   // material.081
    public Material menuEN;   // Material.081v2

    [Tooltip("Posição do material a trocar no MENU (0 = primeiro)")]
    public int indiceMaterialMenu = 0;

    void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += Aplicar;
    }

    void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= Aplicar;
    }

    IEnumerator Start()
    {
        // Espera que o sistema de localização esteja pronto
        yield return LocalizationSettings.InitializationOperation;
        Aplicar(LocalizationSettings.SelectedLocale);
    }

    void Aplicar(Locale locale)
    {
        if (locale == null) return;

        bool isPT = locale.Identifier.Code.StartsWith("pt");

        // Papéis
        Material[] materiais = isPT ? materiaisPT : materiaisEN;

        for (int i = 0; i < papeis.Length && i < materiais.Length; i++)
        {
            papeis[i].sharedMaterial = materiais[i];
        }

        // MENU
        if (menu != null)
        {
            Material[] mats = menu.sharedMaterials;

            if (indiceMaterialMenu >= 0 && indiceMaterialMenu < mats.Length)
            {
                mats[indiceMaterialMenu] = isPT ? menuPT : menuEN;
                menu.sharedMaterials = mats;
            }
        }
    }
}