using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public bool showTutorial = true;
    public bool storageActive = false;
    [SerializeField] private GameObject tutorialContainer;
    [SerializeField] private RectTransform tutorialRectTransform;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private GameObject closeButton;
    [SerializeField] private GameObject storageContainerBackground;

    public int tutorialIndexNum = 0;

    [TextArea(10, 10)]
    [SerializeField] private string[] tutorialStrings;

    public static TutorialManager instance;

    private Animator myAnimator;

    public bool hasShownAutoSellTutorial = false;


    private void Awake() {
        instance = this;

        myAnimator = tutorialContainer.GetComponent<Animator>();
    }

    private void Start() {
        if (showTutorial) {
            storageActive = false;
            tutorialContainer.SetActive(true);
            storageContainerBackground.SetActive(false);
        }

        ActivateNextTutorial();

    }

    public void MoveBoxToLeft() {
        myAnimator.SetTrigger("MoveLeft");
    }

    public void MoveBoxToBottom() {
        myAnimator.SetTrigger("MoveDown");
    }

    public void ActivateNextTutorial() {
        if (!showTutorial) { return; }

        tutorialText.text = tutorialStrings[tutorialIndexNum];
        tutorialContainer.SetActive(true);

        // ChangeTutorialWindowSizeToFitText();
    }

    // private void ChangeTutorialWindowSizeToFitText() {
    //     print(tutorialText.preferredHeight);
    //     Vector2 backgroundSize = new Vector2(tutorialRectTransform.sizeDelta.x, tutorialText.preferredHeight);
    //     tutorialRectTransform.sizeDelta = backgroundSize;
    // }


    public void ShowCloseButton() {
        closeButton.SetActive(true);
    }

    public void CompleteThisSectionOfTutorial() {
        AudioManager.instance.Play("UI Click");
        tutorialContainer.SetActive(false);
    }

    public void ShowWorldSpaceCanvas() {
        storageContainerBackground.SetActive(true);
        storageActive = true;
    }

}
