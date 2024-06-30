using Microsoft.Data.Sqlite;
using Microsoft.Xna.Framework;
using System.Data;
using Terraria;
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
        public override System.Version Version => new System.Version(1, 3, 0);
        public override string Author => "Soofa 羽学";
        public override string Description => "仅未参战玩家可领宝藏袋.";
        #endregion

        #region 实例变量
        private static IDbConnection db = new SqliteConnection("Data Source=" + Path.Combine(TShock.SavePath, "Bagger.sqlite"));
        public static DatabaseManager dbManager = new DatabaseManager(db);
        internal static Config Config = null!;
        private List<int> DownedBosses = new List<int>();
        #endregion

        #region 注册与卸载
        public Bagger(Main game) : base(game)
        {
        }
        public override void Initialize()
        {
            LoadConfig();
            GeneralHooks.ReloadEvent += LoadConfig;
            ServerApi.Hooks.GamePostInitialize.Register(this, OnGamePostInitialize);
            ServerApi.Hooks.NpcKilled.Register(this, OnNpcKilled);
            Commands.ChatCommands.Add(new Command("bagger.getbags", GetBagsCmd, "gbag", "礼包","pk")
            {
                AllowServer = false,
                HelpText = "获取你错过的那些Boss的宝藏袋"
            });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GeneralHooks.ReloadEvent -= LoadConfig;
                ServerApi.Hooks.GamePostInitialize.Deregister(this, OnGamePostInitialize);
                ServerApi.Hooks.NpcKilled.Deregister(this, OnNpcKilled);
            }
            base.Dispose(disposing);
        }
        #endregion

        #region 配置文件创建与重读加载方法
        private static void LoadConfig(ReloadEventArgs args = null!)
        {
            Config = Config.Read(Config.ConfigPath);
            Config.Write(Config.ConfigPath);
            if (args != null && args.Player != null)
            {
                args.Player.SendSuccessMessage("[Bagger]重新加载配置完毕。");
            }
        }
        #endregion

        #region 游戏后初始化事件
        private void OnGamePostInitialize(EventArgs args)
        {
            if (IsDefeated(NPCID.KingSlime)) { DownedBosses.Add(NPCID.KingSlime); }
            if (IsDefeated(NPCID.EyeofCthulhu)) { DownedBosses.Add(NPCID.EyeofCthulhu); }
            if (IsDefeated(NPCID.EaterofWorldsHead)) { DownedBosses.Add(NPCID.EaterofWorldsHead); }
            if (IsDefeated(NPCID.BrainofCthulhu)) { DownedBosses.Add(NPCID.BrainofCthulhu); }
            if (IsDefeated(NPCID.QueenBee)) { DownedBosses.Add(NPCID.QueenBee); }
            if (IsDefeated(NPCID.SkeletronHead)) { DownedBosses.Add(NPCID.SkeletronHead); }
            if (IsDefeated(NPCID.Deerclops)) { DownedBosses.Add(NPCID.Deerclops); }
            if (IsDefeated(NPCID.WallofFlesh)) { DownedBosses.Add(NPCID.WallofFlesh); }
            if (IsDefeated(NPCID.QueenSlimeBoss)) { DownedBosses.Add(NPCID.QueenSlimeBoss); }
            if (IsDefeated(NPCID.TheDestroyer)) { DownedBosses.Add(NPCID.TheDestroyer); }
            if (IsDefeated(NPCID.Spazmatism)) { DownedBosses.Add(NPCID.Spazmatism); }
            if (IsDefeated(NPCID.SkeletronPrime)) { DownedBosses.Add(NPCID.SkeletronPrime); }
            if (IsDefeated(NPCID.Plantera)) { DownedBosses.Add(NPCID.Plantera); }
            if (IsDefeated(NPCID.Golem)) { DownedBosses.Add(NPCID.Golem); }
            if (IsDefeated(NPCID.DukeFishron)) { DownedBosses.Add(NPCID.DukeFishron); }
            if (IsDefeated(NPCID.HallowBoss)) { DownedBosses.Add(NPCID.HallowBoss); }
            if (IsDefeated(NPCID.CultistBoss)) { DownedBosses.Add(NPCID.CultistBoss); }
            if (IsDefeated(NPCID.MoonLordCore)) { DownedBosses.Add(NPCID.MoonLordCore); }
        }
        #endregion

        #region 已击杀NPC事件
        private void OnNpcKilled(NpcKilledEventArgs args)
        {
            if (!args.npc.boss || DownedBosses.Contains(args.npc.type) || !IsDefeated(args.npc.type))
            {
                return;
            }

            DownedBosses.Add(args.npc.type);

            foreach (TSPlayer plr in TShock.Players)
            {
                if (plr != null && plr.Active)
                {
                    if (dbManager.IsPlayerInDb(plr.Name))
                    {
                        int claimedMask = dbManager.GetClaimedBossMask(plr.Name);
                        claimedMask = AddToTheMask(claimedMask, args.npc.type);

                        dbManager.SavePlayer(plr.Name, claimedMask);
                    }
                    else
                    {
                        dbManager.InsertPlayer(plr.Name, AddToTheMask(0, args.npc.type));
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
                NPCID.MoonLordCore => mask | 131072,
                _ => mask,
            };
        }
        #endregion

        #region 获取礼包的指令方法
        private void GetBagsCmd(CommandArgs args)
        {

            #region 默认指令
            if (args.Parameters.Count == 0)
            {
                help(args.Player);
                return;
            }
            #endregion

            #region 列出击败BOSS
            if (args.Parameters.Count == 1 && args.Parameters[0].ToLower() == "list")
            {
                Lists(args.Player);
                return;
            }
            #endregion

            #region 重置礼包
            if (args.Parameters.Count == 1 && (args.Parameters[0].ToLower() == "重置" || args.Parameters[0].ToLower() == "reset"))
            {
                Reset(args.Player);
                return;
            }
            #endregion

            #region 领取全部
            if (args.Parameters.Count == 1 && (args.Parameters[0].ToLower() == "领取全部" || args.Parameters[0].ToLower() == "all"))
            {
                int claimMask = 0;
                if (dbManager.IsPlayerInDb(args.Player.Name))
                {
                    claimMask = dbManager.GetClaimedBossMask(args.Player.Name);
                    SendMessage2(args.Player);
                }
                else
                {
                    dbManager.InsertPlayer(args.Player.Name);
                    SendMessage(args.Player);
                }

                if ((claimMask & 1) != 1 && DownedBosses.Contains(NPCID.KingSlime))
                {
                    claimMask |= 1;
                    foreach (var itemData in Config.KingSlimeDrop)
                    {
                        int id = itemData.ID;
                        int stack = itemData.Stack;
                        args.Player.GiveItem(id, stack);
                    }
                }

                if ((claimMask & 2) != 2 && DownedBosses.Contains(NPCID.EyeofCthulhu))
                {
                    claimMask |= 2;
                    foreach (var itemData in Config.EyeOFCthulhuDrop)
                    {
                        int id = itemData.ID;
                        int stack = itemData.Stack;
                        args.Player.GiveItem(id, stack);
                    }
                }

                if ((claimMask & 4) != 4 && DownedBosses.Contains(NPCID.EaterofWorldsHead))
                {
                    claimMask |= 4;
                    foreach (var itemData in Config.EaterOfWorldsDrop)
                    {
                        int id = itemData.ID;
                        int stack = itemData.Stack;
                        args.Player.GiveItem(id, stack);
                    }
                }

                if ((claimMask & 8) != 8 && DownedBosses.Contains(NPCID.BrainofCthulhu))
                {
                    claimMask |= 8;
                    foreach (var itemData in Config.BrainOfCthulhuDrop)
                    {
                        int id = itemData.ID;
                        int stack = itemData.Stack;
                        args.Player.GiveItem(id, stack);
                    }
                }

                if ((claimMask & 16) != 16 && DownedBosses.Contains(NPCID.QueenBee))
                {
                    claimMask |= 16;
                    foreach (var itemData in Config.QueenBeeDrop)
                    {
                        int id = itemData.ID;
                        int stack = itemData.Stack;
                        args.Player.GiveItem(id, stack);
                    }
                }

                if ((claimMask & 32) != 32 && DownedBosses.Contains(NPCID.SkeletronHead))
                {
                    claimMask |= 32;
                    foreach (var itemData in Config.SkeletronDrop)
                    {
                        int id = itemData.ID;
                        int stack = itemData.Stack;
                        args.Player.GiveItem(id, stack);
                    }
                }

                if ((claimMask & 64) != 64 && DownedBosses.Contains(NPCID.Deerclops))
                {
                    claimMask |= 64;
                    foreach (var itemData in Config.Deerclops)
                    {
                        int id = itemData.ID;
                        int stack = itemData.Stack;
                        args.Player.GiveItem(id, stack);
                    }
                }

                if ((claimMask & 128) != 128 && DownedBosses.Contains(NPCID.WallofFlesh))
                {
                    claimMask |= 128;
                    foreach (var itemData in Config.WallOfFleshDrop)
                    {
                        int id = itemData.ID;
                        int stack = itemData.Stack;
                        args.Player.GiveItem(id, stack);
                    }
                }

                if ((claimMask & 256) != 256 && DownedBosses.Contains(NPCID.QueenSlimeBoss))
                {
                    claimMask |= 256;
                    foreach (var itemData in Config.QueenSlimeDrop)
                    {
                        int id = itemData.ID;
                        int stack = itemData.Stack;
                        args.Player.GiveItem(id, stack);
                    }
                }

                if ((claimMask & 512) != 512 && DownedBosses.Contains(NPCID.TheDestroyer))
                {
                    claimMask |= 512;
                    foreach (var itemData in Config.TheDestroyerDrop)
                    {
                        int id = itemData.ID;
                        int stack = itemData.Stack;
                        args.Player.GiveItem(id, stack);
                    }
                }

                if ((claimMask & 1024) != 1024 && DownedBosses.Contains(NPCID.Spazmatism))
                {
                    claimMask |= 1024;
                    foreach (var itemData in Config.TheTwinsDrop)
                    {
                        int id = itemData.ID;
                        int stack = itemData.Stack;
                        args.Player.GiveItem(id, stack);
                    }
                }

                if ((claimMask & 2048) != 2048 && DownedBosses.Contains(NPCID.SkeletronPrime))
                {
                    claimMask |= 2048;
                    foreach (var itemData in Config.SkeletronPrimeDrop)
                    {
                        int id = itemData.ID;
                        int stack = itemData.Stack;
                        args.Player.GiveItem(id, stack);
                    }
                }

                if ((claimMask & 4096) != 4096 && DownedBosses.Contains(NPCID.Plantera))
                {
                    claimMask |= 4096;
                    foreach (var itemData in Config.PlanteraDrop)
                    {
                        int id = itemData.ID;
                        int stack = itemData.Stack;
                        args.Player.GiveItem(id, stack);
                    }
                }

                if ((claimMask & 8192) != 8192 && DownedBosses.Contains(NPCID.Golem))
                {
                    claimMask |= 8192;
                    foreach (var itemData in Config.GolemDrop)
                    {
                        int id = itemData.ID;
                        int stack = itemData.Stack;
                        args.Player.GiveItem(id, stack);
                    }
                }

                if ((claimMask & 16384) != 16384 && DownedBosses.Contains(NPCID.DukeFishron))
                {
                    claimMask |= 16384;
                    foreach (var itemData in Config.DukeFishronDrop)
                    {
                        int id = itemData.ID;
                        int stack = itemData.Stack;
                        args.Player.GiveItem(id, stack);
                    }
                }

                if ((claimMask & 32768) != 32768 && DownedBosses.Contains(NPCID.HallowBoss))
                {
                    claimMask |= 32768;
                    foreach (var itemData in Config.EmpressOfLight)
                    {
                        int id = itemData.ID;
                        int stack = itemData.Stack;
                        args.Player.GiveItem(id, stack);
                    }
                }

                if ((claimMask & 65536) != 65536 && DownedBosses.Contains(NPCID.CultistBoss))
                {
                    claimMask |= 65536;
                    foreach (var itemData in Config.LunaticCultistDrop)
                    {
                        int id = itemData.ID;
                        int stack = itemData.Stack;
                        args.Player.GiveItem(id, stack);
                    }
                }

                if ((claimMask & 131072) != 131072 && DownedBosses.Contains(NPCID.MoonLordCore))
                {
                    claimMask |= 131072;
                    foreach (var itemData in Config.MoonlordDrop)
                    {
                        int id = itemData.ID;
                        int stack = itemData.Stack;
                        args.Player.GiveItem(id, stack);
                    }
                }
                dbManager.SavePlayer(args.Player.Name, claimMask);
            }

            #endregion
        }
        #endregion

        #region 指令辅助方法
        private void help(TSPlayer plr)
        {
            var NpcDeadInfo = string.Join(", ", DownedBosses.Select(x => TShock.Utils.GetNPCById(x)?.FullName));
            if (plr == null) return;
            else
            {
                plr.SendMessage("【Bagger】主命令可用 /pk 或 /礼包\n" +
                 "/礼包 或 /pk — 查看Bagger指令菜单\n" +
                 "/pk list — 查看已击败BOSS\n" +
                 "/pk all 或 领取全部 — 领取所有礼包(打过不准领)\n" +
                 "/pk reset 或 重置 — 重置礼包领取状态\n" +
                 "/reload — 重载礼包配置文件", Color.GreenYellow);
            }
        }

        private void Lists(TSPlayer plr)
        {
            var NpcDeadInfo = string.Join(", ", DownedBosses.Select(x => TShock.Utils.GetNPCById(x)?.FullName));
            if (plr == null) return;
            else
            {
                plr.SendInfoMessage("【Bagger】已击败Boss如下：\n" +
                    $"[c/FDFEAF:{NpcDeadInfo}]\n");
            }
        }

        private void Reset(TSPlayer plr)
        {
            if (!plr.HasPermission("bagger.admin"))
            {
                plr.SendErrorMessage("你没有权限重置[Bagger]进度礼包");
                return;
            }
            else
            {
                if (dbManager.ClearData())
                {
                    plr.SendSuccessMessage("已成功重置[Bagger]进度礼包");
                }
            }
        }

        private void SendMessage(TSPlayer plr)
        {
            var NpcDeadInfo = string.Join(", ", DownedBosses.Select(x => TShock.Utils.GetNPCById(x)?.FullName));
            if (!string.IsNullOrEmpty(NpcDeadInfo))
                plr.SendMessage($"【Bagger】" +
                $"你[c/AFE0F4:未参与过]BOSS战，可领取礼包以下：\n" +
                $"[c/FDFEAF:{NpcDeadInfo}]", Color.GreenYellow);
            else
                plr.SendMessage($"【Bagger】因未击败[c/:FF908C任何Boss],无法领取礼包", Color.GreenYellow);
        }

        private void SendMessage2(TSPlayer plr)
        {
            var NpcDeadInfo = string.Join(", ", DownedBosses.Select(x => TShock.Utils.GetNPCById(x)?.FullName));
            if (!string.IsNullOrEmpty(NpcDeadInfo))
                plr.SendMessage($"【Bagger】\n" +
                $"你[c/AFE0F4:已参与]Boss战 或 [c/FF5F59:已领取]\n" +
                $"无法再领取礼包:[c/FDFEAF:{NpcDeadInfo}]", Color.GreenYellow);
            else
                plr.SendMessage($"【Bagger】因未击败[c/:FF908C任何Boss],无法领取礼包", Color.GreenYellow);
        }
        #endregion
    }
}