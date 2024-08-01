using Discord;
using System;
using UnityEngine;

public class DiscordManager : MonoBehaviour
{
    public static DiscordManager Instance { get; private set; }

    [SerializeField]
    private Discord.Discord discord;

    [Header("Rich Presence Settings")]
    [SerializeField]
    private string details;

    [SerializeField]
    private string state;

    [SerializeField]
    private string largeImage;

    [SerializeField]
    private string largeText;

    [SerializeField]
    private string smallImage;

    [SerializeField]
    private string smallText;

    [SerializeField]
    private int currentPartySize;

    [SerializeField]
    private int maxPartySize;

    [SerializeField]
    private long timestamp;

    [SerializeField]
    private long endTimestamp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        discord = new Discord.Discord(1268613606937985056, (long)CreateFlags.Default);
        UpdateRichPresence(updateTimestamp: true);
    }

    void UpdateRichPresence(string details = null, string state = null, string largeImage = null, string largeText = null, string smallImage = null, string smallText = null, int currentPartySize = -1, int maxPartySize = -1, bool updateTimestamp = false, long endTimestamp = -1)
    {
        this.details = details ?? this.details;
        this.state = state ?? this.state;
        this.largeImage = largeImage ?? this.largeImage;
        this.largeText = largeText ?? this.largeText;
        this.smallImage = smallImage ?? this.smallImage;
        this.smallText = smallText ?? this.smallText;
        this.currentPartySize = currentPartySize != -1 ? currentPartySize : this.currentPartySize;
        this.maxPartySize = maxPartySize != -1 ? maxPartySize : this.maxPartySize;
        this.timestamp = updateTimestamp ? new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() : this.timestamp;
        this.endTimestamp = endTimestamp != -1 ? endTimestamp : this.endTimestamp;

        ActivityManager activityManager = discord.GetActivityManager();
        Activity activity = new()
        {
            Details = this.details,
            State = this.state,
            Assets = { LargeImage = this.largeImage, LargeText = this.largeText, SmallImage = this.smallImage, SmallText = this.smallText },
            Timestamps = { Start = this.timestamp, End = this.endTimestamp },
            Party = { Size = { CurrentSize = this.currentPartySize, MaxSize = this.maxPartySize } }
        };
        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res == Result.Ok)
            {
                Debug.Log("Daj ziobu");
            }
            else
            {
                Debug.LogError("xpp");
            }
        });
    }

    void Update()
    {
        if (discord != null)
        {
            discord.RunCallbacks();
        }
    }

    void OnDisable()
    {
        if (discord != null)
        {
            discord.Dispose();
        }
    }
}
