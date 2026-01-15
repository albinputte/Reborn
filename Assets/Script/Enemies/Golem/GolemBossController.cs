using System.Collections.Generic;
using UnityEngine;

public class GolemBossController : EnemyBaseController
{
    [Header("References")]
    public EnemyStateMachine<GolemBossController> stateMachine;
    public Health crystalHealth;

    [Header("Pillars")]
    public GolemPillar[] pillars;

    [Header("Ability System")]
    public int abilityPoints;
    public Queue<GolemAbilityType> abilityQueue = new();
    public Animator animator;
    public float ChargeTimePhase;
    public float[] TimeBetweenPhases;

    private BossHealthBar healthBar;


    private void Start()
    {
        stateMachine = new EnemyStateMachine<GolemBossController> ();
        stateMachine.InstantiateState(new GolemIdleState(stateMachine, this));
        SceneManger.instance.OnAllEssentialScenesLoaded += StartBoss;
    }

    private void Update()
    {
        stateMachine.CurrentState?.LogicUpdate();
    }

    public void StartBoss()
    {
        healthBar = BossHealthBar.instance;
        healthBar.SetBossHealthBar(crystalHealth);
    }
    public void AddAbilityPoint(GolemAbilityType ability)
    {
        abilityPoints++;
        abilityQueue.Enqueue(ability);
    }

    public bool CanExecuteAbility()
    {
        return abilityPoints > 1 && abilityQueue.Count > 1;
    }

    public GolemAbilityType GetNextAbility()
    {
        abilityPoints--;
        if (abilityPoints <= 0) {
            abilityPoints = 0;
            return GolemAbilityType.none;
        }
        return abilityQueue.Dequeue();
    }

    public void StartPillarCharging()
    {
        foreach (var pillar in pillars)
            pillar.StartCharge();
    }
}

public enum GolemAbilityType
{
    Meteor,
    Mage,
    Creature,
    Wall,
    none
}