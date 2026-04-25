using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("References")]
    public DialogueWriter dialogueWriter;
    public DialogueLoader loader;

    private string lastNodeId; // Zapamiętujemy, o czym rozmawiamy

    private void OnEnable()
    {
        // Nasłuchiwanie eventów
        GameEvents.OnWordleRequired += HandleWordleRequested;
        GameEvents.OnWordleSuccess += HandleWordleResult;
    }

    private void OnDisable()
    {
        GameEvents.OnWordleRequired -= HandleWordleRequested;
        GameEvents.OnWordleSuccess -= HandleWordleResult;
    }


    public void Write(string nodeId)
    {
        lastNodeId = nodeId;
        dialogueWriter.Write(nodeId);
    }

    // 1. Gdy Wordle jest requestowane - wyłączamy okno dialogowe
    private void HandleWordleRequested(string solution)
    {
        Debug.Log("[DialogueManager] Wordle w toku - chowam dialog.");
        dialogueWriter.Hide();
    }

    // 2. Gdy Wordle się kończy (OnWordleSuccess)
    private void HandleWordleResult(string foundKeyword)
    {
        if (string.IsNullOrEmpty(lastNodeId)) return;

        if (!string.IsNullOrEmpty(foundKeyword))
        {
            // SUKCES: Wyświetlamy natychmiast z niebieskim słowem
            Debug.Log($"[DialogueManager] Sukces! Odświeżam dialog z keywordem: {foundKeyword}");
            dialogueWriter.Write(lastNodeId, foundKeyword);
        }
        else
        {
            // PORAŻKA: Wyświetlamy natychmiast, ale bez zmian (alien font)
            Debug.Log("[DialogueManager] Porażka. Odświeżam dialog w wersji oryginalnej.");
            // Przekazujemy null jako keyword, ale dzięki temu, że 
            // WordleManager wywołał ten event, wiemy że chcemy to wyświetlić od razu
            dialogueWriter.Write(lastNodeId);
        }
    }
}