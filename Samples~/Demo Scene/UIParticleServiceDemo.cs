using System.Collections;
using System.Collections.Generic;
using UIParticle.Service;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
public class UIParticleServiceDemo : MonoBehaviour
{
    [SerializeField] private UIParticlesEffectsConfiguration _particleConfiguration;

    [SerializeField] private Transform _transform;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Transform _rotation;

    [SerializeField] private Toggle _isRotateToggle;
    [SerializeField] private Button _toRectButton, _toTransformButton;
    [SerializeField] private Sprite _sprite1, _sprite2, _sprite3;
    [SerializeField] private Texture _texture;

    [SerializeField] private InputField _inputField;
    [SerializeField] private Button _add;
    [SerializeField] private Text _currencyText;
    public DefaultUIParticleService UIParticleService { get; private set; }

    private int m_currency;

    private void Awake()
    {
        //Only for demostration, for real projects it's betetr to have only one instance IUIParticleService at once and receive it from DI Container instead of creating manualy
        UIParticleService = new DefaultUIParticleService(_particleConfiguration);
    }


    private void AddAnimatedCurrency()
    {
        int current = m_currency;
        int amount = int.Parse(_inputField.text);
        m_currency += amount;
        int fixedAmount = Mathf.Clamp(amount, 0, _particleConfiguration.MaxAttractParticlesAmount);
        
        float totalDuration = 0.1f * fixedAmount;


        IEnumerator StartAnimate(int from, int to)
        {
            for (float i = 0; i <= 1f; i += Time.deltaTime / totalDuration)
            {
                _currencyText.text = ((int)Mathf.Lerp(from, to, i)).ToString();
                yield return null;
            }

            _currencyText.text = to.ToString();
        }

        UIParticlesGlobal.Instance.Attract(
            new UIParticleConfiguration.Builder(new[] { _sprite2 }, amount, _currencyText.rectTransform)
                .WithEmitDelay(0.1f)
                .WithFirstAttractCallback(() =>
                {
                    StopAllCoroutines();
                    StartCoroutine(StartAnimate(current, current + amount));
                })
                .Build()
        );
    }

    private void Start()
    {
        _add.onClick.AddListener(AddAnimatedCurrency);

        _toRectButton.onClick.AddListener(() =>
        {
            UIParticlesGlobal.Instance.Attract(
                new UIParticleConfiguration.Builder(_texture, 20, _rectTransform)
                    .WithEmitDelay(0.01f)
                    .WithFirstAttractCallback(() => Debug.Log("First Attract"))
                    .WithAttractCallback(() => Debug.Log("Attract"))
                    .WithEndCallback(() => Debug.Log("Attract End"))
                    .Build());
        });
        _toTransformButton.onClick.AddListener(() =>
        {
            UIParticlesGlobal.Instance.Attract(
                new UIParticleConfiguration.Builder(_texture, 20, _transform)
                    .WithEmitDelay(0.01f)
                    .Build());
        });
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            UIParticleConfiguration.Builder cofig = new(new []{_sprite1}, 20, PositionInfo.ScreenCenter);
            cofig.WithStartPosition(Input.mousePosition);
            UIParticlesGlobal.Instance.Attract(cofig.Build());
        }

        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            UIParticleConfiguration.Builder cofig = new(new List<Sprite>() { _sprite1, _sprite2, _sprite3 }, 20,
                PositionInfo.ScreenCenter);
            cofig.WithStartPosition(Input.mousePosition);
            UIParticlesGlobal.Instance.Attract(cofig.Build());
        }

        if (_isRotateToggle.isOn)
            _rotation.Rotate(Vector3.one);
    }

    private void OnDestroy()
    {
        UIParticleService.Dispose();
    }
}