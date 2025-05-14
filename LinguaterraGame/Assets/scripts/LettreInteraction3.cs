using UnityEngine;

public class LettreInteraction3 : MonoBehaviour
{
    public string lettreName;
    public AudioClip sonCorrect;
    public GameObject quizUI;
    public AudioClip[] sonsLeurres;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && quizUI != null)
        {
            if (sonCorrect == null || sonsLeurres.Length < 2 || sonsLeurres[0] == null || sonsLeurres[1] == null)
            {
                Debug.LogError("Configuration audio incompl�te!", this);
                return;
            }

            AudioClip[] optionsSons = {
                sonCorrect,
                sonsLeurres[0],
                sonsLeurres[1]
            };

            // M�langer al�atoirement
            for (int i = 0; i < optionsSons.Length; i++)
            {
                int rnd = Random.Range(i, optionsSons.Length);
                (optionsSons[rnd], optionsSons[i]) = (optionsSons[i], optionsSons[rnd]);
            }

            quizUI.GetComponent<QuizUI3>().InitialiserQuiz(lettreName, sonCorrect, optionsSons);
            quizUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && quizUI != null)
        {
            // Lorsque le joueur quitte la zone, on cache le canvas sans changer le mot.
            quizUI.SetActive(false);
        }
    }
}
