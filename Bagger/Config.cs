using Terraria.ID;
using Newtonsoft.Json;
using Terraria;
using TShockAPI;

namespace Bagger
{

    public class Config
    {
        public static string ConfigPath = Path.Combine(TShock.SavePath, "BaggerConfig.json");

        [JsonProperty("史莱姆王")]
        public DropInfo KingSlimeDrop = new DropInfo(ItemID.KingSlimeBossBag, 1);
        [JsonProperty("克眼")]
        public DropInfo EyeOFCthulhuDrop = new DropInfo(ItemID.EyeOfCthulhuBossBag, 1);
        [JsonProperty("世吞")]
        public DropInfo EaterOfWorldsDrop = new DropInfo(ItemID.EaterOfWorldsBossBag, 1);
        [JsonProperty("克脑")]
        public DropInfo BrainOfCthulhuDrop = new DropInfo(ItemID.BrainOfCthulhuBossBag, 1);
        [JsonProperty("蜂王")]
        public DropInfo QueenBeeDrop = new DropInfo(ItemID.QueenBeeBossBag, 1);
        [JsonProperty("骷髅王")]
        public DropInfo SkeletronDrop = new DropInfo(ItemID.SkeletronBossBag, 1);
        [JsonProperty("鹿角怪")]
        public DropInfo Deerclops = new DropInfo(ItemID.DeerclopsBossBag, 1);
        [JsonProperty("血肉墙")]
        public DropInfo WallOfFleshDrop = new DropInfo(ItemID.WallOfFleshBossBag, 1);
        [JsonProperty("史莱姆女皇")]
        public DropInfo QueenSlimeDrop = new DropInfo(ItemID.QueenSlimeBossBag, 1);
        [JsonProperty("毁灭者")]
        public DropInfo TheDestroyerDrop = new DropInfo(ItemID.DestroyerBossBag, 1);
        [JsonProperty("双子眼")]
        public DropInfo TheTwinsDrop = new DropInfo(ItemID.TwinsBossBag, 1);
        [JsonProperty("机械骷髅王")]
        public DropInfo SkeletronPrimeDrop = new DropInfo(ItemID.SkeletronPrimeBossBag, 1);
        [JsonProperty("世纪之花")]
        public DropInfo PlanteraDrop = new DropInfo(ItemID.PlanteraBossBag, 1);
        [JsonProperty("石巨人")]
        public DropInfo GolemDrop = new DropInfo(ItemID.GolemBossBag, 1);
        [JsonProperty("猪鲨")]
        public DropInfo DukeFishronDrop = new DropInfo(ItemID.FishronBossBag, 1);
        [JsonProperty("光女")]
        public DropInfo EmpressOfLight = new DropInfo(ItemID.FairyQueenBossBag, 1);
        [JsonProperty("拜月教徒")]
        public DropInfo LunaticCultistDrop = new DropInfo(ItemID.CultistBossBag, -1);
        [JsonProperty("月球领主")]
        public DropInfo MoonlordDrop = new DropInfo(ItemID.MoonLordBossBag, 1);

        public static Config Reload()
        {
            Config? c = null;

            if (File.Exists(ConfigPath))
            {
                c = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigPath));
            }

            if (c == null)
            {
                c = new Config();
                File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(c, Formatting.Indented));
            }

            return c;
        }
    }
}