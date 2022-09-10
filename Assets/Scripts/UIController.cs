using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject DialogBox;
    public TMP_Text TextToDisplay;
    public TMP_Text DaysText;
    public TMP_Text MushroomText;
    public TMP_Text MoneyText;
    public TMP_Text HoeText;
    public TMP_Text InfoText;
    public Image MushroomImage;
    public Image HoeImage;
    public Image MoneyImage;
    public Image KettleImage;
    public TMP_Text KettleText;
    public GameObject InfoBox;
    public Image TimeOfDayImage;
    public Image MenuImage;
    private bool dialogInUse;
    private int scrollPos;
    private Color highlightColor;
    private Color defaultColor;
    public bool isDisplayingMessage;
    // Start is called before the first frame update
    void Start()
    {
        DialogBox.SetActive(false);
        MenuImage.gameObject.SetActive(true);
        dialogInUse = false;
        isDisplayingMessage = false;
        scrollPos = 0;
        highlightColor = new Color(0f, .4f, 0f, .9f);
        defaultColor = new Color(1f, 1f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayText(string text)
    {
        if (!dialogInUse)
            StartCoroutine(DisplayTextIncrementally(text));
    }

    private IEnumerator DisplayTextIncrementally(string text)
    {
        isDisplayingMessage = true;
        dialogInUse = true;
        TextToDisplay.text = "";
        if (!DialogBox.activeSelf)
            DialogBox.SetActive(true);
        for (int i = 0; i < text.Length; ++i)
        {
            TextToDisplay.text += text[i].ToString();
            yield return new WaitForSeconds(.07f);
        }
        yield return new WaitForSeconds(1.5f);
        DialogBox.SetActive(false);
        dialogInUse = false;
        isDisplayingMessage = false;
    }

    public void Scroll(char direction, int overridePos = -1)
    {
        if (direction == 'r')
            scrollPos = (scrollPos + 1) % 4;
        else if (direction == 'l')
        {
            scrollPos = (scrollPos - 1);
            if (scrollPos < 0)
                scrollPos = 0;
        }
        if (overridePos != -1)
            scrollPos = overridePos;
        Debug.Log($"{scrollPos}, {direction}");
        switch(scrollPos)
        {
            case 0:
                {
                    if (Managers.Inventory.GetItemCount("mushroom") > 0)
                    {
                        RestoreAllColors();
                        MushroomImage.color = highlightColor;
                        MushroomText.color = MushroomImage.color;
                        if (Managers.Inventory.GetItemCount("mushroom") > 0)
                            Managers.Inventory.EquipItem("mushroom");

                    }
                    
                    break;
                }
            case 1:
                {
                    if (Managers.Inventory.GetItemCount("hoe") > 0)
                    {
                        RestoreAllColors();
                        HoeImage.color = highlightColor;
                        HoeText.color = HoeImage.color;
                        Managers.Inventory.EquipItem("hoe");
                      
                    }
                    break;
                }
            case 2:
                {
                    if (Managers.Inventory.GetItemCount("kettle") > 0)
                    {
                        RestoreAllColors();
                        KettleImage.color = highlightColor;
                        KettleText.color = KettleImage.color;

                        Managers.Inventory.EquipItem("kettle");
                    }
                    break;
                }
            case 3:
                {
                    if (Managers.Inventory.GetItemCount("money") > 0)
                    {
                        RestoreAllColors();
                        MoneyImage.color = highlightColor;
                        MoneyText.color = MoneyImage.color;

                        Managers.Inventory.EquipItem("money");
                    }
                    break;
                }
            default:
                RestoreAllColors();
                Debug.Log("Undefined position");
                break;
        }
    }

    private void RestoreAllColors()
    {
        MushroomImage.color = defaultColor;
        MushroomText.color = defaultColor;
        HoeImage.color = defaultColor;
        HoeText.color = defaultColor;
        MoneyImage.color = defaultColor;
        MoneyText.color = defaultColor;
        KettleImage.color = defaultColor;
        KettleText.color = defaultColor;
    }

    public void StartOrResumeGame()
    {
        MenuImage.gameObject.SetActive(false);
    }
    public void EndGame()
    {
        Application.Quit();
    }
}
