using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;

namespace Wilson.Game
{
    //시간값을 임의로 조정해서 스카이박스에 접근하는 클래스
    public class TimeLerp : MonoBehaviour
    {
        #region Variables
        //참조
        public Light directionalLight;
        public LightingPreset preset;

        //스카이박스 텍스쳐
        [SerializeField] private Texture2D skyboxNight;
        [SerializeField] private Texture2D skyboxSunrise;
        [SerializeField] private Texture2D skyboxDay;
        [SerializeField] private Texture2D skyboxSunset;

        [SerializeField] [Range(0,24)] private float tValue = 0;

        [SerializeField] private float updateInterval = 1f;  // 환경광 갱신 주기
        private float lastUpdateTime = 0f;
        private float lastBlendValue = -1f;  // 변화 감지용
        #endregion

        #region Property
        public float TValue { get { return tValue; } set { tValue = value; } }
        #endregion

        #region Unity Event Method
        private void Start()
        {
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = Color.black;

            directionalLight.color = Color.white;
            directionalLight.intensity = 1f;
            directionalLight.shadows = LightShadows.Soft;
        }
        private void Update()
        {
            //tValue += Time.deltaTime;
            tValue %= 24;
            float quarteredTvalue = tValue / 6;
            switch (quarteredTvalue)
            {
                case < 1: SkyboxLerp(skyboxNight, skyboxSunrise, quarteredTvalue);
                    break;
                case < 2: SkyboxLerp(skyboxSunrise, skyboxDay, (quarteredTvalue) - 1);
                    break;
                case < 3: SkyboxLerp(skyboxDay, skyboxSunset, (quarteredTvalue) - 2);
                    break;
                case <= 4: SkyboxLerp(skyboxSunset, skyboxNight, (quarteredTvalue) - 3);
                    break;
            }
            UpdateDirectionalLight(tValue);
        }
        #endregion

        #region Custom Method
        private void SkyboxLerp(Texture2D a, Texture2D b, float value)
        {
            Material mat = RenderSettings.skybox;
            mat.SetTexture("_Texture1", a);
            mat.SetTexture("_Texture2", b);
            mat.SetFloat("_Blend", value);
            //환경광 업데이트
            //DynamicGI.UpdateEnvironment();

            // 변화량이 충분히 크고 일정 시간이 지났으면 환경광 업데이트 << 그나마 최적화
            if (Mathf.Abs(value - lastBlendValue) > 0.1f && Time.time - lastUpdateTime > updateInterval)
            {
                DynamicGI.UpdateEnvironment();
                lastUpdateTime = Time.time;
                lastBlendValue = value;
            }


        }

        private void UpdateDirectionalLight(float hour)
        {
            if (directionalLight != null)
            {
                // 태양 위치 회전
                directionalLight.transform.localRotation =
                    Quaternion.Euler(new Vector3((hour / 24f * 360f) - 90f, 170f, 0));

                // 정오(12시)에 최대, 자정(0시/24시)에 최소가 되도록 Cos 위상 이동
                float t = (hour / 24f) * Mathf.PI * 2f;
                float lightFactor = Mathf.Clamp01(Mathf.Cos(t - Mathf.PI));  // 낮에 1, 밤에 0

                // Intensity 보간 (밤에도 약간의 빛 남기려면 0.1f ~ 1f 사이)
                directionalLight.intensity = Mathf.Lerp(0.1f, 1f, lightFactor);

                RenderSettings.ambientLight = preset.AmbientColor.Evaluate(hour / 24);
                RenderSettings.fogColor = preset.FogColor.Evaluate(hour / 24);

                // 그림자 항상 유지
                directionalLight.shadows = LightShadows.Soft;
            }
        }
        #endregion
    }

}
