using TShockAPI;
using System.Data;
using Terraria.ID;
using static Bagger.Bagger;
using Microsoft.Xna.Framework;

namespace Bagger;

internal class Commands
{
    #region 获取礼包的指令方法
    public static void GetBagsCmd(CommandArgs args)
    {
        if (args.Parameters.Count == 0)
        {
            help(args.Player);
            return;
        }

        if (args.Parameters.Count == 1 && args.Parameters[0].ToLower() == "list")
        {
            Lists(args.Player);
            return;
        }

        if (args.Parameters.Count == 1 && (args.Parameters[0].ToLower() == "重置" || args.Parameters[0].ToLower() == "reset"))
        {
            Reset(args.Player);
            return;
        }

        if (args.Parameters.Count == 1 && (args.Parameters[0].ToLower() == "领取全部" || args.Parameters[0].ToLower() == "all"))
        {
            int claimMask = 0;
            if (DB.IsPlayerInDb(args.Player.Name))
            {
                claimMask = DB.GetClaimedBossMask(args.Player.Name);
                SendMessage2(args.Player);
            }
            else
            {
                DB.InsertPlayer(args.Player.Name);
                SendMessage(args.Player);
            }

            if ((claimMask & 1) != 1 && Config.DownedBosses.Contains(NPCID.KingSlime))
            {
                claimMask |= 1;
                foreach (var itemData in Config.KingSlimeDrop)
                {
                    int id = itemData.ID;
                    int stack = itemData.Stack;
                    args.Player.GiveItem(id, stack);
                }
            }

            if ((claimMask & 2) != 2 && Config.DownedBosses.Contains(NPCID.EyeofCthulhu))
            {
                claimMask |= 2;
                foreach (var itemData in Config.EyeOFCthulhuDrop)
                {
                    int id = itemData.ID;
                    int stack = itemData.Stack;
                    args.Player.GiveItem(id, stack);
                }
            }

            if ((claimMask & 4) != 4 && Config.DownedBosses.Contains(NPCID.EaterofWorldsHead))
            {
                claimMask |= 4;
                foreach (var itemData in Config.EaterOfWorldsDrop)
                {
                    int id = itemData.ID;
                    int stack = itemData.Stack;
                    args.Player.GiveItem(id, stack);
                }
            }

            if ((claimMask & 8) != 8 && Config.DownedBosses.Contains(NPCID.BrainofCthulhu))
            {
                claimMask |= 8;
                foreach (var itemData in Config.BrainOfCthulhuDrop)
                {
                    int id = itemData.ID;
                    int stack = itemData.Stack;
                    args.Player.GiveItem(id, stack);
                }
            }

            if ((claimMask & 16) != 16 && Config.DownedBosses.Contains(NPCID.QueenBee))
            {
                claimMask |= 16;
                foreach (var itemData in Config.QueenBeeDrop)
                {
                    int id = itemData.ID;
                    int stack = itemData.Stack;
                    args.Player.GiveItem(id, stack);
                }
            }

            if ((claimMask & 32) != 32 && Config.DownedBosses.Contains(NPCID.SkeletronHead))
            {
                claimMask |= 32;
                foreach (var itemData in Config.SkeletronDrop)
                {
                    int id = itemData.ID;
                    int stack = itemData.Stack;
                    args.Player.GiveItem(id, stack);
                }
            }

            if ((claimMask & 64) != 64 && Config.DownedBosses.Contains(NPCID.Deerclops))
            {
                claimMask |= 64;
                foreach (var itemData in Config.Deerclops)
                {
                    int id = itemData.ID;
                    int stack = itemData.Stack;
                    args.Player.GiveItem(id, stack);
                }
            }

            if ((claimMask & 128) != 128 && Config.DownedBosses.Contains(NPCID.WallofFlesh))
            {
                claimMask |= 128;
                foreach (var itemData in Config.WallOfFleshDrop)
                {
                    int id = itemData.ID;
                    int stack = itemData.Stack;
                    args.Player.GiveItem(id, stack);
                }
            }

            if ((claimMask & 256) != 256 && Config.DownedBosses.Contains(NPCID.QueenSlimeBoss))
            {
                claimMask |= 256;
                foreach (var itemData in Config.QueenSlimeDrop)
                {
                    int id = itemData.ID;
                    int stack = itemData.Stack;
                    args.Player.GiveItem(id, stack);
                }
            }

            if ((claimMask & 512) != 512 && Config.DownedBosses.Contains(NPCID.TheDestroyer))
            {
                claimMask |= 512;
                foreach (var itemData in Config.TheDestroyerDrop)
                {
                    int id = itemData.ID;
                    int stack = itemData.Stack;
                    args.Player.GiveItem(id, stack);
                }
            }

            if ((claimMask & 1024) != 1024 && Config.DownedBosses.Contains(NPCID.Retinazer) && Config.DownedBosses.Contains(NPCID.Spazmatism))
            {
                claimMask |= 1024;
                foreach (var itemData in Config.TheTwinsDrop)
                {
                    int id = itemData.ID;
                    int stack = itemData.Stack;
                    args.Player.GiveItem(id, stack);
                }
            }

            if ((claimMask & 2048) != 2048 && Config.DownedBosses.Contains(NPCID.SkeletronPrime))
            {
                claimMask |= 2048;
                foreach (var itemData in Config.SkeletronPrimeDrop)
                {
                    int id = itemData.ID;
                    int stack = itemData.Stack;
                    args.Player.GiveItem(id, stack);
                }
            }

            if ((claimMask & 4096) != 4096 && Config.DownedBosses.Contains(NPCID.Plantera))
            {
                claimMask |= 4096;
                foreach (var itemData in Config.PlanteraDrop)
                {
                    int id = itemData.ID;
                    int stack = itemData.Stack;
                    args.Player.GiveItem(id, stack);
                }
            }

            if ((claimMask & 8192) != 8192 && Config.DownedBosses.Contains(NPCID.Golem))
            {
                claimMask |= 8192;
                foreach (var itemData in Config.GolemDrop)
                {
                    int id = itemData.ID;
                    int stack = itemData.Stack;
                    args.Player.GiveItem(id, stack);
                }
            }

            if ((claimMask & 16384) != 16384 && Config.DownedBosses.Contains(NPCID.DukeFishron))
            {
                claimMask |= 16384;
                foreach (var itemData in Config.DukeFishronDrop)
                {
                    int id = itemData.ID;
                    int stack = itemData.Stack;
                    args.Player.GiveItem(id, stack);
                }
            }

            if ((claimMask & 32768) != 32768 && Config.DownedBosses.Contains(NPCID.HallowBoss))
            {
                claimMask |= 32768;
                foreach (var itemData in Config.EmpressOfLight)
                {
                    int id = itemData.ID;
                    int stack = itemData.Stack;
                    args.Player.GiveItem(id, stack);
                }
            }

            if ((claimMask & 65536) != 65536 && Config.DownedBosses.Contains(NPCID.CultistBoss))
            {
                claimMask |= 65536;
                foreach (var itemData in Config.LunaticCultistDrop)
                {
                    int id = itemData.ID;
                    int stack = itemData.Stack;
                    args.Player.GiveItem(id, stack);
                }
            }

            if ((claimMask & 131072) != 131072 && Config.DownedBosses.Contains(NPCID.DD2Betsy))
            {
                claimMask |= 131072;
                foreach (var itemData in Config.BetsyDrop)
                {
                    int id = itemData.ID;
                    int stack = itemData.Stack;
                    args.Player.GiveItem(id, stack);
                }
            }

            if ((claimMask & 262144) != 262144 && Config.DownedBosses.Contains(NPCID.MoonLordCore))
            {
                claimMask |= 262144;
                foreach (var itemData in Config.MoonlordDrop)
                {
                    int id = itemData.ID;
                    int stack = itemData.Stack;
                    args.Player.GiveItem(id, stack);
                }
            }
            DB.SavePlayer(args.Player.Name, claimMask);
        }
    }
    #endregion

