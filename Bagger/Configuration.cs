using Newtonsoft.Json;
using System.Text;
using Terraria.ID;
using TShockAPI;

namespace Bagger;

public class Configuration
{

    [JsonProperty("使用说明", Order = -10)]
    public string Text = " 指令：/礼包 || 没打过BOSS才能领礼包 || 领取权限（bagger.getbags）|| 重置权限（bagger.admin）||";

    [JsonProperty("已击败BOSS", Order = -1)]
    public List<int> DownedBosses = new List<int>();

    [JsonProperty("史莱姆王", Order = 0)]
    [JsonConverter(typeof(ItemListConverter))]
    public List<ItemData> KingSlimeDrop { get; set; } = new List<ItemData>();

    [JsonProperty("克眼", Order = 1)]
    [JsonConverter(typeof(ItemListConverter))]
    public List<ItemData> EyeOFCthulhuDrop { get; set; } = new List<ItemData>();

    [JsonProperty("世吞", Order = 2)]
    [JsonConverter(typeof(ItemListConverter))]
    public List<ItemData> EaterOfWorldsDrop { get; set; } = new List<ItemData>();

    [JsonProperty("克脑", Order = 3)]
    [JsonConverter(typeof(ItemListConverter))]
    public List<ItemData> BrainOfCthulhuDrop { get; set; } = new List<ItemData>();

    [JsonProperty("蜂王", Order = 4)]
    [JsonConverter(typeof(ItemListConverter))]
    public List<ItemData> QueenBeeDrop { get; set; } = new List<ItemData>();

    [JsonProperty("骷髅王", Order = 5)]
    [JsonConverter(typeof(ItemListConverter))]
    public List<ItemData> SkeletronDrop { get; set; } = new List<ItemData>();

    [JsonProperty("鹿角怪", Order = 6)]
    [JsonConverter(typeof(ItemListConverter))]
    public List<ItemData> Deerclops { get; set; } = new List<ItemData>();

    [JsonProperty("血肉墙", Order = 7)]
    [JsonConverter(typeof(ItemListConverter))]
    public List<ItemData> WallOfFleshDrop { get; set; } = new List<ItemData>();

    [JsonProperty("史莱姆女皇", Order = 8)]
    [JsonConverter(typeof(ItemListConverter))]
    public List<ItemData> QueenSlimeDrop { get; set; } = new List<ItemData>();

    [JsonProperty("毁灭者", Order = 9)]
    [JsonConverter(typeof(ItemListConverter))]
    public List<ItemData> TheDestroyerDrop { get; set; } = new List<ItemData>();

    [JsonProperty("双子眼", Order = 10)]
    [JsonConverter(typeof(ItemListConverter))]
    public List<ItemData> TheTwinsDrop { get; set; } = new List<ItemData>();

    [JsonProperty("机械骷髅王", Order = 11)]
    [JsonConverter(typeof(ItemListConverter))]
    public List<ItemData> SkeletronPrimeDrop { get; set; } = new List<ItemData>();

    [JsonProperty("世纪之花", Order = 12)]
    [JsonConverter(typeof(ItemListConverter))]
    public List<ItemData> PlanteraDrop { get; set; } = new List<ItemData>();

    [JsonProperty("石巨人", Order = 13)]
    [JsonConverter(typeof(ItemListConverter))]
    public List<ItemData> GolemDrop { get; set; } = new List<ItemData>();

    [JsonProperty("猪鲨", Order = 14)]
    [JsonConverter(typeof(ItemListConverter))]
    public List<ItemData> DukeFishronDrop { get; set; } = new List<ItemData>();

    [JsonProperty("光女", Order = 15)]
    [JsonConverter(typeof(ItemListConverter))]
    public List<ItemData> EmpressOfLight { get; set; } = new List<ItemData>();

    [JsonProperty("拜月教徒", Order = 16)]
    [JsonConverter(typeof(ItemListConverter))]
    public List<ItemData> LunaticCultistDrop { get; set; } = new List<ItemData>();

    [JsonProperty("双足翼龙", Order = 17)]
    [JsonConverter(typeof(ItemListConverter))]
    public List<ItemData> BetsyDrop { get; set; } = new List<ItemData>();

    [JsonProperty("月球领主", Order = 18)]
    [JsonConverter(typeof(ItemListConverter))]
    public List<ItemData> MoonlordDrop { get; set; } = new List<ItemData>();

