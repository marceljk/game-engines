using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject bot;
    [SerializeField] GameObject player;
    [SerializeField] GameObject overviewCameraObject;
    [SerializeField] GameObject deadText;

    [Range(2, 20)] public int botAmount;

    Camera overviewCam;
    GameObject[] botsArray;
    // Start is called before the first frame update
    void Start()
    {
        botsArray = new GameObject[botAmount];
        SpawnBots();
        overviewCam = overviewCameraObject.GetComponent<Camera>();
    }

    void SpawnBots()
    {
        for (int i = 0; i < botAmount; i++)
        {
            float x = Random.Range(-10f, 11f);
            float z = Random.Range(15f, 31f);
            int randomSign = Random.Range(0, 2) * 2 - 1;

            botsArray[i] = Instantiate(bot, new Vector3(x, 0.5f, z * randomSign), Quaternion.identity);
            botsArray[i].gameObject.GetComponent<AgentControls>().player = player;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            // overviewCamera should be enabled when the player died
            overviewCam.enabled = true;
            deadText.SetActive(true);
            foreach(GameObject currentBot in botsArray)
            {
                Destroy(currentBot);
            }
        }
        else
        {
            overviewCam.enabled = false;
        }
    }
}
