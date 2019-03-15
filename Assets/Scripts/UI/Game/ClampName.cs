using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClampName : MonoBehaviour
{
    const float OFFSETY = 0.6f;
    const float OFFSETX = 0.8f;

    public Text nameLabel;
    Vector3 namePos;

    // Start is called before the first frame update
    void Start()
    {
        SetTextPosition();
    }

    public void SetTextPosition()
    {
        if (nameLabel != null)
        {
            Vector3 v3 = gameObject.transform.parent.position;
            v3.y += OFFSETY;
            v3.x -= OFFSETX;
            namePos = Camera.main.WorldToScreenPoint(v3);
            nameLabel.transform.position = namePos;
        }
    }
}
