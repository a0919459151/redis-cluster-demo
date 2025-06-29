# Redis cluster in win11 wsl (ubuntu 20)

## Redis cluster setup

### Write docker compose
``` yml
services:
  redis-node-1:
    image: redis:6.0.8
    container_name: redis-node-1
    command: redis-server --port 6381 --cluster-enabled yes --appendonly yes
    volumes:
      - /data/redis-node-1/data:/data
    network_mode: host

  redis-node-2:
    image: redis:6.0.8  
    container_name: redis-node-2
    command: redis-server --port 6382 --cluster-enabled yes --appendonly yes
    volumes:
      - /data/redis-node-2/data:/data
    network_mode: host

  redis-node-3:
    image: redis:6.0.8    
    container_name: redis-node-3
    command: redis-server --port 6383 --cluster-enabled yes --appendonly yes
    volumes:
      - /data/redis-node-3/data:/data
    network_mode: host

  redis-node-4:
    image: redis:6.0.8    
    container_name: redis-node-4
    command: redis-server --port 6384 --cluster-enabled yes --appendonly yes
    volumes:
      - /data/redis-node-4/data:/data
    network_mode: host

  redis-node-5:
    image: redis:6.0.8
    container_name: redis-node-5
    command: redis-server --port 6385 --cluster-enabled yes --appendonly yes
    volumes:
      - /data/redis-node-5/data:/data
    network_mode: host

  redis-node-6:
    image: redis:6.0.8
    container_name: redis-node-6
    command: redis-server --port 6386 --cluster-enabled yes --appendonly yes
    volumes:
      - /data/redis-node-6/data:/data
    network_mode: host
```


### Run docker compose
``` bash
docker compose -p redis-cluster up -d
```

### Check container
```bash
docker ps
```


### Create cluster
``` bash
joe@DESKTOP-I46A06U:~$ docker exec -it redis-node-2 sh
# redis-cli --cluster create 172.20.39.210:6381 172.20.39.210:6382 172.20.39.210:6383 172.20.39.210:6384 172.20.39.210:6385 172.20.39.210:6386 --cluster-replicas 1
>>> Performing hash slots allocation on 6 nodes...
Master[0] -> Slots 0 - 5460
Master[1] -> Slots 5461 - 10922
Master[2] -> Slots 10923 - 16383
Adding replica 172.20.39.210:6385 to 172.20.39.210:6381
Adding replica 172.20.39.210:6386 to 172.20.39.210:6382
Adding replica 172.20.39.210:6384 to 172.20.39.210:6383
>>> Trying to optimize slaves allocation for anti-affinity
[WARNING] Some slaves are in the same host as their master
M: d79fb1e96558be3a3d50b8a72ec13f2fcd6b79de 172.20.39.210:6381
   slots:[0-5460] (5461 slots) master
M: 15eb4fe4fd9feaed3759aec310b16a7037b4c506 172.20.39.210:6382
   slots:[5461-10922] (5462 slots) master
M: 4e58436f1cea504587f3d7d1894e448b1017101e 172.20.39.210:6383
   slots:[10923-16383] (5461 slots) master
S: 782d417cbee43f189f86c32fea567c995e8d67ee 172.20.39.210:6384
   replicates d79fb1e96558be3a3d50b8a72ec13f2fcd6b79de
S: d331bb4844874156df41f81ba356c4afca932d95 172.20.39.210:6385
   replicates 15eb4fe4fd9feaed3759aec310b16a7037b4c506
S: 548314c7a2b01e00761d54cb89f5b780e81222cf 172.20.39.210:6386
   replicates 4e58436f1cea504587f3d7d1894e448b1017101e
Can I set the above configuration? (type 'yes' to accept): yes
>>> Nodes configuration updated
>>> Assign a different config epoch to each node
>>> Sending CLUSTER MEET messages to join the cluster
Waiting for the cluster to join
....
>>> Performing Cluster Check (using node 172.20.39.210:6381)
M: d79fb1e96558be3a3d50b8a72ec13f2fcd6b79de 172.20.39.210:6381
   slots:[0-5460] (5461 slots) master
   1 additional replica(s)
M: 4e58436f1cea504587f3d7d1894e448b1017101e 172.20.39.210:6383
   slots:[10923-16383] (5461 slots) master
   1 additional replica(s)
S: d331bb4844874156df41f81ba356c4afca932d95 172.20.39.210:6385
   slots: (0 slots) slave
   replicates 15eb4fe4fd9feaed3759aec310b16a7037b4c506
S: 782d417cbee43f189f86c32fea567c995e8d67ee 172.20.39.210:6384
   slots: (0 slots) slave
   replicates d79fb1e96558be3a3d50b8a72ec13f2fcd6b79de
S: 548314c7a2b01e00761d54cb89f5b780e81222cf 172.20.39.210:6386
   slots: (0 slots) slave
   replicates 4e58436f1cea504587f3d7d1894e448b1017101e
M: 15eb4fe4fd9feaed3759aec310b16a7037b4c506 172.20.39.210:6382
   slots:[5461-10922] (5462 slots) master
   1 additional replica(s)
[OK] All nodes agree about slots configuration.
>>> Check for open slots...
>>> Check slots coverage...
[OK] All 16384 slots covered.
```

