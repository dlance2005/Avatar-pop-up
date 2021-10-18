using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI : MonoBehaviour
{
    public Dropdown micList;
    public Slider sensSlider;
    public Text sensLabel;
    public Text sensValue;
    public Slider hueSlider;
    public Text hueLabel;
    public Camera mainCam;
    public micDetect micReference;

    float H, S, V;

    // Start is called before the first frame update
    void Start()
    {

        //getting HSV values of camera's BG color
        Color.RGBToHSV(mainCam.backgroundColor, out H, out S, out V);

        sensValue.text = sensSlider.value.ToString();

        //adding listeners for when UI values are changed
        micList.onValueChanged.AddListener(delegate {
            MicValueChanged(micList);
        });
        sensSlider.onValueChanged.AddListener(delegate
        {
            SensSliderChanged(sensSlider);
        });
        hueSlider.onValueChanged.AddListener(delegate
        {
            HueSliderValueChanged(hueSlider);
        });

        //Populating mic dropdown with system's audio devices
        if (Microphone.devices.Length > 0)
        {
            List<string> options = new List<string>();
            foreach (var device in Microphone.devices)
            {
                options.Add(device);
            }
            micList.ClearOptions();
            micList.AddOptions(options);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (micList.gameObject.activeSelf == true)
            {
                micList.gameObject.SetActive(false);
                sensSlider.gameObject.SetActive(false);
                sensLabel.gameObject.SetActive(false);
                sensValue.gameObject.SetActive(false);
                hueSlider.gameObject.SetActive(false);
                hueLabel.gameObject.SetActive(false);
            }
            else
            {
                micList.gameObject.SetActive(true);
                sensSlider.gameObject.SetActive(true);
                sensLabel.gameObject.SetActive(true);
                sensValue.gameObject.SetActive(true);
                hueSlider.gameObject.SetActive(true);
                hueLabel.gameObject.SetActive(true);
            }
        }
    }
    void MicValueChanged(Dropdown change)
    {
        micReference.microphoneInput = Microphone.Start(Microphone.devices[change.value], true, 999, 44100);
    }
    void SensSliderChanged(Slider change)
    {
        micReference.sensitivity = change.value;
        sensValue.text = change.value.ToString("F2");
    }
    void HueSliderValueChanged(Slider change)
    {
        mainCam.backgroundColor = Color.HSVToRGB(change.value, S, V);
    }
}