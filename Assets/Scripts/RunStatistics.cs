// the statistics data class.
// access it with e.g.
// RunStatistics.Instance.bubblesCleared++
//
public class RunStatistics : Singleton<RunStatistics>
{
    // (Optional) Prevent non-singleton constructor use.
    protected RunStatistics() { }

    // these are stats for a single run, not sure if totals should
    // be separate or also accessed here

    public string playerName;
    //don't set this, save manager will calculate at run finish
    public float time;
    public int controlMode;
    public int stagesCleared;
    public int totalScore;
    public int bubblesCleared;

    public int bubblesChainClearedRed;
    public int bubblesChainClearedBlue;
    public int bubblesChainClearedYellow;
    public int bubblesChainClearedOther;
    // Total Chain Clears: calculate when displaying
    // Clears Per Minute: calculate when displaying

    public int trapsUsed;
    public int capturesUsed;
    public int trapsMissed; //set when player bullet hits wall/expires

    public float grazeTime;
    public float focusTime;
    public int rollsPerformed;
}