### Check cluster status
```bash
joe@DESKTOP-I46A06U:/mnt/c/Users/Joe/source/repos/redis-cluster/docker$ redis-cli -p 6381
127.0.0.1:6381> cluster info
cluster_state:ok
cluster_slots_assigned:16384
cluster_slots_ok:16384
cluster_slots_pfail:0
cluster_slots_fail:0
cluster_known_nodes:6
cluster_size:3
cluster_current_epoch:6
cluster_my_epoch:1
cluster_stats_messages_ping_sent:3082
cluster_stats_messages_pong_sent:2941
cluster_stats_messages_sent:6023
cluster_stats_messages_ping_received:2936
cluster_stats_messages_pong_received:3082
cluster_stats_messages_meet_received:5
cluster_stats_messages_received:6023

127.0.0.1:6381> cluster nodes
953f84c23110489138ca7ecec14ce17fa4b7309e 172.20.39.210:6382@16382 master - 0 1751162026224 2 connected 5461-10922
f6602c6a334b62b6f6a5ff13939bea29bd2d0a43 172.20.39.210:6381@16381 myself,master - 0 1751162027000 1 connected 0-5460
a9811af1ae3191d895bd31b6d0ea6e9521e1a039 172.20.39.210:6385@16385 slave 449595b832b057e3d5a491eb5c614dc2d62279e6 0 1751162024000 3 connected
449595b832b057e3d5a491eb5c614dc2d62279e6 172.20.39.210:6383@16383 master - 0 1751162027229 3 connected 10923-16383
7b2cba17a513067134f3609827de79178fa9521b 172.20.39.210:6386@16386 slave f6602c6a334b62b6f6a5ff13939bea29bd2d0a43 0 1751162026000 1 connected
005cb9f66aad945e29ba0252869a176ed2c0eb36 172.20.39.210:6384@16384 slave 953f84c23110489138ca7ecec14ce17fa4b7309e 0 1751162026000 2 connected

joe@DESKTOP-I46A06U:/mnt/c/Users/Joe/source/repos/redis-cluster/docker$ redis-cli --cluster check 172.20.39.210:6381
172.20.39.210:6381 (f6602c6a...) -> 2 keys | 5461 slots | 1 slaves.
172.20.39.210:6382 (953f84c2...) -> 1 keys | 5462 slots | 1 slaves.
172.20.39.210:6383 (449595b8...) -> 1 keys | 5461 slots | 1 slaves.
[OK] 4 keys in 3 masters.
0.00 keys per slot on average.
>>> Performing Cluster Check (using node 172.20.39.210:6381)
M: f6602c6a334b62b6f6a5ff13939bea29bd2d0a43 172.20.39.210:6381
   slots:[0-5460] (5461 slots) master
   1 additional replica(s)
M: 953f84c23110489138ca7ecec14ce17fa4b7309e 172.20.39.210:6382
   slots:[5461-10922] (5462 slots) master
   1 additional replica(s)
S: a9811af1ae3191d895bd31b6d0ea6e9521e1a039 172.20.39.210:6385
   slots: (0 slots) slave
   replicates 449595b832b057e3d5a491eb5c614dc2d62279e6
M: 449595b832b057e3d5a491eb5c614dc2d62279e6 172.20.39.210:6383
   slots:[10923-16383] (5461 slots) master
   1 additional replica(s)
S: 7b2cba17a513067134f3609827de79178fa9521b 172.20.39.210:6386
   slots: (0 slots) slave
   replicates f6602c6a334b62b6f6a5ff13939bea29bd2d0a43
S: 005cb9f66aad945e29ba0252869a176ed2c0eb36 172.20.39.210:6384
   slots: (0 slots) slave
   replicates 953f84c23110489138ca7ecec14ce17fa4b7309e
[OK] All nodes agree about slots configuration.
>>> Check for open slots...
>>> Check slots coverage...
[OK] All 16384 slots covered.

## 可以得知:
## Master <-> Slave | Slots
## 6381   <-> 6386  | 0-5460
## 6382   <-> 6384  | 5461-10922
## 6383   <-> 6385  | 10923-16383
## p.s. 每次 create cluster, master slave 配對是由 redis engine 決定, 配對結果不是固定的
```

