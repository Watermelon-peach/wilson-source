using UnityEngine;

namespace Wilson.Game
{
    [ExecuteAlways]
    public class LightingManager : MonoBehaviour
    {
        #region Variables
        //참조
        [SerializeField] private Light DirectionalLight;
        //[SerializeField] private LightingPreset preset;

        //시간
        [SerializeField, Range(0, 24)] private float TimeOfDay;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            /*if (preset == null)
                return;*/

            if (Application.isPlaying)
            {
                TimeOfDay += Time.deltaTime;
                TimeOfDay %= 24; //0~24
                UpdateLighting(TimeOfDay / 24f);
            }
            else
            {
                UpdateLighting(TimeOfDay / 24f);
            }
        }
        private void OnValidate()
        {
            if (DirectionalLight != null)
                return;
            // RenderSetting상의 해 찾기
            if (RenderSettings.sun != null)
            {
                DirectionalLight = RenderSettings.sun;
            }
            else
            {
                //해 없으면 아무 Directional light 찾아서 적용
                Light[] lights = FindObjectsByType<Light>(FindObjectsSortMode.None);
                foreach (Light light in lights)
                {
                    if (light.type == LightType.Directional)
                    {
                        DirectionalLight = light;
                        return;
                    }
                }
            }
        }
        #endregion

        #region Custom Method
        private void UpdateLighting(float timePercent)
        {
            //RenderSettings.ambientLight = preset.AmbientColor.Evaluate(timePercent);
            //RenderSettings.fogColor = preset.FogColor.Evaluate(timePercent);

            if (DirectionalLight != null)
            {
                //DirectionalLight.color = preset.DirectionalColor.Evaluate(timePercent);
                DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
            }
        }
        #endregion
    }

}
