using UnityEngine;
using UnityEngine.UI;

public class TabMenu : MonoBehaviour
{
    [System.Serializable]
    public struct SubTab
    {
        public Button button;
        public GameObject image;
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

    private void Awake()
    {
        foreach (var main in mainTabs)
            foreach (var sub in main.subTabs)
                if (sub.image != null)
                    sub.image.SetActive(false);
    }

    private void Start()
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

        SelectMainTab(0);
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
