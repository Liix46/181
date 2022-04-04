using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    public GameObject Arrow;
    private Vector3 _arrowStartPosition;
    private Quaternion _arrowStarRotation;


    private Rigidbody _rb;
    private AudioSource _strike;
    private AudioSource _rollingSound;
    private Vector3 _forceDirection;
    private float _forceMagnitude = 2000;
    private Vector3 _startPosition;
    private bool _isBallReady;

    private TMP_Text _throwText;
    private TMP_Text _downText;
    private TMP_Text _leftText;
    private TMP_Text _scoreText;

    private int _countThrow;
    private int _countDown;
    private int _score;

    private int _lastcountDown;
    private int _lastScore;

    private GameObject[] _kegels;

    private float _moveStartTime;
    private float _moveEndTime;

    private const int BonusTime = 5;

    int kegelsTurn = 0;

    private UserMenu _menu;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        //_strike = GetComponent<AudioSource>(); // get first component
        AudioSource[] sounds = GetComponents<AudioSource>();
        _strike = sounds[0];
        _rollingSound = sounds[1];
        _forceDirection.Set(0, 0, 2000);
        _startPosition = transform.position;
        _isBallReady = true;
        _arrowStartPosition = Arrow.transform.position;
        _arrowStarRotation = Arrow.transform.rotation;

        ForceIndicator.ForceFactor = 0f;

        _menu = GameObject.Find("UserMenuCanvas").GetComponent<UserMenu>();

    }

    private void Awake()
    {
        _throwText = GameObject.Find("Throw Text").GetComponent<TMP_Text>();
        _downText = GameObject.Find("Down Text").GetComponent<TMP_Text>();
        _leftText = GameObject.Find("Left Text").GetComponent<TMP_Text>();
        _scoreText = GameObject.Find("Score Text").GetComponent<TMP_Text>();

        if (_throwText == null
            || _downText == null
            || _leftText == null
            || _scoreText == null)
        {
            Debug.Log("NULL Object.GetComponent<TMP_Text>()");
        }

        _countThrow = 0;
        _countDown = 0;
        _score = 0;

        _lastcountDown = 0;
        _lastScore = 0;

        _kegels = GameObject.FindGameObjectsWithTag("Kegel");

        UpdateStatistic(Statistic.LEFT, _leftText);
    }

    [System.Obsolete]
    void Update()
    {
        if (UserMenu.IsShown)
        {
            _strike.Stop();
            _rollingSound.Stop();
        }
        if (UserMenu.IsTimeStop())
        {
            return;
        }

        if(Input.GetKey(KeyCode.Space) && _isBallReady)
        {
            //_rb.AddForce(_forceDirection);
            ForceIndicator.ForceFactor += 0.002f;
            //Debug.Log("DOWN");

        }

        if (_rb.velocity.magnitude > 0.5)
        {
            if (_rb.velocity.magnitude < 1)
            {
                OnBallStop();
                _isBallReady = true;
                Arrow.active = true;
                if (!_rollingSound.isPlaying) _rollingSound.Stop();
            }
            else // moving state
            {
                //_rollingSound.
                //Debug.Log(_rb.velocity.sqrMagnitude);
                _rollingSound.volume = (_rb.velocity.sqrMagnitude / 1600) * _menu.SoundSliderValue;
                if (!_rollingSound.isPlaying) _rollingSound.Play();
                
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Arrow.active = false;

            _rb.AddForce(ForceIndicator.ForceFactor * _forceMagnitude * Arrow.transform.forward);
            _isBallReady = false;
            ForceIndicator.ForceFactor = 0f;

            _moveStartTime = _moveEndTime;
            _moveEndTime = Time.timeSinceLevelLoad;
            //Debug.Log("UP");
        }
    }

    private void OnBallStop()
    {
        _countDown = 0;


        Arrow.transform.position = _arrowStartPosition;
        Arrow.transform.rotation = _arrowStarRotation;


        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        transform.position = _startPosition;

        kegelsTurn = 0;
        //Kegels position
        foreach (var kegel in _kegels)
        {
            if (kegel.transform.position.y > 0.1f)
            {
                // Debug.Log(kegel.name + " Down\t" + kegel.transform.position.y);
                kegel.SetActive(false);
                //Debug.Log($"kegelsFall {kegelsFall}");
                UpdateStatistic(Statistic.DOWN, _downText);
                UpdateStatistic(Statistic.LEFT, _leftText);
                
            }
            else
            {
                //Debug.Log(kegel.name + " Up\t" + kegel.transform.position.y);
                kegel.transform.localPosition = Vector3.zero;
                kegel.transform.rotation = Quaternion.Euler(0, 0, 0);
                Rigidbody krb = kegel.GetComponent<Rigidbody>();
                krb.velocity = Vector3.zero;
                krb.angularVelocity = Vector3.zero;
                kegelsTurn++;
            }
            
        }

        

        ///////info/////

        UpdateStatistic(Statistic.THROW, _throwText);
        UpdateStatistic(Statistic.SCORE, _scoreText);

        //Debug.Log($"last: {_lastcountDown}  |  countDown {_countDown}");
        _lastcountDown = _countDown;

       
        if (kegelsTurn == 0) // Game over
        {
            //Show menu
            UserMenu.IsShown = true;

            /// Button text
            ///UserMenu.ButtonText = "Play again";
            /// stop time
            ///UserMenu.StopTime();

            // MenuMode
            UserMenu.userMenuMode = UserMenuMode.GAMEOVER;


        }

    }

    private enum Statistic
    {
        THROW,
        DOWN,
        LEFT,
        SCORE
    }

    private void UpdateStatistic(Statistic statistic, TMP_Text tmp)
    {
        switch (statistic)
        {
            case Statistic.THROW:
                ++_countThrow;
                tmp.text = $"Throw : {_countThrow}";
                break;
            case Statistic.DOWN:
                ++_countDown;
                tmp.text = $"Downed : {_countDown}";
               // Debug.Log($"Down {_countDown}");
                break;
            case Statistic.LEFT:
                tmp.text = "Left : " + (_kegels.Length - _countDown).ToString();
                break;
            case Statistic.SCORE:
                int moveTime = (int)Mathf.Ceil(_moveEndTime - _moveStartTime);
                _lastScore = _score;
                _score += ((_countThrow < 5)
                    ? (_countDown - _lastcountDown) * (5 - _countThrow)
                    : (_countDown - _lastcountDown))
                    * (moveTime < BonusTime
                        ? BonusTime - moveTime
                        : 1);
                //Debug.Log($"Score : {_score}");

                tmp.text = $"Score : {_score}";

                _moveStartTime = Time.time;
                //Debug.Log("add");
                GameStat.Moves.Add(new MoveData
                {
                    Num = _countThrow,
                    Time = moveTime,
                    KegelsFall = _countDown - _lastcountDown,
                    Score = _score - _lastScore

                });
                //Debug.Log($"add {GameStat.Moves.Count}");
                break;
            default:
                break;
        }

    }

    // Срабатывает только если один их колайдеров - тригер
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"Trigger {other.name}");
    }

    // Срабатывает при любых столкновениях
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"Collision  {collision.gameObject.name}");

        if (collision.gameObject.CompareTag("Kegel"))
        {
            _strike.volume = _menu.SoundSliderValue;

            if (!_strike.isPlaying)
            {
                _strike.Play();
            }
           
        }
    }
}