using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
public class AchievementNotificationController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI achievementTitleLabel;

    private Animator Notification;

    private void Awake()
    {
        Notification = GetComponent<Animator>();
    }
    public void ShowNotification(Achievement achievement)
    {
        achievementTitleLabel.text = achievement.title;
        Notification.SetTrigger("Appear");
    }
}
