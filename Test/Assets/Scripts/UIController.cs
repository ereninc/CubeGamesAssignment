using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject uiMenu;
    [SerializeField] private GameObject showButton;
    [SerializeField] private Text departurePosText;
    [SerializeField] private Text arrivalPosText;
    [SerializeField] private Text projectilePosText;
    [SerializeField] private Text hMaxText;
    [SerializeField] private Text speedText;

    [SerializeField] private SimulationManager simulationManager;
    [SerializeField] private ObliqueMotion obliqueMotion;

    public int uiHmax;
    public int uiSpeed;

    private void Start()
    {
        ShowUI();
    }

    private void Update()
    {
        SetCoordinateTexts();
        SetHeightSpeedTexts();
        
        SetMaximumHeight();
        SetSpeed();
    }

    public void ShowUI()
    {
        uiMenu.SetActive(true);
        showButton.SetActive(false);
    }

    public void HideUI()
    {
        uiMenu.SetActive(false);
        showButton.SetActive(true);
    }

    private void SetHeightSpeedTexts()
    {
        hMaxText.text = obliqueMotion.hMax.ToString();
        speedText.text = obliqueMotion.speed.ToString();
    }

    private void SetCoordinateTexts()
    {
        departurePosText.text = simulationManager.GetDeparturePosition().ToString();
        arrivalPosText.text = simulationManager.GetArrivalPosition().ToString();
        projectilePosText.text = simulationManager.GetProjectilePosition().ToString();
    }

    private void SetMaximumHeight()
    {
        if (int.Parse(hMaxText.text) <= 0 || hMaxText.text == null)
        {
            uiHmax = 10;
        }
        else
        {
            int.TryParse(hMaxText.text, out uiHmax);
        }
    }

    private void SetSpeed()
    {
        if (int.Parse(speedText.text) <= 0 || speedText.text == null)
        {
            uiSpeed = 1;
        }
        else
        {
            int.TryParse(speedText.text, out uiSpeed);
        }
    }
}