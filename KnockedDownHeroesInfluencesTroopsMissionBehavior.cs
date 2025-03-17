using System.Collections.Generic;
using MCM.Abstractions.Base.Global;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace KnockedDownHeroesInfluencesTroops
{
    public class KnockedDownHeroesInfluencesTroopsMissionBehavior : MissionBehavior
    {
        public override MissionBehaviorType BehaviorType => MissionBehaviorType.Other;

        private readonly MCMSettings settings = AttributeGlobalSettings<MCMSettings>.Instance ?? new MCMSettings();

        private const int rangeForTroopsToRespondToUnassignedHero = 10;
        private const int rangeForTroopsToRespondToCaptainHero = 20;
        private const int rangeForTroopsToRespondToGeneralHero = 30;

        private float elapsedTime = 0f;
        private Agent? _mainAgent;
        private readonly List<Agent> _friendlyHeroes = new();
        private readonly List<Agent> _enemyHeroes = new();
        private readonly Dictionary<Agent, List<Agent>> _heroTroops = new();

        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);
            UpdateHeroAndTroopLists(dt);
        }

        private void UpdateHeroAndTroopLists(float dt)
        {
            if (Mission.Current == null || (!Mission.Current.IsFieldBattle && !Mission.Current.IsSiegeBattle)) return;

            elapsedTime += dt;
            if (elapsedTime < settings.UpdateIntervalInSeconds) return;

            SetupMainAgents();
            SetupTroopsOfHeroes();
            elapsedTime = 0f;
        }

        private void SetupMainAgents()
        {
            if (Mission.Current.MainAgent != null)
                _mainAgent = Mission.Current.MainAgent;

            if (settings.LoggingEnabled)
                InformationManager.DisplayMessage(new InformationMessage("Setting up AI heroes list:", Colors.Yellow));

            _friendlyHeroes.Clear();
            _enemyHeroes.Clear();
            foreach (var agent in Mission.Current.AllAgents)
            {
                if (agent.IsHero && agent.IsAIControlled && agent.State == AgentState.Active)
                {
                    if (agent.Team.IsPlayerAlly)
                    {
                        if (settings.LoggingEnabled)
                            InformationManager.DisplayMessage(new InformationMessage($"Friendly hero: {agent.Name}", Colors.Green));
                        _friendlyHeroes.Add(agent);
                    }
                    else
                    {
                        if (settings.LoggingEnabled)
                            InformationManager.DisplayMessage(new InformationMessage($"Enemy hero: {agent.Name}", Colors.Red));
                        _enemyHeroes.Add(agent);
                    }
                }
            }
        }

        public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
        {
            base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);

            if (Mission.Current == null || (!Mission.Current.IsFieldBattle && !Mission.Current.IsSiegeBattle) ||
                affectedAgent == null || affectorAgent == null || affectedAgent == affectorAgent || !affectedAgent.IsHero) return;

            if (settings.LoggingEnabled)
                DisplayKnockdownMessage(affectorAgent, affectedAgent);

            if (affectorAgent.IsHero)
            {
                if (affectorAgent.Team != null && affectorAgent == affectorAgent.Team.GeneralAgent)
                    HandleGeneralKnockdown(affectorAgent, affectedAgent);
                else if (affectorAgent.Formation != null && affectorAgent == affectorAgent.Formation.Captain)
                    HandleCaptainKnockdown(affectorAgent, affectedAgent);
                else
                    HandleUnassignedHeroKnockdown(affectorAgent, affectedAgent);
            }
            else
            {
                HandleTroopKnockdown(affectorAgent, affectedAgent);
            }
        }

        private void DisplayKnockdownMessage(Agent affectorAgent, Agent affectedAgent) =>
            InformationManager.DisplayMessage(new InformationMessage($"{affectorAgent.Name} knocked down {affectedAgent.Name}.", affectorAgent.Team.IsPlayerAlly ? Colors.Green : Colors.Red));

        private void HandleGeneralKnockdown(Agent affectorAgent, Agent affectedAgent)
        {
            SetWantsToYellForTeam(affectorAgent.Team);

            if (affectedAgent == affectedAgent.Team.GeneralAgent)
            {
                ChangeMoraleForTeam(affectedAgent.Team, -settings.MoraleChangeWhenGeneralHeroKillsGeneralHero);
                if (settings.ShowOnScreenNotifications)
                    DisplayQuickInformationMessageWhenGeneralFalls(affectorAgent, affectedAgent);
            }
            else if (affectedAgent == affectedAgent.Formation.Captain)
            {
                ChangeMoraleForFormation(affectedAgent, -settings.MoraleChangeWhenGeneralHeroKillsCaptainHero);
                if (settings.ShowOnScreenNotifications)
                    DisplayQuickInformationMessageWhenCaptainFalls(affectorAgent, affectedAgent);
            }
            else
            {
                ChangeMoraleForNearbyAgents(affectedAgent.Team, affectorAgent, rangeForTroopsToRespondToGeneralHero, -settings.MoraleChangeWhenGeneralHeroKillsUnassignedHero);
                if (settings.ShowOnScreenNotifications)
                    DisplayQuickInformationMessageWhenUnassignedHeroFalls(affectorAgent, affectedAgent);
            }
        }

        private void HandleCaptainKnockdown(Agent affectorAgent, Agent affectedAgent)
        {
            SetWantsToYellForFormation(_heroTroops[affectorAgent]);

            if (affectedAgent == affectedAgent.Team.GeneralAgent)
            {
                ChangeMoraleForTeam(affectedAgent.Team, -settings.MoraleChangeWhenCaptainHeroKillsGeneralHero);
                if (settings.ShowOnScreenNotifications)
                    DisplayQuickInformationMessageWhenGeneralFalls(affectorAgent, affectedAgent);
            }
            else if (affectedAgent == affectedAgent.Formation.Captain)
            {
                ChangeMoraleForFormation(affectedAgent, -settings.MoraleChangeWhenCaptainHeroKillsCaptainHero);
                if (settings.ShowOnScreenNotifications)
                    DisplayQuickInformationMessageWhenCaptainFalls(affectorAgent, affectedAgent);
            }
            else
            {
                ChangeMoraleForNearbyAgents(affectedAgent.Team, affectorAgent, rangeForTroopsToRespondToCaptainHero, -settings.MoraleChangeWhenCaptainHeroKillsUnassignedHero);
                if (settings.ShowOnScreenNotifications)
                    DisplayQuickInformationMessageWhenUnassignedHeroFalls(affectorAgent, affectedAgent);
            }
        }

        private void HandleUnassignedHeroKnockdown(Agent affectorAgent, Agent affectedAgent)
        {
            if (affectedAgent == affectedAgent.Team.GeneralAgent)
            {
                ChangeMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, rangeForTroopsToRespondToGeneralHero, 0, true);
                ChangeMoraleForTeam(affectedAgent.Team, -settings.MoraleChangeWhenUnassignedHeroKillsGeneralHero);
                if (settings.ShowOnScreenNotifications)
                    DisplayQuickInformationMessageWhenGeneralFalls(affectorAgent, affectedAgent);
            }
            else if (affectedAgent == affectedAgent.Formation.Captain)
            {
                ChangeMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, rangeForTroopsToRespondToCaptainHero, 0, true);
                ChangeMoraleForFormation(affectedAgent, -settings.MoraleChangeWhenUnassignedHeroKillsCaptainHero);
                if (settings.ShowOnScreenNotifications)
                    DisplayQuickInformationMessageWhenCaptainFalls(affectorAgent, affectedAgent);
            }
            else
            {
                ChangeMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, rangeForTroopsToRespondToUnassignedHero, 0, true);
                ChangeMoraleForNearbyAgents(affectedAgent.Team, affectedAgent, rangeForTroopsToRespondToUnassignedHero, -settings.MoraleChangeWhenUnassignedHeroKillsUnassignedHero);
                if (settings.ShowOnScreenNotifications)
                    DisplayQuickInformationMessageWhenUnassignedHeroFalls(affectorAgent, affectedAgent);
            }
        }

        private void HandleTroopKnockdown(Agent affectorAgent, Agent affectedAgent)
        {
            if (affectedAgent == affectedAgent.Team.GeneralAgent)
            {
                ChangeMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, rangeForTroopsToRespondToGeneralHero, 0, true);
                ChangeMoraleForTeam(affectedAgent.Team, -settings.MoraleChangeWhenTroopKillsGeneralHero);
                if (settings.ShowOnScreenNotifications)
                    DisplayQuickInformationMessageWhenGeneralFalls(affectorAgent, affectedAgent);
            }
            else if (affectedAgent == affectedAgent.Formation.Captain)
            {
                ChangeMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, rangeForTroopsToRespondToCaptainHero, 0, true);
                ChangeMoraleForFormation(affectedAgent, -settings.MoraleChangeWhenTroopKillsCaptainHero);
                if (settings.ShowOnScreenNotifications)
                    DisplayQuickInformationMessageWhenCaptainFalls(affectorAgent, affectedAgent);
            }
            else
            {
                ChangeMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, rangeForTroopsToRespondToUnassignedHero, 0, true);
                ChangeMoraleForNearbyAgents(affectedAgent.Team, affectedAgent, rangeForTroopsToRespondToUnassignedHero, -settings.MoraleChangeWhenTroopKillsUnassignedHero);
                if (settings.ShowOnScreenNotifications)
                    DisplayQuickInformationMessageWhenUnassignedHeroFalls(affectorAgent, affectedAgent);
            }
        }

        private void DisplayQuickInformationMessageWhenGeneralFalls(Agent affectorAgent, Agent affectedAgent)
        {
            if (affectedAgent.Team.IsPlayerTeam || affectedAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject("{=KDHIT_zSNYnN}Your stupid general is dead! Nobody will save you now!"), 2000, affectorAgent.Character, "event:/ui/notification/death");
            if (!affectedAgent.Team.IsPlayerTeam && !affectedAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject("{=KDHIT_VrpEkb}Enemy's general is dead! We will win this war!"), 2000, affectorAgent.Character, "event:/ui/notification/levelup");
        }

        private void DisplayQuickInformationMessageWhenCaptainFalls(Agent affectorAgent, Agent affectedAgent)
        {
            if (affectedAgent.Team.IsPlayerTeam || affectedAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject("{=KDHIT_44PmYz}Your idiot captain is dead! Give up, you dogs!"), 2000, affectorAgent.Character, "event:/ui/notification/death");
            if (!affectedAgent.Team.IsPlayerTeam && !affectedAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject("{=KDHIT_GeOvFE}Enemy's captain is dead! They stand no chance against us!"), 2000, affectorAgent.Character, "event:/ui/notification/levelup");
        }

        private void DisplayQuickInformationMessageWhenUnassignedHeroFalls(Agent affectorAgent, Agent affectedAgent)
        {
            if (affectedAgent.Team.IsPlayerTeam || affectedAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject("{=KDHIT_XlhfeB}Another one of your lapdogs is dead!"), 2000, affectorAgent.Character, "event:/ui/notification/death");
            if (!affectedAgent.Team.IsPlayerTeam && !affectedAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject("{=KDHIT_SLjiPR}We have slain another one of their lords!"), 2000, affectorAgent.Character, "event:/ui/notification/levelup");
        }

        private void ChangeMoraleForTeam(Team team, float moraleChange)
        {
            foreach (var agent in team.ActiveAgents)
                agent.ChangeMorale(moraleChange);
        }

        private void ChangeMoraleForFormation(Agent heroAgent, float moraleChange)
        {
            if (_heroTroops.TryGetValue(heroAgent, out var formation) && formation != null)
            {
                formation.RemoveAll(agent => agent == null);

                foreach (var agent in formation)
                {
                    agent.ChangeMorale(moraleChange);
                }
            }
        }


        private void ChangeMoraleForNearbyAgents(Team team, Agent referenceAgent, float range, float moraleChange, bool setWantsToYell = false)
        {
            foreach (var agent in team.ActiveAgents)
            {
                if (agent.Position.Distance(referenceAgent.Position) < range)
                {
                    if (setWantsToYell)
                        agent.SetWantsToYell();
                    agent.ChangeMorale(moraleChange);
                }
            }
        }

        private void SetWantsToYellForFormation(List<Agent> formation)
        {
            foreach (var agent in formation)
                agent.SetWantsToYell();
        }

        private void SetWantsToYellForTeam(Team team)
        {
            foreach (var agent in team.ActiveAgents)
                agent.SetWantsToYell();
        }

        private void SetupTroopsOfHeroes()
        {
            _heroTroops.Clear();
            foreach (var hero in _friendlyHeroes)
                _heroTroops[hero] = new List<Agent>();
            foreach (var hero in _enemyHeroes)
                _heroTroops[hero] = new List<Agent>();

            foreach (var agent in Mission.Current.AllAgents)
            {
                if (agent.Team != null && agent.IsHuman && !agent.IsHero && agent.Formation != null)
                {
                    if (_friendlyHeroes.Contains(agent.Formation.Captain))
                        _heroTroops[agent.Formation.Captain].Add(agent);
                    else if (_enemyHeroes.Contains(agent.Formation.Captain))
                        _heroTroops[agent.Formation.Captain].Add(agent);
                }
            }

            foreach (var hero in _friendlyHeroes)
                if (settings.LoggingEnabled)
                    InformationManager.DisplayMessage(new InformationMessage($"{hero.Name} has {_heroTroops[hero].Count} troops in their formation.", Colors.Green));
            

            foreach (var hero in _enemyHeroes)
                if (settings.LoggingEnabled)
                    InformationManager.DisplayMessage(new InformationMessage($"{hero.Name} has {_heroTroops[hero].Count} troops in their formation.", Colors.Red));
        }
    }
}