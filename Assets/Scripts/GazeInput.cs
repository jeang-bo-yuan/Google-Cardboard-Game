using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GazeInput : MonoBehaviour
{
    [Header("設定")]
    public Image loaderImage;       // 剛剛做的圓形進度條
    public float totalTime = 2.0f;  // 需要凝視幾秒才觸發？
    public float maxDistance = 10f; // 點擊距離

    private float timer = 0f;       // 目前累積時間
    private Button currentButton;   // 目前盯著的按鈕
    private bool isClicked = false; // 是否已經觸發過(避免一直連點)

    void Update()
    {


        // 從攝影機發射射線
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);

        // 1. 偵測是否打到東西
        // 注意：按鈕必須要有 Collider (Box Collider) 才能被 Physics.Raycast 打到
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            Button btn = hit.collider.GetComponent<Button>();

            if (btn != null)
            {
                // 如果看的是同一個按鈕，且還沒觸發過
                if (currentButton == btn && !isClicked)
                {
                    // 計時開始
                    timer += Time.deltaTime;

                    // 更新圓圈進度條 (0 ~ 1)
                    loaderImage.fillAmount = timer / totalTime;

                    // 如果時間到了
                    if (timer >= totalTime)
                    {
                        btn.onClick.Invoke(); // 觸發按鈕
                        isClicked = true;     // 標記已點擊，避免下一幀繼續觸發
                        OnGazeComplete();     // (選用) 觸發後的視覺重置
                    }
                }
                else if (currentButton != btn)
                {
                    // 如果看著新按鈕，重置計時
                    ResetGaze(btn);
                }
            }
            else
            {
                // 打到東西但不是按鈕
                ResetGaze(null);
            }
        }
        else
        {
            // 沒打到任何東西
            ResetGaze(null);
        }
    }

    // 重置狀態
    void ResetGaze(Button newButton)
    {
        currentButton = newButton;
        timer = 0f;
        loaderImage.fillAmount = 0f; // 隱藏進度條
        isClicked = false;           // 重置點擊狀態
    }

    // 點擊完成後的處理
    void OnGazeComplete()
    {
        // 點擊後你可以選擇把進度條歸零，或是保持滿的直到視線移開
        loaderImage.fillAmount = 0f;

        // 這裡可以加個音效或震動
        // Handheld.Vibrate(); 
    }
}