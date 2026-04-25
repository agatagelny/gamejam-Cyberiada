using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("References")]
    public DialogueWriter dialogueWriter;
    public DialogueLoader loader;
    public ButtonCreator buttonCreator;

    [Header("Character System (Sprite2D)")]
    public CharacterDatabase characterDB;
    public SpriteRenderer characterPortraitRenderer;

    private string lastNodeId;

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
        
        DialogueNode node = loader.GetNode(nodeId);

        if (node != null)
        {
            UpdatePortrait(node.speaker);

            dialogueWriter.Write(nodeId);
            buttonCreator.ShowContinue(() => {
                StartWordleChallenge();
            });
        }
    }

    private void UpdatePortrait(string speakerName)
    {
        if (characterDB == null || characterPortraitRenderer == null) return;

        Sprite speakerSprite = characterDB.GetSprite(speakerName);

        if (speakerSprite != null)
        {
            characterPortraitRenderer.sprite = speakerSprite;
            
            characterPortraitRenderer.gameObject.SetActive(true);
            
            characterPortraitRenderer.color = Color.white;
        }
        else
        {
            characterPortraitRenderer.gameObject.SetActive(false);
        }
    }

    private void StartWordleChallenge()
    {
        DialogueNode node = loader.GetNode(lastNodeId);
        if (node != null && !string.IsNullOrEmpty(node.wordle_solution))
        {
            GameEvents.TriggerWordleRequired(node.wordle_solution);
        }
    }

    private void HandleWordleRequested(string solution)
    {
        dialogueWriter.Hide();
        buttonCreator.ClearButtons();
    }

    private void HandleWordleResult(string foundKeyword)
    {
        if (string.IsNullOrEmpty(lastNodeId)) return;
        DialogueNode node = loader.GetNode(lastNodeId);

        if (!string.IsNullOrEmpty(foundKeyword))
            dialogueWriter.Write(lastNodeId, foundKeyword, true);
        else
            dialogueWriter.Write(lastNodeId, null, true);

        buttonCreator.ShowChoices(node, (choice) => {
        GameEvents.TriggerStatsChanged(choice.plus_paragon, choice.plus_renegade);

        dialogueWriter.WriteRaw(choice.follow_up, node.speaker);

        buttonCreator.ShowContinue(() => {
            if (!string.IsNullOrEmpty(node.next_node))
            {
                // Przejście do właściwego następnego węzła
                Write(node.next_node);
            }
            else
            {
                // Koniec dialogu
                dialogueWriter.Hide();
                buttonCreator.ClearButtons();
            }
        });
    });
    }
}