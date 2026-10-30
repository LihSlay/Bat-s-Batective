using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class TabMenu : MonoBehaviour
{
    [System.Serializable]
    public struct SubTab
    {
        public Button button;
        public GameObject image;
        public Sprite spriteEN;   // opcional: imagem a mostrar quando o jogo está em inglês
    }

    [System.Serializable]
    public struct MainTab
    {
        public Button button;
        public SubTab[] subTabs;
    }

    [SerializeField] private MainTab[] mainTabs;
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = new Color(0.6f, 0.6f, 0.6f, 1f);

    // Sprites originais (português), guardados no arranque
    private readonly Dictionary<Image, Sprite> spritesPT = new Dictionary<Image, Sprite>();
    private bool isEnglish;

    private void Awake()
    {
        foreach (var main in mainTabs)
        {
            foreach (var sub in main.subTabs)
            {
                if (sub.image == null) continue;

                Image img = sub.image.GetComponent<Image>();
                if (img != null)
                    spritesPT[img] = img.sprite;

                sub.image.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private IEnumerator Start()
    {
        for (int i = 0; i < mainTabs.Length; i++)
        {
            int mainIndex = i;
            mainTabs[i].button.onClick.AddListener(() => SelectMainTab(mainIndex));

            for (int j = 0; j < mainTabs[i].subTabs.Length; j++)
            {
                int subIndex = j;
                mainTabs[i].subTabs[j].button.onClick.AddListener(() => SelectSubTab(mainIndex, subIndex));
            }
        }

        // Espera que o sistema de localização esteja pronto
        yield return LocalizationSettings.InitializationOperation;

        isEnglish = !LocalizationSettings.SelectedLocale.Identifier.Code.StartsWith("pt");
        ApplyLanguage();

        SelectMainTab(0);
    }

    private void OnLocaleChanged(Locale locale)
    {
        if (locale == null) return;

        isEnglish = !locale.Identifier.Code.StartsWith("pt");
        ApplyLanguage();
    }

    // Põe em cada imagem o sprite do idioma atual
    private void ApplyLanguage()
    {
        foreach (var main in mainTabs)
        {
            foreach (var sub in main.subTabs)
            {
                if (sub.image == null) continue;

                Image img = sub.image.GetComponent<Image>();
                if (img == null || !spritesPT.ContainsKey(img)) continue;

                img.sprite = (isEnglish && sub.spriteEN != null)
                    ? sub.spriteEN
                    : spritesPT[img];
            }
        }
    }

    public void SelectMainTab(int mainIndex)
    {
        for (int i = 0; i < mainTabs.Length; i++)
        {
            if (mainTabs[i].button.image != null)
                mainTabs[i].button.image.color = i == mainIndex ? activeColor : inactiveColor;
        }

        SelectSubTab(mainIndex, 0);
    }

    public void SelectSubTab(int mainIndex, int subIndex)
    {
        // esconde todas as imagens de todos os grupos
        foreach (var main in mainTabs)
            foreach (var sub in main.subTabs)
                if (sub.image != null)
                    sub.image.SetActive(false);

        // mostra só a selecionada
        var subTabs = mainTabs[mainIndex].subTabs;
        if (subTabs[subIndex].image != null)
            subTabs[subIndex].image.SetActive(true);

        // atualiza cores dos botões do submenu ativo
        for (int i = 0; i < subTabs.Length; i++)
        {
            if (subTabs[i].button != null && subTabs[i].button.image != null)
                subTabs[i].button.image.color = i == subIndex ? activeColor : inactiveColor;
        }
    }
}