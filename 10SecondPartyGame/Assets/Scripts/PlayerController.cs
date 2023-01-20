using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{

    Rigidbody2D rd2d;
    public float speed;
    Animator anim;

    public GameObject Player;
    public GameObject Coin1;
    public GameObject Coin2;
    public GameObject Finish;
    public GameObject Trap;

    public AudioClip BackgroundMusic;
    public AudioClip Won;
    public AudioClip Lost;
    public AudioClip StartSound;
    public AudioClip collectedClip;

    private bool facingRight = true;
    private bool facingLeft = false;

    public static int level;

    Vector2 lookDirection = new Vector2(1, 0);
    float horizontal;
    float vertical;

    public Text Score;
    public int ScoreValue = 0;
    bool gameStart = true;
    public float deltatime;
    public float timervalue;
    public Text timer;

    public Text Win;
    public Text Lose;

    public bool gameOver = false;
    public bool gameWin = false;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        level = 0;
        ScoreValue = 0;
        Score.text = ScoreValue.ToString();
        timervalue = 12.0f;
        timer.text = timervalue.ToString();
        audioSource = GetComponent<AudioSource>();
        PlaySound(StartSound);
        PlaySound(BackgroundMusic);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        rd2d.AddForce(new Vector2(moveHorizontal * speed, moveVertical * speed));

        if (facingRight == false && moveHorizontal > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveHorizontal < 0)
        {
            Flip();
        }
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        horizontal = Input.GetAxis("Horizontal");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        if (gameStart = true)
        {
            timervalue -= Time.deltaTime;
            timer.text = timervalue.ToString();
        }

        if ((timervalue <= 0) && (gameWin == false))
        {
            timer = null;
            gameOver = true;
        }

        if (gameOver == true)
        {
            speed = 0.0f;
            Lose.text = "You Lost! Press R to Restart";
        }

        if (gameWin == true)
        {
            gameOver = false;
        }
    }

    void Flip()
    {
        {
            facingRight = !facingRight;
            facingLeft = !facingLeft;
            Vector2 Scaler = transform.localScale;
            Scaler.x = Scaler.x * -1;
            transform.localScale = Scaler;
        }
    }


    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
                anim.SetInteger("State", 2);
            }
            else
            {
                anim.SetInteger("State", 1);
            }
        }


        if (collision.collider.tag == "Coin")
        {
            ScoreValue += 100;
            Score.text = ScoreValue.ToString();
            Destroy(Coin1);
            PlaySound(collectedClip);
        }

        if (collision.collider.tag == "Coin2")
        {
            ScoreValue += 100;
            Score.text = ScoreValue.ToString();
            Destroy(Coin2);
            PlaySound(collectedClip);
        }

        if (collision.collider.tag == "Trap")
        {
            Lose.text = "You Lost! Press R to Restart";

            gameOver = true;
            speed = 0.0f;
            transform.position = new Vector2(-7.05f, 0.0f);
            PlaySound(Lost);
        }

        if (Input.GetKey(KeyCode.R))

        {

            if (gameOver == true)

            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                SceneManager.LoadScene("SampleScene");

            }

        }


        if ((collision.collider.tag == "Finish") && (ScoreValue == 200))
        {
            gameWin = true;
            Win.text = "Congrats, you collected the coins and escaped in time!";
            speed = 0.0f;
            timer = null;
            Destroy(Finish);
            PlaySound(Won);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}