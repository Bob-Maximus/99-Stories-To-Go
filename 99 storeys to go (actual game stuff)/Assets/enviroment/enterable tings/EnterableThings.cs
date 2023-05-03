using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterableThings : MonoBehaviour
{
    public LevelData levelData;

    private Button enterButton;
    private Transform enterButtonFollow;
    private float iconOffset;

    [HideInInspector]
    public bool isButtonVisible;

    private void Awake()
    {
        enterButton = GameObject.Find("Player").transform.Find("UI").transform.Find("click to enter icon").GetComponent<Button>();
        GameObject.Find("background canvas").GetComponentInChildren<SpriteRenderer>().sprite = levelData.backDrop;
    }

    private void Update()
    {
        enterButton.gameObject.SetActive(isButtonVisible);
        if (enterButtonFollow != null)
        {
            var buttonPos = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>().WorldToScreenPoint(enterButtonFollow.position);
            enterButton.GetComponent<RectTransform>().position = new Vector3(buttonPos.x, buttonPos.y + iconOffset, buttonPos.z);
        }
    }

    public void SwichScene(Collider col)
    {
        for (int i = 0; i < levelData.changeLevelData.Count; i++)
        {
            if (col.gameObject.name == levelData.changeLevelData[i].changeLevelTrigger.name)
            {
                enterButtonFollow = levelData.changeLevelData[i].changeLevelTrigger.transform;
                if (levelData.changeLevelData[i].needsInput == false)
                {
                    SceneManager.LoadScene(levelData.changeLevelData[i].newLevel.Name.ToString());
                } else
                {
                    iconOffset = levelData.changeLevelData[i].iconOffset * 10;
                    isButtonVisible = true;
                    enterButton.onClick.AddListener(delegate { swichScene(i - 1); });
                }
            } 
        }
    }

    void  swichScene(int i)
    {
        isButtonVisible = false;
        DontDestroyOnLoad(GameObject.Find("Player"));
        GameObject.Find("Player").transform.position = levelData.changeLevelData[i].spawnPoint;
        SceneManager.LoadScene(levelData.changeLevelData[i].newLevel.Name);
    }
}
