using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterableThings : MonoBehaviour
{
    public LevelData levelData;

    public void SwichScene(Collision col)
    {
        for(int i = 0; i < levelData.changeLevelData.Count; i++)
        {
            if (col.gameObject == levelData.changeLevelData[i].changeLevelTrigger)
            {
                //SceneManager.LoadScene(levelData.changeLevelData[i].newLevel);
            }
        }
    }
}
