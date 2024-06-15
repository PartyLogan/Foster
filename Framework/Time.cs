namespace Foster.Framework;

public static class Time
{
	/// <summary>
	/// The delta to use if you want it to be affected by the game speed, unsure if this is what should be used below for the things like OnInterval, probably not since they seem like real x second based functions
	/// </summary>
	public static float Delta
	{
		get => DeltaRaw * TimeMod;
	}

	/// <summary>
	/// The game speed modifier, used to slow down and speed up the game by modifing the delta time.
	/// </summary>
	public static float TimeMod { set => timemod = Calc.Clamp(value, 0.01f, 1000f); get => timemod; }
	private static float timemod = 1f;

	/// <summary>
	/// Time in Seconds since our last Update.
	/// In Fixed Timestep this always returns a constant value.
	/// </summary>
	public static float DeltaRaw;

	/// <summary>
	/// An Accumulation of the Delta Time, incremented each Update.
	/// In Fixed Timestep this is incremented by a constant value.
	/// </summary>
	public static TimeSpan Duration;

	/// <summary>
	/// Requests the current time since the Application was started.
	/// This is different than Time.Duration which is only incremented once per frame.
	/// This is also not affected by Fixed Timestep.
	/// </summary>
	public static TimeSpan Now => App.Timer.Elapsed;

	/// <summary>
	/// Current frame index
	/// </summary>
	public static ulong Frame = 0;

	/// <summary>
	/// If the Application should run in Fixed Timestep mode
	/// </summary>
	public static bool FixedStep = true;

	/// <summary>
	/// What the Fixed Timestep target is (defaults to 60fps, or 0.016s per frame)
	/// </summary>
	public static TimeSpan FixedStepTarget = TimeSpan.FromSeconds(1.0f / 60.0f);

	/// <summary>
	/// The maximum amount of time a Fixed Update is allowed to take before the Application starts dropping frames.
	/// </summary>
	public static TimeSpan FixedStepMaxElapsedTime = TimeSpan.FromSeconds(5.0f / 60.0f);

	/// <summary>
	/// Advances the Time.Duration by the given delta, and assigns Time.Delta.
	/// </summary>
	public static void Advance(TimeSpan delta)
	{
		DeltaRaw = (float)delta.TotalSeconds;
		Duration += delta;
	}

	/// <summary>
	/// Returns true when the elapsed time passes a given interval based on the delta time
	/// </summary>
	public static bool OnInterval(double time, double delta, double interval, double offset)
	{
		return Math.Floor((time - offset - delta) / interval) < Math.Floor((time - offset) / interval);
	}

	/// <summary>
	/// Returns true when the elapsed time passes a given interval based on the delta time
	/// </summary>
	public static bool OnInterval(double delta, double interval, double offset)
	{
		return OnInterval(Duration.TotalSeconds, delta, interval, offset);
	}

	/// <summary>
	/// Returns true when the elapsed time passes a given interval based on the delta time
	/// </summary>
	public static bool OnInterval(double interval, double offset = 0.0)
	{
		return OnInterval(Duration.TotalSeconds, DeltaRaw, interval, offset);
	}

	/// <summary>
	/// Returns true when the elapsed time is between the given interval. Ex: an interval of 0.1 will be false for 0.1 seconds, then true for 0.1 seconds, and then repeat.
	/// </summary>
	public static bool BetweenInterval(double time, double interval, double offset)
	{
		return (time - offset) % (interval * 2) >= interval;
	}

	/// <summary>
	/// Returns true when the elapsed time is between the given interval. Ex: an interval of 0.1 will be false for 0.1 seconds, then true for 0.1 seconds, and then repeat.
	/// </summary>
	public static bool BetweenInterval(double interval, double offset = 0.0)
	{
		return BetweenInterval(Duration.TotalSeconds, interval, offset);
	}

	/// <summary>
	/// Sine-wave a value between `from` and `to` with a period of `duration`.
	/// You can use `offsetPercent` to offset the sine wave.
	/// </summary>
	public static float SineWave(float from, float to, float duration, float offsetPercent)
	{
		float total = (float)Duration.TotalSeconds;
		float range = (to - from) * 0.5f;
		return from + range + MathF.Sin(((total + duration * offsetPercent) / duration) * MathF.Tau) * range;
	}
}
