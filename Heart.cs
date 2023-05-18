#if UNITY_EDITOR
    UnityEditor.EditorApplication.delayCall += () =>
    {
        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
    };
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    public GameObject heartModel1;
    public GameObject heartModel2;
    public GameObject infoPanel;
    public Text infoText;
    public Button switchButton;
    public Button infoButton;
    public Image infoImage;
    //public VideoPlayer infoVideo;
    public Sprite leftAtriumSprite;
    public Sprite rightAtriumSprite;
    public Sprite leftVentricleSprite;
    public Sprite rightVentricleSprite;
    public Material HighlightMaterial;

    private bool isInteracting = false;
    private bool isModel2Active = false;
    private string currentInfo = "";
    private Vector2 prevMousePos = Vector2.zero;

    void Start()
    {
        heartModel2.SetActive(false);
        infoPanel.SetActive(false);
        switchButton.onClick.AddListener(ToggleModel);
        infoButton.onClick.AddListener(ToggleInteraction);
    }

    //void Update()
    //{
    //    if (isModel2Active && isInteracting)
    //    {
    //        // 心脏模型2自由旋转、缩放
    //        Vector2 mousePos = Input.mousePosition;
    //        if (Input.GetMouseButton(0))
    //        {
    //            if (prevMousePos == Vector2.zero)
    //            {
    //                prevMousePos = mousePos;
    //            }
    //            Vector2 delta = mousePos - prevMousePos;
    //            heartModel2.transform.Rotate(Vector3.up, -delta.x * Time.deltaTime * 10);
    //            heartModel2.transform.Rotate(Vector3.right, delta.y * Time.deltaTime * 10);
    //            prevMousePos = mousePos;
    //        }
    //        else
    //        {
    //            prevMousePos = Vector2.zero;
    //        }
    //        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
    //        heartModel2.transform.localScale += Vector3.one * scrollDelta;
    //    }
    //}

    public void DetermineCurrentInfo(Vector2 localPos)
    {
        // 根据不同的局部位置更新当前信息
        if (localPos.y > 0.2f && localPos.x > 0.05f)
        {
            currentInfo = "Right Atrium Info";
            infoImage.sprite = rightAtriumSprite;
            infoImage.gameObject.SetActive(true);
            //infoVideo.gameObject.SetActive(false);
        }
        else if (localPos.y > -0.2f && localPos.x > 0.05f)
        {
            currentInfo = "Left Atrium Info";
            infoImage.sprite = leftAtriumSprite;
            infoImage.gameObject.SetActive(true);
            //infoVideo.gameObject.SetActive(false);
        }
        else if (localPos.y < -0.2f && localPos.x > 0.05f)
        {
            currentInfo = "Left Ventricle Info";
            infoImage.sprite = leftVentricleSprite;
            infoImage.gameObject.SetActive(true);
            //infoVideo.gameObject.SetActive(false);
        }
        else if (localPos.x < -0.05f)
        {
            currentInfo = "Right Ventricle Info";
            infoImage.sprite = rightVentricleSprite;
            infoImage.gameObject.SetActive(true);
            //infoVideo.gameObject.SetActive(false);
        }
        else
        {
            currentInfo = "";
            infoImage.gameObject.SetActive(false);
            //infoVideo.gameObject.SetActive(false);
        }
        UpdateInfoText();
    }

    //void UpdateInfoText()
    //{
    //    // 更新信息面板中的文本
    //    infoText.text = currentInfo;
    //}



    public void ToggleInteraction()
    {
        isInteracting = !isInteracting;
        if (isInteracting)
        {
            infoPanel.SetActive(true);
            // 显示默认信息
            DisplayInfo(currentInfo);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            infoPanel.SetActive(false);
            // 取消高亮
            RemoveHighlight();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ToggleModel()
    {
        isModel2Active = !isModel2Active;
        if (isModel2Active)
        {
            heartModel1.SetActive(false);
            heartModel2.SetActive(true);
            switchButton.GetComponentInChildren<Text>().text = "Show Normal Model";
        }
        else
        {
            heartModel1.SetActive(true);
            heartModel2.SetActive(false);
            switchButton.GetComponentInChildren<Text>().text = "Show Detailed Model";
            infoPanel.SetActive(false);
        }
    }
    public void HighlightPart(string partName)
    {
        RemoveHighlight();

        MeshRenderer[] renders = heartModel2.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer render in renders)
        {
            if (render.gameObject.name.Contains(partName))
            {
                Material[] materials = render.materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    Material newMat = new Material(highlightMaterial);
                    newMat.SetColor("_BaseColor", materials[i].GetColor("_BaseColor"));
                    materials[i] = newMat;
                }
                render.materials = materials;
            }
        }
    }

    public void RemoveHighlight()
    {
        MeshRenderer[] renders = heartModel2.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer render in renders)
        {
            Material[] materials = render.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].name.Contains("Highlight"))
                {
                    materials[i] = new Material(materials[i].shader);
                }
            }
            render.materials = materials;
        }
    }
}
