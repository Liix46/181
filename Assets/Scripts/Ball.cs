using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject Arrow;
    private Vector3 _arrowStartPosition;
    private Quaternion _arrowStarRotation;


    private Rigidbody _rb;
    private Vector3 _forceDirection;
    private float _forceMagnitude = 2000;
    private Vector3 _startPosition;
    private bool _isBallReady;

    private TMP_Text _throwText;
    private TMP_Text _downText;
    private TMP_Text _leftText;

    private int _countThrow;
    private int _countDown;

    private GameObject[] _kegels;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _forceDirection.Set(0, 0, 2000);
        _startPosition = this.transform.position;
        _isBallReady = true;
        _arrowStartPosition = Arrow.transform.position;
        _arrowStarRotation = Arrow.transform.rotation;

        ForceIndicator.ForceFactor = 0f;


    }

    private void Awake()
    {
        _throwText = GameObject.Find("Throw Text").GetComponent<TMP_Text>();
        _downText = GameObject.Find("Down Text").GetComponent<TMP_Text>();
        _leftText = GameObject.Find("Left Text").GetComponent<TMP_Text>();

        if (_throwText == null || _downText == null || _leftText == null)
        {
            Debug.Log("NULL Object.GetComponent<TMP_Text>()");
        }

        _countThrow = 0;
        _countDown = 0;
        _kegels = GameObject.FindGameObjectsWithTag("Kegel");

        UpdateStatistic(Statistic.LEFT, _leftText);
    }

    [System.Obsolete]
    void Update()
    {
        if(Input.GetKey(KeyCode.Space) && _isBallReady)
        {
            //_rb.AddForce(_forceDirection);
            ForceIndicator.ForceFactor += 0.002f;
            //Debug.Log("DOWN");

        }


        if (_rb.velocity.magnitude < 1 && _rb.velocity.magnitude > 0.5)
        {
            OnBallStop();
            _isBallReady = true;
            Arrow.active = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Arrow.active = false;

            _rb.AddForce(ForceIndicator.ForceFactor * _forceMagnitude * Arrow.transform.forward);
            _isBallReady = false;
            ForceIndicator.ForceFactor = 0f;
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
        this.transform.position = _startPosition;

        //Kegels position
        foreach (var kegel in _kegels)
        {
            if (kegel.transform.position.y > 0.3f)
            {
                // Debug.Log(kegel.name + " Down\t" + kegel.transform.position.y);
                kegel.SetActive(false);
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
            }
            
        }

        ///////info/////

        UpdateStatistic(Statistic.THROW, _throwText);

    }

    private enum Statistic
    {
        THROW,
        DOWN,
        LEFT
    }

    private void UpdateStatistic(Statistic statistic, TMP_Text tmp)
    {
        switch (statistic)
        {
            case Statistic.THROW:
                ++_countThrow;
                tmp.text = "Throw : " + _countThrow.ToString();
                break;
            case Statistic.DOWN:
                ++_countDown;
                tmp.text = "Downed : " + _countDown.ToString();
                Debug.Log("Down " + _countDown);
                break;
            case Statistic.LEFT:
                tmp.text = "Left : " + (_kegels.Length - _countDown).ToString();
                break;
            default:
                break;
        }

    }
}