using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Settings")]
    public DialogueLoader loader;
    public GameObject textPrefab; 
    public Transform parent;      // Musi mieć GridLayoutGroup (Constraint: Fixed Column Count = 20)
    public TMP_FontAsset fontAsset;
    public TMP_FontAsset alienFontAsset;
    public TextAsset wordFile;

    [Header("Grid Settings")]
    public int charsPerLine = 24;
    public float typingSpeed = 0.05f;

    private string[] dictionaryWords;
    private int currentColumn = 0;

    void Start()
    {
        PrepareDictionary();
        DialogueNode node = loader.GetFirstNode();
        if (node != null)
        {
            StartCoroutine(TypeTextRoutine(node.text_original));
        }
    }

    private IEnumerator TypeTextRoutine(string fullText)
    {
        string[] words = fullText.Split(' ');

        foreach (string word in words)
        {
            string clean = CleanWord(word);
            bool exists = dictionaryWords.Contains(clean.ToLower());
            TMP_FontAsset fontToUse = exists ? fontAsset : alienFontAsset;

            // 1. Logika zawijania
            if (word.Length > (charsPerLine - currentColumn))
            {
                // Wypełniamy resztę linii TYLKO jeśli nie jesteśmy już na początku
                if (currentColumn != 0)
                {
                    int spacesToFill = charsPerLine - currentColumn;
                    for (int i = 0; i < spacesToFill; i++)
                    {
                        CreateText(' ', fontAsset);
                    }
                    currentColumn = 0;
                }
            }

            // 2. Wypisywanie słowa
            for (int i = 0; i < word.Length; i++)
            {
                CreateText(word[i], fontToUse);
                currentColumn++;
                
                if (currentColumn >= charsPerLine) 
                    currentColumn = 0;

                yield return new WaitForSeconds(typingSpeed);
            }

            // 3. POPRAWIONA LOGIKA SPACJI
            // Dodaj spację tylko jeśli:
            // - NIE jesteśmy na końcu linii (currentColumn == 0 po resecie)
            // - NIE jesteśmy na samym początku nowej linii
            if (currentColumn > 0 && currentColumn < charsPerLine)
            {
                CreateText(' ', fontAsset);
                currentColumn++;
                
                // Jeśli spacja zajęła ostatnie miejsce w linii
                if (currentColumn >= charsPerLine) 
                    currentColumn = 0;
            }

            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void CreateText(char letter, TMP_FontAsset font)
    {
        GameObject obj = Instantiate(textPrefab, parent);
        TextMeshProUGUI textComp = obj.GetComponent<TextMeshProUGUI>();
        textComp.text = letter.ToString();
        textComp.font = font;
    }

    private void PrepareDictionary()
    {
        if (wordFile != null)
        {
            dictionaryWords = wordFile.text
                .Replace(",", " ")
                .Split(new char[] { ' ', '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries)
                .Select(w => w.Trim().ToLower())
                .ToArray();
        }
    }

    private string CleanWord(string word)
    {
        char[] punctuations = { ',', '.', '?', '!' };
        string clean = word;
        if (clean.Length > 0 && punctuations.Contains(clean[clean.Length - 1]))
        {
            clean = clean.Substring(0, clean.Length - 1);
        }
        return clean;
    }
}