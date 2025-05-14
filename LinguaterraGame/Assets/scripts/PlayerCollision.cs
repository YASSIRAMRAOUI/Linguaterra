// using UnityEngine;
// using UnityEngine.UI;
// using TMPro; // Use this if you're using TextMeshPro
// using UnityEngine.SceneManagement;
// using System.Collections.Generic;

// public class PlayerCollision : MonoBehaviour
// {
//     [Header("Collision Settings")]
//     public bool restartOnCollision = false;
    
//     [Header("UI References")]
//     public GameObject soundChoicePanel;
//     public Button option1Button;
//     public Button option2Button;
//     public TextMeshProUGUI option1Text; // Or use Text if not using TMP
//     public TextMeshProUGUI option2Text; // Or use Text if not using TMP
//     public TextMeshProUGUI feedbackText; // Optional feedback text
    
//     [Header("Audio References")]
//     public AudioSource option1Sound;
//     public AudioSource option2Sound;
//     public AudioSource correctSound;
//     public AudioSource incorrectSound;
    
//     // [Header("Sound Choices")]
//     [System.Serializable]
//     public class SoundPair
//     {
//         public string option1Text = "ma";
//         public string option2Text = "am";
//         public AudioClip option1Clip;
//         public AudioClip option2Clip;
//         public bool isOption1Correct = true;
//     }
    
//     public List<SoundPair> soundPairs = new List<SoundPair>();
//     private SoundPair currentPair;
    
//     // Reference to player movement
//     private PlayerMovement playerMovement;
    
//     void Start()
//     {
//         Debug.Log("PlayerCollision script is active");
//         playerMovement = GetComponent<PlayerMovement>();
        
//         // Hide panel at start
//         if (soundChoicePanel != null)
//             soundChoicePanel.SetActive(false);
            
//         // Set up button listeners
//         if (option1Button != null)
//             option1Button.onClick.AddListener(OnOption1Selected);
            
//         if (option2Button != null)
//             option2Button.onClick.AddListener(OnOption2Selected);
            
//         // Initialize default sound pairs if empty
//         if (soundPairs.Count == 0)
//         {
//             soundPairs.Add(new SoundPair());
//         }
//     }

//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         // Ignore if it's layer 4 (Ground)
//         if (collision.gameObject.layer == 4)
//             return;

//         // Rest of your collision logic for Obstacles
//         if (collision.gameObject.CompareTag("Obstacle"))
//         {
//             Debug.Log("Player hit an obstacle! Opening popup...");

//             if (restartOnCollision)
//             {
//                 SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//             }
//             else
//             {
//                 if (playerMovement != null)
//                 {
//                     playerMovement.DisableMovement();
//                     Debug.Log("Player movement disabled");
//                 }
                
//                 ShowRandomSoundChoice();
//             }
//         }
//     }
    
//     void ShowRandomSoundChoice()
//     {
//         // Choose a random pair from our list
//         if (soundPairs.Count > 0)
//         {
//             int randomIndex = Random.Range(0, soundPairs.Count);
//             currentPair = soundPairs[randomIndex];
            
//             // Update UI
//             if (option1Text != null)
//                 option1Text.text = currentPair.option1Text;
            
//             if (option2Text != null)
//                 option2Text.text = currentPair.option2Text;
            
//             // Set audio clips
//             if (option1Sound != null && currentPair.option1Clip != null)
//                 option1Sound.clip = currentPair.option1Clip;
                
//             if (option2Sound != null && currentPair.option2Clip != null)
//                 option2Sound.clip = currentPair.option2Clip;
            
//             // Clear feedback text if we have it
//             if (feedbackText != null)
//                 feedbackText.text = "";
                
//             // Show the panel
//             if (soundChoicePanel != null)
//                 soundChoicePanel.SetActive(true);
//         }
//     }
    
//     void OnOption1Selected()
//     {
//         // Play the sound
//         if (option1Sound != null && option1Sound.clip != null)
//             option1Sound.Play();
            
//         // Wait for sound to play before checking answer
//         Invoke("CheckOption1Answer", option1Sound.clip != null ? option1Sound.clip.length : 0.5f);
//     }
    
//     void OnOption2Selected()
//     {
//         // Play the sound
//         if (option2Sound != null && option2Sound.clip != null)
//             option2Sound.Play();
            
//         // Wait for sound to play before checking answer
//         Invoke("CheckOption2Answer", option2Sound.clip != null ? option2Sound.clip.length : 0.5f);
//     }
    
//     void CheckOption1Answer()
//     {
//         bool isCorrect = currentPair != null && currentPair.isOption1Correct;
//         GiveFeedback(isCorrect);
//     }
    
//     void CheckOption2Answer()
//     {
//         bool isCorrect = currentPair != null && !currentPair.isOption1Correct;
//         GiveFeedback(isCorrect);
//     }
    
//     void GiveFeedback(bool isCorrect)
//     {
//         // Play correct/incorrect sound
//         if (isCorrect && correctSound != null)
//             correctSound.Play();
//         else if (!isCorrect && incorrectSound != null)
//             incorrectSound.Play();
            
//         // Show feedback text if available
//         if (feedbackText != null)
//         {
//             feedbackText.text = isCorrect ? "Correct! 👍" : "Try again! 🔄";
//             feedbackText.color = isCorrect ? Color.green : Color.red;
//         }
        
//         // Close panel after delay (longer if incorrect to give time to try again)
//         if (isCorrect)
//             Invoke("ClosePanel", 2.0f);  // Close after 2 seconds if correct
//         else
//             Invoke("ResetChoices", 2.0f); // Allow retry if incorrect
//     }
    
//     void ResetChoices()
//     {
//         // Clear feedback and allow new selection
//         if (feedbackText != null)
//             feedbackText.text = "";
//     }
    
//     void ClosePanel()
//     {
//         // Hide the panel
//         if (soundChoicePanel != null)
//             soundChoicePanel.SetActive(false);
            
//         // Resume player movement
//         if (playerMovement != null)
//             playerMovement.EnableMovement();
//     }
// }