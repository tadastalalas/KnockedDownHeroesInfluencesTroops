using System.Collections.Generic;
using System.Linq;
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

        private Agent? infantryFormationCaptain;
        private Agent? archersFormationCaptain;
        private Agent? cavalryFormationCaptain;
        private Agent? horseArchersFormationCaptain;

        private Agent? infantryEnemyFormationCaptain;
        private Agent? archersEnemyFormationCaptain;
        private Agent? cavalryEnemyFormationCaptain;
        private Agent? horseArchersEnemyFormationCaptain;

        private float elapsedTime = 0f;
        private Agent? _mainAgent;
        private readonly List<Agent> _friendlyHeroes = new();
        private readonly List<Agent> _enemyHeroes = new();
        private readonly Dictionary<Agent, List<Agent>> _heroTroops = new();

        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);
            if (!settings.EnableThisModification) return;
            UpdateHeroAndTroopLists(dt);
        }

        private void UpdateHeroAndTroopLists(float dt)
        {
            if (Mission.Current == null || (!Mission.Current.IsFieldBattle && !Mission.Current.IsSiegeBattle)) return;

            elapsedTime += dt;
            if (elapsedTime < settings.UpdateIntervalInSeconds) return;

            SetupMainAgents();
            SetupTroopsOfHeroes();
            SetupTeamsAndFormations();
            // SetupFormationsCaptains();
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

        private void SetupTeamsAndFormations()
        {
            var totalTeamsCount = 0;
            var friendlyTeamsCount = 0;
            var enemyTeamsCount = 0;

            List<Formation> enemyTeamsFormations = new List<Formation>();
            List<Formation> friendlyTeamsFormations = new List<Formation>();

            List<Formation> friendlyTeamsInfantryFormations = new List<Formation>();
            List<Formation> friendlyTeamsArchersFormations = new List<Formation>();
            List<Formation> friendlyTeamsCavalryFormations = new List<Formation>();
            List<Formation> friendlyTeamsHorseArchersFormations = new List<Formation>();
            List<Formation> enemyTeamsInfantryFormations = new List<Formation>();
            List<Formation> enemyTeamsArchersFormations = new List<Formation>();
            List<Formation> enemyTeamsCavalryFormations = new List<Formation>();
            List<Formation> enemyTeamsHorseArchersFormations = new List<Formation>();

            foreach (Team team in Enumerable.ToList<Team>(Mission.Current.Teams))
            {
                totalTeamsCount++;
                if (team.IsPlayerAlly)
                {
                    friendlyTeamsCount++;
                    foreach (Formation formation in Enumerable.ToList<Formation>(Enumerable.Where<Formation>(team.FormationsIncludingEmpty, (Formation f) => f.CountOfUnits > 0)))
                    {
                        friendlyTeamsFormations.Add(formation);
                        if (formation != null && formation.CountOfUnits > 0 && formation.QuerySystem.IsInfantryFormation)
                        {
                            friendlyTeamsInfantryFormations.Add(formation);
                        }
                        else if (formation != null && formation.CountOfUnits > 0 && formation.QuerySystem.IsRangedFormation)
                        {
                            friendlyTeamsArchersFormations.Add(formation);
                        }
                        else if (formation != null && formation.CountOfUnits > 0 && formation.QuerySystem.IsCavalryFormation)
                        {
                            friendlyTeamsCavalryFormations.Add(formation);
                        }
                        else if (formation != null && formation.CountOfUnits > 0 && formation.QuerySystem.IsRangedCavalryFormation)
                        {
                            friendlyTeamsHorseArchersFormations.Add(formation);
                        }
                    }
                }
                else
                {
                    enemyTeamsCount++;
                    foreach (Formation formation in Enumerable.ToList<Formation>(Enumerable.Where<Formation>(team.FormationsIncludingEmpty, (Formation f) => f.CountOfUnits > 0)))
                    {
                        enemyTeamsFormations.Add(formation);
                        if (formation != null && formation.CountOfUnits > 0 && formation.QuerySystem.IsInfantryFormation)
                        {
                            enemyTeamsInfantryFormations.Add(formation);
                        }
                        else if (formation != null && formation.CountOfUnits > 0 && formation.QuerySystem.IsRangedFormation)
                        {
                            enemyTeamsArchersFormations.Add(formation);
                        }
                        else if (formation != null && formation.CountOfUnits > 0 && formation.QuerySystem.IsCavalryFormation)
                        {
                            enemyTeamsCavalryFormations.Add(formation);
                        }
                        else if (formation != null && formation.CountOfUnits > 0 && formation.QuerySystem.IsRangedCavalryFormation)
                        {
                            enemyTeamsHorseArchersFormations.Add(formation);
                        }
                    }
                }
                    
            }
            if (settings.LoggingEnabled)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Teams count: {totalTeamsCount}", Colors.Magenta));
                InformationManager.DisplayMessage(new InformationMessage($"Friendly teams count: {friendlyTeamsCount}", Colors.Green));
                InformationManager.DisplayMessage(new InformationMessage($"Enemy teams count: {enemyTeamsCount}", Colors.Red));
                InformationManager.DisplayMessage(new InformationMessage($"Friendly formations count: {friendlyTeamsFormations.Count}", Colors.Green));
                InformationManager.DisplayMessage(new InformationMessage($"Enemy formations count: {enemyTeamsFormations.Count}", Colors.Red));
                InformationManager.DisplayMessage(new InformationMessage($"Friendly infantry formations count: {friendlyTeamsInfantryFormations.Count}", Colors.Green));
                InformationManager.DisplayMessage(new InformationMessage($"Friendly archers formations count: {friendlyTeamsArchersFormations.Count}", Colors.Green));
                InformationManager.DisplayMessage(new InformationMessage($"Friendly cavalry formations count: {friendlyTeamsCavalryFormations.Count}", Colors.Green));
                InformationManager.DisplayMessage(new InformationMessage($"Friendly horse archers formations count: {friendlyTeamsHorseArchersFormations.Count}", Colors.Green));
                InformationManager.DisplayMessage(new InformationMessage($"Enemy infantry formations count: {enemyTeamsInfantryFormations.Count}", Colors.Red));
                InformationManager.DisplayMessage(new InformationMessage($"Enemy archers formations count: {enemyTeamsArchersFormations.Count}", Colors.Red));
                InformationManager.DisplayMessage(new InformationMessage($"Enemy cavalry formations count: {enemyTeamsCavalryFormations.Count}", Colors.Red));
                InformationManager.DisplayMessage(new InformationMessage($"Enemy horse archers formations count: {enemyTeamsHorseArchersFormations.Count}", Colors.Red));
            }
                
        }

        private void SetupFormationsCaptains()
        {
            var friendlyInfantryCaptains = new List<Agent>();
            var friendlyArchersCaptains = new List<Agent>();
            var friendlyCavalryCaptains = new List<Agent>();
            var friendlyHorseArchersCaptains = new List<Agent>();

            var enemyInfantryCaptains = new List<Agent>();
            var enemyArchersCaptains = new List<Agent>();
            var enemyCavalryCaptains = new List<Agent>();
            var enemyHorseArchersCaptains = new List<Agent>();

            if (Mission.Current == null || Mission.Current.AllAgents == null) return;

            foreach (var agent in Mission.Current.AllAgents)
            {
                if (agent.Team == null || agent.Formation == null || agent.Formation.QuerySystem == null) continue;



                if (agent.Team.IsPlayerAlly)
                {
                    if (agent == agent.Formation.Captain)
                    {
                        if (agent.Formation.QuerySystem.IsInfantryFormation)
                        {
                            friendlyInfantryCaptains.Add(agent);
                            if (settings.LoggingEnabled)
                                InformationManager.DisplayMessage(new InformationMessage($"[F] Infantry captain: {agent.Name}", Colors.Yellow));
                        }
                        else if (agent.Formation.QuerySystem.IsRangedFormation)
                        {
                            friendlyArchersCaptains.Add(agent);
                            if (settings.LoggingEnabled)
                                InformationManager.DisplayMessage(new InformationMessage($"[F] Archers captain: {agent.Name}", Colors.Yellow));
                        }
                        else if (agent.Formation.QuerySystem.IsCavalryFormation)
                        {
                            friendlyCavalryCaptains.Add(agent);
                            if (settings.LoggingEnabled)
                                InformationManager.DisplayMessage(new InformationMessage($"[F] Cavalry captain: {agent.Name}", Colors.Yellow));
                        }
                        else if (agent.Formation.QuerySystem.IsRangedCavalryFormation)
                        {
                            friendlyHorseArchersCaptains.Add(agent);
                            if (settings.LoggingEnabled)
                                InformationManager.DisplayMessage(new InformationMessage($"[F] Horse archers captain: {agent.Name}", Colors.Yellow));
                        }
                    }
                }
                else
                {
                    if (agent == agent.Formation.Captain)
                    {
                        if (agent.Formation.QuerySystem.IsInfantryFormation)
                        {
                            enemyInfantryCaptains.Add(agent);
                            if (settings.LoggingEnabled)
                                InformationManager.DisplayMessage(new InformationMessage($"[E] Infantry captain: {agent.Name}", Colors.Red));
                        }
                        else if (agent.Formation.QuerySystem.IsRangedFormation)
                        {
                            enemyArchersCaptains.Add(agent);
                            if (settings.LoggingEnabled)
                                InformationManager.DisplayMessage(new InformationMessage($"[E] Archers captain: {agent.Name}", Colors.Red));
                        }
                        else if (agent.Formation.QuerySystem.IsCavalryFormation)
                        {
                            enemyCavalryCaptains.Add(agent);
                            if (settings.LoggingEnabled)
                                InformationManager.DisplayMessage(new InformationMessage($"[E] Cavalry captain: {agent.Name}", Colors.Red));
                        }
                        else if (agent.Formation.QuerySystem.IsRangedCavalryFormation)
                        {
                            enemyHorseArchersCaptains.Add(agent);
                            if (settings.LoggingEnabled)
                                InformationManager.DisplayMessage(new InformationMessage($"[E] Horse archers captain: {agent.Name}", Colors.Red));
                        }
                    }
                }
            }

            if (settings.LoggingEnabled)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Friendly Infantry Formations: {friendlyInfantryCaptains.Count}", Colors.Yellow));
                InformationManager.DisplayMessage(new InformationMessage($"Friendly Archers Formations: {friendlyArchersCaptains.Count}", Colors.Yellow));
                InformationManager.DisplayMessage(new InformationMessage($"Friendly Cavalry Formations: {friendlyCavalryCaptains.Count}", Colors.Yellow));
                InformationManager.DisplayMessage(new InformationMessage($"Friendly Horse Archers Formations: {friendlyHorseArchersCaptains.Count}", Colors.Yellow));

                InformationManager.DisplayMessage(new InformationMessage($"Enemy Infantry Formations: {enemyInfantryCaptains.Count}", Colors.Red));
                InformationManager.DisplayMessage(new InformationMessage($"Enemy Archers Formations: {enemyArchersCaptains.Count}", Colors.Red));
                InformationManager.DisplayMessage(new InformationMessage($"Enemy Cavalry Formations: {enemyCavalryCaptains.Count}", Colors.Red));
                InformationManager.DisplayMessage(new InformationMessage($"Enemy Horse Archers Formations: {enemyHorseArchersCaptains.Count}", Colors.Red));
            }
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

        public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
        {
            base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);

            if (!settings.EnableThisModification || Mission.Current == null || (!Mission.Current.IsFieldBattle && !Mission.Current.IsSiegeBattle) ||
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
            {

                MBInformationManager.AddQuickInformation(new TextObject("{=KDHIT_44PmYz}Your idiot captain is dead! Give up, you dogs!"), 2000, affectorAgent.Character, "event:/ui/notification/death");
            }
            if (!affectedAgent.Team.IsPlayerTeam && !affectedAgent.Team.IsPlayerAlly)
            {

                MBInformationManager.AddQuickInformation(new TextObject("{=KDHIT_GeOvFE}Enemy's captain is dead! They stand no chance against us!"), 2000, affectorAgent.Character, "event:/ui/notification/levelup");
            }
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
    }
}