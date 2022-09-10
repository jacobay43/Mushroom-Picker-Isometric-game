using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBehaviour : MonoBehaviour
{
    [SerializeField] string toolName = "";
    [SerializeField] GameObject toolUser;
    [SerializeField] UIController uiController;
    bool canPick = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canPick)
            {
                Managers.Inventory.AddItem(toolName);
                if (toolName == "hoe")
                {
                    uiController.HoeText.text = $"x{1}";
                    uiController.Scroll('r', 1);
                }
                else if (toolName == "kettle")
                {
                    uiController.KettleText.text = $"x{1}";
                    uiController.Scroll('r', 2);
                }
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            canPick = true;
            uiController.InfoBox.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            canPick = false;
            uiController.InfoBox.SetActive(false);
        }
    }
}
