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

    [Header("Português")]
    public Material menuPTNormal;    // Menuptsn
    public Material menuPTNoturna;   // material.081

    [Header("Inglês")]
    public Material menuENNormal;    // menuennormal
    public Material menuENNoturna;   // menuenvisao

    [Tooltip("Posição do material a trocar no MENU (0 = primeiro)")]
    public int indiceMaterialMenu = 0;

    private bool isPT = true;
    private bool ultimaVisaoNoturna = false;
    private bool pronto = false;

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
        pronto = true;
        Aplicar(LocalizationSettings.SelectedLocale);
    }

    void Update()
    {
        if (!pronto) return;

        // Só reaplica o MENU quando a visão noturna muda
        if (NightVision.IsNightVisionOn != ultimaVisaoNoturna)
        {
            AplicarMenu();
        }
    }

    void Aplicar(Locale locale)
    {
        if (locale == null) return;

        isPT = locale.Identifier.Code.StartsWith("pt");

        // Papéis
        Material[] materiais = isPT ? materiaisPT : materiaisEN;

        for (int i = 0; i < papeis.Length && i < materiais.Length; i++)
        {
            papeis[i].sharedMaterial = materiais[i];
        }

        AplicarMenu();
    }

    void AplicarMenu()
    {
        ultimaVisaoNoturna = NightVision.IsNightVisionOn;

        if (menu == null) return;

        Material novo;
        if (isPT)
            novo = ultimaVisaoNoturna ? menuPTNoturna : menuPTNormal;
        else
            novo = ultimaVisaoNoturna ? menuENNoturna : menuENNormal;

        if (novo == null) return;

        Material[] mats = menu.sharedMaterials;

        if (indiceMaterialMenu >= 0 && indiceMaterialMenu < mats.Length)
        {
            mats[indiceMaterialMenu] = novo;
            menu.sharedMaterials = mats;
        }
    }
}