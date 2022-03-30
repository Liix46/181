using UnityEngine;

public class Kegel : MonoBehaviour
{
    private AudioSource _strikeSound;
    private Rigidbody _rb;
    private UserMenu _menu;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _strikeSound = GetComponent<AudioSource>();
        //_strikeSound.volume = .3f;
        _menu = GameObject.Find("UserMenuCanvas").GetComponent<UserMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {

        //Debug.Log($"Kegel Collision  {collision.gameObject.tag}");
        if (collision.gameObject.name.Equals("Ball"))
        {
            //Debug.Log($"Kegel Collision  {collision.gameObject.name}");
           float relv = (_rb.velocity -
                collision.gameObject.GetComponent<Rigidbody>().velocity).magnitude;
            _strikeSound.volume = relv * _menu.SoundSliderValue / 16;
            _strikeSound.Play();
        }
        if (collision.gameObject.CompareTag("Kegel")) 
        {
            //Debug.Log((rb.velocity -
            //    collision.gameObject.GetComponent<Rigidbody>().velocity).magnitude);
            float relv = (_rb.velocity -
               collision.gameObject.GetComponent<Rigidbody>().velocity).magnitude;
            _strikeSound.volume = relv * _menu.SoundSliderValue / 13;
            // _strikeSound.Play();
        }
    }
}
