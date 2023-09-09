using UnityEngine;
using TMPro;
public enum alphaValue
{
    SHRINKING,
    GROWING,
}
public class TextFlashing : MonoBehaviour
{
    private alphaValue currentAlphaValue;
    private float CommentminAlpha;
    private float CommentmaxAlpha;
    private float CommentCurrentAlpha;

    [SerializeField] private TextMeshProUGUI MyText;

    void Start()
    {
        CommentminAlpha = 0.2f;
        CommentmaxAlpha = 1.0f;
        CommentCurrentAlpha = 1.0f;
        currentAlphaValue = alphaValue.SHRINKING;
    }

    void Update()
    {
        alphaComments();
    }

    public void alphaComments()
    {
        if (currentAlphaValue == alphaValue.SHRINKING)
        {
            CommentCurrentAlpha = CommentCurrentAlpha - 0.01f;
            MyText.color = new Color(Color.white.r, Color.white.g, Color.white.b, CommentCurrentAlpha);
            if (CommentCurrentAlpha <= CommentminAlpha)
            {
                currentAlphaValue = alphaValue.GROWING;
            }
        }
        else if (currentAlphaValue == alphaValue.GROWING)
        {
            CommentCurrentAlpha = CommentCurrentAlpha + 0.01f;
            MyText.color = new Color(Color.white.r, Color.white.g, Color.white.b, CommentCurrentAlpha);
            if (CommentCurrentAlpha >= CommentmaxAlpha)
            {
                currentAlphaValue = alphaValue.SHRINKING;
            }
        }
    }
}