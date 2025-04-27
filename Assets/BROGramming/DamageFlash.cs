using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float flashTime = 0.25f;

    private SpriteRenderer[] _spriterenderer;
    private Material[] _materials;

    private Coroutine _damageFlashCoroutine;

    private void Awake()
    {
        _spriterenderer = GetComponentsInChildren<SpriteRenderer>();

        Init();
    }


    public void CallDamageFlash()
    {
        _damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }
    private void Init()
    {
        _materials = new Material[_spriterenderer.Length];

        for (int i = 0; i < _spriterenderer.Length; i++)
        {
            _materials[i] = _spriterenderer[i].material;
        }
    }

    private IEnumerator DamageFlasher()
    {
        SetFlashColor();

        float currentFlashAmount = 0f;
        float elapsedtime = 0f;
        while (elapsedtime < flashTime)
        {
            elapsedtime += Time.unscaledDeltaTime;

            currentFlashAmount = Mathf.Lerp(1f, 0f, (elapsedtime / flashTime));
            SetFlashAmount(currentFlashAmount);

            yield return null;
        }
    }

    private void SetFlashColor()
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetColor("_FlashColor", _flashColor);
        }
    }

    private void SetFlashAmount(float amount)
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetFloat("_FlashAmount", amount);
        }
    }
}
