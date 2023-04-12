using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int p1Score = 0;
    private int p2Score = 0;
    public TMP_Text textMesh;
    public static GameManager instance;
    public int P1Score {
        set {
            p1Score = value;
            updateScoreDisplay();
        }
        get
        {
            return p1Score;
        }
    }
        public int P2Score {
        set {
            p2Score = value;
            updateScoreDisplay();
        }
        get
        {
            return p2Score;
        }
    }

    void updateScoreDisplay()
    {
        textMesh.text = p1Score + " : " + p2Score;
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else 
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
