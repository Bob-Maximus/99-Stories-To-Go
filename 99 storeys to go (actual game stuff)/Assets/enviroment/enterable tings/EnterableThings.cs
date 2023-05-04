using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
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
        if (levelData.backDrop != null)
        {
            GameObject.Find("background canvas").GetComponentInChildren<SpriteRenderer>().sprite = levelData.backDrop;
        }

        if (GameObject.Find("Player") == null)
        {
            GameObject[] swichSceneCols = new GameObject[0];
            swichSceneCols = GameObject.FindGameObjectsWithTag("scene swich");
            GameObject playerPrefab = Resources.Load<GameObject>("game objects/Player");
            var player = Instantiate(playerPrefab, new Vector3(swichSceneCols[0].transform.position.x, swichSceneCols[0].transform.position.y, playerPrefab.transform.position.z), playerPrefab.transform.rotation);
            player.name = "Player";
        }
        enterButton = GameObject.Find("Player").transform.Find("UI").transform.Find("click to enter icon").GetComponent<Button>();
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
                    swichScene(i);
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
        Debug.Log(levelData.changeLevelData[i].spawnPoint);
        SceneManager.LoadScene(levelData.changeLevelData[i].newLevel.Name);
    }
}
