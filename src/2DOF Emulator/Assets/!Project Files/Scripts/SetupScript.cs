using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField mapNameInputField;
    [SerializeField] private Button startButton;

    public static string MapNameKey => Constants.MAP_NAME_KEY;
    public static string MapNameDefault => Constants.MAP_NAME_DEFAULT;
    
    private void Awake()
    {
        mapNameInputField.text = PlayerPrefs.HasKey(MapNameKey)
            ? PlayerPrefs.GetString(MapNameKey)
            : MapNameDefault;

        if (string.IsNullOrEmpty(mapNameInputField.text))
        {
            mapNameInputField.text = MapNameDefault;
        }
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetString(MapNameKey, mapNameInputField.text);
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