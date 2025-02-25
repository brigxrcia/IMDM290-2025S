// UMD IMDM290 
// Instructor: Myungin Lee
// All the same Lerp but using audio
using System.Collections;
using UnityEngine;

public class AudioLerp : MonoBehaviour
{
    GameObject[] spheres;
    static int numSphere = 400;
    float time = 0f;
    Vector3[] initPos;
    Vector3[] startPosition, endPosition;
    float lerpFraction; // Lerp point between 0~1
    float t, phase;
    GameObject mother;
    float direction = 2f;
    //yassss bts logo
    Vector3[] bts_shape = new Vector3[]
    {
        new Vector3(1.5f, -1.8f, 0),
        new Vector3(1.5f, 3.15f, 0),
        new Vector3(-0.15f, 2f, 0), // left diagonal
        new Vector3(-1.5f, 3.15f, 0),
        new Vector3(-1.5f, -1.8f, 0),
        new Vector3(-1.5f, (2f / 5f) * -1.5f - 1.2f, 0),
        new Vector3(-0.15f, (2f / 5f) * -0.15f - 1.2f, 0),
        new Vector3(0.15f, (-2f / 5f) * 0.15f - 1.2f, 0), //bottom
        new Vector3(0.15f, 2f, 0), // start of right line
        new Vector3(1.5f, 3.15f, 0),
        new Vector3(1.5f, (-2f / 5f) * 1.5f - 1.2f, 0),
        new Vector3(-0.15f, -1.26f, 0),
        new Vector3(-0.15f, 2f, 0), // top v1
        new Vector3(0.15f, -1.26f, 0),
        new Vector3(0.15f, 2f, 0) // top v 2
    };

    // Start is called before the first frame update
    void Start()
    {
        // Assign proper types and sizes to the variables.
        spheres = new GameObject[numSphere];
        initPos = new Vector3[numSphere]; // Start positions
        startPosition = new Vector3[numSphere];
        endPosition = new Vector3[numSphere];
        mother = GameObject.Find("leafBody");
        phase = 0f;

        // Define target positions. Start = random, End = heart 
        for (int i = 0; i < numSphere; i++)
        {
            // Random start positions
            float x = 50f;
            float y = 50f;
            float r = 50f;
            //startPosition[i] = new Vector3(r * UnityEngine.Random.Range(-1f, 1f), r * UnityEngine.Random.Range(-1f, 1f), r * UnityEngine.Random.Range(-1f, 1f));
            startPosition[i] = new Vector3(y * Random.Range(-1f, 1f), y * Random.Range(-1f, 1f), x * Random.Range(-1f, 1f));   
            r = 3f;
            endPosition[i] = new Vector3(r * Mathf.Sin(i * 2 * Mathf.PI / numSphere), r * Mathf.Cos(i * 2 * Mathf.PI / numSphere), 0); 

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
           

            // Heart end position 
            t = i* 2 * Mathf.PI / numSphere;
            endPosition[i] = new Vector3( 
                        5f*Mathf.Sqrt(2f) * Mathf.Sin(t) *  Mathf.Sin(t) *  Mathf.Sin(t),
                        5f* (- Mathf.Cos(t) * Mathf.Cos(t) * Mathf.Cos(t) - Mathf.Cos(t) * Mathf.Cos(t) + 2 *Mathf.Cos(t)) + 3f,
                        10f + Mathf.Sin(time));
            /////////////////////////////////////////////////////////////////////////
        }

        // Let there be spheres..
        for (int i = 0; i < numSphere; i++)
        {
            // Draw primitive elements:
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.CreatePrimitive.html
            //spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spheres[i] = Instantiate(mother);

            // Position
            initPos[i] = startPosition[i];
            spheres[i].transform.position = initPos[i];
            spheres[i].transform.localRotation = Quaternion.Euler(UnityEngine.Random.Range(-180f, 180f), UnityEngine.Random.Range(-180f, 180f), UnityEngine.Random.Range(-180f, 180f));
            spheres[i].transform.localScale = new Vector3(UnityEngine.Random.Range(0.2f, 0.3f), UnityEngine.Random.Range(0.2f, 0.3f), UnityEngine.Random.Range(0.2f, 0.3f));

            // Color
            // Get the renderer of the spheres and assign colors.
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            // HSV color space: https://en.wikipedia.org/wiki/HSL_and_HSV
            //float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            float hue = 0.7f + (0.1f * (float)i / numSphere);
            Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and brightness **DA GLOW
            sphereRenderer.material.color = color;
        }
    }

