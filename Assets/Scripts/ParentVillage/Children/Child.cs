using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Assertions;

public class Child : IData
{
    public enum ChildState
    {
        kAlive,
        kGraduated,
        kDead,
        kUnborn
    }

    public delegate void LockedInHandler(Child child);

    public const float MaxEducation = 100;
    public const float MaxHealth = 100;
    public const float MaxSafety = 100;
    public const float MaxHappiness = 100;

    private float education;
    public float Education
    {
        get { return education; }
        private set
        {
            education = value;
            if (education >= MaxEducation)
            {
                ChildManager.Instance.GraduateChild(this);
            }
        }
    }

    private float health;
    public float Health
    {
        get { return health; }
        private set
        {
            health = value;
            if (health <= 0)
            {
                ChildManager.Instance.KillChild(this);
            }
        }
    }

    public float Safety { get; private set; }

    public float Happiness { get; private set; }

    public ChildState State { get; set; }
    public bool IsSelected { get; set; }

    public BuildingType BuildingType { get; private set; }

    public const string Surname = "Keita";
    public string Name { get; private set; }

    public Child(string name)
    {
        BuildingType = BuildingType.Idle;
        State = ChildState.kUnborn;
        Name = name;
        Education = 0;
        Health = MaxHealth;
        Safety = MaxSafety;
        Happiness = MaxHappiness;
    }

    public void Apply(DataPacket data)
    {
        if (State == ChildState.kAlive)
        {
            Education = MathUtils.Clamp(Education + data.Education, 0, MaxEducation);
            Health = MathUtils.Clamp(Health + data.Health, 0, MaxHealth);
            Safety = MathUtils.Clamp(Safety + data.Safety, 0, MaxSafety);
            Happiness = MathUtils.Clamp(Happiness + data.Happiness, 0, MaxHappiness);
        }
    }

    public void LockIn(BuildingType buildingType)
    {
        Assert.IsTrue(buildingType == BuildingType.Idle || BuildingType == BuildingType.Idle);
        BuildingType = buildingType;
    }
}