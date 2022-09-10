using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedBehaviour : MonoBehaviour
{
    private bool canInteract = false;
    private int dayCounter;
    [SerializeField] UIController uiController;
    // Start is called before the first frame update
    void Start()
    {
        dayCounter = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canInteract && !Managers.Player.isSleeping)
        {
            Managers.Player.isSleeping = true;
            StartCoroutine(GoToNextDay());
        }
    }

    private IEnumerator GoToNextDay()
    {
        uiController.TimeOfDayImage.color = new Color(0f,0f,0f,0f);
        float alphaValue = 0f;
        for(int i = 0; i < 100; ++i)
        {
            alphaValue += 1f/100f;
            uiController.TimeOfDayImage.color = new Color(0f, 0f, 0f, alphaValue);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 100; ++i)
        {
            alphaValue -= 1f/100f;
            uiController.TimeOfDayImage.color = new Color(0f, 0f, 0f, alphaValue);
            yield return new WaitForSeconds(0.05f);
        }
        dayCounter += 1;
        uiController.DaysText.text = $"Day x{dayCounter}";
        Managers.Player.isSleeping = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            canInteract = true;
            uiController.InfoBox.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            canInteract = false;
            uiController.InfoBox.SetActive(false);
        }
    }
}
