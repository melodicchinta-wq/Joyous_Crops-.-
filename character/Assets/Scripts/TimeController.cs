using UnityEngine;

public class TimeController : MonoBehaviour
{
	public static TimeController instance;

	[Header("How Long is 1 in-game DAY in real life? (in minutes)")]
	public float realMinutesPerDay = 7f;  // kamu atur di inspector

	[Header("Currnet Time")]
	public int hour = 6;
	public int minute = 0;
	public int day = 1;

	private float timer;
	private float realSecondsPer1InGameMinute;

	void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(gameObject);

		//Hitung Otomatis;
		//1440 menit in-game dibagi durasi real 
		realSecondsPer1InGameMinute = (realMinutesPerDay * 60f) / 1440f;
	}

	void Update()
	{
		timer += Time.deltaTime;

		if (timer >= realSecondsPer1InGameMinute)
		{
			timer = 0f;
			AddMinutes(1);
		}
	}

	void AddMinutes(int amout)
	{
		minute += amout;

		while (minute >= 60)
		{
			minute -= 60;
			hour++;
		}

		if (hour >= 24)
		{
			hour = 0;
			day++;
		}
	}

	public string GetTimeString()
	{
		return hour.ToString("00") + ":" + minute.ToString("00");
	}

	public float GetTimeNormalized()
	{
		float totalMinutes = hour * 60 + minute;
		return totalMinutes / 1440f;
	}
}