    float speed = 50f;

    // Update is called once per frame
    void Update()
    {
        // ***Here, we use audio Amplitude, where else do you want to use?
        // Measure Time 
        // Time.deltaTime = The interval in seconds from the last frame to the current one
        // but what if time flows according to the music's amplitude?
        //time += Time.deltaTime * AudioSpectrum.audioAmp;
        time += Time.deltaTime;
        speed += Time.deltaTime;
        // time and phase is different.
        phase += Time.deltaTime * direction;

        // what to update over time?
        for (int i = 0; i < numSphere; i++)
        {
            // Lerp : Linearly interpolates between two points.
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.Lerp.html
            // Vector3.Lerp(startPosition, endPosition, lerpFraction)

            // lerpFraction variable defines the point between startPosition and endPosition (0~1)
            float start = 0f;
            float end = 1f;

            lerpFraction = Mathf.Lerp(start, end, phase / 10f);

            // Lerp logic. Update position       
            t = i * 2 * Mathf.PI / numSphere;
            spheres[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction);
            float scale = (1f + AudioSpectrum.audioAmp) * 0.15f;
            //spheres[i].transform.localScale = new Vector3(scale, 0.2f, 0.2f); //changes length
            spheres[i].transform.Rotate(AudioSpectrum.audioAmp, 1f, 1f);

            // Color Update over time
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            //float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            float hue = 0.7f + (0.1f * (float)i / numSphere);
            Color color = Color.HSVToRGB(Mathf.Abs(hue * Mathf.Cos(time)), Mathf.Cos(AudioSpectrum.audioAmp / 10f), 2f + Mathf.Cos(time)); // Full saturation and brightness
            sphereRenderer.material.color = color;

        }


        // Change trigger happens when..
        if (phase >= 20.0f || phase <= 0f) // phase >= 20.0f making shapes hold
        {
            Debug.Log("switch");
            //StartCoroutine(StopForOneSec());
            // change phase sequence direction 
            direction = direction * (-1f);

            // update the end position 
            if (phase <= 0f)
            {
                for (int i = 0; i < numSphere; i++)
                {
                    if (numSphere == 1)
                    {
                        t = 0f;
                    }
                    else
                    {
                        t = (float)i / (numSphere - 1);
                    }
                    float realT = t * (bts_shape.Length - 1);
                    int idx = (int)realT;
                    float lerpFactor = realT - idx;

                    if (idx >= bts_shape.Length - 1)
                    {
                        idx = bts_shape.Length - 2;
                        lerpFactor = 1f;
                    }
                    int nextIndex = idx + 1;

                    endPosition[i] = Vector3.Lerp(bts_shape[idx], bts_shape[nextIndex], lerpFactor);
                }
            }
        }
//////////////////////////////////////////////////////////////////
/// makes them go crazy for some reason idk LOL
        float[] spectrumData = new float[64];
AudioListener.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

for (int i = 0; i < numSphere; i++)
{
    float scale = (1f + spectrumData[i % 5] * 2.5f) * 0.15f; // React to different frequency bands (1f + spectrumData[i % 8] * 2f) makes it smaller
    spheres[i].transform.localScale = new Vector3(scale, scale, scale);
}
////////////////////////////////////////////////////////////////////
    }

    // IEnumerator StopForOneSec()
    // {
    //     Time.timeScale = 1;
    //     yield return new WaitForSecondsRealtime(1); // Wait for 1 second.
    //     Time.timeScale = 0; // Set time scale to freeze.
    //     yield return new WaitForSecondsRealtime(1); // Wait for 1 second.
    //     Time.timeScale = 1;
    // }
}