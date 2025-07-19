using StackExchange.Redis;

/*
// 基礎的 Redis cluster get & set 操作
var connStr = "172.20.39.210:6381,172.20.39.210:6382,172.20.39.210:6383,172.20.39.210:6384,172.20.39.210:6385,172.20.39.210:6386";
var redis = ConnectionMultiplexer.Connect(connStr);
var db = redis.GetDatabase();
await db.StringSetAsync("k1", "v1");
var value = await db.StringGetAsync("k1");
Console.WriteLine(value);
*/


// 測試: 連線字串只包含一個節點, 但仍能正常操作
// 注意: 這種方式在實際生產環境中不推薦使用, 因為如果該節點停止了, 會導致無法連接到集群
var connStr = "172.20.39.210:6386,allowAdmin=true";
var redis = ConnectionMultiplexer.Connect(connStr);
var db = redis.GetDatabase();
var slot = redis.HashSlot("k2");
var server = redis.GetServer("172.20.39.210:6384");
var nodes = server.ClusterNodes()!;
var e = nodes.GetBySlot(slot)!.EndPoint;
Console.WriteLine($@"key ""k2"" is in slot {slot}");
Console.WriteLine($@"key ""k2"" is on node {e}");
await db.StringSetAsync("k2", "v2");
var value = await db.StringGetAsync("k2");
Console.WriteLine(value);
// result:
// key "k2" is in slot 449
// key "k2" is on node 172.20.39.210:6381
// v2
