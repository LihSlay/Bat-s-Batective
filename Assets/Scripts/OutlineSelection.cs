
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
        if (!EventSystem.current.IsPointerOverGameObject()) //Make sure you have EventSystem in the hierarchy before using EventSystem
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

                // Primeiro collider "real" (não-FlipZone): é este que decide.
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
                }

                // Se o primeiro collider não-FlipZone não for selecionável (ex.: uma parede),
                // pára aqui: está a tapar o objeto, não deve haver outline.
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
