using UnityEngine;

public class GameInit : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public string testNodeID = "node_001";
    public float delayBeforeWordle = 3.0f;

    void Start()
    {
        Invoke("LaunchTest", 0.5f);
    }

    void LaunchTest()
    {
        // Wywołujemy pisanie przez Managera
        dialogueManager.Write(testNodeID);

        // Wordle odpali się automatycznie przez system eventów
        // (W prawdziwej grze pewnie triggered przez przycisk albo koniec tekstu)
        Invoke("TriggerManualWordle", delayBeforeWordle);
    }

    void TriggerManualWordle()
    {
        // Pobieramy rozwiązanie z loadera (tak jak wcześniej)
        var node = dialogueManager.loader.GetNode(testNodeID);
        if (node != null)
        {
            GameEvents.TriggerWordleRequired(node.wordle_solution);
        }
    }
}