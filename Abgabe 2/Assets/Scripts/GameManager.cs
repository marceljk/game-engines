using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject bot;
    [SerializeField] GameObject player;

    [Range(2, 20)] public int botAmount;
    // Start is called before the first frame update
    void Start()
    {
        SpawnBots();
    }

    void SpawnBots()
    {
        for (int i = 0; i < botAmount; i++)
        {
            float x = Random.Range(-10f, 11f);
            float z = Random.Range(15f, 31f);
            int randomSign = Random.Range(0, 2) * 2 - 1;

            GameObject spawn = Instantiate(bot, new Vector3(x, 0.5f, z * randomSign), Quaternion.identity);
            spawn.gameObject.GetComponent<AgentControls>().player = player;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
