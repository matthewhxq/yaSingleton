﻿using System;
using System.Collections.Generic;
using Elarion.Singleton.Utililty;
using UnityEngine;

namespace Elarion.Singleton.Helpers {
    /// <summary>
    /// Singleton updater class. Instantiates a single MonoBehaviour and uses it to send Unity's events to all singletons.
    /// </summary>
    public class SingletonUpdater : ExecutorBehavior {
        [SerializeField, HideInInspector]
        private List<ScriptableObject> _singletons = new List<ScriptableObject>();

        private static SingletonUpdater _updater;

        private static SingletonUpdater Updater {
            get {
                if(_updater == null) {
                    _updater = Create<SingletonUpdater>("Singleton Updater", true);
                }

                return _updater;
            }
        }
        
        /// <summary>
        /// Registers a singleton to the global updater. Once registered there will be no need to unregister it since it'll only happen when the application quits.
        /// </summary>
        /// <param name="singleton">The Singleton to call Unity functions on</param>
        /// <param name="deinitialize">The deinitialize method of the singleton. The original method is protected to prevent unintended access.</param>
        internal static void RegisterSingleton(BaseSingleton singleton, Action deinitialize) {
            Updater.DestroyEvent += deinitialize;

            Updater.FixedUpdateEvent += singleton.OnFixedUpdate;
            Updater.UpdateEvent += singleton.OnUpdate;
            Updater.LateUpdateEvent += singleton.OnLateUpdate;


            Updater.ApplicationFocusEvent += singleton.OnApplicationFocus;
            Updater.ApplicationPauseEvent += singleton.OnApplicationPause;
            Updater.ApplicationQuitEvent += singleton.OnApplicationQuit;

            Updater.DrawGizmosEvent += singleton.OnDrawGizmos;
            Updater.PostRenderEvent += singleton.OnPostRender;
            Updater.PreCullEvent += singleton.OnPreCull;
            Updater.PreRenderEvent += singleton.OnPreRender;
            Updater.ResetEvent += singleton.OnReset;

            Updater._singletons.Add(singleton);
        }
    }
}