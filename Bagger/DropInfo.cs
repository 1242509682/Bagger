
using Newtonsoft.Json;

namespace Bagger
{
    public class DropInfo
    {
        [JsonProperty("��ƷID")]
        public int ItemID { get; set; }

        [JsonProperty("����")]
        public int Stack { get; set; }

        public DropInfo(int itemID, int stack)
        {
            ItemID = itemID;
            Stack = stack;
        }
    }
}