---

## Demo get / set string from redis cluster

### Set string
``` bash
joe@DESKTOP-I46A06U:/mnt/c/Users/Joe/source/repos/redis-cluster/docker$ redis-cli -p 6381 redis-cli -p 6381
127.0.0.1:6381> set k1 v1
(error) MOVED 12706 172.20.39.210:6383
127.0.0.1:6381> set k2 v2
OK
127.0.0.1:6381> set k3 v4
OK
127.0.0.1:6381> set k4 v4
(error) MOVED 8455 172.20.39.210:6382
127.0.0.1:6381> exit

```

### Set string (with cluster mode)
``` bash
joe@DESKTOP-I46A06U:/mnt/c/Users/Joe/source/repos/redis-cluster/docker$ redis-cli -p 6381 -c
127.0.0.1:6381> set k1 v1
-> Redirected to slot [12706] located at 172.20.39.210:6383
OK
172.20.39.210:6383> set k4 v4
-> Redirected to slot [8455] located at 172.20.39.210:6382
OK
```

### Get string
``` bash
joe@DESKTOP-I46A06U:/mnt/c/Users/Joe/source/repos/redis-cluster/docker$ redis-cli -p 6381
127.0.0.1:6381> get k1
(error) MOVED 12706 172.20.39.210:6383
127.0.0.1:6381> get k2
"v2"
127.0.0.1:6381> exit
```

### Get string (with cluster mode)
``` bash
joe@DESKTOP-I46A06U:/mnt/c/Users/Joe/source/repos/redis-cluster/docker$ redis-cli -p 6381 -c
127.0.0.1:6381> get k1
-> Redirected to slot [12706] located at 172.20.39.210:6383
"v1"
172.20.39.210:6383> get k2
-> Redirected to slot [449] located at 172.20.39.210:6381
"v2"
172.20.39.210:6381> exit
```

---

## 延伸
### 加入新的 redis node 到集群
1. run container (ex: port=6387, port=6388)
2. redis cluster add node join cluster
``` bash
redis-cli --cluster add-node 172.20.39.210:6387 172.20.39.210:6381
## redis-cli --cluster add-node {new redis} {anyone old redis in cluster}
```
3. redis cluster reshard
``` bash
redis-cli --cluster reshard 172.20.39.210:6381
## how many slot do you want to move? 4096
## what is the receiving node ID?  XXXXXXXXXXXXXXXX

## p.s. 原先的 Master node 會平均切一些 slots 給新的 Master node
## Master4 -> [0-1364], [5461-6826], [10923-12287]
```

4. redis cluster add node join cluster (slave to master 4)
``` bash
redis-cli --cluster add-node 172.29.39.210:6388 172.29.39.210:6387 --cluster-slave --cluster-master-id XXXXXXXXXXXXXXXX
```
