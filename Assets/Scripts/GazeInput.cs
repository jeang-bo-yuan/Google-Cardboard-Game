using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GazeInput : MonoBehaviour
{
    [Header("Gaze Input")]
    public Image loaderImage;       
    public float totalTime = 2.0f;  
    public float maxDistance = 10f; 

    private float timer = 0f;       
    private Button currentButton;   
    private bool isClicked = false; 

    private PointerEventData pointerData;

    void Start()
    {
        pointerData = new PointerEventData(EventSystem.current);
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            Button btn = hit.collider.GetComponent<Button>();

            if (btn != null)
            {
                // A: Keep looking at the same button
                if (currentButton == btn && !isClicked)
                {
                    timer += Time.deltaTime;
                    loaderImage.fillAmount = timer / totalTime;

                    if (timer >= totalTime)
                    {
                        btn.onClick.Invoke(); 
                        isClicked = true;     
                        OnGazeComplete();     
                    }
                }
                // B: Switch to a new button
                else if (currentButton != btn)
                {
                    ChangeTarget(btn);
                }
            }
            else
            {
                ChangeTarget(null);
            }
        }
        else
        {
            ChangeTarget(null);
        }
    }

    // Change the target button and trigger Unity's native UI events
    void ChangeTarget(Button newButton)
    {
        if (currentButton != null)
        {
            ExecuteEvents.Execute(currentButton.gameObject, pointerData, ExecuteEvents.pointerExitHandler);
        }

        // Hover over the new button
        currentButton = newButton;
        if (currentButton != null)
        {
            ExecuteEvents.Execute(currentButton.gameObject, pointerData, ExecuteEvents.pointerEnterHandler);
        }

        ResetTimer();
    }

    void ResetTimer()
    {
        timer = 0f;
        loaderImage.fillAmount = 0f; 
        isClicked = false;           
    }

    void OnGazeComplete()
    {
        loaderImage.fillAmount = 0f;
        
        // (optional) in case want to remove the highlight after clicking
        if (currentButton != null) 
            ExecuteEvents.Execute(currentButton.gameObject, pointerData, ExecuteEvents.pointerExitHandler);
    }
}