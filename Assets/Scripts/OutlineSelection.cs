
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    private Transform highlight;

    void Update()
    {
        // Highlight
        if (highlight != null)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = false;
            highlight = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Fora do zoom, não faz outline quando o rato está sobre UI. Dentro do
        // zoom do painel há UI (moldura/botão) que ficaria por baixo do rato e
        // bloquearia o outline dos fios, por isso ignora essa guarda no zoom
        // (tal como o corte já ignora).
        if (SafeInteraction.AnyZoomed || !EventSystem.current.IsPointerOverGameObject()) //Make sure you have EventSystem in the hierarchy before using EventSystem
        {
            // Apanha TODOS os colliders no caminho do raio e ordena do mais perto ao mais longe,
            // para podermos ignorar as FlipZones que estejam à frente do objeto.
            RaycastHit[] hits = Physics.RaycastAll(ray);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (RaycastHit h in hits)
            {
                // Ignora os colliders de FlipZone: o raio "atravessa-os" para chegar
                // ao objeto selecionável que está por trás.
                if (h.collider.GetComponentInParent<FlipZone>() != null)
                {
                    continue;
                }

                // Sobe na hierarquia à procura da tag "Selectable".
                Transform target = GetSelectable(h.transform);
                if (target != null)
                {
                    Outline outline = target.gameObject.GetComponent<Outline>();
                    if (outline == null)
                    {
                        outline = target.gameObject.AddComponent<Outline>();
                        outline.OutlineColor = Color.magenta;
                    }
                    // Aplica a MESMA largura a todos (mesmo aos que já tinham Outline),
                    // para a grossura ser igual independentemente da escala do objeto.
                    outline.OutlineWidth = 100.0f;
                    outline.enabled = true;
                    highlight = target;
                    break;
                }

                // Collider não-selecionável. Fora do zoom pára aqui: está a tapar
                // o que está atrás, não deve haver outline através dele. Dentro do
                // zoom do painel a câmara está encostada ao painel, por isso
                // atravessa-o até encontrar os fios (Selectable) por trás.
                if (!SafeInteraction.AnyZoomed)
                    break;
            }
        }
    }

    // A partir do transform atingido pelo raio, sobe na hierarquia à procura
    // do ascendente com a tag "Selectable". Devolve null se nenhum for selecionável.
    Transform GetSelectable(Transform t)
    {
        while (t != null)
        {
            if (t.CompareTag("Selectable"))
            {
                return t;
            }
            t = t.parent;
        }
        return null;
    }
}
