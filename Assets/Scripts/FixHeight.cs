using UnityEngine;

public class FixHeight : MonoBehaviour
{
    [SerializeField] GameObject mannequine;
    [SerializeField] GameObject playerVrPrefab;
    [SerializeField] GameObject plane;

    private bool isTheCorrectOne = false;
    private bool isFirstOne = true;
    private float time = 0f;

    void Update()
    {
        time += Time.deltaTime;
        if (!isTheCorrectOne)
        {
            if (gameObject.transform.position.y <= plane.transform.position.y)
            {
                mannequine.transform.localScale -= new Vector3(0.001f, 0.001f, 0.001f);
            }
            else if (gameObject.transform.position.y >= plane.transform.position.y)
            {
                mannequine.transform.localScale += new Vector3(0.001f, 0.001f, 0.001f);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (time >= 2 && isFirstOne)
        {
            isTheCorrectOne = true;

            GameObject playerVR = Instantiate(playerVrPrefab);
            playerVR.GetComponent<InitHeight>().Init(mannequine.transform.localScale);
            isFirstOne = false;
            //Destroy(mannequine.gameObject);
            Destroy(this.transform.root.gameObject);
        }
    }
}