    #region 指令辅助方法
    private static void help(TSPlayer plr)
    {
        var NpcDeadInfo = string.Join(", ", Config.DownedBosses.Select(x => TShock.Utils.GetNPCById(x)?.FullName));
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

    private static void Lists(TSPlayer plr)
    {
        var NpcDeadInfo = string.Join(", ", Config.DownedBosses.Select(x => TShock.Utils.GetNPCById(x)?.FullName));
        if (plr == null) return;
        else
        {
            plr.SendInfoMessage("【Bagger】已击败Boss如下：\n" +
                $"[c/FDFEAF:{NpcDeadInfo}]\n");
        }
    }

    private static void Reset(TSPlayer plr)
    {
        if (!plr.HasPermission("bagger.admin"))
        {
            plr.SendErrorMessage("你没有权限重置[Bagger]进度礼包");
            return;
        }
        else if (DB.ClearData())
        {
            Config.DownedBosses.Clear();
            Config.Write();
            plr.SendSuccessMessage("已成功重置[Bagger]进度礼包");
        }
    }

    private static void SendMessage(TSPlayer plr)
    {
        var NpcDeadInfo = string.Join(", ", Config.DownedBosses.Select(x => TShock.Utils.GetNPCById(x)?.FullName));
        if (!string.IsNullOrEmpty(NpcDeadInfo))
            plr.SendMessage($"【Bagger】" +
            $"你[c/AFE0F4:未参与过]BOSS战，可领取礼包以下：\n" +
            $"[c/FDFEAF:{NpcDeadInfo}]", Color.GreenYellow);
        else
            plr.SendMessage($"【Bagger】因未击败[c/FF908C:任何Boss],无法领取礼包", Color.GreenYellow);
    }

    private static void SendMessage2(TSPlayer plr)
    {
        var NpcDeadInfo = string.Join(", ", Config.DownedBosses.Select(x => TShock.Utils.GetNPCById(x)?.FullName));
        if (!string.IsNullOrEmpty(NpcDeadInfo))
            plr.SendMessage($"【Bagger】\n" +
            $"你[c/AFE0F4:已参与]Boss战 或 [c/FF5F59:已领取]\n" +
            $"无法再领取礼包:[c/FDFEAF:{NpcDeadInfo}]", Color.GreenYellow);
        else
            plr.SendMessage($"【Bagger】因未击败[c/FF908C:任何Boss],无法领取礼包", Color.GreenYellow);
    }
    #endregion
}
