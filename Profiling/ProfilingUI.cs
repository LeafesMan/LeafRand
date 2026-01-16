using LeafEditor;
using UnityEngine;

namespace LeafRand.Profiling
{
    public class ProfilingUI : MonoBehaviour
    {
        [InterfaceReference]
        public Object[] profileables;

        
        int iterations = 10_000_000;
        string iterationsString = "10000000";
        string results;


        void OnGUI()
        {
            // Results Label
            GUIStyle style = new GUIStyle() { fontSize = 26 };
            GUI.Label(new Rect(Screen.width/2, Screen.height * 0.2f, Screen.width, Screen.height * 0.8f), results, style);



            // Iterations Field
            int width = (int)(Screen.width * 1/11f);
            const int height = 60;

            float x = 0;
            float y = 10f;

            iterationsString = GUI.TextField(new Rect(x, y, width, height), iterationsString);

            if (int.TryParse(iterationsString, out int parsed))
                iterations = parsed;

            x += width;


            // Profileable Buttons
            for(int i = 0; i < profileables.Length; i++)
            {
                IProfileable profileable = profileables[i] as IProfileable;
                if (profileable == null) 
                {
                    Debug.LogWarning($"Profileable at {i} does not implement IProfileable! Skipping.");
                    continue; 
                }


                // Button and Profile call
                if (GUI.Button(new Rect(x, 0, width, height), profileable.GetType().Name))
                {
                    uint seed = (uint)Random.Range(int.MinValue, int.MaxValue);
                    results = profileable.Profile(iterations, seed);
                }


                x += width;
            }
        }

    }

    public interface IProfileable { public string Profile(int iterations, uint seed); }
}