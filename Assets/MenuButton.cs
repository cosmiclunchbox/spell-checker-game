using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private TextMeshProUGUI buttonText;

    // Loads the given scene without transferring any information between scenes. NOTE: This function is called
    // by the OnClick button event.
    public static void SwitchToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    // Triggers when the mouse is hovering over the button.
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = Color.green;
    }

    // Triggers when the mouse stops hovering over the button.
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = Color.white;
    }
}
