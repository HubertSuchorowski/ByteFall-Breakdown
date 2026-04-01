using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BossTTS : MonoBehaviour
{
    public bool isSpeaking = false;
    public float cooldown = 15f;
    private float timer = 0f;
    private Boss bossComponent;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bossComponent = GetComponentInChildren<Boss>(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (bossComponent != null) bossComponent.enabled = false;

            string dialog = "Wszedłeś do mej komnaty, śmiałku. Przygotuj się na walkę z potężnym bossem!";
            string url = "https://bytefalltts-production.up.railway.app/generate?dialog=" + UnityWebRequest.EscapeURL(dialog);
            
            StartCoroutine(GetAudioClip(url));
            
            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;
        }
    }

    IEnumerator GetAudioClip(string url)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            www.method = UnityWebRequest.kHttpVerbPOST;
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("BossTTS Error: " + www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                myClip.name = "BossTTS";
                if (audioSource != null)
                {
                    audioSource.clip = myClip;
                    audioSource.Play();
                    isSpeaking = true;
                    timer = 0f;
                }
            }
        }
    }

    private void Update()
    {
        if (bossComponent == null)
        {
            bossComponent = GetComponentInChildren<Boss>(true);
        }

        if (bossComponent != null && bossComponent.gameObject.activeInHierarchy)
        {
            if (isSpeaking)
            {
                bossComponent.enabled = false;
                timer += Time.deltaTime;
                if (timer >= cooldown)
                {
                    isSpeaking = false;
                    timer = 0f;
                }
            }
            else
            {
                if (!bossComponent.enabled)
                {
                    bossComponent.enabled = true;
                }
            }
        }
    }
}



    
    

}