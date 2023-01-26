using AxieMixer.Unity;
using UnityEngine;

namespace Assignment.Scene
{
    public class SceneInitializer : MonoBehaviour
    {
        private void Awake()
        {
            Mixer.Init();
        }
    }
}