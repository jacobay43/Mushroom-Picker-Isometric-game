using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    private bool canInteract = false;
    private bool haveMetBefore = false;
    private bool finishedObjective = false;
    
    [SerializeField] UIController uiController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canInteract && !uiController.isDisplayingMessage)
        {
            if (!haveMetBefore)
            {
                uiController.DisplayText("Hello Friend. You need to find 5 mushrooms. They can be found near the trees.");
                Debug.Log("Hello Friend. You need to find 5 mushrooms. They can be found near the trees.");
                haveMetBefore = true;
            }
            else if (haveMetBefore && !finishedObjective && Managers.Inventory.GetItemCount("mushroom") >= 5)
            {
                uiController.DisplayText("Thank you. You've found them all!. Here, have this for your troubles");
                Debug.Log("Thank you. You've found them all!. Here, have this for your troubles");
                Managers.Inventory.SetItem("mushroom", Managers.Inventory.GetItemCount("mushroom") - 5);
                uiController.MushroomText.text = $"x{Managers.Inventory.GetItemCount("mushroom")}";
                Managers.Inventory.SetItem("money",100);
                uiController.MoneyText.text = $"x{100}";
                finishedObjective = true;
            }
            else if (haveMetBefore && !finishedObjective && Managers.Inventory.GetItemCount("mushroom") < 5)
            {
                uiController.DisplayText("You haven't found all 5 mushrooms yet, have you? Come back to me when you have.");
                Debug.Log("You haven't found all 5 mushrooms yet, have you? Come back to me when you have.");

            }
            else if (finishedObjective)
            {
                uiController.DisplayText("Thanks again for your help.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("Can Interact. Press E");
            canInteract = true;
            uiController.InfoBox.SetActive(true);
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("Can No Longer Interact.");
            canInteract = false;
            uiController.InfoBox.SetActive(false);
        }
    }
}
