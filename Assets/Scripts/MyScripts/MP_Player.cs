using UnityEngine;
using TMPro;

public class MP_Player : MonoBehaviour
{
    #region Fields/Properties
    [SerializeField] string playerName = "";
    [SerializeField] KeyCode jumpButton = KeyCode.None;

    [SerializeField] TMP_Text playerLife = null;
    [SerializeField] Animator animator = null;
    [SerializeField] GameObject fxHit = null;
    [SerializeField] MP_PlayerSettings settings = new MP_PlayerSettings();
    Vector3 initialPos = Vector3.zero;

    public bool IsValid => jumpButton != KeyCode.None && !string.IsNullOrEmpty(playerName) && playerLife && animator && fxHit;
    public string Name => playerName;
    public MP_PlayerSettings Settings => settings;
    #endregion

    #region Unity Methods
    private void OnTriggerEnter(Collider other) => LooseLife();
    private void Update() => settings.UpdateCoolDownLife(Time.deltaTime);
    private void Start()
    {
        initialPos = transform.position;
        MP_InputManager.Instance?.RegisterButton(jumpButton, Jump);
    }
    #endregion

    #region Custom Methods
    public void InitPlayer(string _name)
    {
        if (!IsValid) return;
        playerName = string.IsNullOrEmpty(_name) ? playerName : _name;
        settings = new MP_PlayerSettings();
        playerLife.text = settings.NbLife.ToString();
    }
    void Jump(bool _action)
    {
        if (_action && settings.CanJump)
        {
            animator.SetTrigger("Jump");
            settings.UpdateCurrentManaJump(- .5f);
            transform.position = Vector3.Lerp(transform.position, initialPos + Vector3.up * settings.HeightJump, Time.deltaTime * settings.SpeedJump);
            return;
        }
        settings.SetIsFalling(true);
        transform.position = Vector3.Lerp(transform.position, initialPos, Time.deltaTime * settings.SpeedJump);
        if (settings.CurrentManaJump < settings.ManaJumpMax)
            settings.UpdateCurrentManaJump(+.5f);
        if (settings.CurrentManaJump > 10) settings.SetIsFalling(false);

    }
    public void LooseLife()
    {
        if (!settings.IsAlive || settings.HaveCoolDownLife) return;
        settings.SetNbLife();
        playerLife.text = settings.NbLife.ToString();
        InstanciateFxHitObject();
        if (!settings.IsAlive)
            MP_GameManager.Instance?.VerifyPlayers();
    }
    void InstanciateFxHitObject()
    {
        GameObject _object = Instantiate(fxHit, transform.position, Quaternion.identity);
        Destroy(_object, 1);
    }
    #endregion
}
