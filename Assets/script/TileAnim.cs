using UnityEngine;
using System.Collections;

public class TileAnim : MonoBehaviour
{
    private Animator anim;
    private MonoBehaviour childScript;
    [SerializeField] float waitDuration = 1f;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);

            foreach (var script in child.GetComponents<MonoBehaviour>())
            {
                var method = script.GetType().GetMethod("PlayMana");
                if (method != null)
                {
                    childScript = script;
                    break;
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PlayManaAnim());
        }
    }

    IEnumerator PlayManaAnim()
    {
        //play animasi spritesheet trus call func child mana

        anim.SetBool("Mana", true);
        yield return null;
        anim.SetBool("Mana", false);

        yield return new WaitForSeconds(waitDuration);

        if (childScript != null)
        {
            var method = childScript.GetType().GetMethod("PlayMana");
            method?.Invoke(childScript, null);
        }
    }
}
