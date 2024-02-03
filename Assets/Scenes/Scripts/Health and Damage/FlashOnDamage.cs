using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class FlashOnDamage : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private string _shaderProperty = "_Color";
    [SerializeField] private Color _flashColor;
    [SerializeField] private float _flashTime = 0.1f;
    [SerializeField] private float _blinkTime = 0.025f;
   
    private Color _originalColor;

    private void Start()
    {
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();

        _originalColor = _renderer.material.color;
    }
    
    private void OnEnable()
    {
        GetComponent<Health>().OnDamageTaken += Flash;
    }

    private void OnDisable()
    {
        GetComponent<Health>().OnDamageTaken -= Flash;
    }

    private void Flash(float oldHealth, float newHealth)
    {
        StopAllCoroutines();
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        _renderer.material.SetColor(_shaderProperty, _originalColor);
        yield return new WaitForSeconds(_blinkTime);
        _renderer.material.SetColor(_shaderProperty, _flashColor);
        yield return new WaitForSeconds(_flashTime);
        _renderer.material.SetColor(_shaderProperty, _originalColor);
    }
}
