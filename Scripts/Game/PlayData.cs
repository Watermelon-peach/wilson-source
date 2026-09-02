using UnityEngine;
using Wilson.Utility;

namespace Wilson.Game
{
    public class PlayData : PersistanceSingleton<PlayData>
    {
        protected override void Awake()
        {
            //싱글톤 불러오기
            base.Awake();
        }

        //참조

    }

}
