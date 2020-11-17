//using UnityEngine;
//using System;

//public class TimeAlarm
//{
//    public static double SystemTimeInMilliseconds { get { return (System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalMilliseconds; } }

//    #region Member Variables

//    public string Key { get; private set; } = null;
//    private double _ringingAfterMillisecond = 0;

//    #endregion

//    #region Properties

//    private string StartTimeKey { get { return Key + "StartTime"; } }
//    private string RingingTimeKey { get { return Key + "RingingTime"; } }
//    private bool isRang = false;

//    private double StartTime
//    {
//        get
//        {
//            return Convert.ToDouble(PlayerPrefs.GetString(StartTimeKey, "0"));
//        }
//        set
//        {
//            PlayerPrefs.SetString(StartTimeKey, value.ToString());
//        }
//    }

//    private double RingingTime
//    {
//        get
//        {
//            return Convert.ToDouble(PlayerPrefs.GetString(RingingTimeKey, "0"));
//        }
//        set
//        {
//            PlayerPrefs.SetString(RingingTimeKey, value.ToString());
//        }
//    }

//    public Action<TimeAlarm> OnAlarm { get; set; } = null;
//    public Action<TimeAlarm> OnTimeUpdated { get; set; } = null;

//    #endregion

//    #region Initialize Methods

//    public TimeAlarm(string key)
//    {
//        this.Key = key;
//    }

//    public static TimeAlarm StartTimer(string key, double ringingAfterMillisecond, Action<TimeAlarm> onAlarm)
//    {
//        TimeAlarm timeAlarm = new TimeAlarm(key);
//        timeAlarm.StartTimer(ringingAfterMillisecond);
//        timeAlarm.OnAlarm = onAlarm;
//        return timeAlarm;
//    }

//    #endregion

//    #region Public Methods

//    // Set start.
//    // ex) If the alarm goes off after 3 seconds : RingingAfterMillisecond = 3000.
//    public void StartTimer(double ringingAfterMillisecond)
//    {
//        _ringingAfterMillisecond = ringingAfterMillisecond;
//        StartTime = SystemTimeInMilliseconds;
//        RingingTime = StartTime + ringingAfterMillisecond;
//        isRang = false;
//    }

//    public void UpdateTimer()
//    {
//        if (isRang)
//            return;

//        OnTimeUpdated?.Invoke(this);

//        if (GetRemainedTime() == 0)
//        {
//            OnAlarm?.Invoke(this);
//            Dispose();
//        }
//    }

//    public double GetElapsedTime()
//    {
//        double now = SystemTimeInMilliseconds;
//        double elapsedTime = now - StartTime;

//        return elapsedTime;
//    }

//    public double GetRemainedTime()
//    {
//        double now = SystemTimeInMilliseconds;
//        double remainedTime = RingingTime - now;

//        if (remainedTime <= 0)
//        {
//            return 0;
//        }

//        return remainedTime;
//    }

//    public string GetRemainedTimeToString()
//    {
//        return TimeSpan.FromMilliseconds(GetRemainedTime()).ToString(@"m\:ss");
//    }

//    public static bool Contains(string key)
//    {
//        var alarm = new TimeAlarm(key);
//        if (alarm.GetRemainedTime() > 0)
//        {
//            return true;
//        }
//        else
//        {
//            alarm.Dispose();
//            return false;
//        }
//    }

//    public void Dispose()
//    {
//        if (PlayerPrefs.HasKey(StartTimeKey))
//            PlayerPrefs.DeleteKey(StartTimeKey);
//        if (PlayerPrefs.HasKey(RingingTimeKey))
//            PlayerPrefs.DeleteKey(RingingTimeKey);
//        isRang = true;
//    }

//    #endregion


//}
