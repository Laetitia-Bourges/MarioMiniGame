using UnityEngine;
using TMPro;

public class MP_Player : MonoBehaviour
{
    #region Fields/Properties
    [SerializeField] string playerName = "";
    [SerializeField] KeyCode jumpButton = KeyCode.None;
    [SerializeField, Range(0, 10)] float heightJump = 2;
    [SerializeField, Range(0, 50)] float speedJump = 5;
    [SerializeField, Range(0, 200)] float manaJumpMax = 20;
    [SerializeField] TMP_Text playerLife = null;
    [SerializeField] Animator animator = null;
    int nbLife = 3;
    float cooldownLife = 0, currentManaJump = 0;
    Vector3 initialPos = Vector3.zero;
    bool isFalling = false;

    public bool IsValid => jumpButton != KeyCode.None && !string.IsNullOrEmpty(playerName) && playerLife && animator;
    public bool IsAlive => nbLife > 0;
    public string Name => playerName;
    public bool HaveCoolDownLife => cooldownLife > 0;
    public bool CanJump => currentManaJump > 0 && !isFalling && IsAlive;
    #endregion

    #region Unity Methods
    private void OnTriggerEnter(Collider other) => LooseLife();
    private void Update() => UpdateCooldownTimer();
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
        currentManaJump = manaJumpMax;
        nbLife = 3;
        playerLife.text = nbLife.ToString();
        cooldownLife = 0;
        isFalling = false;
    }
    void Jump(bool _action)
    {
        if (_action && CanJump)
        {
            animator.SetTrigger("Jump");
            currentManaJump -= .5f;
            transform.position = Vector3.Lerp(transform.position, initialPos + Vector3.up * heightJump, Time.deltaTime * speedJump);
            return;
        }
        isFalling = true;
        transform.position = Vector3.Lerp(transform.position, initialPos, Time.deltaTime * speedJump);
        if (currentManaJump < manaJumpMax)
            currentManaJump += .5f;
        if (currentManaJump > 10) isFalling = false;

    }
    public void LooseLife()
    {
        if (!IsAlive || HaveCoolDownLife) return;
        nbLife--;
        playerLife.text = nbLife.ToString();
        cooldownLife = 1;
        if (!IsAlive)
            MP_GameManager.Instance?.VerifyPlayers();
    }
    void UpdateCooldownTimer()
    {
        cooldownLife -= Time.deltaTime;
        cooldownLife = cooldownLife <= 0 ? 0 : cooldownLife;
    }
    #endregion
}
