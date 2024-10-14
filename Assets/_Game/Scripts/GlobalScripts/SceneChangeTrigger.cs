using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Vector3 offset = new(0f, 0f, 8f);

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out _))
        {
            SavePosition.DictPositions[SceneManager.GetActiveScene().name] = new Vector3(transform.position.x, 3.0f,
                    transform.position.z) + offset;
            Debug.Log("sceneName to load: " + sceneName);
            StartCoroutine(LoadScene(sceneName));
        }
    }

    private IEnumerator LoadScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName);
    }
}
