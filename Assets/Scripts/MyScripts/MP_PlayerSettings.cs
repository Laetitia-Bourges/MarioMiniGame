using System;
using UnityEngine;
using TMPro;

[Serializable]
public class MP_PlayerSettings
{
    [SerializeField, Range(0, 10)] float heightJump = 5;
    [SerializeField, Range(0, 50)] float speedJump = 10;
    [SerializeField, Range(0, 200)] float manaJumpMax = 40;
    [SerializeField] int nbLife = 3;
    float cooldownLife = 0, currentManaJump = 0;
    bool isFalling = false;

    public float HeightJump => heightJump;
    public float SpeedJump => speedJump;
    public float ManaJumpMax => manaJumpMax;
    public float CooldownLife => cooldownLife;
    public float CurrentManaJump => currentManaJump;
    public int NbLife => nbLife;
    public bool IsFalling => isFalling;

    public bool HaveCoolDownLife => CooldownLife > 0;
    public bool IsAlive => NbLife > 0;
    public bool CanJump => CurrentManaJump > 0 && !IsFalling && IsAlive;

    public void SetNbLife()
    {
        nbLife--;
        cooldownLife = 1;
    }
    public void UpdateCoolDownLife(float _value)
    {
        cooldownLife -= _value;
        cooldownLife = cooldownLife <= 0 ? 0 : cooldownLife;
    }
    public void UpdateCurrentManaJump(float _value) => currentManaJump += _value;
    public void SetIsFalling(bool _value) => isFalling = _value;
}
