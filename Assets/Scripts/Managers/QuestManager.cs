using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("NPC Material")]
    public NPCManager NPC;

    [Header("UI Component Assignment")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameWindow;
    public TextMeshProUGUI textWindow;
    public Animator portraitAnimator;
    public CanvasGroup dialogueCanvasGroup;

    [Header("Configuration")]
    public float typewritingSpeed = 0.02f;
    public float fadeDuration = 0.3f;

    private ScriptableData activeScenario;
    private int currentStepIndex = 0;
    private bool isTyping = false;
    private string activeFullText = "";
    private Coroutine typingCoroutine;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialoguePanel.SetActive(false);
    }

    public void StartScenario(ScriptableData scenarioData, int startAtStep = 0)
    {
        if (scenarioData == null || scenarioData.scenarioSteps.Length == 0) return;

        activeScenario = scenarioData;
        currentStepIndex = startAtStep;

        ProcessCurrentStep();
    }

    private void Update()
    {
        if (dialoguePanel.activeSelf && (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.eKey.wasPressedThisFrame))
        {
            if (isTyping)
            {
                if (typingCoroutine != null)
                    StopCoroutine(typingCoroutine);

                textWindow.text = activeFullText;
                isTyping = false;

                //if (portraitAnimator != null && portraitAnimator.gameObject.activeSelf)
                //    portraitAnimator.SetBool("idle", true);
            }
            else
            {
                AdvanceScenario();
            }
        }
    }

    private void ProcessCurrentStep()
    {
        if (currentStepIndex >= activeScenario.scenarioSteps.Length)
        {
            EndScenario();
            return;
        }

        ScenarioAction currentStep = activeScenario.scenarioSteps[currentStepIndex];
        if (currentStep.actionType == ActionType.PlayDialogueLine)
        {
            ExecuteDialogue(currentStep);
        }
        else if (currentStep.actionType == ActionType.TriggerTimelineCutscene)
        {
            ExecuteTimeline(currentStep);
        }
    }

    private void ExecuteDialogue(ScenarioAction step)
    {
        bool wasClosed = !dialoguePanel.activeSelf;

        dialoguePanel.SetActive(true);

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        if (wasClosed)
        {
            dialogueCanvasGroup.alpha = 0f;
            fadeCoroutine = StartCoroutine(FadeCanvas(0f, 1f));
        }
        else
        {
            dialogueCanvasGroup.alpha = 1f;
        }

        nameWindow.text = step.characterName;
        activeFullText = step.conversationText;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(activeFullText));
    }

    private void ExecuteTimeline(ScenarioAction step)
    {
        dialoguePanel.SetActive(false);
        GameObject timelineObj = GameObject.Find(step.timelineDirectorGameObjectName);
        if (timelineObj != null)
        {
            PlayableDirector director = timelineObj.GetComponent<PlayableDirector>();
            if (director != null)
            {
                director.stopped += OnTimelineFinished;
                director.Play();
                return;
            }
        }
        AdvanceScenario();
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        director.stopped -= OnTimelineFinished;
        AdvanceScenario();
    }

    private IEnumerator TypeText(string line)
    {
        isTyping = true;
        textWindow.text = "";

        foreach (char letter in line.ToCharArray())
        {
            textWindow.text += letter;
            yield return new WaitForSeconds(typewritingSpeed);
        }

        isTyping = false;

        //if (portraitAnimator != null && portraitAnimator.gameObject.activeSelf)
        //    portraitAnimator.SetBool("idle", true);
    }

    public void AdvanceScenario()
    {
        currentStepIndex++;
        ProcessCurrentStep();
    }

    private void EndScenario()
    {
        StartCoroutine(FadeOutDialogue());
    }

    private IEnumerator FadeCanvas(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            dialogueCanvasGroup.alpha =
                Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);

            yield return null;
        }

        dialogueCanvasGroup.alpha = endAlpha;
    }

    private IEnumerator FadeOutDialogue()
    {
        yield return StartCoroutine(FadeCanvas(1f, 0f));

        dialoguePanel.SetActive(false);

        if (NPC != null)
            NPC.EndDialogue();

        PlayerController player = FindFirstObjectByType<PlayerController>();

        if (player != null)
            player.EndTalking();
    }
}