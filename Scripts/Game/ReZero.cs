using UnityEngine;
using Wilson.Utility;
using System.Collections;

public class ReZero : MonoBehaviour
{
    #region Variables
    [Header("참조")]
	public SceneFader fader;
    #endregion

    #region Unity Event Method
    private void Start()
    {
        fader.FadeStart();
        StartCoroutine(timeCount());
    }
    #endregion

    #region Custom Method
    private IEnumerator timeCount()
    {
        yield return new WaitForSeconds(10f);
        fader.FadeTo("TitleScene");
    }
    #endregion
}
