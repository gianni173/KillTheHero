using UnityEngine;
using UnityEngine.EventSystems;

public class ConnectionToggle : MonoBehaviour, IPointerClickHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked on ConnectionToggle");
    }
}
