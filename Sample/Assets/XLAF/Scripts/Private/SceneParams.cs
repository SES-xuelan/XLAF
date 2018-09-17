using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLAF.Public;
using System;

namespace XLAF.Private
{
    /// <summary>
    /// Scene parameters.
    /// </summary>
    public class SceneParams
    {
        public string sceneName;
        public SceneAnimation anim = SceneAnimation.fade;
        public float oldSceneTime = 0.5f;
        public float newSceneTime = 0.5f;
        public iTween.EaseType ease = iTween.EaseType.defaultType;
        public Action cb = null;
        public object data = "";
        //diy animation, param `Action` is `onComplete` action
        public Action<SceneObject,Action> diyOldSceneAnimAction = null;
        public Action<SceneObject,Action> diyNewSceneAnimAction = null;

        /// <summary>
        /// The background alpha.<para></para>
        /// Only useful for popup.
        /// </summary>
        public float bgAlpha = 0.8f;

        public override string ToString()
        {
            return " sceneName:" + sceneName
            + "\t anim:" + anim.ToString()
            + "\t oldSceneTime:" + oldSceneTime
            + "\t newSceneTime:" + newSceneTime
            + "\t EaseType:" + ease.ToString()
            + "\t bgAlpha:" + bgAlpha.ToString()
            + "\t data:" + data.ToString()
            + "\t callback is null:" + (cb == null);
        }

    }
}
