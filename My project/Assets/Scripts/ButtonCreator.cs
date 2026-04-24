using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonCreator : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform parentPanel;
    public DialogueLoader loader;
    private int plusParagon=0;
    private int plusRenegade=0;

    void Start()
    {
        DialogueNode node = loader.GetFirstNode();
        foreach(var choice in node.choices)
        {
            plusParagon=choice.plus_paragon;
            plusRenegade=choice.plus_renegade;
            
            CreateButton(choice.text, plusParagon, plusRenegade);
        }
    }

    void CreateButton(string buttonText, int plusPar, int plusRen)
    {
        // Instantiate button
        GameObject newButton = Instantiate(buttonPrefab, parentPanel);

        // Set button text
        TextMeshProUGUI textComponent = newButton.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = buttonText;
        }

        // Add click listener
        Button btn = newButton.GetComponent<Button>();
        btn.onClick.AddListener(() =>OnButtonClicked(plusPar, plusRen));
    }

    // Functions for buttons
    void OnButtonClicked(int plusPar, int plusRen)
    {
        Debug.Log(plusPar.ToString());
    }

}
