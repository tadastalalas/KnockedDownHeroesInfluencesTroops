using System;
using MCM.Abstractions.Base.Global;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace KnockedDownHeroesInfluencesTroops
{
    public class KnockedDownHeroesInfluencesTroopsMissionBehavior : MissionBehavior
    {
        public override MissionBehaviorType BehaviorType => MissionBehaviorType.Other;

        private readonly MCMSettings settings = AttributeGlobalSettings<MCMSettings>.Instance ?? new MCMSettings();

        private const int rangeForTroopsToReactToUnassignedHeroFall = 10;
        private const int rangeForTroopsToReactToCaptainHeroFall = 20;
        private const int rangeForTroopsToReactToGeneralHeroFall = 30;

        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);

            if (!settings.EnableThisModification)
                return;

            MissionUtilities.ProcessCheerQueue(dt);
        }

        public override void OnRemoveBehavior()
        {
            MissionUtilities.ResetCheerState();
            base.OnRemoveBehavior();
        }

        public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
        {
            base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);

            if (!settings.EnableThisModification)
                return;

            if (Mission.Current == null || (!Mission.Current.IsFieldBattle && !Mission.Current.IsSiegeBattle))
                return;

            if (affectedAgent == null || affectorAgent == null || affectedAgent == affectorAgent)
                return;

            if (agentState != AgentState.Killed && agentState != AgentState.Unconscious)
                return;

            if (!affectedAgent.IsHero)
                return;

            if (affectorAgent.IsHero)
                HeroKnockedDownAgent(affectorAgent, affectedAgent);
            else
                SimpleTroopKnockedDownAgent(affectorAgent, affectedAgent);

            if (settings.LoggingEnabled)
                MissionUtilities.DisplayKnockdownMessage(affectorAgent, affectedAgent);
        }

        private void HeroKnockedDownAgent(Agent attacker, Agent victim)
        {
            if (MissionUtilities.IsAgentGeneral(attacker))
                GeneralKnockedDownAgent(attacker, victim);
            else if (MissionUtilities.IsAgentCaptain(attacker))
                CaptainKnockedDownAgent(attacker, victim);
            else
                UnassignedHeroKnockedDownAgent(attacker, victim);
        }

        private void GeneralKnockedDownAgent(Agent attacker, Agent victim)
        {
            MissionUtilities.SetWantsToYellForTeam(attacker.Team);

            if (MissionUtilities.IsAgentGeneral(victim))
            {
                MissionUtilities.UpdateTeamMorale(victim.Team, -settings.MoraleChangeWhenGeneralHeroKillsGeneralHero);
                MissionUtilities.UpdateTeamMorale(attacker.Team, settings.MoraleGainWhenGeneralHeroKillsGeneralHero);
                ShowOnScreenNotification(attacker, victim, MissionUtilities.DisplayQuickInformationMessageWhenGeneralFalls);
            }
            else if (MissionUtilities.IsAgentCaptain(victim))
            {
                MissionUtilities.UpdateFormationMorale(victim.Formation, -settings.MoraleChangeWhenGeneralHeroKillsCaptainHero);
                MissionUtilities.UpdateTeamMorale(attacker.Team, settings.MoraleGainWhenGeneralHeroKillsCaptainHero);
                ShowOnScreenNotification(attacker, victim, MissionUtilities.DisplayQuickInformationMessageWhenCaptainFalls);
            }
            else
            {
                MissionUtilities.UpdateMoraleForNearbyAgents(victim.Team, attacker, rangeForTroopsToReactToGeneralHeroFall, -settings.MoraleChangeWhenGeneralHeroKillsUnassignedHero);
                MissionUtilities.UpdateMoraleForNearbyAgents(attacker.Team, attacker, rangeForTroopsToReactToGeneralHeroFall, settings.MoraleGainWhenGeneralHeroKillsUnassignedHero);
                ShowOnScreenNotification(attacker, victim, MissionUtilities.DisplayQuickInformationMessageWhenUnassignedHeroFalls);
            }
        }

        private void CaptainKnockedDownAgent(Agent attacker, Agent victim)
        {
            MissionUtilities.SetWantsToYellForFormation(attacker.Formation);

            if (MissionUtilities.IsAgentGeneral(victim))
            {
                MissionUtilities.UpdateTeamMorale(victim.Team, -settings.MoraleChangeWhenCaptainHeroKillsGeneralHero);
                MissionUtilities.UpdateTeamMorale(attacker.Team, settings.MoraleGainWhenCaptainHeroKillsGeneralHero);
                ShowOnScreenNotification(attacker, victim, MissionUtilities.DisplayQuickInformationMessageWhenGeneralFalls);
            }
            else if (MissionUtilities.IsAgentCaptain(victim))
            {
                MissionUtilities.UpdateFormationMorale(victim.Formation, -settings.MoraleChangeWhenCaptainHeroKillsCaptainHero);
                MissionUtilities.UpdateFormationMorale(attacker.Formation, settings.MoraleGainWhenCaptainHeroKillsCaptainHero);
                ShowOnScreenNotification(attacker, victim, MissionUtilities.DisplayQuickInformationMessageWhenCaptainFalls);
            }
            else
            {
                MissionUtilities.UpdateMoraleForNearbyAgents(victim.Team, attacker, rangeForTroopsToReactToUnassignedHeroFall + 10, -settings.MoraleChangeWhenCaptainHeroKillsUnassignedHero);
                MissionUtilities.UpdateMoraleForNearbyAgents(attacker.Team, attacker, rangeForTroopsToReactToUnassignedHeroFall + 10, settings.MoraleGainWhenCaptainHeroKillsUnassignedHero);
                ShowOnScreenNotification(attacker, victim, MissionUtilities.DisplayQuickInformationMessageWhenUnassignedHeroFalls);
            }
        }

        private void UnassignedHeroKnockedDownAgent(Agent attacker, Agent victim)
        {
            if (MissionUtilities.IsAgentGeneral(victim))
            {
                MissionUtilities.SetWantsToYellInRange(attacker, rangeForTroopsToReactToGeneralHeroFall + 5);
                MissionUtilities.UpdateTeamMorale(victim.Team, -settings.MoraleChangeWhenUnassignedHeroKillsGeneralHero);
                MissionUtilities.UpdateTeamMorale(attacker.Team, settings.MoraleGainWhenUnassignedHeroKillsGeneralHero);
                ShowOnScreenNotification(attacker, victim, MissionUtilities.DisplayQuickInformationMessageWhenGeneralFalls);
            }
            else if (MissionUtilities.IsAgentCaptain(victim))
            {
                MissionUtilities.SetWantsToYellInRange(attacker, rangeForTroopsToReactToCaptainHeroFall + 5);
                MissionUtilities.UpdateFormationMorale(victim.Formation, -settings.MoraleChangeWhenUnassignedHeroKillsCaptainHero);
                MissionUtilities.UpdateMoraleForNearbyAgents(attacker.Team, attacker, rangeForTroopsToReactToCaptainHeroFall + 5, settings.MoraleGainWhenUnassignedHeroKillsCaptainHero);
                ShowOnScreenNotification(attacker, victim, MissionUtilities.DisplayQuickInformationMessageWhenCaptainFalls);
            }
            else
            {
                MissionUtilities.SetWantsToYellInRange(attacker, rangeForTroopsToReactToUnassignedHeroFall + 5);
                MissionUtilities.UpdateMoraleForNearbyAgents(victim.Team, victim, rangeForTroopsToReactToUnassignedHeroFall, -settings.MoraleChangeWhenUnassignedHeroKillsUnassignedHero);
                MissionUtilities.UpdateMoraleForNearbyAgents(attacker.Team, attacker, rangeForTroopsToReactToUnassignedHeroFall, settings.MoraleGainWhenUnassignedHeroKillsUnassignedHero);
                ShowOnScreenNotification(attacker, victim, MissionUtilities.DisplayQuickInformationMessageWhenUnassignedHeroFalls);
            }
        }

        private void SimpleTroopKnockedDownAgent(Agent attacker, Agent victim)
        {
            if (MissionUtilities.IsAgentGeneral(victim))
            {
                MissionUtilities.SetWantsToYellInRange(attacker, rangeForTroopsToReactToGeneralHeroFall);
                MissionUtilities.UpdateTeamMorale(victim.Team, -settings.MoraleChangeWhenTroopKillsGeneralHero);
                MissionUtilities.UpdateTeamMorale(attacker.Team, settings.MoraleGainWhenTroopKillsGeneralHero);
                ShowOnScreenNotification(attacker, victim, MissionUtilities.DisplayQuickInformationMessageWhenGeneralFalls);
            }
            else if (MissionUtilities.IsAgentCaptain(victim))
            {
                MissionUtilities.SetWantsToYellInRange(attacker, rangeForTroopsToReactToCaptainHeroFall);
                MissionUtilities.UpdateFormationMorale(victim.Formation, -settings.MoraleChangeWhenTroopKillsCaptainHero);
                MissionUtilities.UpdateMoraleForNearbyAgents(attacker.Team, attacker, rangeForTroopsToReactToCaptainHeroFall, settings.MoraleGainWhenTroopKillsCaptainHero);
                ShowOnScreenNotification(attacker, victim, MissionUtilities.DisplayQuickInformationMessageWhenCaptainFalls);
            }
            else
            {
                MissionUtilities.SetWantsToYellInRange(attacker, rangeForTroopsToReactToUnassignedHeroFall);
                MissionUtilities.UpdateMoraleForNearbyAgents(victim.Team, victim, rangeForTroopsToReactToUnassignedHeroFall, -settings.MoraleChangeWhenTroopKillsUnassignedHero);
                MissionUtilities.UpdateMoraleForNearbyAgents(attacker.Team, attacker, rangeForTroopsToReactToUnassignedHeroFall, settings.MoraleGainWhenTroopKillsUnassignedHero);
                ShowOnScreenNotification(attacker, victim, MissionUtilities.DisplayQuickInformationMessageWhenUnassignedHeroFalls);
            }
        }

        private void ShowOnScreenNotification(Agent attackerAgent, Agent affectedAgent, Action<Agent, Agent> displayNotification)
        {
            if (settings.ShowOnScreenNotifications)
                displayNotification(attackerAgent, affectedAgent);
        }
    }
}