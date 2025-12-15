using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeWriterEffect : MonoBehaviour
{

    [Header("Text Input")]
    [TextAreaAttribute]
    [SerializeField] private string[] textBoxList;

    [Header("Typewriter Settings")]
    [SerializeField] private float charactersPerSecond = 20;
    [SerializeField] private float interPunctuationDelay = 0.5f;

    [Header("Skip Options")]
    [SerializeField] private bool quickSkip;
    [SerializeField][Min(1)] private int skipSpeedup = 5;


    private TMP_Text textComp;

    // Typewriter Functionality
    private int _currentVisibleCharacterIndex;
    private Coroutine _typeWriterCoroutine;

    private WaitForSeconds _simpleDelay;
    private WaitForSeconds _interpunctuationDelay; // between sentences

    // Skipping Functionality
    public bool CurrentlySkipping;
    private WaitForSeconds _skipDelay;

    // Next Text Functionality
    private int chatInd = 0;


    // Start is called before the first frame update
    void Awake()
    {
        textComp = gameObject.GetComponent<TMP_Text>();

        _simpleDelay = new WaitForSeconds(1 / charactersPerSecond);
        _interpunctuationDelay = new WaitForSeconds(interPunctuationDelay);

        _skipDelay = new WaitForSeconds(1 / (charactersPerSecond * skipSpeedup));
    }

    void Start()
    {
        SetText(textBoxList[chatInd]);
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnRMC()
    {
        // Debug.Log("Right");
        if (textComp.maxVisibleCharacters != textComp.textInfo.characterCount - 1)
        {
            Skip();
        }
    }

    private void OnLMC()
    {
        // Debug.Log("Left");
        if (textComp.maxVisibleCharacters != textComp.textInfo.characterCount)
        {
            return;
        }

        chatInd++;
        if (chatInd >= textBoxList.Length)
        {
            chatInd = textBoxList.Length - 1;
            return;
        }

        SetText(textBoxList[chatInd]);
        return;
    }

    void Skip()
    {
        if (CurrentlySkipping)
        {
            return;
        }

        CurrentlySkipping = true;

        StartCoroutine(SkipSpeedupReset());
        return;
    }

    private IEnumerator SkipSpeedupReset()
    {
        yield return new WaitUntil(() => textComp.maxVisibleCharacters == textComp.textInfo.characterCount - 1);
        CurrentlySkipping = false;
    }



    public void SetText(string text)
    {
        if (_typeWriterCoroutine != null)
        {
            StopCoroutine(_typeWriterCoroutine);
        }
        textComp.text = text;
        textComp.maxVisibleCharacters = 0;
        _currentVisibleCharacterIndex = 0;

        // Force TMP to update textInfo immediately
        textComp.ForceMeshUpdate();

        _typeWriterCoroutine = StartCoroutine(Typewriter());
    }

    private IEnumerator Typewriter()
    {
        TMP_TextInfo textInfo = textComp.textInfo;

        while (_currentVisibleCharacterIndex < textInfo.characterCount)
        {
            char character = textInfo.characterInfo[_currentVisibleCharacterIndex].character;
            textComp.maxVisibleCharacters++;

            // Interpunctuation check
            if (!CurrentlySkipping && IsPunctuation(character))
            {
                yield return _interpunctuationDelay;
            }
            else
            {
                yield return CurrentlySkipping ? _skipDelay : _simpleDelay;
            }

            _currentVisibleCharacterIndex++;

        }
    }

    private bool IsPunctuation(char c)
    {
        return c == '?' || c == '.' || c == ',' || c == ':' ||
               c == ';' || c == '!' || c == '-';
    }
}
