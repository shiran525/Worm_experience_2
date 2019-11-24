using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wormMovement : MonoBehaviour
{
    
    public List<Transform> BodyPart = new List<Transform>();

    [SerializeField]
    private float mindistance = 0.25f;

    [SerializeField]
    private float speed = 1;

    [SerializeField]
    private float rotationSpeed = 50;

    [SerializeField]
    private GameObject bodyPrefab;

    [SerializeField]
    private float dis;

    [SerializeField]
    private Transform curBodyPart;

    [SerializeField]
    private Transform prevBodyPart;

    [SerializeField]
    private int BeginSize;

    public float TimeFromLastRetry;
    
    [SerializeField]
    private GameObject deadScreen;
   
    [SerializeField]
    private Text CurrentScore;
   
    [SerializeField]
    private Text ScoreText;
   
    public bool IsAlive;





    // Start is called before the first frame update
    void Start()
    {
        StartLevel();


    }

    public void StartLevel()
    {
        TimeFromLastRetry = Time.time;

        deadScreen.SetActive(false);

        for(int i = BodyPart.Count - 1; i > 1; i--)
        {
            Destroy(BodyPart[i].gameObject);
            BodyPart.Remove(BodyPart[i]);
        }

        BodyPart[0].position = new Vector3(0, 0.5f, 0);

        BodyPart[0].rotation = Quaternion.identity;

        CurrentScore.gameObject.SetActive(true);
        CurrentScore.text = "Score : 0";

        IsAlive = true;

        

        for (int i = 0; i < BeginSize - 1; i++)
        {
            addBodyPart();
        }

        BodyPart[0].position = new Vector3(2, 0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(IsAlive)
            Move();
        
        if (Input.GetKey(KeyCode.Q))
            addBodyPart();
        
    }

    public void Move()
    {
        float curSpeed = speed;

        if (Input.GetKey(KeyCode.W))
            curSpeed *= 2;
        BodyPart[0].Translate(BodyPart[0].forward * curSpeed * Time.smoothDeltaTime, Space.World);

        if (Input.GetKey(KeyCode.RightArrow))
            BodyPart[0].Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftArrow))
            BodyPart[0].Rotate(Vector3.down * rotationSpeed * Time.deltaTime);

        for (int i = 1; i < BodyPart.Count; i++)
        {
            curBodyPart = BodyPart[i];
            prevBodyPart = BodyPart[i - 1];
            dis = Vector3.Distance(prevBodyPart.position, curBodyPart.position);
            Vector3 newPos = prevBodyPart.position;
            newPos.y = BodyPart[0].position.y;
            float T = Time.deltaTime * dis / mindistance * curSpeed;

            if (T > 0.5f)
                T = 0.5f;
            curBodyPart.position = Vector3.Slerp(curBodyPart.position, newPos, T);
            curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, prevBodyPart.rotation, T);

        }
    }


    public void addBodyPart()
    {
        Transform newPart = (Instantiate(bodyPrefab, BodyPart[BodyPart.Count - 1].position, BodyPart[BodyPart.Count - 1].rotation) as GameObject).transform;

        newPart.SetParent(transform);
        BodyPart.Add(newPart);

        CurrentScore.text = "Score: " + (BodyPart.Count - BeginSize).ToString();
    }

    public void DIE()
    {
        IsAlive = false;
        ScoreText.text = "Your score was: " + (BodyPart.Count - BeginSize).ToString();

        CurrentScore.gameObject.SetActive(false);

        deadScreen.SetActive(true);
    }
}
