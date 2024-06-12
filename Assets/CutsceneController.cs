using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Spine.Unity;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    public TextMeshProUGUI dialogueBox;
    public TextMeshProUGUI charBox1;
    public TextMeshProUGUI charBox2;


    public Image char1;
    public Image char2;

   

    public float waitTime = 4.0f;

    public string[] dialogueText;

    private Color activeColor = Color.white;
    private Color dimColor = new Color32(95, 95, 95, 255);

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        for (int i = 0; i < dialogueText.Length; i++)
        {
            dialogueBox.text = dialogueText[i];
            float elapsedTime = 0f;

            if (i % 2 == 0)
            {
                char1.color = activeColor;
                char2.color = dimColor;
                charBox1.gameObject.SetActive(true); 
                charBox2.gameObject.SetActive(false);
            }
            else
            {
                char1.color = dimColor;
                char2.color = activeColor;
                charBox1.gameObject.SetActive(false);
                charBox2.gameObject.SetActive(true);
            }

            while (elapsedTime < waitTime)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    yield return null;
                    break;
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}
