using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;

namespace Bagger
{
    [ApiVersion(2, 1)]
    public class Bagger : TerrariaPlugin
    {
        #region 插件信息
        public override string Name => "Bagger";
        public override System.Version Version => new System.Version(1, 4, 1);
        public override string Author => "Soofa 羽学";
        public override string Description => "仅未参战玩家可领礼包.";
        #endregion

        #region 实例变量
        public static DatabaseManager DB = new DatabaseManager();
        internal static Configuration Config = new Configuration();
        #endregion

        #region 注册与卸载
        public Bagger(Main game) : base(game){}
        public override void Initialize()
        {
            LoadConfig();
            GeneralHooks.ReloadEvent += ReloadConfig;
            ServerApi.Hooks.GamePostInitialize.Register(this, OnGamePostInitialize);
            ServerApi.Hooks.NpcKilled.Register(this, OnNpcKilled);
            TShockAPI.Commands.ChatCommands.Add(new Command("bagger.getbags", Commands.GetBagsCmd, "gbag", "礼包","pk")
            {
                HelpText = "获取你错过的那些Boss的宝藏袋"
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GeneralHooks.ReloadEvent -= ReloadConfig;
                ServerApi.Hooks.GamePostInitialize.Deregister(this, OnGamePostInitialize);
                ServerApi.Hooks.NpcKilled.Deregister(this, OnNpcKilled);
                TShockAPI.Commands.ChatCommands.RemoveAll(x => x.CommandDelegate == Commands.GetBagsCmd);
            }
            base.Dispose(disposing);
        }
        #endregion

        #region 配置重载读取与写入方法
        private static void ReloadConfig(ReloadEventArgs args = null!)
        {
            LoadConfig();
            args.Player.SendInfoMessage("[Bagger]重新加载配置完毕。");
        }
        private static void LoadConfig()
        {
            Config = Configuration.Read();
            Config.Write();
        }
        #endregion

        #region 游戏后初始化事件
        private void OnGamePostInitialize(EventArgs args)
        {
            if (IsDefeated(NPCID.KingSlime) && NPC.downedSlimeKing) { Config.DownedBosses.Add(NPCID.KingSlime); }
            if (IsDefeated(NPCID.EyeofCthulhu) && NPC.downedBoss1) { Config.DownedBosses.Add(NPCID.EyeofCthulhu); }
            if (IsDefeated(NPCID.EaterofWorldsHead) && NPC.downedBoss2) { Config.DownedBosses.Add(NPCID.EaterofWorldsHead); }
            if (IsDefeated(NPCID.BrainofCthulhu) && NPC.downedBoss2) { Config.DownedBosses.Add(NPCID.BrainofCthulhu); }
            if (IsDefeated(NPCID.QueenBee) && NPC.downedQueenBee) { Config.DownedBosses.Add(NPCID.QueenBee); }
            if (IsDefeated(NPCID.SkeletronHead) && NPC.downedBoss3) { Config.DownedBosses.Add(NPCID.SkeletronHead); }
            if (IsDefeated(NPCID.Deerclops) && NPC.downedDeerclops) { Config.DownedBosses.Add(NPCID.Deerclops); }
            if (IsDefeated(NPCID.WallofFlesh) && Main.hardMode) { Config.DownedBosses.Add(NPCID.WallofFlesh); }
            if (IsDefeated(NPCID.QueenSlimeBoss) && NPC.downedQueenSlime) { Config.DownedBosses.Add(NPCID.QueenSlimeBoss); }
            if (IsDefeated(NPCID.TheDestroyer) && NPC.downedMechBoss1) { Config.DownedBosses.Add(NPCID.TheDestroyer); }
            if ((IsDefeated(NPCID.Retinazer) || IsDefeated(NPCID.Spazmatism)) && NPC.downedMechBoss2) 
            { 
                Config.DownedBosses.Add(NPCID.Retinazer); 
                Config.DownedBosses.Add(NPCID.Spazmatism); 
            }
            if (IsDefeated(NPCID.SkeletronPrime) && NPC.downedMechBoss3) { Config.DownedBosses.Add(NPCID.SkeletronPrime); }
            if (IsDefeated(NPCID.Plantera) && NPC.downedPlantBoss) { Config.DownedBosses.Add(NPCID.Plantera); }
            if (IsDefeated(NPCID.Golem) && NPC.downedGolemBoss) { Config.DownedBosses.Add(NPCID.Golem); }
            if (IsDefeated(NPCID.DukeFishron) && NPC.downedFishron) { Config.DownedBosses.Add(NPCID.DukeFishron); }
            if (IsDefeated(NPCID.HallowBoss) && NPC.downedEmpressOfLight) { Config.DownedBosses.Add(NPCID.HallowBoss); }
            if (IsDefeated(NPCID.CultistBoss) && NPC.downedAncientCultist) { Config.DownedBosses.Add(NPCID.CultistBoss); }
            if (IsDefeated(NPCID.DD2Betsy) && DD2Event._spawnedBetsyT3) { Config.DownedBosses.Add(NPCID.DD2Betsy); }
            if (IsDefeated(NPCID.MoonLordCore) && NPC.downedMoonlord) { Config.DownedBosses.Add(NPCID.MoonLordCore); }

            Config.Write();
        }
        #endregion

        #region 已击杀NPC事件
        private void OnNpcKilled(NpcKilledEventArgs args)
        {
            if (!args.npc.boss || Config.DownedBosses.Contains(args.npc.type) || !IsDefeated(args.npc.type))
            {
                return;
            }

            Config.DownedBosses.Add(args.npc.type);
            Config.Write();

            foreach (TSPlayer plr in TShock.Players)
            {
                if (plr != null && plr.Active)
                {
                    if (DB.IsPlayerInDb(plr.Name))
                    {
                        int claimedMask = DB.GetClaimedBossMask(plr.Name);
                        claimedMask = AddToTheMask(claimedMask, args.npc.type);

                        DB.SavePlayer(plr.Name, claimedMask);
                    }
                    else
                    {
                        DB.InsertPlayer(plr.Name, AddToTheMask(0, args.npc.type));
                    }
                }
            }
        }
        #endregion

        #region 根据怪物图鉴进一步判断进度方法
        private bool IsDefeated(int type)
        {
            var unlockState = Main.BestiaryDB.FindEntryByNPCID(type).UIInfoProvider.GetEntryUICollectionInfo().UnlockState;
            return unlockState == Terraria.GameContent.Bestiary.BestiaryEntryUnlockState.CanShowDropsWithDropRates_4;
        }
        #endregion

        #region 添加到掩码（数据库用）
        private int AddToTheMask(int mask, int type)
        {
            return type switch
            {
                NPCID.KingSlime => mask | 1,
                NPCID.EyeofCthulhu => mask | 2,
                NPCID.EaterofWorldsHead or NPCID.EaterofWorldsBody or NPCID.EaterofWorldsTail => mask | 4,
                NPCID.BrainofCthulhu => mask | 8,
                NPCID.QueenBee => mask | 16,
                NPCID.SkeletronHead => mask | 32,
                NPCID.Deerclops => mask | 64,
                NPCID.WallofFlesh => mask | 128,
                NPCID.QueenSlimeBoss => mask | 256,
                NPCID.TheDestroyer => mask | 512,
                NPCID.Spazmatism or NPCID.Retinazer => mask | 1024,
                NPCID.SkeletronPrime => mask | 2048,
                NPCID.Plantera => mask | 4096,
                NPCID.Golem => mask | 8192,
                NPCID.DukeFishron => mask | 16384,
                NPCID.HallowBoss => mask | 32768,
                NPCID.CultistBoss => mask | 65536,
                NPCID.DD2Betsy => mask | 131072,
                NPCID.MoonLordCore => mask | 262144,
                _ => mask,
            };
        }
        #endregion
    }
}