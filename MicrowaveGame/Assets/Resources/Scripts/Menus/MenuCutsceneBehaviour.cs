using Scripts.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Scripts.Menus
{
    public class MenuCutsceneBehaviour : MenuBehaviour
    {
        public VideoPlayer VideoPlayer;

        /// <summary>
        /// Adds this to the history and sets up the transition to Hub
        /// </summary>
        private void Start()
        {
            MenuManager.Init(this);
            VideoPlayer.loopPointReached += MoveToNextScene;
        }

        void MoveToNextScene(VideoPlayer _videoPlayer)
        {
            SceneFaderBehaviour.Instance.FadeInto("Hub");
        }

        public void SkipCutscene()
        {
            SceneFaderBehaviour.Instance.FadeInto("Hub");
        }
    }
}

