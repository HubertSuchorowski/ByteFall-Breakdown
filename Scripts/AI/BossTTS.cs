using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Unity.VisualScripting;

public class BossTTS : MonoBehaviour
{
    public bool isSpeaking = true;
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            GetComponentInChildren<Boss>().enabled = false;

            string dialog = "Wszedłeś do mej komnaty, śmiałku. Przygotuj się na walkę z potężnym bossem!";
            string url = "https://bytefalltts-production.up.railway.app/generate?dialog=" + UnityWebRequest.EscapeURL(dialog);
            StartCoroutine(GetAudioClip(url));
            Debug.Log("BossTTS: " + dialog);
            GetComponent<Collider>().enabled = false;

            
        }
        
    }
    
    IEnumerator GetAudioClip(string url)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            www.method = UnityWebRequest.kHttpVerbPOST;

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("BossTTS Error: " + www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                myClip.name = "BossTTS";
                AudioSource audioSource = GetComponent<AudioSource>();
                audioSource.clip = myClip;
                audioSource.Play();
                isSpeaking = true;
            }
        }
    }
    float timer = 0f;
    public float cooldown = 15f;
    private void Update()
    {
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAA");
        if (GetComponentInChildren<Boss>().gameObject.activeSelf == true)
        {
            if (isSpeaking == true)
                    {
                        GetComponentInChildren<Boss>().enabled = false;
                        timer += Time.deltaTime;
                        if (timer >= cooldown)
                        {
                            isSpeaking = false;
                        }
                    }
                    else if (isSpeaking == false) {
                            Debug.Log("boss enabled");
                            GetComponentInChildren<Boss>().enabled = true; }
        }
        
    }




    
    

}