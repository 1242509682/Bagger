using MySql.Data.MySqlClient;
using TShockAPI;
using TShockAPI.DB;

namespace Bagger;

public class DatabaseManager
{
    public DatabaseManager()
    {
        var sqlCreator = new SqlTableCreator(TShock.DB, new SqliteQueryCreator());
        sqlCreator.EnsureTableStructure(new SqlTable("Bagger",
            new SqlColumn("Name", MySqlDbType.String) { Primary = true, Unique = true },
            new SqlColumn("ClaimedBossesMask", MySqlDbType.Int32)));
    }

    public int GetClaimedBossMask(string name)
    {
        using var reader = TShock.DB.QueryReader("SELECT * FROM Bagger WHERE Name = @0", name);

        while (reader.Read())
        {
            return reader.Get<int>("ClaimedBossesMask");
        }
        throw new NullReferenceException();
    }

    public bool InsertPlayer(string name, int mask = 0)
    {
        return TShock.DB.Query("INSERT INTO Bagger (Name, ClaimedBossesMask) VALUES (@0, @1)", name, mask) != 0;
    }

    public bool SavePlayer(string name, int mask)
    {
        return TShock.DB.Query("UPDATE Bagger SET ClaimedBossesMask = @0 WHERE Name = @1", mask, name) != 0;
    }

    public bool IsPlayerInDb(string name)
    {
        return TShock.DB.QueryScalar<int>("SELECT COUNT(*) FROM Bagger WHERE Name = @0", name) > 0;
    }

    public bool ClearData()
    {
        return TShock.DB.Query("DELETE FROM Bagger") != 0;
    }
}
