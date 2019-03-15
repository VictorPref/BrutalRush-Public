using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuTitle
{
    GameObject titleMenu;
    GameObject imageTitle;

    bool _isLerping = true;
    float _timeStartedLerping;
    float timeTakenDuringLerp = 1;
    float percentageComplete;
    Vector3 endPosition;

    GameObject alphaText;
    Image imageAlpha;
    bool AlphaUp = false;
    bool AlphaDown = true;
    bool showText = false;
    float alpha = 1;
    float timeTakenDuringAlpha = 1;
    float _timeStartedAlpha;

    // Start is called before the first frame update
    public void Start()
    {

        titleMenu = GameObject.Instantiate(Resources.Load("Prefabs/UI/TitleScreen/TitleMenu", typeof(GameObject))) as GameObject;
        imageTitle = titleMenu.transform.GetChild(0).GetChild(0).gameObject;
        alphaText = titleMenu.transform.GetChild(0).GetChild(1).gameObject;
        imageAlpha = alphaText.GetComponent<Image>();
        alphaText.SetActive(false);
        _timeStartedLerping = Time.time;
        SoundManager.Instance.Init();
    }

    // Update is called once per frame
    public void Update()
    {
        if (_isLerping)
        {

            float timeSinceStarted = Time.time - _timeStartedLerping;
            float percentageComplete = timeSinceStarted / timeTakenDuringLerp;
            imageTitle.transform.localScale = Vector3.Lerp(new Vector3(30, 30, 1), new Vector3(1, 1, 1), percentageComplete);

            //When we've completed the lerp, we set _isLerping to false
            if (percentageComplete >= 1)
            {

                imageTitle.transform.localScale = new Vector3(1, 1, 1);
                // transform.position = new Vector3(endPosition.x,transform.position.y,endPosition.z);
                _isLerping = false;
                alphaText.SetActive(true);
                showText = true;
                _timeStartedAlpha = Time.time;
            }
        }

        if (showText)
        {
            changeAlphaText();
        }
    }

    public void changeAlphaText()
    {

        if (AlphaDown)
        {

            float timeSinceStarted = Time.time - _timeStartedAlpha;
            float percentageComplete = timeSinceStarted / timeTakenDuringAlpha;
            alpha = Mathf.Lerp(1, 0.1f, percentageComplete);

            Color tempColor = imageAlpha.color;
            tempColor.a = alpha;
            imageAlpha.color = tempColor;

          
            if (percentageComplete >= 1)
            {
                alpha = 0.1f;

                Color tempColor1 = imageAlpha.color;
                tempColor.a = alpha;
                imageAlpha.color = tempColor;

                AlphaDown = false;
                AlphaUp = true;
                _timeStartedAlpha = Time.time;
            }
        }
        if (AlphaUp)
        {

            float timeSinceStarted = Time.time - _timeStartedAlpha;
            float percentageComplete = timeSinceStarted / timeTakenDuringAlpha;
            alpha = Mathf.Lerp(0.1f, 1, percentageComplete);

            Color tempColor = imageAlpha.color;
            tempColor.a = alpha;
            imageAlpha.color = tempColor;
            
            if (percentageComplete >= 1)
            {
                alpha = 1;
                Color tempColor1 = imageAlpha.color;
                tempColor.a = alpha;
                imageAlpha.color = tempColor;
               
                AlphaDown = true;
                AlphaUp = false;
                _timeStartedAlpha = Time.time;
            }
        }
    }
    public void DestroyGO()
    {
        GameObject.Destroy(titleMenu);
    }
}
