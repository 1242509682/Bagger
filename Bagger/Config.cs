using Terraria.ID;
using Newtonsoft.Json;
using Terraria;
using TShockAPI;

namespace Bagger
{

    public class Config
    {
        public static string ConfigPath = Path.Combine(TShock.SavePath, "BaggerConfig.json");

        [JsonProperty("ʷ��ķ��")]
        public DropInfo KingSlimeDrop = new DropInfo(ItemID.KingSlimeBossBag, 1);
        [JsonProperty("����")]
        public DropInfo EyeOFCthulhuDrop = new DropInfo(ItemID.EyeOfCthulhuBossBag, 1);
        [JsonProperty("����")]
        public DropInfo EaterOfWorldsDrop = new DropInfo(ItemID.EaterOfWorldsBossBag, 1);
        [JsonProperty("����")]
        public DropInfo BrainOfCthulhuDrop = new DropInfo(ItemID.BrainOfCthulhuBossBag, 1);
        [JsonProperty("����")]
        public DropInfo QueenBeeDrop = new DropInfo(ItemID.QueenBeeBossBag, 1);
        [JsonProperty("������")]
        public DropInfo SkeletronDrop = new DropInfo(ItemID.SkeletronBossBag, 1);
        [JsonProperty("¹�ǹ�")]
        public DropInfo Deerclops = new DropInfo(ItemID.DeerclopsBossBag, 1);
        [JsonProperty("Ѫ��ǽ")]
        public DropInfo WallOfFleshDrop = new DropInfo(ItemID.WallOfFleshBossBag, 1);
        [JsonProperty("ʷ��ķŮ��")]
        public DropInfo QueenSlimeDrop = new DropInfo(ItemID.QueenSlimeBossBag, 1);
        [JsonProperty("������")]
        public DropInfo TheDestroyerDrop = new DropInfo(ItemID.DestroyerBossBag, 1);
        [JsonProperty("˫����")]
        public DropInfo TheTwinsDrop = new DropInfo(ItemID.TwinsBossBag, 1);
        [JsonProperty("��е������")]
        public DropInfo SkeletronPrimeDrop = new DropInfo(ItemID.SkeletronPrimeBossBag, 1);
        [JsonProperty("����֮��")]
        public DropInfo PlanteraDrop = new DropInfo(ItemID.PlanteraBossBag, 1);
        [JsonProperty("ʯ����")]
        public DropInfo GolemDrop = new DropInfo(ItemID.GolemBossBag, 1);
        [JsonProperty("����")]
        public DropInfo DukeFishronDrop = new DropInfo(ItemID.FishronBossBag, 1);
        [JsonProperty("��Ů")]
        public DropInfo EmpressOfLight = new DropInfo(ItemID.FairyQueenBossBag, 1);
        [JsonProperty("���½�ͽ")]
        public DropInfo LunaticCultistDrop = new DropInfo(ItemID.CultistBossBag, -1);
        [JsonProperty("��������")]
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