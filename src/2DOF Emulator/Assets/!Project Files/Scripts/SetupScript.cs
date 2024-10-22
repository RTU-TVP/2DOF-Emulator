using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField mapNameInputField;
    [SerializeField] private Button startButton;

    private const string MAP_NAME_DEFAULT = "2DOFMemoryDataGrabber";
    private const string MAP_NAME_KEY = "MapName";

    private void Awake()
    {
        mapNameInputField.text = PlayerPrefs.HasKey(MAP_NAME_KEY)
            ? PlayerPrefs.GetString(MAP_NAME_KEY)
            : MAP_NAME_DEFAULT;

        if (string.IsNullOrEmpty(mapNameInputField.text))
        {
            mapNameInputField.text = MAP_NAME_DEFAULT;
        }
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetString(MAP_NAME_KEY, mapNameInputField.text);
    }

    private void OnEnable()
    {
        mapNameInputField.onValueChanged.AddListener(OnMapNameChanged);
    }

    private void OnDisable()
    {
        mapNameInputField.onValueChanged.RemoveListener(OnMapNameChanged);
    }

    private void OnMapNameChanged(string mapName)
    {
        startButton.interactable = !string.IsNullOrEmpty(mapName);
    }
}