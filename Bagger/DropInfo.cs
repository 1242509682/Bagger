
using Newtonsoft.Json;

namespace Bagger
{
    public class DropInfo
    {
        [JsonProperty("物品ID")]
        public int ItemID { get; set; }

        [JsonProperty("数量")]
        public int Stack { get; set; }

        public DropInfo(int itemID, int stack)
        {
            ItemID = itemID;
            Stack = stack;
        }
    }
}