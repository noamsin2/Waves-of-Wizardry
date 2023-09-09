using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance { get; private set; }
 
    public delegate void ExperienceChangeHandler(float exp);
    public event ExperienceChangeHandler OnExpChange;
    [field: SerializeField] private float expMultiplier;
    private void Awake()
    {
        Instance = this;
    }
    public void AddExperience(int exp)
    {
        OnExpChange?.Invoke(exp*(1 + expMultiplier));
    }
    public void IncreaseExpMultiplier(float multiplier)
    {
        expMultiplier += (multiplier / 100);
    }
}
