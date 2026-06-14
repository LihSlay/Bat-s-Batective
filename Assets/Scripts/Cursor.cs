using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    public Texture2D cursorTexture;
    public Vector2 cursorHotspot = Vector2.zero;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ApplyDefaultCursor();
    }

    // Reaplica o cursor padrão do jogo. Pode ser chamado de qualquer sítio
    // (ex.: ao sair de um zoom) para garantir o cursor correto.
    public void ApplyDefaultCursor()
    {
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }
}