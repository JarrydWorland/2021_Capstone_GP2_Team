using Scripts.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Scripts.Menus
{
    public class MenuCutsceneBehaviour : MenuBehaviour
    {
        private VideoPlayer _videoPlayer;


        /// <summary>
        /// Adds this to the history and sets up the transition to Hub
        /// </summary>
        private void Start()
        {
            MenuManager.Init(this);
            _videoPlayer.loopPointReached += MoveToNextScene;
        }

        private void Awake()
        {
            _videoPlayer = GetComponent<VideoPlayer>();
        }

        void MoveToNextScene(VideoPlayer _videoPlayer)
        {
            SceneFaderBehaviour.Instance.FadeInto("Hub");
        }
    }
}