    #region 读取与创建配置文件方法
    public static string ConfigPath = Path.Combine(TShock.SavePath, "Bagger.json");
    public void Write()
    {
        var json = JsonConvert.SerializeObject(this, Formatting.Indented);
        File.WriteAllText(ConfigPath, json);
    }

    public static Configuration Read()
    {
        if (!File.Exists(ConfigPath))
        {
            var NewConfig = new Configuration();
            NewConfig.SetDefault();
            new Configuration().Write();
            return NewConfig;
        }
        else
        {
            var jsonContent = File.ReadAllText(ConfigPath);
            return JsonConvert.DeserializeObject<Configuration>(jsonContent)!;
        }
    }
    #endregion

    #region 预设物品
    public void SetDefault()
    {
        KingSlimeDrop = new List<ItemData>
        {
            new ItemData(ItemID.KingSlimeBossBag, 1),
            new ItemData(74, 1)
        };

        EyeOFCthulhuDrop = new List<ItemData>
        {
            new ItemData(ItemID.EyeOfCthulhuBossBag, 1)
        };

        EaterOfWorldsDrop = new List<ItemData>
        {
            new ItemData(ItemID.EaterOfWorldsBossBag, 1)
        };

        BrainOfCthulhuDrop = new List<ItemData>
        {
            new ItemData(ItemID.BrainOfCthulhuBossBag, 1)
        };

        QueenBeeDrop = new List<ItemData>
        {
            new ItemData(ItemID.QueenBeeBossBag, 1)
        };

        SkeletronDrop = new List<ItemData>
        {
            new ItemData(ItemID.SkeletronBossBag, 1)
        };

        Deerclops = new List<ItemData>
        {
            new ItemData(ItemID.DeerclopsBossBag, 1)
        };

        WallOfFleshDrop = new List<ItemData>
        {
            new ItemData(ItemID.WallOfFleshBossBag, 1),
            new ItemData(122, 1)
        };

        QueenSlimeDrop = new List<ItemData>
        {
            new ItemData(ItemID.QueenSlimeBossBag, 1),
            new ItemData(499, 50)
        };

        TheDestroyerDrop = new List<ItemData>
        {
            new ItemData(ItemID.DestroyerBossBag, 1),
            new ItemData(548, 30)
        };

        TheTwinsDrop = new List<ItemData>
        {
            new ItemData(ItemID.TwinsBossBag, 1),
            new ItemData(549, 30)
        };

        SkeletronPrimeDrop = new List<ItemData>
        {
            new ItemData(ItemID.SkeletronPrimeBossBag, 1),
            new ItemData(547, 30)
        };

        PlanteraDrop = new List<ItemData>
        {
            new ItemData(ItemID.PlanteraBossBag, 1)
        };

        GolemDrop = new List<ItemData>
        {
            new ItemData(ItemID.GolemBossBag, 1),
            new ItemData(1292, 1)
        };

        DukeFishronDrop = new List<ItemData>
        {
            new ItemData(ItemID.FishronBossBag, 1)
        };

        EmpressOfLight = new List<ItemData>
        {
            new ItemData(ItemID.FairyQueenBossBag, 1)
        };

        LunaticCultistDrop = new List<ItemData>
        {
            new ItemData(ItemID.CultistBossBag, 1),
            new ItemData(3549, 1)
        };

        BetsyDrop = new List<ItemData>
        {
            new ItemData(ItemID.BossBagBetsy, 1)
        };

        MoonlordDrop = new List<ItemData>
        {
            new ItemData(ItemID.MoonLordBossBag, 1)
        };

    }
    #endregion

    #region 物品数据结构
    public class ItemData
    {
        [JsonProperty("物品ID")]
        public int ID { get; set; }
        [JsonProperty("物品数量")]
        public int Stack { get; set; }

        public ItemData(int id, int stack)
        {
            ID = id;
            Stack = stack;
        }
    }
    #endregion

    #region 将List转换Dict字典
    public class ItemListConverter : JsonConverter<List<ItemData>>
    {
        public override void WriteJson(JsonWriter writer, List<ItemData> value, JsonSerializer serializer)
        {
            var itemDict = value.ToDictionary(item => item.ID, item => item.Stack);
            serializer.Serialize(writer, itemDict);
        }

        public override List<ItemData> ReadJson(JsonReader reader, Type objectType, List<ItemData> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var itemDict = serializer.Deserialize<Dictionary<int, int>>(reader);
            return itemDict?.Select(kv => new ItemData(kv.Key, kv.Value)).ToList() ?? new List<ItemData>();
        }
    } 
    #endregion
}