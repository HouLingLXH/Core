public class Deserializer
{
    public static void Deserialize(SerializableSet set)
    {
       
        for (int i = 0, l = set.PlayerDatas.Length; i < l; i++)
        {
            PlayerDataConfig.GetDictionary().Add(set.PlayerDatas[i].id, set.PlayerDatas[i]);
        }

    }
}
