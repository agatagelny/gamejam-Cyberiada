using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;


public class NewMonoBehaviourScript : MonoBehaviour
{
    public DialogueLoader loader;
    public GameObject textPrefab; // przypisz prefab w Inspectorze
    public Transform parent;      // np. Canvas
    public TMP_FontAsset fontAsset;
    public TMP_FontAsset alienFontAsset;
    public TextAsset wordFile;

    void Start()
    {
        DialogueNode node = loader.GetFirstNode();
        Debug.Log(node.text_original);
        
        string[] words = node.text_original.Split(' ');

        foreach (string word in words)
        {
        for (int i=0; i<word.Length; i++)
        {  
            CreateText(word[i],word);
        }
        CreateText(' ', word);
        }
    
    void CreateText(char letter, string word)
    {
        GameObject obj = Instantiate(textPrefab, parent);

        TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();
        text.text = letter.ToString();

        string content = wordFile.text;

        string[] words = content
            .Replace(",", " ")
            .Split(' ')
            .Where(w => !string.IsNullOrWhiteSpace(w))
            .ToArray();

        char[] punctuations={',','.','?','!'};
        
        string searchWord = word;
        if(string.IsNullOrWhiteSpace(searchWord))
        {

        }
        else
        {
        char lastLetter = searchWord[searchWord.Length - 1];
        if(punctuations.Contains(lastLetter)) searchWord=searchWord.Substring(0, searchWord.Length - 1);
        }
        bool exists = words.Contains(searchWord);

        if(exists==true)
        {
        text.font = fontAsset;
        }
        else
        {
        text.font = alienFontAsset;
        }

    }
}
}
